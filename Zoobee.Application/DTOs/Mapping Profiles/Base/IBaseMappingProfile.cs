using Zoobee.Domain;

namespace Zoobee.Application.Interfaces.Services.Products.ProductsMapperService.Mapping_Profiles
{
	/// <summary>
	/// Базовый класс профиля маппинга (преимущественно для DTO и Сущностей)
	/// Соглашение 
	/// - Map используется для трансформации "внутрь" - DTO -> Entity или Request -> DTO
	/// - RevMap (Если определен), используется для обратной транформации - Entity -> DTO
	/// </summary>
	/// <typeparam name="From"></typeparam>
	/// <typeparam name="To"></typeparam>
	public interface IBaseMappingProfile<From, To>
	{
		public abstract OperationResult<To> Map(From from);
		public virtual OperationResult<From> RevMap(To from) => throw new NotImplementedException("TODO Reversal Mapping was not overrided");

		/// <summary>
		/// //TODO В будущем переписать на статическую проверку, 
		/// или рассмотреть другие варианты маппинга
		/// </summary>
		public bool IsRevMappable
		{
			get
			{
				var method = GetType().GetMethod(nameof(RevMap));
				return method.DeclaringType != typeof(IBaseMappingProfile<From, To>);
			}
		}
	}
}
