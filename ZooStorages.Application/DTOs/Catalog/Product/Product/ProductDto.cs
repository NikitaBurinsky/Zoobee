using ZooStorages.Domain.DataEntities.Products.Components;
using ZooStorages.Application.Interfaces.DTOs;
using ZooStorages.Application.Models.Catalog.Product.Attributes;

namespace ZooStorages.Application.Models.Catalog.Product.Product
{
    public class ProductDto : IPrimitiveDtoFromEntity<ProductDto, ProductEntity>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Information { get; set; }
        public string ProductType { get; set; }
        //PRODUCT ATTRIBUTES
        public ManufactureAttributesDto ManufactureAttributes { get; set; }//owned
        public PhysicalAttributesDto PhysicalAttributes { get; set; }//owned
        public PetAttributesDto PetInfoAttributes { get; set; }//owned
        public List<string> MediaURI { get; set; } = new List<string>();
        public Dictionary<string, string> ExternalAttributes { get; set; }

        public static ProductDto FromEntity(ProductEntity entity)
        {
            var extAttributes = new Dictionary<string, string>();
            foreach (var atribute in entity.ExtendedAttributes)
            {
                extAttributes.Add(atribute.AttributeType.AttributeName, atribute.AttributeValue);
            }
            return new ProductDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Information = entity.Information,
                ManufactureAttributes = ManufactureAttributesDto.FromEntity(entity.ManufactureAttributes),
                MediaURI = entity.MediaURI,
                PetInfoAttributes = PetAttributesDto.FromEntity(entity.PetInfoAttributes),
                PhysicalAttributes = PhysicalAttributesDto.FromEntity(entity.PhysicalAttributes),
                ProductType = entity.ProductType.Name,
                ExternalAttributes = extAttributes
            };
        }
    }
}
