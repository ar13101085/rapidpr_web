using System;

namespace RapidPRWeb.Models
{
    public class Video
    {
        public int VideoId { get; set; }
        public string VideoName { get; set; }
        public string VideoLiveLink { get; set; }
        public long VideoDuration { get; set; }
        public DateTime VideoUploadTime { get; set; }
        public DateTime VideoPublishTime { get; set; }
        public bool NowPublish { get; set; }
    }
}