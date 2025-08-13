using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Application.Interfaces.DTOs;
using ZooStorages.Domain.DataEntities.Products;

namespace ZooStorages.Application.Features.Products.Catalog_Features.PetTypes.Queries.ListPetTypes
{
	public class ListPetKindDto : IPrimitiveDtoFromEntity<ListPetKindDto, PetKindEntity>
	{
		public string PetKindName { get; set; }

		public static ListPetKindDto FromEntity(PetKindEntity entity)
		{
			return new ListPetKindDto
			{
				PetKindName = entity.PetKindName
			};
		}
	}
}
