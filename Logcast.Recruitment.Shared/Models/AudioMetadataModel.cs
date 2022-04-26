using System;

namespace Logcast.Recruitment.Shared.Models
{
    public class AudioMetadataModel
    {
        public Guid Id { get; set; }
        public Guid AudioFileId { get; set; }
        public string Artist { get; set; }
        public string Title { get; set; }
        public int Bitrate { get; set; }
        public string Codec { get; set; }
        public double Duration { get; set; }
    }
}