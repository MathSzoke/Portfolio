using System.Text.RegularExpressions;

namespace Backend.Data;

public partial class ValidEmail
{
    [GeneratedRegex("^[a-zA-Z0-9._-]+@[a-zA-Z0-9.]+\\.[a-zA-Z]{2,}$")]
    private static partial Regex MyRegex();
    public static bool IsValidEmail(string email) { return MyRegex().IsMatch(email); }
}
