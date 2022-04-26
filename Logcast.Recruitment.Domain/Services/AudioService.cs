using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Logcast.Recruitment.DataAccess.Repositories;
using Logcast.Recruitment.Shared.Exceptions;
using Logcast.Recruitment.Shared.Models;

namespace Logcast.Recruitment.Domain.Services
{
    public interface IAudioService
    {
        Task<Guid> SaveAudioFileAsync(AudioModel audio, Stream stream);
        Task<Stream> GetAudioFileAsync(Guid audioFileId);
    }

    public class AudioService : IAudioService
    {
        private readonly IAudioRepository _audioRepository;
        private readonly BlobServiceClient _blobServiceClient;

        public AudioService(IAudioRepository audioRepository, BlobServiceClient blobServiceClient)
        {
            _audioRepository = audioRepository;
            _blobServiceClient = blobServiceClient;
        }

        public async Task<Guid> SaveAudioFileAsync(AudioModel audio, Stream stream)
        {
            var audioFileId = await _audioRepository.SaveAudioFileAsync(audio);
            
            var blobClient = await GetBlobClient();
            var response = await blobClient.UploadBlobAsync(audioFileId.ToString(), stream);
            
            return audioFileId;
        }
        
        public async Task<Stream> GetAudioFileAsync(Guid audioFileId)
        {
            var blobClient = GetBlobClient(ContainerNames.AudioContainer, audioFileId.ToString());
            var response = await blobClient.OpenReadAsync();

            if (response is null) throw new NotFoundException();

            return response;
        }
        
        private async Task<BlobContainerClient> GetBlobClient()
        {
            await _blobServiceClient.GetBlobContainerClient(ContainerNames.AudioContainer).CreateIfNotExistsAsync();
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(ContainerNames.AudioContainer);

            return blobContainerClient;
        }
        private BlobClient GetBlobClient(string blobContainerName, string blobName)
        {
            return _blobServiceClient.GetBlobContainerClient(blobContainerName).GetBlobClient(blobName);
        }
    }
}