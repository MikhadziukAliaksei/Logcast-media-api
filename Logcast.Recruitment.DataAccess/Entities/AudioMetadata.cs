using System;
using Logcast.Recruitment.Shared.Models;

namespace Logcast.Recruitment.DataAccess.Entities
{
    public class AudioMetadata
    {
        public Guid Id { get; set; }
        public Guid AudioFileId { get; set; }
        public string Artist { get; set; }
        public string Title { get; set; }
        public int Bitrate { get; set; }
        public string Codec { get; set; }
        public double Duration { get; set; }

        public virtual Audio Audio { get; set; }

        public AudioMetadataModel ToDomainModel()
        {
            return new AudioMetadataModel()
            {
                Id = Id,
                AudioFileId = AudioFileId,
                Artist = Artist,
                Title = Title,
                Bitrate = Bitrate,
                Codec = Codec,
                Duration = Duration
            };
        }
    }
}