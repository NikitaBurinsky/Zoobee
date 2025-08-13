using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooStorages.Domain.Enums;

namespace ZooStorages.Domain.DataEntities.Products.Components.Attributes
{
	[Owned]
	public class PetProductAttributes
	{
		public uint? PetAgeWeeksMin { get; set; }
		public uint? PetAgeWeeksMax { get; set; }
		public PetSize? PetSize { get; set; }
		public PetGender? PetGender { get; set; }
		public PetKindEntity PetKind { get; set; }
	}
}
