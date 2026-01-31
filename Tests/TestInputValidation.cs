using NUnit.Framework;
[TestFixture]
public class TestInputValidation 
{
    [Test]
    
    public void TestForSQLInjection() 
    {
        // Attempt to inject SQL through the username field
        var maliciousInput = "admin'; DROP TABLE Users;--";

        var e = Assert.Throws<ArgumentException>(() =>
        {
            ValidateInput.ValidateUsername(maliciousInput);
        });

        Assert.That(e, Is.Not.Null);
        Assert.That(e!.Message, Does.Contain("invalid characters"));
    }
    
    
    [Test]
    
    public void TestForXSS() 
    {
        // Simulate a malicious script injection
        var xssPayload = "<script>alert('xss');</script>";

        // EncodeForHtml to neutralize the script tags
        var encoded = ValidateInput.EncodeForHtml(xssPayload);

        Assert.That(encoded, Does.Not.Contain("<script>"));
        Assert.That(encoded, Does.Contain("&lt;script&gt;"));
    }
}