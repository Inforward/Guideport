using System;

namespace Portal.Model
{
    public class Email
    {
        public int EmailId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? SentDate { get; set; }
        public int? Attempts { get; set; }
        public DateTime? FailDate { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}