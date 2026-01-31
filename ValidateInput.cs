using System.Text.RegularExpressions;
using System.Net;

public static class ValidateInput
{
    public static string ValidateUsername(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
            throw new ArgumentException("Username is required.");

        var trimmedUserName = userName.Trim();

        // Allow only letters, digits, underscore
        if (!Regex.IsMatch(trimmedUserName, @"^[a-zA-Z0-9_]{3,10}$"))
            throw new ArgumentException("Username contains invalid characters.");

        return trimmedUserName;
    }


    public static string ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.");

        var emailTrimmed = email.Trim();

        if (!Regex.IsMatch(emailTrimmed, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new ArgumentException("Email format is invalid.");

        return emailTrimmed;
    }


    // Used when displaying user data back to the UI
    public static string EncodeForHtml(string value)
    {
        if (value == null)
        {
            return string.Empty;
        }
            
        return WebUtility.HtmlEncode(value);
    }
}