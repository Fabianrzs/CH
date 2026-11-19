namespace GA2.Domain.Core.Dodo.Token.Models
{
    public class GetTokenRequest
    {
        public string User { get; set; }
        public string Password { get; set; }
        public double? ExpirationTimeSeconds { get; set; }
    }

    public class GetTokenResponse
    {
        public string User { get; set; }
        public string Token { get; set; }
        public string TokenDate { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
