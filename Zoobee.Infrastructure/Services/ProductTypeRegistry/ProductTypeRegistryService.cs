using Zoobee.Application.DTOs.Products.Base;
using Zoobee.Application.DTOs.Products.Types;
using Zoobee.Application.Interfaces.Services.ProductTypeRegistry;
using Zoobee.Domain.DataEntities.Products;

public class ProductTypeRegistry : IProductTypeRegistryService
{
	private readonly Dictionary<Type, (Type DtoType, Type EntityType)> _typeMappings;
	private readonly IServiceProvider _serviceProvider;

	public ProductTypeRegistry(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
		_typeMappings = new Dictionary<Type, (Type, Type)>
		{
			{ typeof(FoodProductDto), (typeof(FoodProductDto), typeof(FoodProductEntity)) },
			{ typeof(ToiletProductDto), (typeof(ToiletProductDto), typeof(ToiletProductEntity)) },
			{ typeof(BaseProductDto), (typeof(BaseProductDto), typeof(BaseProductEntity)) }
		};
	}

	public bool TryGetMapping(Type dtoType, out (Type DtoType, Type EntityType) mapping)
	{
		// Пытаемся найти точное соответствие
		if (_typeMappings.TryGetValue(dtoType, out mapping))
		{
			return true;
		}

		// Если точного соответствия нет, ищем среди базовых типов
		foreach (var kvp in _typeMappings)
		{
			if (dtoType.IsAssignableFrom(kvp.Key) || kvp.Key.IsAssignableFrom(dtoType))
			{
				mapping = kvp.Value;
				return true;
			}
		}

		// В последнюю очередь используем базовый тип
		if (_typeMappings.TryGetValue(typeof(BaseProductDto), out mapping))
		{
			return true;
		}

		mapping = default;
		return false;
	}

	public void RegisterMapping(Type dtoType, Type entityType)
	{
		if (!dtoType.IsAssignableTo(typeof(BaseProductDto)))
			throw new ArgumentException($"DTO type must inherit from {nameof(BaseProductDto)}", nameof(dtoType));

		_typeMappings[dtoType] = (dtoType, entityType);
	}

	public IEnumerable<Type> GetAllRegisteredDtoTypes() => _typeMappings.Keys;

	public bool IsRegistered(Type dtoType)
	{
		return _typeMappings.ContainsKey(dtoType) ||
			   _typeMappings.Keys.Any(t => t.IsAssignableFrom(dtoType) || dtoType.IsAssignableFrom(t));
	}

	public (Type DtoType, Type EntityType) GetMappingOrDefault(Type dtoType)
	{
		if (TryGetMapping(dtoType, out var mapping))
			return mapping;

		// Возвращаем маппинг по умолчанию
		return (typeof(BaseProductDto), typeof(BaseProductEntity));
	}

	public Type GetEntityType(Type dtoType)
	{
		if (TryGetMapping(dtoType, out var mapping))
			return mapping.EntityType;

		throw new InvalidOperationException($"No entity type registered for DTO type: {dtoType.Name}");
	}
}