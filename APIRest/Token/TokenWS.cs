using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServices.Token
{
    public class TokenWS
    {
        public TokenWS()
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
