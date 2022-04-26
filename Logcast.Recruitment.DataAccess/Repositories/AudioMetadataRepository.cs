using System;
using System.Threading.Tasks;
using Logcast.Recruitment.DataAccess.Entities;
using Logcast.Recruitment.DataAccess.Exceptions;
using Logcast.Recruitment.DataAccess.Factories;
using Logcast.Recruitment.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Logcast.Recruitment.DataAccess.Repositories
{
    public interface IAudioMetadataRepository
    {
        Task<Guid> AddAudioMetadata(AudioMetadataModel audioMetadata);
        Task<AudioMetadataModel> GetAudioMetadata(Guid id);
    }
    
    public class AudioMetadataRepository : IAudioMetadataRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public AudioMetadataRepository(IDbContextFactory dbContextFactory)
        {
            _applicationDbContext = dbContextFactory.Create();
        }

        public async Task<Guid> AddAudioMetadata(AudioMetadataModel audioMetadata)
        {
            var newAudioMetadata = new AudioMetadata
            {
                AudioFileId = audioMetadata.AudioFileId,
                Artist = audioMetadata.Artist,
                Title = audioMetadata.Title,
                Bitrate = audioMetadata.Bitrate,
                Codec = audioMetadata.Codec,
                Duration = audioMetadata.Duration
            };
            var createdAudioMetadata = await _applicationDbContext.AddAsync(newAudioMetadata);
            await _applicationDbContext.SaveChangesAsync();

            return createdAudioMetadata.Entity.Id;
        }

        public async Task<AudioMetadataModel> GetAudioMetadata(Guid id)
        {
            var audioMetadata = await _applicationDbContext.AudioMetadatas.FirstOrDefaultAsync(s => s.AudioFileId == id);
            
            if (audioMetadata == null) throw new AudioMetadataNotFoundException();
            return audioMetadata.ToDomainModel();
        }
    }
}