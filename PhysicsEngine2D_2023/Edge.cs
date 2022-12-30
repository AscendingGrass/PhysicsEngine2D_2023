using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsEngine2D_2023;

internal class Edge
{
    public static LPDData CalculateDistance(Vec2 lineStart, Vec2 lineEnd, Vec2 point)
    {
        var line = lineEnd - lineStart; //the line represented by a vector
        double slope = line.Y / line.X; //slope of the line
        double perpSlope = double.IsInfinity(slope) ? 0 : -1 / slope;  //the perpendicular of the slope
        

        //calculate the intersection point of the line and another line ('point' with perpSlope as its slope)
        //to find the closest point from the line to 'point'
        double closestX = double.IsInfinity(perpSlope) ? point.X : ( double.IsInfinity(slope) ? 0 : ((perpSlope * -point.X) - (slope * -lineStart.X) + point.Y - lineStart.Y) / (slope-perpSlope) );
        double closestY = double.IsInfinity(slope) ? point.Y : slope * (closestX - lineStart.X) + lineStart.Y;

        var closestPoint = new Vec2(closestX, closestY);

        bool isPerpendicular = true;
        //check if the closest point is actually in the line, if not then set the closest point to the closest edge of the line
        if (closestPoint.X < lineStart.X && closestPoint.X < lineEnd.X)
        {
            closestPoint = lineStart.X < lineEnd.X ? lineStart : lineEnd;
            isPerpendicular = false;
        }
        else if (closestPoint.X > lineStart.X && closestPoint.X > lineEnd.X)
        {
            closestPoint = lineStart.X > lineEnd.X ? lineStart : lineEnd;
            isPerpendicular = false;
        }

        //calculate the distance with pythagorean theorem
        double distance = (closestPoint - point).Length;

        return new LPDData(lineStart, lineEnd, closestPoint, distance, isPerpendicular);
    }
}
