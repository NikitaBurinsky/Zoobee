using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Application.DTOs.Products.Base;
using Zoobee.Application.DTOs.Products.Types;
using Zoobee.Domain.DataEntities.Products;

namespace Zoobee.Application.Interfaces.Services.ProductTypeRegistry
{
	public interface IProductTypeRegistryService
	{
		/// <summary>
		/// Пытается получить маппинг типов для указанного DTO типа
		/// </summary>
		/// <param name="dtoType">Тип DTO объекта</param>
		/// <param name="mapping">Найденный маппинг (DtoType, EntityType)</param>
		/// <returns>True, если маппинг найден</returns>
		bool TryGetMapping(Type dtoType, out (Type DtoType, Type EntityType) mapping);

		/// <summary>
		/// Регистрирует новый маппинг типов
		/// </summary>
		/// <param name="dtoType">Тип DTO</param>
		/// <param name="entityType">Тип сущности</param>
		/// <exception cref="ArgumentException">Если DTO тип не наследуется от BaseProductDto</exception>
		void RegisterMapping(Type dtoType, Type entityType);

		/// <summary>
		/// Возвращает все зарегистрированные DTO типы
		/// </summary>
		IEnumerable<Type> GetAllRegisteredDtoTypes();

		/// <summary>
		/// Проверяет, зарегистрирован ли указанный DTO тип
		/// </summary>
		bool IsRegistered(Type dtoType);

		/// <summary>
		/// Получает маппинг или возвращает маппинг по умолчанию (BaseProductDto)
		/// </summary>
		(Type DtoType, Type EntityType) GetMappingOrDefault(Type dtoType);

		/// <summary>
		/// Получает Entity тип для указанного DTO типа
		/// </summary>
		Type GetEntityType(Type dtoType);
	}
}
