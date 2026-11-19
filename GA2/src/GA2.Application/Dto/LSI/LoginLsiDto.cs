namespace GA2.Application.Dto
{
    public class LoginLsiDto
    {
        public string username { get; set; }
        public string  password { get; set; }
        public string grand_type { get; set; }
        public string clientId { get; set; }
    }

    public class ResponseToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
    }
}
