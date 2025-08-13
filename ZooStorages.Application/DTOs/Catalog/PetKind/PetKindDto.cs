using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Application.Interfaces.DTOs;
using ZooStorages.Domain.DataEntities.Products;

namespace ZooStorages.Application.Models.Catalog.PetKind
{
    public class PetKindDto : IPrimitiveDtoFromEntity<PetKindDto, PetKindEntity>
    {
        public string PetKindName { get; set; }
        public static PetKindDto FromEntity(PetKindEntity entity)
        {
            return new PetKindDto
            {
                PetKindName = entity.PetKindName,
            };
        }
    }
}
