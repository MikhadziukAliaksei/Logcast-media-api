using System;

namespace Logcast.Recruitment.Web.Models.Audio
{
	public class AddAudioRequest
	{
		public Guid AudioFileId { get; set; }
		public string Artist { get; set; }
		public string Title { get; set; }
		public int Bitrate { get; set; }
		public string Codec { get; set; }
		public double Duration { get; set; }
	}
}
