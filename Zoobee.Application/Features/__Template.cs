using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace $rootnamespace$
{
    public class $safeitemname$ : IRequest<$returnType$>
    {
    }

	public class $safeitemname$Handler : IRequestHandler <$safeitemname$, $returnType$>
    {
        public async Task<$returnType$> Handle($safeitemname$ request, CancellationToken cancellationToken)
		{
			// Implement handler logic
			throw new System.NotImplementedException();
		}
    }
}