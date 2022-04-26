using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Logcast.Recruitment.DataAccess.Entities;
using Logcast.Recruitment.DataAccess.Exceptions;
using Logcast.Recruitment.DataAccess.Factories;
using Logcast.Recruitment.DataAccess.Repositories;
using Logcast.Recruitment.DataAccess.Tests.Configuration;
using Logcast.Recruitment.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Logcast.Recruitment.DataAccess.Tests.RepositoryTests
{
    [TestClass]
    public class AudioMetadataRepositoryTests
    {
        private readonly IAudioMetadataRepository _audioMetadataRepository;
        private readonly IAudioRepository _audioRepository;
        private readonly ApplicationDbContext _testDbContext;

        private readonly Fixture _fixture;
        
        public AudioMetadataRepositoryTests()
        {
            var dbContextFactoryMock = new Mock<IDbContextFactory>();

            _testDbContext = EfConfig.CreateInMemoryTestDbContext();
            dbContextFactoryMock.Setup(d => d.Create()).Returns(EfConfig.CreateInMemoryApplicationDbContext());

            _audioMetadataRepository = new AudioMetadataRepository(dbContextFactoryMock.Object);
            _audioRepository = new AudioRepository(dbContextFactoryMock.Object);
            
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }
        
        [TestCleanup]
        public void Cleanup()
        {
            _testDbContext.Database.EnsureDeleted();
        }
        
        [TestMethod]
        public async Task AddAudioMetadataAsync_ExistAudioFile_ShouldAddAudioMetadata()
        {
            var audioMetadataEntity = _fixture.Build<AudioMetadata>().Without(e => e.Id).Create();
            var audioMetadataDomainModel = _fixture.Build<AudioMetadataModel>().Create();
            
            await _testDbContext.AudioMetadatas.AddAsync(audioMetadataEntity);
            await _testDbContext.SaveChangesAsync();

            var result = await _audioMetadataRepository.AddAudioMetadata(audioMetadataDomainModel);

            result.Should().NotBeEmpty();
        }
        
        [TestMethod]
        public async Task GetAudioMetadataAsync_ExistAudioFile_ShouldGetAudioData()
        {
            var audioId = new Guid("1BBB0BD5-4A2B-425B-ACAB-CDF16AA331C2");
            
            var audioEntity = _fixture.Build<Audio>().Without(e => e.Id).Create();
            var audioDomainModel = _fixture.Build<AudioModel>().Create();
            
            await _testDbContext.Audios.AddAsync(audioEntity);
            await _testDbContext.SaveChangesAsync();
            
            var result = await _audioMetadataRepository.GetAudioMetadata(_testDbContext.Audios.Single().Id);

            result.Should().NotBeNull();
        }
        
        [TestMethod]
        public async Task GetAudioMetadataAsync_NotExistAudioFile_ShouldThrowException()
        {
            var audioId = new Guid("1BBB0BD5-4A2B-425B-ACAB-CDF16AA331C2");
            
            var audioEntity = _fixture.Build<Audio>().Without(e => e.Id).Create();
            
            await _testDbContext.Audios.AddAsync(audioEntity);
            await _testDbContext.SaveChangesAsync();
            
            Func<Task> act = async () => await _audioMetadataRepository.GetAudioMetadata(audioId);

            await act.Should().ThrowAsync<AudioMetadataNotFoundException>();
        }
    }
}