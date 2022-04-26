using System;
using Logcast.Recruitment.Shared.Models;

namespace Logcast.Recruitment.DataAccess.Entities
{
    public class Audio
    {
        public Audio()
        {
            SaveDate = DateTime.Now;
        }
        
        public Guid Id { get; set; }
        public DateTime SaveDate { get; set; }
        public string AudioFileName { get; set; }
        
        public virtual AudioMetadata AudioMetadata { get; set; }

        public AudioModel ToDomainModel()
        {
            return new AudioModel()
            {
                Id = Id,
                SaveDate = SaveDate,
                AudioFileName = AudioFileName
            };
        }
    }
}