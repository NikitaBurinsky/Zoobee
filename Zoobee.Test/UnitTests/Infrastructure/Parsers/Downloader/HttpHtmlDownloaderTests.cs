using NUnit.Framework;
using Moq;
using Moq.Protected;
using System.Net;
using Zoobee.Infrastructure.Parsers.Services.Downloader;
using Microsoft.Extensions.Logging;

namespace Zoobee.Test.Tests.Infrastructure.Parsers.Downloader
{
	[TestFixture]
	public class HttpHtmlDownloaderTests
	{
		private Mock<HttpMessageHandler> _handlerMock;
		private HttpClient _httpClient;
		private Mock<ILogger<HttpHtmlDownloader>> _loggerMock;
		private HttpHtmlDownloader _downloader;

		[SetUp]
		public void Setup()
		{
			_handlerMock = new Mock<HttpMessageHandler>();
			_httpClient = new HttpClient(_handlerMock.Object);
			_loggerMock = new Mock<ILogger<HttpHtmlDownloader>>();

			_downloader = new HttpHtmlDownloader(_httpClient, _loggerMock.Object);
		}

		[Test]
		public async Task DownloadAsync_ReturnsSuccess_WhenHttp200()
		{
			// Arrange
			var expectedContent = "<html>Success</html>";

			_handlerMock.Protected()
				.Setup<Task<HttpResponseMessage>>(
					"SendAsync",
					ItExpr.IsAny<HttpRequestMessage>(),
					ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.OK,
					Content = new StringContent(expectedContent)
				});

			// Act
			var result = await _downloader.DownloadAsync("http://example.com", CancellationToken.None);

			// Assert
			Assert.That(result.IsSuccess, Is.True);
			Assert.That(result.Content, Is.EqualTo(expectedContent));
			Assert.That(result.StatusCode, Is.EqualTo(200));
		}

		[Test]
		public async Task DownloadAsync_ReturnsFailure_WhenHttp404()
		{
			// Arrange
			_handlerMock.Protected()
				.Setup<Task<HttpResponseMessage>>(
					"SendAsync",
					ItExpr.IsAny<HttpRequestMessage>(),
					ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.NotFound
				});

			// Act
			var result = await _downloader.DownloadAsync("http://example.com/missing", CancellationToken.None);

			// Assert
			Assert.That(result.IsSuccess, Is.False);
			Assert.That(result.StatusCode, Is.EqualTo(404));
			Assert.That(result.ErrorMessage, Does.Contain("Not Found")); // Does.Contain - аналог Assert.Contains
		}
	}
}