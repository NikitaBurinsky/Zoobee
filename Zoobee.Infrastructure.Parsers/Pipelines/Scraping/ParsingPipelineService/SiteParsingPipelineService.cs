using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net;
using Zoobee.Application.DtoTypes.Base;
using Zoobee.Domain;
using Zoobee.Infrastructure.Parsers.Interfaces.Pipelines.Scraping.Parsing;
using Zoobee.Infrastructure.Parsers.Interfaces.Pipelines.Scraping.ParsingPipeline;
using Zoobee.Infrastructure.Parsers.Interfaces.Repositories.IParsedProductsRepository;

namespace Zoobee.Infrastructure.Parsers.Pipelines.Scraping.ParsingPipelineService
{//
	public sealed class SiteParsingPipelineService<ParsedProduct> : ISiteParsingPipelineService
		where ParsedProduct : class
	{

		ISiteProductParser<ParsedProduct> Parser { get; }
		IParsedProductsRepository<ParsedProduct> Repository {get;}
		ILogger<SiteParsingPipelineService<ParsedProduct>> logger;

		//IValidator<ParsedProduct> validator;
		public async Task<ParsingPipelineResults> ParseAndCreateNextProductAsync()
		{
			/*
			 * Пропуск всех распаршенных url для перехода к нераспаршеным. Если список url окончился - выход
			 */
			if (SkipAllParsedUrlsWithCheckForEnd())
				return ParsingPipelineResults.AlreadyParsedSoSkipped;

			(ParsedProduct? parsedProduct, string? parsedUrl) = await Parser.ParseAndGetNextAsync();
			if (parsedProduct == null)
				return ParsingPipelineResults.Error;
			/*TODO Сконфигурировать FluentValidation для обьектов
			if(Validate)
			...
			*/
			var res = await Repository.SaveParsedProduct(parsedProduct, parsedUrl);
			return res.Succeeded ?
				ParsingPipelineResults.Parsed :
				ParsingPipelineResults.Error;
		}
		/// <summary>
		/// Возвращает true если достигнут конец списка (все url распаршены)
		/// </summary>
		private bool SkipAllParsedUrlsWithCheckForEnd()
		{
			List<string> skippedLog = new List<string>();
			string nextUrl;
			while (IsAlreadyParsed(nextUrl = Parser.GetNextParsableUrl()))
			{
				bool isNewCycle = Parser.SkipUrl();
				skippedLog.Add(nextUrl);
				if (isNewCycle)
				{
					logger.LogDebug("Parsed urls skipped Count: {Count}, Urls: {@SkippedLog}", skippedLog.Count, skippedLog);
					return true;
				}
			}
			if (skippedLog.Count > 0)
				logger.LogDebug("Parsed urls skipped Count: {Count}, Urls: {@SkippedLog}", skippedLog.Count, skippedLog);
			return false;
		}

		private bool IsAlreadyParsed(string url)
		{
			//TODO На данный момент только раз в день
			return Repository.GetAll()
				.Any(e => e.ParsedUrl == url &&
					e.Metadata.CreatedAt.Value.Date == DateTime.Now.Date);
		}

		public SiteParsingPipelineService(ISiteProductParser<ParsedProduct> parser,
			ILogger<SiteParsingPipelineService<ParsedProduct>> logger,
			IParsedProductsRepository<ParsedProduct> repository)
		{
			Parser = parser;
			this.logger = logger;
			Repository = repository;
		}


		/*
protected async Task<bool> Validate(ParsedProduct product)
{
	IValidationContext context = new ValidationContext<ParsedProduct>(product);
	var res = await validator.ValidateAsync(context);
	if(res.IsValid)
		return true;
	else
	{
		logger.LogError(...);
		return false;
	}
}*/

	}
}
