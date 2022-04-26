using System;

namespace Logcast.Recruitment.Shared.Models
{
    public class AudioModel
    {
        public Guid Id { get; set; }
        
        public DateTime SaveDate { get; set; }
        
        public string AudioFileName { get; set; }
    }
}