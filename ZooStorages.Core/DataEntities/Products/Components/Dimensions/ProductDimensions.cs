using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooStorages.Domain.DataEntities.Products.Components.Dimensions
{
	[Owned]
    public record ProductDimensions(decimal? Length, decimal? Width, decimal? Heigth);
}
