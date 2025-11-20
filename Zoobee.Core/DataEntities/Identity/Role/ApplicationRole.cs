using Microsoft.AspNetCore.Identity;

namespace Zoobee.Domain.DataEntities.Identity.Role
{
	/// <summary>
	/// Создан для соответствия BaseApplicationUser для использования Guid в качестве ключа
	/// Основной класс роли в приложении
	/// </summary>
	public class ApplicationRole : IdentityRole<Guid>
	{
		public ApplicationRole() : base() { }
		public ApplicationRole(string roleName) : base(roleName) { }

	}
}
