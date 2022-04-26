using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Logcast.Recruitment.DataAccess.Exceptions;
using Logcast.Recruitment.Domain.Services;
using Logcast.Recruitment.Shared.Exceptions;
using Logcast.Recruitment.Shared.Models;
using Logcast.Recruitment.Web.Models.Audio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Logcast.Recruitment.Web.Controllers
{
    [ApiController]
    [Route("api/audio")]
    public class AudioController : ControllerBase
    {
        private readonly IAudioService _audioService;
        private readonly IAudioMetadataService _audioMetadataService;

        public AudioController(IAudioService audioService, IAudioMetadataService audioMetadataService)
        {
            _audioService = audioService;
            _audioMetadataService = audioMetadataService;
        }

        [HttpPost("audio-file")]
        [SwaggerResponse(StatusCodes.Status200OK, "Audio file uploaded successfully", typeof(UploadAudioFileResponse))]
        [ProducesResponseType(typeof(UploadAudioFileResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> UploadAudioFile(IFormFile audioFile)
        {
            try
            {
                var audioFileId = await _audioService.SaveAudioFileAsync(new AudioModel
                {
                    AudioFileName = audioFile.FileName
                }, audioFile.OpenReadStream());
                return Ok(new UploadAudioFileResponse(audioFileId));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("audio-metadata")]
        [SwaggerResponse(StatusCodes.Status200OK, "Audio metadata registered successfully")]
        public async Task<IActionResult> AddAudioMetadata([Required] [FromBody] AddAudioRequest request)
        {
            try
            {
                var audioMetadataId = await _audioMetadataService.AddAudioMetadata(new AudioMetadataModel
                {
                    AudioFileId = request.AudioFileId,
                    Artist = request.Artist,
                    Title = request.Title,
                    Bitrate = request.Bitrate,
                    Codec = request.Codec,
                    Duration = request.Duration
                });

                return Ok(new AudioMetadataResponse
                {
                    Id = audioMetadataId,
                });
            }
            catch (Exception e)
            {
                if (e is AudioMetadataNotFoundException) return NotFound("Audio file not found");
                Console.WriteLine(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{audioId:Guid}")]
        [SwaggerResponse(StatusCodes.Status200OK, "Audio metadata fetched successfully", typeof(AudioMetadataResponse))]
        [ProducesResponseType(typeof(AudioMetadataResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAudioMetadata([FromRoute] Guid audioId)
        {
            try
            {
                var response = await _audioMetadataService.GetAudioMetadata(audioId);
                return Ok(new AudioMetadataResponse
                {
                    Id = response.Id,
                    AudioFileId = response.AudioFileId,
                    Artist = response.Artist,
                    Title = response.Title,
                    Bitrate = response.Bitrate,
                    Codec = response.Codec,
                    Duration = response.Duration
                });
            }
            catch (Exception e)
            {
                if (e is AudioMetadataNotFoundException) return NotFound("Audio file not found");
                Console.WriteLine(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("stream/{audioId:Guid}")]
        [SwaggerResponse(StatusCodes.Status200OK, "Preview stream started successfully", typeof(FileContentResult))]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAudioStream([FromRoute] Guid audioId)
        {
            try
            {
                var stream = await _audioService.GetAudioFileAsync(audioId);
                return File(stream, "audio/mpeg");
            }
            catch (Exception e)
            {
                if (e is NotFoundException) return NotFound("Audio not found");
                Console.WriteLine(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}