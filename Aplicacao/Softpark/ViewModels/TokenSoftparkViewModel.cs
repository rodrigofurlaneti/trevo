using Newtonsoft.Json;

namespace Aplicacao.ViewModels
{
    public class TokenSoftparkViewModel
    {
        public TokenSoftparkViewModel()
        {
            AccessToken = string.Empty;
            TokenType = string.Empty;
            ExpiresIn = string.Empty;
            RefreshToken = string.Empty;
            ErrorMessage = string.Empty;
        }
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }
        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }
        [JsonProperty(PropertyName = "expires_in")]
        public string ExpiresIn { get; set; }
        [JsonProperty(PropertyName = "refresh_token")]
        public string RefreshToken { get; set; }
        public string ErrorMessage { get; set; }
        public bool Status { get; internal set; }
    }
}
