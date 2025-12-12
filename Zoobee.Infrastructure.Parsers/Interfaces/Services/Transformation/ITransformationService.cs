using System.Threading;
using System.Threading.Tasks;

namespace Zoobee.Infrastructure.Parsers.Interfaces.Services.Transformation
{
	public interface ITransformationService
	{
		/// <summary>
		/// Выбирает из БД скачанные, но не обработанные страницы и запускает трансформацию.
		/// </summary>
		Task ProcessPendingDataAsync(CancellationToken ct);
	}
}