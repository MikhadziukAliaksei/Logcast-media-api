using System;

namespace Logcast.Recruitment.Web.Models.Audio
{
	public class UploadAudioFileResponse
	{
		public UploadAudioFileResponse(Guid audioFileId)
		{
			AudioFileId = audioFileId;
		}
		
		public Guid AudioFileId { get; set; }
	}
}
