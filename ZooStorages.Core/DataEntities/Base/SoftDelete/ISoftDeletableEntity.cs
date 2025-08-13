using Microsoft.EntityFrameworkCore;

namespace ZooStorages.Domain.DataEntities.Base.SoftDelete
{
	public interface ISoftDeletableEntity
    {
		public SoftDeleteData DeleteData { get; set; }

		[Owned]
		public class SoftDeleteData{
			public DateTimeOffset? DeletedAt { get; set; }
			public bool IsDeleted { get; set; }
		}
        public void Ressurect()
        {
			DeleteData.IsDeleted = true;
            DeleteData.DeletedAt = null;
        }
    }



}
