using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.PeerToPeer;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsEngine2D_2023;

internal class Edge
{
    // p1 : point1
    // p2 : point2
    // m1 : slope1
    // m2 : slope2
    public static Vec2 IntersectingPoint(Vec2 p1, double m1,  Vec2 p2, double m2)
    {
        if (m1 == m2) return Vec2.Infinity;

        double intersectionX = double.IsInfinity(m2) ? p2.X : (double.IsInfinity(m1) ? p1.X : ((m2 * - p2.X) - (m1 * - p1.X) + p2.Y - p1.Y) / (m1 - m2));
        double intersectionY = double.IsInfinity(m1) ? (m2 * (intersectionX - p2.X) + p2.Y) : (m1 * (intersectionX - p1.X) + p1.Y);
        return new Vec2(intersectionX, intersectionY);
    }
    public static Vec2 IntersectingPoint(Vec2 lineStart, Vec2 lineEnd, Vec2 point, double pointSlope, double buffer = 0)
    {
        Vec2 line = lineEnd - lineStart;
        Vec2 result = IntersectingPoint(lineStart, (line.Y/line.X), point, pointSlope );
        if( result == Vec2.Infinity ) return Vec2.Infinity;
        if ((result.X < lineStart.X-buffer && result.X < lineEnd.X-buffer) ||
            (result.X > lineStart.X+buffer && result.X > lineEnd.X+buffer) ||
            (result.Y < lineStart.Y-buffer && result.Y < lineEnd.Y-buffer) ||
            (result.Y > lineStart.Y+buffer && result.Y > lineEnd.Y+buffer)
            )
        {
            return Vec2.Infinity;
        }

        return result;
    }
    public static Vec2 IntersectingPoint(Vec2 lineStart1, Vec2 lineEnd1, Vec2 lineStart2, Vec2 lineEnd2, double buffer = 0)
    {
        Vec2 line = lineEnd2 - lineStart2;
        Vec2 result = IntersectingPoint(lineStart1, lineEnd1, lineStart2, (line.Y / line.X), buffer);
        if (result == Vec2.Infinity) return Vec2.Infinity;
        if ((result.X < lineStart2.X - buffer && result.X < lineEnd2.X - buffer) ||
            (result.X > lineStart2.X + buffer && result.X > lineEnd2.X + buffer) ||
            (result.Y < lineStart2.Y - buffer && result.Y < lineEnd2.Y - buffer) ||
            (result.Y > lineStart2.Y + buffer && result.Y > lineEnd2.Y + buffer)
            )
        {
            return Vec2.Infinity;
        }

        return result;
    }
    public static LPDData CalculateDistance(Vec2 lineStart, Vec2 lineEnd, Vec2 point)
    {
        var line = lineEnd - lineStart; //the line represented by a vector
        double slope = line.Y / line.X; //slope of the line
        double perpSlope = double.IsInfinity(slope) ? 0 : -1 / slope;  //the perpendicular of the slope

        //double closestX = double.IsInfinity(perpSlope) ? point.X : ( double.IsInfinity(slope) ? lineStart.X : ((perpSlope * -point.X) - (slope * -lineStart.X) + point.Y - lineStart.Y) / (slope-perpSlope) );
        //double closestY = double.IsInfinity(slope) ? point.Y : slope * (closestX - lineStart.X) + lineStart.Y;

        //calculate the intersection point of the line and another line ('point' with perpSlope as its slope)
        //to find the closest point from the line to 'point'
        var closestPoint = IntersectingPoint(lineStart, lineEnd, point, perpSlope);

        //Return Empty if there is no intersection point
        if (closestPoint == Vec2.Infinity) return LPDData.Empty;

        //calculate the distance with pythagorean theorem
        double distance = (closestPoint - point).Magnitude;

        return new LPDData(lineStart, lineEnd, point, closestPoint, distance, true);
    }
}
