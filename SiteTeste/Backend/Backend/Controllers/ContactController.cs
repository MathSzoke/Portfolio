using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models;
using AutoMapper;
using Backend.Data.ContactData.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Backend.Data.ContactData;
using Backend.Data;

namespace Backend.Controllers;

[ApiController]
[Route("[controller]")]
public partial class ContactController : ControllerBase
{
    private readonly ContactContext _context;
    private readonly IMapper _mapper;

    public ContactController(ContactContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // POST: Contact
    [HttpPost]
    public async Task<ActionResult<Contact>> PostContact([FromBody] PostContactDTO pc)
    {
        Contact c = _mapper.Map<Contact>(pc);

        string name = pc.Name;
        string email = pc.Email;
        string subject = pc.Subject;
        string message = pc.Message;

        var body = $"Mensagem: {message} <br><br> Enviado em: {DateTime.Now}";

        if (!ValidEmail.IsValidEmail(email))
        {
            string errorEmailFormat = "Email format is invalid!";
            return BadRequest(new { errorEmailFormat });
        }
        else if (string.IsNullOrEmpty(subject) && string.IsNullOrEmpty(message) && string.IsNullOrEmpty(name) && string.IsNullOrEmpty(email))
        {
            string errorEmpty = "Preencha todos os campos, não é legal enviar um email vázio para alguém!";
            return BadRequest(new { errorEmpty });
        }

        SendEmail.Send(subject, body, name, email);
        _context.Contact.Add(c);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetContact), new { id = c.IDContact }, c);
    }

    // GET: Contact
    [HttpGet]
    public List<ActionResult<IEnumerable<GetContactDTO>>> GetContacts([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        return (List<ActionResult<IEnumerable<GetContactDTO>>>)(IAsyncEnumerable<GetContactDTO>)_mapper.Map<List<GetContactDTO>>(_context.Contact.Skip(skip).Take(take));
    }

    // GET: Contact/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Contact>> GetContact(int id)
    {
        if (_context.Contact == null)
        {
            return NotFound();
        }
        var contact = await _context.Contact.FirstOrDefaultAsync(x => x.IDContact == id);

        if (contact == null)
        {
            return NotFound();
        }

        return contact;
    }

    // PUT: Contact/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutContact(int id, Contact c)
    {
        if (id != c.IDContact)
        {
            return BadRequest();
        }

        _context.Entry(c).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ContactExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchContact(int id, JsonPatchDocument<UpdateContactDTO> patch)
    {
        var contact = await _context.Contact.FindAsync(id);
        if (contact == null) return NotFound();
        var contactToUpdate = _mapper.Map<UpdateContactDTO>(contact);

        patch.ApplyTo(contactToUpdate, (Microsoft.AspNetCore.JsonPatch.Adapters.IObjectAdapter)ModelState);

        if (!TryValidateModel(contactToUpdate))
        {
            return ValidationProblem(ModelState);
        }

        _mapper.Map(contactToUpdate, contact);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: Contact/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContact(int id)
    {
        var contact = await _context.Contact.FindAsync(id);
        if (contact == null) return NotFound();
        var contactToDelete = _mapper.Map<UpdateContactDTO>(contact);

        _mapper.Map(contactToDelete, contact);
        _context.Contact.Remove(contact);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ContactExists(int id)
    {
        return (_context.Contact?.Any(e => e.IDContact == id)).GetValueOrDefault();
    }
}