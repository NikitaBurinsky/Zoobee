namespace Zoobee.Web.Models
{
	public class ErrorViewModel
	{
		public string? DtoId { get; set; }

		public bool ShowDtoId => !string.IsNullOrEmpty(DtoId);
	}
}
