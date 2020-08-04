namespace Domain.Settings
{
    public class OktaConfig
    {
        public string Authority { get; set; }
        public string TokenEndpoint { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Domain { get; set; }
        public string AuthorizationEndpoint { get; set; }
        public string UserInformationEndpoint { get; set; }
        public string SwaggerAuthorizationUrl { get; set; }
        public string Audience { get; set; }
    }
}
