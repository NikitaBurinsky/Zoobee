using ZooStorages.Application.Interfaces.DTOs;
using ZooStorages.Domain.DataEntities.Products.Components.Attributes;
using ZooStorages.Domain.Enums;

namespace ZooStorages.Application.Models.Catalog.Product.Attributes
{
    public class PetAttributesDto : IPrimitiveDtoFromEntity<PetAttributesDto, PetProductAttributes>
    {
        public uint? PetAgeWeeksMin { get; set; }
        public uint? PetAgeWeeksMax { get; set; }
        public uint? PetSize { get; set; }
        public uint? PetGender { get; set; }
        public string PetKind { get; set; }

        public static PetAttributesDto FromEntity(PetProductAttributes entity)
        {
            return new PetAttributesDto
            {
                PetAgeWeeksMax = entity.PetAgeWeeksMax,
                PetAgeWeeksMin = entity.PetAgeWeeksMin,
                PetGender = (uint)entity.PetGender,
                PetKind = entity.PetKind.PetKindName,
                PetSize = (uint)entity.PetSize,
            };
        }



    }
}
