using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Application.DtoTypes.Base;
using Zoobee.Domain;

namespace Zoobee.Application.Interfaces.Repositories
{
    public interface IRepositoryBase<T> 
		where T : BaseEntity
    {
		public IQueryable<T> GetAll();
		public bool IsEntityExists(Guid Id);
		public Task<OperationResult<Guid>> CreateAsync(T newEntity);
		public T Get(Guid Id);
		public OperationResult Delete(T entity);
		public OperationResult Update(T productTypeToUpdate, Action<T> action);
		public int SaveChanges();
		public Task<int> SaveChangesAsync();
    }
}
