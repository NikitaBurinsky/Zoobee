namespace Zoobee.Infrastructure.Parsers.ParsedDataStorage.ParsedDataEntities.ParsedDataIdentifiers
{
	public class ParsedProductIdentifierEntity
	{
		public Guid Id { get; set; }
		public string ParsedName { get; set; }
		public string ParsedUrl { get; set; }
		public List<Guid> ParsedDatasOfProduct { get; set; }
	}
}
