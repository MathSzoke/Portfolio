using AutoMapper;
using Backend.Data;
using Backend.Data.ErrorData;
using Backend.Data.UserRegistrationData;
using Backend.Data.UserRegistrationData.DTO;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class UserRegistrationController : ControllerBase
{
    private readonly UserRegistrationContext _context;
    private readonly IMapper _mapper;

    public UserRegistrationController(UserRegistrationContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<UserRegistration>> PostRegister([FromBody] PostUserRegistrationDTO pur)
    {
        UserRegistration ur = _mapper.Map<UserRegistration>(pur);

        string name = pur.Name;
        string email = pur.Email;
        DateTime birthday = pur.Birthday;
        string pass = pur.Password;
        string confirmPass = pur.ConfirmPassword;

        List<string> uID = new()
        {
            "UserID"
        };

        if (!ValidEmail.IsValidEmail(email))
        {
            string errorEmailFormat = "Email format is invalid!";
            return BadRequest(new { errorEmailFormat });
        }
        else if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(email) && string.IsNullOrEmpty(pass) && string.IsNullOrEmpty(confirmPass))
        {
            string errorEmpty = "Fill all fields!";
            return BadRequest(new { errorEmpty });
        }
        else if(pass != confirmPass)
        {
            string errorComparation = "Both passwords don't be equals";
            return Unauthorized(new { errorComparation });
        }
        else if(ValidEmail.IsValidEmail(email) && !string.IsNullOrEmpty(email))
        {
            string whereClause = string.Format("\"{0}\"", "Email") + " = " + string.Format("\'{0}\'", email);
            DataTable? data = DatabaseExecs.ExecAnySelect("Users", uID, whereClause);
            string errorExists = "This email is already used";
            if(data.Rows.Count == 1) return Conflict(new { errorExists });
        }

        try
        {
            Dictionary<string, object> data = new()
            {
                { "Name", name},
                { "Email", email},
                { "Birthday", birthday},
                { "Password", pass},
            };

            DatabaseExecs.InsertData("Users", data);
        }
        catch(Exception ex)
        {
            ErrorLog.LogError(ex);
            string errorMessage = "Error on register";
            return BadRequest(new { errorMessage });
        }

        return CreatedAtAction(nameof(GetRegister), new { id = ur.UserID }, ur);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserRegistration>> GetRegister(int id)
    {
        if(_context.UserRegister == null)
        {
            return NotFound();
        }

        var user = await _context.UserRegister.FirstOrDefaultAsync(x => x.UserID == id);

        if (user == null)
        {
            return NotFound();
        }

        return user;
    }
}
