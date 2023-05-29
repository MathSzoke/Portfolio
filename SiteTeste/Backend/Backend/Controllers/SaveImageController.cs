using Amazon.S3.Model;
using Amazon.S3;
using AutoMapper;
using Backend.Data.ErrorData;
using Backend.Data.SaveImageData;
using Backend.Data.SaveImageData.DTO;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Amazon.Runtime;
using Backend.Data;
using System.Data;
using Newtonsoft.Json;

namespace Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class SaveImageController : Controller
{
    private readonly SaveImageContext _context;
    private readonly IMapper _mapper;
    private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;

    public SaveImageController(SaveImageContext context, IMapper mapper, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
    {
        _context = context;
        _mapper = mapper;
        _environment = environment;
    }

    public class ImageInfo
    {
        public string OriginalUrl { get; set; } = null!;
        public string Base64Image { get; set; } = null!;
    }

    [HttpPost("PostSaveImage")]
    public async Task<ActionResult<SavePhoto>> PostSaveImage([FromBody] PostSaveImageDTO psiDTO)
    {
        SavePhoto sp = _mapper.Map<SavePhoto>(psiDTO);

        string base64URL = psiDTO.UrlPhoto;
        int u = psiDTO.UserID;
        try
        {
            var credentials = new BasicAWSCredentials("AKIAW37BVX5X5DLNIOKY", "Ycpuw0bfjRZsvVx/AEOhrQfmeYGfeDH7sO99Lxy8");
            // Criar uma nova instância do AmazonS3Client
            using var client = new AmazonS3Client(credentials, Amazon.RegionEndpoint.SAEast1);

            // Converter a string base64 em um array de bytes
            byte[] imageBytes = Convert.FromBase64String(base64URL);

            // Gerar um nome único para o arquivo
            var fileName = Guid.NewGuid().ToString() + ".png";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
            System.IO.File.WriteAllBytes(filePath, imageBytes);

            // Criar um novo objeto PutObjectRequest
            var request = new PutObjectRequest
            {
                BucketName = "photousers",
                Key = "images/" + fileName,
                ContentType = "image/png",
                InputStream = new MemoryStream(imageBytes)
            };

            // Fazer o upload do arquivo para o Amazon S3
            var response = await client.PutObjectAsync(request);

            // Atualizar a URL da foto no objeto SavePhoto
            sp.UrlPhoto = "https://photousers.s3.amazonaws.com/images/" + fileName;
            sp.UserID = u;
            sp.IsActive = true;

            // Adicionar o objeto SavePhoto ao contexto e salvar as mudanças
            _context.SavePhoto.Add(sp);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, fileName });
        }
        catch (Exception ex)
        {
            ErrorLog.LogError(ex);
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpGet("GetPhoto/{id}")]
    public async Task<ActionResult<string>> GetPhoto(int id)
    {
        var photo = await _context.SavePhoto.Where(x => x.UserID == id && x.IsActive == true).Select(x => x.UrlPhoto).FirstOrDefaultAsync();

        if (photo == null) return NotFound();

        return photo;
    }

    [HttpGet("GetAllPhotos/{id}")]
    public async Task<ActionResult<List<ImageInfo>>> GetAllPhotos(int id)
    {
        var photos = await _context.SavePhoto.Where(x => x.UserID == id).OrderBy(x => x.PhotoID).Select(x => x.UrlPhoto).ToListAsync();

        var images = new List<ImageInfo>();

        if (photos.Count == 0) return NotFound();

        foreach (var photo in photos)
        {
            var credentials = new BasicAWSCredentials("AKIAW37BVX5X5DLNIOKY", "Ycpuw0bfjRZsvVx/AEOhrQfmeYGfeDH7sO99Lxy8");
            // Criar uma nova instância do AmazonS3Client
            using var client = new AmazonS3Client(credentials, Amazon.RegionEndpoint.SAEast1);
            var getObjectRequest = new GetObjectRequest
            {
                BucketName = "photousers",
                Key = photo.Replace("https://photousers.s3.amazonaws.com/", "")
            };
            using var getObjectResponse = await client.GetObjectAsync(getObjectRequest);
            using var memoryStream = new MemoryStream();
            getObjectResponse.ResponseStream.CopyTo(memoryStream);
            var imageBytes = memoryStream.ToArray();
            var base64String = "data:image/jpeg;base64," + Convert.ToBase64String(imageBytes);
            images.Add(new ImageInfo { OriginalUrl = photo, Base64Image = base64String });
        }

        return images;
    }

