using System;
using System.Threading.Tasks;
using Logcast.Recruitment.DataAccess.Entities;
using Logcast.Recruitment.DataAccess.Factories;
using Logcast.Recruitment.Shared.Models;

namespace Logcast.Recruitment.DataAccess.Repositories
{
    public interface IAudioRepository
    {
        Task<Guid> SaveAudioFileAsync(AudioModel audio);
    }
    
    public class AudioRepository : IAudioRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public AudioRepository(IDbContextFactory dbContextFactory)
        {
            _applicationDbContext = dbContextFactory.Create();
        }

        public async Task<Guid> SaveAudioFileAsync(AudioModel audio)
        {
            var audioFile = new Audio
            {
                AudioFileName = audio.AudioFileName
            };
            
            var newAudioFile = await _applicationDbContext.Audios.AddAsync(audioFile);
            await _applicationDbContext.SaveChangesAsync();
            
            return newAudioFile.Entity.Id;
        }
    }
}