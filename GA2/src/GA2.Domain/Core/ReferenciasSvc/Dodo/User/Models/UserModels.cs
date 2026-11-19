using GA2.Domain.Core.Dodo.Common;

namespace GA2.Domain.Core.Dodo.User.Models
{
    public class UserInfoRequest
    {
        public string UserName { get; set; }
        public DodoHeader Header { get; set; }
    }

    public class UserInfoData
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }

    public class UserInfoResponse
    {
        public UserInfoData UserInfo { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
