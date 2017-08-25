using System;

namespace RapidPRWeb.Models
{
    public class Text
    {
        public int TextId { get; set; }
        public string TextContent { get; set; }
        public DateTime TextUploadTime { get; set; }
        public DateTime TextPublishTime { get; set; }
        public bool IsPublishNow { get; set; }
    }
}