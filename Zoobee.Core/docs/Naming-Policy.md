# Core Naming Policies

## Domain Classes

### Database Entities
### "DomainClass"Entity
	Examples:
	- FoodProductEntity
	- MediaFileEntity

## Operation Results Naming Policy

### Errors Messages 
### "Error.{DomainContextObject or process(s)}.{Optional: ErrorContextField}.{Error}"
	Examples:
	-	OperationResult.Error(localizer["Error.CreatorCompanies.CreatorCompanyNotFound"]...
	-	OperationResult.Error(localizer["Error.SellerCompanies.WriteDbError"]...
	-   OperationResult.Error(localizer["Error.Seed.DeliveryAreas.SellerCompanyNotFound"]...
