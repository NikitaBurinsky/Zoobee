using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Domain.DataEntities.Products.Components.Dimensions;
using ZooStorages.Domain.Enums;

namespace ZooStorages.Domain.DataEntities.Products.Components.Attributes
{
	[Owned]
	public class PhysicalAttributes
	{
		public ProductDimensions? Dimensions { get; set; }
		public List<string>? Materials { get; set; }
		public decimal? WeightOfProducts { get; set; }
		public string? Color { get; set; }
		public ContentMeasurementUnits ContentMeasurementsUnits { get; set; }
	}
}
