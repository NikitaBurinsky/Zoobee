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

	}
}
