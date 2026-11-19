using GA2.Domain.Core.Dodo.Common;

namespace GA2.Domain.Core.Dodo.Notification.Models
{
    public class SendEmailRequest
    {
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool IsHtml { get; set; }
        public string User { get; set; }
        public string Host { get; set; }
        public DodoHeader Header { get; set; }
    }

    public class SendEmailResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
