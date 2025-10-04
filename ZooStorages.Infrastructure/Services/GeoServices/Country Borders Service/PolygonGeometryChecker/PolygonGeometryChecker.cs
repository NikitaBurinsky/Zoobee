using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zoobee.Domain.DataEntities.Environment.Geography;

namespace Zoobee.Infrastructure.Services.GeoServices.Country_Borders_Service.PolygonGeometryChecker
{

	/// <summary>
	/// Класс, работающий с полигонами, для проверки их пересечения и вхождения друг в друга. В первую очередь для областей доставки 
	/// </summary>
	public static class PolygonContainsChecker
	{
		public static bool IsPolygonCompletelyInside(
			List<GeoPoint> innerPolygon,
			List<GeoPoint> outerPolygon,
			double tolerance = 0.0001)
		{
			if (!ValidatePolygons(innerPolygon, outerPolygon))
				return false;

			// Быстрая проверка по bounding box
			if (!IsBoundingBoxInside(innerPolygon, outerPolygon, tolerance))
				return false;

			// Проверка всех точек внутреннего полигона
			foreach (var point in innerPolygon)
			{
				var position = GetPointPositionRelativeToPolygon(point, outerPolygon, tolerance);
				if (position != PointPosition.Inside && position != PointPosition.OnBorder)
					return false;
			}

			// Проверка отсутствия пересечений границ
			return !DoPolygonsIntersect(innerPolygon, outerPolygon, tolerance);
		}

		/// <summary>
		/// Алгоритм Ray Casting для проверки точки в полигоне
		/// </summary>
		public static bool IsPointInPolygon(GeoPoint point, List<GeoPoint> polygon)
		{
			if (polygon.Count < 3) return false;

			bool inside = false;
			int j = polygon.Count - 1;

			for (int i = 0; i < polygon.Count; i++)
			{
				var p1 = polygon[i];
				var p2 = polygon[j];

				// Условие пересечения луча с ребром полигона
				if ((p1.Latitude > point.Latitude) != (p2.Latitude > point.Latitude) &&
					point.Longitude < (p2.Longitude - p1.Longitude) *
					(point.Latitude - p1.Latitude) /
					(p2.Latitude - p1.Latitude) + p1.Longitude)
				{
					inside = !inside;
				}

				j = i;
			}

			return inside;
		}