    [HttpGet("GetImageByName/{imageName}")]
    public ActionResult<string> GetImageByName(string imageName)
    {
        var imageLocalUrl = Path.Combine(_environment.WebRootPath.Replace("/", "\\"), "images", imageName);
        if (System.IO.File.Exists(imageLocalUrl))
            return imageLocalUrl;
        else
            return NotFound();
    }

    [HttpDelete("DeletePhoto/{url}")]
    public async Task<IActionResult> DeletePhoto(string url)
    {
        string errorAccess = "Parece que não foi possível encontrar sua foto no sistema! Por favor, tente novamente.";

        try
        {
            var unescapedUrl = Uri.UnescapeDataString(url);

            var photoIsActive = await _context.SavePhoto.Where(x => x.UrlPhoto == unescapedUrl && x.IsActive == true).FirstOrDefaultAsync();

            List<string> column = new()
            {
                "PhotoID"
            };

            string whereClause = string.Format("\"{0}\"", "UrlPhoto") + " = " + string.Format("\'{0}\'", unescapedUrl) + " and " + string.Format("\"{0}\"", "IsActive") + " = " + string.Format("\'{0}\'", "false");
            DataTable? data = DatabaseExecs.ExecAnySelect("SavePhoto", column, whereClause);

            if (data.Rows.Count == 0) return NotFound();

            int id = (int)data.Rows[0]["PhotoID"];

            var photo = await _context.SavePhoto.FindAsync(id);
            if (photo == null) return NotFound();
            var photoToDelete = _mapper.Map<UpdateSaveImageDTO>(photo);

            _mapper.Map(photoToDelete, photo);
            _context.SavePhoto.Remove(photo);
            await _context.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            ErrorLog.LogError(ex);
            return Conflict(new { errorAccess });
        }

        return NoContent();
    }

    [HttpPut("RemovePhoto/{id}")]
    public async Task<IActionResult> RemovePhoto(int id)
    {
        var photo = await _context.SavePhoto.Where(x => x.UserID == id && x.IsActive == true).FirstOrDefaultAsync();
        if (photo == null) return NotFound();

        var photoToUpdate = _mapper.Map<UpdateSaveImageDTO>(photo);

        photoToUpdate.IsActive = false;

        _mapper.Map(photoToUpdate, photo);
        _context.SavePhoto.Update(photo);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPut("AlterPhoto/{id}/{urlPhoto}")]
    public async Task<IActionResult>AlterPhoto(int id, string urlPhoto)
    {
        string errorAlter = "Parece que ocorreu um erro inesperado";

        try
        {
            var unescapedUrl = Uri.UnescapeDataString(urlPhoto);

            var photo = await _context.SavePhoto.Where(x => x.UserID == id && x.IsActive == true).FirstOrDefaultAsync();

            if(photo != null)
            {
                var photoToAlter = _mapper.Map<UpdateSaveImageDTO>(photo);

                photoToAlter.IsActive = false;

                _mapper.Map(photoToAlter, photo);
                _context.SavePhoto.Update(photo);
            }
            else
            {
                // Atualizar a nova foto com IsActive = true
                var newPhoto = await _context.SavePhoto.Where(x => x.UserID == id && x.IsActive == false && x.UrlPhoto == unescapedUrl).FirstOrDefaultAsync();

                if (newPhoto == null) return NotFound();

                newPhoto.IsActive = true;

                _context.SavePhoto.Update(newPhoto);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }
        catch(Exception e)
        {
            ErrorLog.LogError(e);
            return Conflict(new { errorAlter });
        }
    }
}