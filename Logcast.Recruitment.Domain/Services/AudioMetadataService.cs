using System;
using System.Threading.Tasks;
using Logcast.Recruitment.DataAccess.Repositories;
using Logcast.Recruitment.Shared.Models;

namespace Logcast.Recruitment.Domain.Services
{
    public interface IAudioMetadataService
    {
        Task<AudioMetadataModel> GetAudioMetadata(Guid id);
        Task<Guid> AddAudioMetadata(AudioMetadataModel audioMetadata);
    }
    
    public class AudioMetadataService : IAudioMetadataService
    {
        private readonly IAudioMetadataRepository _audioMetadataRepository;

        public AudioMetadataService(IAudioMetadataRepository audioMetadataRepository)
        {
            _audioMetadataRepository = audioMetadataRepository;
        }

        public async Task<AudioMetadataModel> GetAudioMetadata(Guid id)
        {
            var audioMetadata = await _audioMetadataRepository.GetAudioMetadata(id);
            return audioMetadata;
        }

        public async Task<Guid> AddAudioMetadata(AudioMetadataModel audioMetadata)
        {
            var audioMetadataId = await _audioMetadataRepository.AddAudioMetadata(audioMetadata);
            return audioMetadataId;
        }
    }
}