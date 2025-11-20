using Zoobee.Domain;

namespace Zoobee.Infrastructure.Parsers.Interfaces.Pipelines.Scraping.ParsingPipeline
{
	public enum ParsingPipelineResults
	{
		Parsed,
		Skiped,
		AlreadyParsedSoSkipped,
		Error,
		ParsingCycleComplete,
	}
	public interface ISiteParsingPipelineService
	{
		public Task<ParsingPipelineResults> ParseAndCreateNextProductAsync();
	}
}
