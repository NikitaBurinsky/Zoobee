using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoobee.Application.Shared.Constants
{
	public static class UserRoles
	{
		public const string SuperAdmin = "super-admin"; // Бог системы
		public const string DatabaseAdmin = "db-admin"; // Доступ к админке парсинга
		public const string Customer = "customer"; // Обычный юзер
	}
}
