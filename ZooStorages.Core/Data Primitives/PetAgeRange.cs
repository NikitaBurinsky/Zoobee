using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoobee.Domain.DataEntities.Data_Primitives
{
	/// <summary>
	/// Weeks 
	/// </summary>
	[Owned]	
	public class PetAgeRange
	{
		public uint PetAgeWeeksMin { get; set; }
		public uint PetAgeWeeksMax { get; set; }
		public PetAgeRange(uint petAgeWeeksMin, uint petAgeWeeksMax)
		{
			PetAgeWeeksMin = petAgeWeeksMin;
			PetAgeWeeksMax = petAgeWeeksMax;
		}
		public PetAgeRange()
		{
		}
		/// <summary>
		/// Проверяет, входит ли текущий PetAgeRange в другой
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool IsRangeInsideOf(PetAgeRange other)
		{
			return PetAgeWeeksMin >= other.PetAgeWeeksMin 
				&& PetAgeWeeksMax <= other.PetAgeWeeksMax;
		}
	}
}
