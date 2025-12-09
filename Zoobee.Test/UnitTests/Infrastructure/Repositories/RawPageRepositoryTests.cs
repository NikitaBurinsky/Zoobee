using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Zoobee.Infrastructure.Parsers.Core.Entities;
using Zoobee.Infrastructure.Parsers.Core.Entities.Zoobee.Infrastructure.Parsers.Core.Entities;
using Zoobee.Infrastructure.Parsers.Core.Enums;
using Zoobee.Infrastructure.Parsers.Data;
using Zoobee.Infrastructure.Parsers.Services.Storage;

namespace Zoobee.Test.Tests.Infrastructure.Parsers.Repositories
{
	[TestFixture]
	public class RawPageRepositoryTests
	{
		private ParsersDbContext _context;
		private RawPageRepository _repository;

		[SetUp]
		public void Setup()
		{
			// Каждый тест получает уникальную базу данных в памяти
			var options = new DbContextOptionsBuilder<ParsersDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			_context = new ParsersDbContext(options);
			_repository = new RawPageRepository(_context);
		}

		[TearDown]
		public void TearDown()
		{
			_context.Dispose();
		}

		[Test]
		public async Task AddOrUpdateAsync_ShouldSaveToDatabase()
		{
			// Arrange
			var page = new RawPageEntity { Url = "http://google.com", Status = RawPageStatus.Pending , SourceName = "Google"};

			// Act
			await _repository.AddOrUpdateAsync(page, CancellationToken.None);

			// Assert
			var savedPage = await _context.RawPages.FirstOrDefaultAsync(p => p.Url == "http://google.com");
			Assert.That(savedPage, Is.Not.Null);
			Assert.That(savedPage.Status, Is.EqualTo(RawPageStatus.Pending));
		}
	}
}