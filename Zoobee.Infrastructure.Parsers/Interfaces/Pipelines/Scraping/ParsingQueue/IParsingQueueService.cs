using Zoobee.Domain;

namespace Zoobee.Infrastructure.Parsers.Interfaces.Pipelines.Scraping.ParsingQueue
{
	public interface IParsingQueueService
	{
		public Task HandleNext();
	}
}
