using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Logcast.Recruitment.DataAccess.Entities;
using Logcast.Recruitment.DataAccess.Factories;
using Logcast.Recruitment.DataAccess.Repositories;
using Logcast.Recruitment.DataAccess.Tests.Configuration;
using Logcast.Recruitment.Shared.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Logcast.Recruitment.DataAccess.Tests.RepositoryTests
{
    [TestClass]
    public class AudioRepositoryTests
    {
        private readonly IAudioRepository _audioRepository;
        private readonly ApplicationDbContext _testDbContext;

        private readonly Fixture _fixture;
        
        public AudioRepositoryTests()
        {
            var dbContextFactoryMock = new Mock<IDbContextFactory>();

            _testDbContext = EfConfig.CreateInMemoryTestDbContext();
            dbContextFactoryMock.Setup(d => d.Create()).Returns(EfConfig.CreateInMemoryApplicationDbContext());

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
        public async Task SaveAudioFileAsync_ValidEntity_ShouldSaveAudio()
        {
            var audioEntity = _fixture.Build<Audio>().Without(e => e.Id).Create();
            var audioDomainModel = _fixture.Build<AudioModel>().Create();
            
            await _testDbContext.Audios.AddAsync(audioEntity);
            await _testDbContext.SaveChangesAsync();

            var result = await _audioRepository.SaveAudioFileAsync(audioDomainModel);

            result.Should().NotBeEmpty();
        }
    }
}