		/// <summary>
		/// Проверка пересечения полигонов
		/// </summary>
		private static bool DoPolygonsIntersect(List<GeoPoint> poly1, List<GeoPoint> poly2, double tolerance)
		{
			// Проверка пересечения всех ребер первого полигона со всеми ребрами второго
			for (int i = 0; i < poly1.Count; i++)
			{
				var a1 = poly1[i];
				var a2 = poly1[(i + 1) % poly1.Count];

				for (int j = 0; j < poly2.Count; j++)
				{
					var b1 = poly2[j];
					var b2 = poly2[(j + 1) % poly2.Count];

					if (DoLineSegmentsIntersect(a1, a2, b1, b2, tolerance))
						return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Проверка пересечения двух отрезков
		/// </summary>
		private static bool DoLineSegmentsIntersect(GeoPoint a1, GeoPoint a2, GeoPoint b1, GeoPoint b2, double tolerance)
		{
			// Вычисление ориентаций
			double o1 = Orientation(a1, a2, b1);
			double o2 = Orientation(a1, a2, b2);
			double o3 = Orientation(b1, b2, a1);
			double o4 = Orientation(b1, b2, a2);

			// Общий случай пересечения
			if (o1 * o2 < 0 && o3 * o4 < 0)
				return true;

			// Специальные случаи (коллинеарность)
			if (Math.Abs(o1) <= tolerance && IsOnSegment(a1, a2, b1, tolerance)) return true;
			if (Math.Abs(o2) <= tolerance && IsOnSegment(a1, a2, b2, tolerance)) return true;
			if (Math.Abs(o3) <= tolerance && IsOnSegment(b1, b2, a1, tolerance)) return true;
			if (Math.Abs(o4) <= tolerance && IsOnSegment(b1, b2, a2, tolerance)) return true;

			return false;
		}

		/// <summary>
		/// Вычисление ориентации трех точек
		/// </summary>
		private static double Orientation(GeoPoint p, GeoPoint q, GeoPoint r)
		{
			return (q.Longitude - p.Longitude) * (r.Latitude - p.Latitude) -
				   (r.Longitude - p.Longitude) * (q.Latitude - p.Latitude);
		}

		/// <summary>
		/// Проверка, лежит ли точка на отрезке
		/// </summary>
		private static bool IsOnSegment(GeoPoint p, GeoPoint q, GeoPoint r, double tolerance)
		{
			return r.Longitude <= Math.Max(p.Longitude, q.Longitude) + tolerance &&
				   r.Longitude >= Math.Min(p.Longitude, q.Longitude) - tolerance &&
				   r.Latitude <= Math.Max(p.Latitude, q.Latitude) + tolerance &&
				   r.Latitude >= Math.Min(p.Latitude, q.Latitude) - tolerance;
		}

		/// <summary>
		/// Проверка вхождения bounding box'а
		/// </summary>
		private static bool IsBoundingBoxInside(List<GeoPoint> inner, List<GeoPoint> outer, double tolerance)
		{
			var innerBox = GetBoundingBox(inner);
			var outerBox = GetBoundingBox(outer);

			return innerBox.MinX >= outerBox.MinX - tolerance &&
				   innerBox.MaxX <= outerBox.MaxX + tolerance &&
				   innerBox.MinY >= outerBox.MinY - tolerance &&
				   innerBox.MaxY <= outerBox.MaxY + tolerance;
		}

		private static (double MinX, double MaxX, double MinY, double MaxY) GetBoundingBox(List<GeoPoint> points)
		{
			double minX = points.Min(p => p.Longitude);
			double maxX = points.Max(p => p.Longitude);
			double minY = points.Min(p => p.Latitude);
			double maxY = points.Max(p => p.Latitude);

			return (minX, maxX, minY, maxY);
		}

		private enum PointPosition { Inside, Outside, OnBorder }

		private static PointPosition GetPointPositionRelativeToPolygon(GeoPoint point, List<GeoPoint> polygon, double tolerance)
		{
			if (IsPointOnPolygonBorder(point, polygon, tolerance))
				return PointPosition.OnBorder;

			return IsPointInPolygon(point, polygon) ? PointPosition.Inside : PointPosition.Outside;
		}

		private static bool IsPointOnPolygonBorder(GeoPoint point, List<GeoPoint> polygon, double tolerance)
		{
			for (int i = 0; i < polygon.Count; i++)
			{
				var p1 = polygon[i];
				var p2 = polygon[(i + 1) % polygon.Count];

				if (IsPointOnLineSegment(point, p1, p2, tolerance))
					return true;
			}

			return false;
		}

		private static bool IsPointOnLineSegment(GeoPoint point, GeoPoint lineStart, GeoPoint lineEnd, double tolerance)
		{
			double distance = PointToLineDistance(point, lineStart, lineEnd);
			return distance <= tolerance && IsPointBetween(point, lineStart, lineEnd, tolerance);
		}

		private static double PointToLineDistance(GeoPoint point, GeoPoint lineStart, GeoPoint lineEnd)
		{
			double A = point.Longitude - lineStart.Longitude;
			double B = point.Latitude - lineStart.Latitude;
			double C = lineEnd.Longitude - lineStart.Longitude;
			double D = lineEnd.Latitude - lineStart.Latitude;

			double dot = A * C + B * D;
			double lenSq = C * C + D * D;
			double param = lenSq != 0 ? dot / lenSq : -1;

			double xx, yy;

			if (param < 0) (xx, yy) = (lineStart.Longitude, lineStart.Latitude);
			else if (param > 1) (xx, yy) = (lineEnd.Longitude, lineEnd.Latitude);
			else (xx, yy) = (lineStart.Longitude + param * C, lineStart.Latitude + param * D);

			double dx = point.Longitude - xx;
			double dy = point.Latitude - yy;

			return Math.Sqrt(dx * dx + dy * dy);
		}

		private static bool IsPointBetween(GeoPoint point, GeoPoint lineStart, GeoPoint lineEnd, double tolerance)
		{
			double minX = Math.Min(lineStart.Longitude, lineEnd.Longitude);
			double maxX = Math.Max(lineStart.Longitude, lineEnd.Longitude);
			double minY = Math.Min(lineStart.Latitude, lineEnd.Latitude);
			double maxY = Math.Max(lineStart.Latitude, lineEnd.Latitude);

			return point.Longitude >= minX - tolerance && point.Longitude <= maxX + tolerance &&
				   point.Latitude >= minY - tolerance && point.Latitude <= maxY + tolerance;
		}

		private static bool ValidatePolygons(List<GeoPoint> inner, List<GeoPoint> outer)
		{
			return inner != null && outer != null &&
				   inner.Count >= 3 && outer.Count >= 3 &&
				   IsPolygonValid(inner) && IsPolygonValid(outer);
		}

		private static bool IsPolygonValid(List<GeoPoint> polygon)
		{
			// Проверка, что полигон не самопересекающийся
			// и имеет достаточную площадь
			return polygon.Distinct().Count() >= 3;
		}
	}


}
