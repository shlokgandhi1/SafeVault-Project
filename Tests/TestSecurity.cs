using NUnit.Framework;

[TestFixture]
public class TestSecurity
{
    private const string TestConnectionString =
        "Server=localhost;Database=SafeVault;Trusted_Connection=True;";

    [Test]
    public void InsertUser_WithSQLInjectionPayload_ShouldThrowValidationError()
    {
        var test = new sqlCommands(TestConnectionString);
        var malicious = "admin'; DROP TABLE Users;--";

        var e = Assert.Throws<ArgumentException>(() =>
        {
            test.InsertUser(malicious, "safe@example.com");
        });

        Assert.That(e, Is.Not.Null);
        Assert.That(e!.Message, Does.Contain("invalid"));
    }

    [Test]
    public void InsertUser_WithValidData_ShouldNotThrow()
    {
        var test = new sqlCommands(TestConnectionString);

        Assert.That(() =>
        {
            test.InsertUser("safe_user", "safe@example.com");
        }, Throws.Nothing);
    }
}