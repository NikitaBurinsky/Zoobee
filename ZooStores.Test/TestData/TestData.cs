using ZooStorages.Application.Features.Administration.Products.Products.Commands.CreateProductCommand;
using ZooStorages.Application.Features.Catalog_Features.PetKinds;
using ZooStorages.Application.Features.Products.Categories.Commands;
using ZooStorages.Application.Features.Products.Types.Commands;

public class PetKindTestData
{
	public List<CreatePetKindCommand> ValidCommands { get; set; }
	public List<CreatePetKindCommand> InvalidCommands { get; set; }
	public List<CreatePetKindCommand> EdgeCases { get; set; }
}
public class ProductCategoryTestData
{
	public List<CreateProductCategoryCommand> ValidCategories { get; set; }
	public List<CreateProductCategoryCommand> InvalidCategories { get; set; }
	public List<CreateProductCategoryCommand> EdgeCases { get; set; }
}
public class ProductTypeTestData
{
	public List<CreateProductTypeCommand> ValidProductTypes { get; set; }
	public List<CreateProductTypeCommand> InvalidProductTypes { get; set; }
	public List<CreateProductTypeCommand> EdgeCases { get; set; }
	public List<CreateProductTypeCommand> SpecialProductTypes { get; set; }
}
public class ProductTestData
{
	public List<CreateProductCommand> ValidProducts { get; set; }
	public List<CreateProductCommand> InvalidProducts { get; set; }
	public List<CreateProductCommand> EdgeCases { get; set; }
	public List<CreateProductCommand> SpecialProducts { get; set; }
}
