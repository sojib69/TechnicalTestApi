namespace TechnicalTest.Api.Configurations
{
    public class AzureAdB2cSetting
    {
        public string Instance { get; set; } = "https://multiplieradb2c.b2clogin.com/";
        public string Domain { get; set; } = "multiplieradb2c.onmicrosoft.com";
        public string? TenantId { get; set; }
        public string? ClientId { get; set; }
        public string? SignUpSignInPolicyId { get; set; }
        public string? CallbackPath { get; set; }
        public string? Scopes { get; set; }

        public string? ClientSecret { get; set; }
    }
}
