using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoobee.Domain.DataEntities.Data_Primitives
{
	[Owned]
	public record SizeDimensions(float X, float Y, float Z)
	{
		/// <summary>
		/// Is A less than B
		/// </summary>
		public bool AllLessOrEqual(SizeDimensions b) { 
			return X <= b.X && Y <= b.Y && Z <= b.Z;
		}
		public bool AllLess(SizeDimensions b) {
			return X < b.X && Y < b.Y && Z < b.Z;
		}
		public bool AllEquals(SizeDimensions b) { 
			return X == b.X && Y == b.Y && Z == b.Z;
		}

	}
}
