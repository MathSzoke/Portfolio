using AutoMapper;
using Backend.Data;
using Backend.Data.ErrorData;
using Backend.Data.UserLoginData;
using Backend.Data.UserLoginData.DTO;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using NpgsqlTypes;
using System.Data;

namespace Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class UserLoginController : ControllerBase
{
    private readonly UserLoginContext _context;
    private readonly IMapper _mapper;

    public UserLoginController(UserLoginContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost("PostUserLogin")]
    public ActionResult<UserLogin> PostUserLogin([FromBody] PostUserLoginDTO pulDTO)
    {
        UserLogin ul = _mapper.Map<UserLogin>(pulDTO);

        string email = pulDTO.Email;
        string password = pulDTO.Password;

        if (!ValidEmail.IsValidEmail(email))
        {
            string error = "Email format is invalid!";
            return BadRequest(new { error });
        }
        else if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(password))
        {
            string errorEmpty = "Preencha todos os campos, não é legal enviar um email vázio para alguém!";
            return BadRequest(new { errorEmpty });
        }

        try
        {
            List<NpgsqlParameter> parameters = new()
            {
                new NpgsqlParameter("email", NpgsqlDbType.Text) { Value = email },
                new NpgsqlParameter("pass", NpgsqlDbType.Text) { Value = password }
            };

            bool execProc = DatabaseExecs.ExecProcedure("usp_Login", parameters); // Validate if user and password exists, if yes, update IsActive to true

            if(!execProc)
            {
                string errorAccess = "Parece que o email ou a senha não coincidem! Por favor, tente novamente.";
                return Unauthorized(new { errorAccess });
            }
        }
        catch (Exception ex)
        {
            ErrorLog.LogError(ex);
        }

        return Ok();
    }

    [HttpGet("GetLogout/{id}")]
    public ActionResult<UserLogin> GetLogout(int id)
    {
        try
        {
            List<NpgsqlParameter> parameters = new()
            {
                new NpgsqlParameter("userid", NpgsqlDbType.Integer) { Value = id }
            };

            bool execProc = DatabaseExecs.ExecProcedure("usp_Logout", parameters); // Execute the logout procedure

            if (!execProc)
            {
                string error = "Failed to logout user.";
                return BadRequest(new { error });
            }

            return Ok();
        }
        catch (Exception ex)
        {
            ErrorLog.LogError(ex);
            string error = "An error occurred while processing the logout request.";
            return StatusCode(500, new { error });
        }
    }


    [HttpGet("GetUserID/{email}")]
    public async Task<ActionResult<int>> GetUserID(string email)
    {
        UserLogin ul = new();

        List<string> column = new()
        {
            "UserID"
        };
        string errorAccess = "Parece que o email ou a senha não coincidem! Por favor, tente novamente.";

        string whereClause = string.Format("\"{0}\"", "Email") + " ILIKE " + string.Format("\'{0}\'", email) + " AND " + string.Format("\"{0}\"", "IsActive") + " = " + string.Format("\'{0}\'", "true");
        DataTable? data = DatabaseExecs.ExecAnySelect("Users", column, whereClause);

        if (data.Rows.Count == 0) return Conflict(new { errorAccess });

        var userId = (int)data.Rows[0]["UserID"];

        int id = ul.UserID = userId;

        return id;
    }

    [HttpGet("GetUserName/{id}")]
    public async Task<ActionResult<string>> GetUserName(int id)
    {
        List<string> column = new()
        {
            "Name"
        };

        string errorAccess = "Parece que não foi possível capturar o seu nome no sistema! Por favor, tente novamente.";
        string whereClause = string.Format("\"{0}\"", "UserID") + " = " + string.Format("\'{0}\'", id) + " AND " + string.Format("\"{0}\"", "IsActive") + " = " + string.Format("\'{0}\'", "true");
        DataTable? data = DatabaseExecs.ExecAnySelect("Users", column, whereClause);

        if (data.Rows.Count == 0) return Conflict(new { errorAccess });

        var name = (string)data.Rows[0]["Name"];

        return AbbreviateMiddleNames(name);
    }

    static string AbbreviateMiddleNames(string fullName)
    {
        string[] splitName = fullName.Split(' ');

        if (splitName.Length > 2)
        {
            for (int i = 1; i < splitName.Length - 1; i++)
            {
                if (splitName[i].Length > 3)
                {
                    splitName[i] = splitName[i][0] + ".";
                }
            }
        }

        string abbreviatedName = string.Join(" ", splitName);

        return abbreviatedName;
    }
}
