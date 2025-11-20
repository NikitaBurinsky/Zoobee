using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Zoobee.Domain;
using Zoobee.Infrastructure.Parsers.Interfaces.Pipelines.Scraping.ParsingPipeline;
using Zoobee.Infrastructure.Parsers.Interfaces.Pipelines.Scraping.ParsingQueue;

namespace Zoobee.Infrastructure.Parsers.Pipelines.Scraping.ParsingQueue
{
	internal class ParsingQueue : IParsingQueueService
	{
		private IList<ISiteParsingPipelineService> ParsersServices;
		private IList<DateTime?> LastParsingTimes;
		private ILogger<ParsingQueue> logger;
		private int ParsingDelaySeconds = -1;//Получить из конфига

		private int currentParserInd = 0;
		public ParsingQueue(IEnumerable<ISiteParsingPipelineService> parsersServices,
			IConfiguration configuration, ILogger<ParsingQueue> logger)
		{
			this.logger = logger;
			if (parsersServices == null || !parsersServices.Any())
				throw new ArgumentNullException("- List of ParsersPipelinesServices is null or empty");
			ParsersServices = parsersServices.ToList();

			ParsingDelaySeconds = configuration.GetValue<int>("Parsing:Delays:BaseParsingSiteDelaySeconds");
			if (ParsingDelaySeconds < 0)
				throw new ArgumentException("- Parsing:Delays:BaseParsingSiteDelaySeconds cannot be less than null");
			LastParsingTimes = Enumerable.Repeat<DateTime?>(null, ParsersServices.Count).ToList();
		}

		public async Task HandleNext()
		{
			if (currentParserInd >= ParsersServices.Count())
				currentParserInd = 0;
			DateTime? LastTimeParsed = LastParsingTimes[currentParserInd];
			if (LastTimeParsed != null && DateTime.Now.Subtract(LastTimeParsed.Value).TotalSeconds < ParsingDelaySeconds) //Еще не прошел откат
				return;

			ISiteParsingPipelineService parser = ParsersServices[currentParserInd];
			ParsingPipelineResults ParsingIterationResult = await parser.ParseAndCreateNextProductAsync();
			
			if(ParsingIterationResult != ParsingPipelineResults.Error)
				logger.LogInformation("Parsing iteration status: {ParsingIterationResult}", ParsingIterationResult);
			else
				logger.LogWarning("Parsing iteration status: {ParsingIterationResult}", ParsingIterationResult);

			LastParsingTimes[currentParserInd] = DateTime.Now;
			return;
		}



	}
}
