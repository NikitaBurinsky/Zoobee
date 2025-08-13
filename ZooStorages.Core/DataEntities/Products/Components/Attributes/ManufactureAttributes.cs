using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooStorages.Domain.DataEntities.Products.Components.Attributes
{
	public class ManufactureAttributes
	{
		public string Brand { get; set; }
		public string? CreatorCountry { get; set; }
		/// <summary>
		/// Длина UPC кода - 12
		/// </summary>
		public string UPC_Code { get; set; }
		/// <summary>
		/// Длина EAN кода - 13
		/// </summary>
		public string EAN_Code { get; set; }
	}

}
