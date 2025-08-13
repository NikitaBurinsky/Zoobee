using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZooStores.Application.DtoTypes.Base;
using ZooStores.Application.DtoTypes.Companies;
using ZooStores.Application.DtoTypes.EntityComponents;

namespace ZooStores.Application.DtoTypes.Clinics
{
	public class VetClinicEntity : BaseEntity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Info { get; set; }
		public string PhoneNumber { get; set; }

		//Owner Company
		//
		public Guid ownerCompanyId { get; set; }
		public CompanyEntity ownerCompany { get; set; }

		//Location
		//
		public Guid locationId { get; set; }
		public LocationEntity location { get; set; }
	}
}
