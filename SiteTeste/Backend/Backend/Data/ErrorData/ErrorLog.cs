using Backend.Data.ContactData.DTO;
using System.Data;
using System.Net;

namespace Backend.Data.ErrorData;
public class ErrorLog
{
    public static void LogError(Exception ex)
    {
        string methodName = ex.TargetSite.Name;
        string fileName = ex.TargetSite.DeclaringType.Assembly.Location;
        string errorMessage = ex.Message.Replace("'", "") + "\n" + ex.InnerException;

        int httpStatus = 0;

        // Obtém o código de status HTTP da exceção
        if (ex is HttpRequestException hre)
        {
            httpStatus = (int)hre.StatusCode;
        }
        else if (ex is WebException we)
        {
            httpStatus = (int)((HttpWebResponse)we.Response).StatusCode;
        }

        PostContactDTO pc = new();

        List<string> uID = new()
        {
            "UserID"
        };

        string? email = pc.Email == null ? string.Format("\"{0}\"", "Email") + " = 'undefined'" : string.Format("\"{0}\"", "Email") + " = " + string.Format("\'{0}\'", pc.Email);

        DataTable? data = DatabaseExecs.ExecAnySelect("Users", uID, email);
        int userId = (int)data.Rows[0]["UserID"];

        Dictionary<string, object> errorData = new()
        {
            { "UserID",  userId },
            { "ErrorPath",  fileName },
            { "MethodError",  methodName },
            { "StackTrace",  ex.StackTrace },
            { "ErrorMessage",  errorMessage },
            { "HttpStatus",  httpStatus },
            { "DateTimeError",  DateTime.Now },
        };

        DatabaseExecs.InsertData("LogError", errorData);
    }
}