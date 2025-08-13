using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooStorages.Domain.DataEntities.Identity
{
	public class TestUser : IdentityUser
	{
		public int Year { get; set; }
	}
}
