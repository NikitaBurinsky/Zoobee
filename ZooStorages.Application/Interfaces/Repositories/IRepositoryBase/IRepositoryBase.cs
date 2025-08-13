using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooStorages.Application.Interfaces.Repositories
{
    public interface IRepositoryBase
    {
		public int SaveChanges();
		public Task<int> SaveChangesAsync();
    }
}
