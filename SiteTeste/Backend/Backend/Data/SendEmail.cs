using Backend.Data.ErrorData;
using SendGrid.Helpers.Mail;
using SendGrid;
using System.Data;

namespace Backend.Data;
public class SendEmail
{
    public static async void Send(string subject, string body, string name, string from)
    {
        string to = "Matheusszoke@gmail.com";

        try
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var client = new SendGridClient(apiKey);

            var msg = new SendGridMessage()
            {
                From = new EmailAddress(from, name),
                Subject = subject,
                PlainTextContent = body,
                HtmlContent = body
            };

            msg.AddTo(new EmailAddress(to, "Matheus Szoke"));

            await client.SendEmailAsync(msg);

            InsertEmailDB("Ok", null, name, from);
        }
        catch (Exception e)
        {
            ErrorLog.LogError(e);

            List<string> eID = new()
            {
                "LogID"
            };

            DataTable? data = DatabaseExecs.ExecAnySelect("LogError", eID, "ErrorMessage = " + string.Format("\'{0}\'", e));
            var logID = (int)data.Rows[0]["LogID"];

            InsertEmailDB("Fail", logID, name, from);
        }
    }
    public static void InsertEmailDB(string status, object? logID, string name, string email)
    {
        List<string> uID = new()
        {
            "UserID"
        };

        string s = status switch
        {
            "Ok" => "Send successfully",
            "Fail" => "Send failed",
            _ => "Send email cancel"
        };

        string? e = email == null ? string.Format("\"{0}\"", "Email") + " = 'undefined'" : string.Format("\"{0}\"", "Email") + " = " + string.Format("\'{0}\'", email);

        try
        {
            DataTable? data = DatabaseExecs.ExecAnySelect("Users", uID, e);
            var userId = data.Rows.Count == 0 ? 1 : (int)data.Rows[0]["UserID"];

            Dictionary<string, object> emailData = new()
            {
                { "UserID",  userId },
                { "Name",  name },
                { "Email",  email },
                { "StatusEmail",  s },
                { "DateSend",  DateTime.Now },
                { "LogErrorID",  logID }
            };

            DatabaseExecs.InsertData("SendEmail", emailData);
        }
        catch(Exception ex)
        {
            ErrorLog.LogError(ex);
        }
    }
}
