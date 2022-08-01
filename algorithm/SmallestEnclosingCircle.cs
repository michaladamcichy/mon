using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* 
 * Smallest enclosing circle - Library (C#)
 * 
 * Copyright (c) 2020 Project Nayuki
 * https://www.nayuki.io/page/smallest-enclosing-circle
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program (see COPYING.txt and COPYING.LESSER.txt).
 * If not, see <http://www.gnu.org/licenses/>.
 */

namespace algorithm
{
	public class SmallestEnclosingCircleAdapter
    {
		static (Position, double) GetCircle(List<MapObject> mapObjects)
        {
			var points = mapObjects.Select(mapObject => new Point(mapObject.Position.Lat, mapObject.Position.Lng)).ToList();
			var circle = SmallestEnclosingCircle.MakeCircle(points);
			return (new Position(circle.c.x, circle.c.y), circle.r);
        }

		public static Position GetCenter(List<MapObject> mapObjects)
        {
			return GetCircle(mapObjects).Item1;
        }

		public static double GetRange(List<MapObject> mapObjects)
		{
			return GetCircle(mapObjects).Item2;
		}

		public static Position GetCenter(List<Station> stations)
        {
			return GetCenter(stations.Cast<MapObject>().ToList());
        }

		public static double GetRange(List<Station> stations)
        {
			return GetRange(stations.Cast<MapObject>().ToList());
        }
	}
	class SmallestEnclosingCircle
	{

		/* 
		 * Returns the smallest circle that encloses all the given points. Runs in expected O(n) time, randomized.
		 * Note: If 0 points are given, a circle of radius -1 is returned. If 1 point is given, a circle of radius 0 is returned.
		 */
		// Initially: No boundary points known
		public static Circle MakeCircle(IList<Point> points)
		{
			// Clone list to preserve the caller's data, do Durstenfeld shuffle
			List<Point> shuffled = new List<Point>(points);
			Random rand = new Random();
			for (int i = shuffled.Count - 1; i > 0; i--)
			{
				int j = rand.Next(i + 1);
				Point temp = shuffled[i];
				shuffled[i] = shuffled[j];
				shuffled[j] = temp;
			}

			// Progressively add points to circle or recompute circle
			Circle c = Circle.INVALID;
			for (int i = 0; i < shuffled.Count; i++)
			{
				Point p = shuffled[i];
				if (c.r < 0 || !c.Contains(p))
					c = MakeCircleOnePoint(shuffled.GetRange(0, i + 1), p);
			}
			return c;
		}


		// One boundary point known
		private static Circle MakeCircleOnePoint(List<Point> points, Point p)
		{
			Circle c = new Circle(p, 0);
			for (int i = 0; i < points.Count; i++)
			{
				Point q = points[i];
				if (!c.Contains(q))
				{
					if (c.r == 0)
						c = MakeDiameter(p, q);
					else
						c = MakeCircleTwoPoints(points.GetRange(0, i + 1), p, q);
				}
			}
			return c;
		}


		// Two boundary points known
		private static Circle MakeCircleTwoPoints(List<Point> points, Point p, Point q)
		{
			Circle circ = MakeDiameter(p, q);
			Circle left = Circle.INVALID;
			Circle right = Circle.INVALID;

			// For each point not in the two-point circle
			Point pq = q.Subtract(p);
			foreach (Point r in points)
			{
				if (circ.Contains(r))
					continue;

				// Form a circumcircle and classify it on left or right side
				double cross = pq.Cross(r.Subtract(p));
				Circle c = MakeCircumcircle(p, q, r);
				if (c.r < 0)
					continue;
				else if (cross > 0 && (left.r < 0 || pq.Cross(c.c.Subtract(p)) > pq.Cross(left.c.Subtract(p))))
					left = c;
				else if (cross < 0 && (right.r < 0 || pq.Cross(c.c.Subtract(p)) < pq.Cross(right.c.Subtract(p))))
					right = c;
			}

			// Select which circle to return
			if (left.r < 0 && right.r < 0)
				return circ;
			else if (left.r < 0)
				return right;
			else if (right.r < 0)
				return left;
			else
				return left.r <= right.r ? left : right;
		}

		public static Circle MakeDiameter(Point a, Point b)
		{
			Point c = new Point((a.x + b.x) / 2, (a.y + b.y) / 2);
			return new Circle(c, Math.Max(c.Distance(a), c.Distance(b)));
		}


		public static Circle MakeCircumcircle(Point a, Point b, Point c)
		{
			//return findCircle(a.x, a.y, b.x, b.y, c.x, c.y);
			// Mathematical algorithm from Wikipedia: Circumscribed circle
			double ox = (Math.Min(Math.Min(a.x, b.x), c.x) + Math.Max(Math.Max(a.x, b.x), c.x)) / 2;
			double oy = (Math.Min(Math.Min(a.y, b.y), c.y) + Math.Max(Math.Max(a.y, b.y), c.y)) / 2;
			double ax = a.x - ox, ay = a.y - oy;
			double bx = b.x - ox, by = b.y - oy;
			double cx = c.x - ox, cy = c.y - oy;
			double d = (ax * (by - cy) + bx * (cy - ay) + cx * (ay - by)) * 2;
			if (d == 0)
				return Circle.INVALID;
			double x = ((ax * ax + ay * ay) * (by - cy) + (bx * bx + by * by) * (cy - ay) + (cx * cx + cy * cy) * (ay - by)) / d;
			double y = ((ax * ax + ay * ay) * (cx - bx) + (bx * bx + by * by) * (ax - cx) + (cx * cx + cy * cy) * (bx - ax)) / d;
			Point p = new Point(ox + x, oy + y);
			double r = Math.Max(Math.Max(p.Distance(a), p.Distance(b)), p.Distance(c));
			return new Circle(p, r);
		}

		static Circle findCircle(double x1, double y1,
						double x2, double y2,
						double x3, double y3)
		{
			double x12 = x1 - x2;
			double x13 = x1 - x3;

			double y12 = y1 - y2;
			double y13 = y1 - y3;

			double y31 = y3 - y1;
			double y21 = y2 - y1;

			double x31 = x3 - x1;
			double x21 = x2 - x1;

			// x1^2 - x3^2
			double sx13 = (double)(Math.Pow(x1, 2) -
							Math.Pow(x3, 2));

			// y1^2 - y3^2
			double sy13 = (double)(Math.Pow(y1, 2) -
							Math.Pow(y3, 2));

			double sx21 = (double)(Math.Pow(x2, 2) -
							Math.Pow(x1, 2));

			double sy21 = (double)(Math.Pow(y2, 2) -
							Math.Pow(y1, 2));

			double f = ((sx13) * (x12)
					+ (sy13) * (x12)
					+ (sx21) * (x13)
					+ (sy21) * (x13))
					/ (2 * ((y31) * (x12) - (y21) * (x13)));
			double g = ((sx13) * (y12)
					+ (sy13) * (y12)
					+ (sx21) * (y13)
					+ (sy21) * (y13))
					/ (2 * ((x31) * (y12) - (x21) * (y13)));

			double c = -(double)Math.Pow(x1, 2) - (double)Math.Pow(y1, 2) - 2 * g * x1 - 2 * f * y1;

			// eqn of circle be x^2 + y^2 + 2*g*x + 2*f*y + c = 0
			// where centre is (h = -g, k = -f) and radius r
			// as r^2 = h^2 + k^2 - c
			double h = -g;
			double k = -f;
			double sqr_of_r = h * h + k * k - c;

			// r is the radius
			double r = Math.Round(Math.Sqrt(sqr_of_r), 5);

			return new Circle(new Point(h, k), r);
			Console.WriteLine("Centre = (" + h + "," + k + ")");
		}

	}



	public struct Circle
	{

		public static readonly Circle INVALID = new Circle(new Point(0, 0), -1);

		private const double MULTIPLICATIVE_EPSILON = 1 + 1e-14;


		public Point c;   // Center
		public double r;  // Radius


		public Circle(Point c, double r)
		{
			this.c = c;
			this.r = r;
		}


		public bool Contains(Point p)
		{
			return c.Distance(p) <= r * MULTIPLICATIVE_EPSILON;
		}


		public bool Contains(ICollection<Point> ps)
		{
			foreach (Point p in ps)
			{
				if (!Contains(p))
					return false;
			}
			return true;
		}

	}



	public struct Point
	{

		public double x;
		public double y;


		public Point(double x, double y)
		{
			this.x = x;
			this.y = y;
		}


		public Point Subtract(Point p)
		{
			return new Point(x - p.x, y - p.y);
		}


		public double Distance(Point p)
		{
			return MapObject.Distance(new Position(p.x, p.y), new Position(x, y));

			double dx = x - p.x;
			double dy = y - p.y;
			return Math.Sqrt(dx * dx + dy * dy);
		}


		// Signed area / determinant thing
		public double Cross(Point p)
		{
			return x * p.y - y * p.x;
		}

	}
}