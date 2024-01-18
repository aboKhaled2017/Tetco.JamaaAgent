namespace Application.Common.Settings
{
    public sealed class AuthSetting
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
