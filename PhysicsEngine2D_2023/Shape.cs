using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsEngine2D_2023
{
    public class Shape
    {
        //should be ordered to form edges that make up a shape
        //ex. {(0,0), (0,1), (1,1), (1,0)} would make a square
        //while this wouldn't {(0,0), (1,1), (0,1), (1,0)} 

        //the outside should be on the left side of the edges
        //ex. {(0,0), (1,0), (1,1), (0,1)} has the correct normals
        //while this has inverted normals {(0,0), (0,1), (1,1), (1,0)} therefore InvertNormals() should be called
        public Vec2[] Vertices { get; private set; }

        public Shape(Vec2[] vertices)
        {
            Vertices = vertices;
        }

        public bool IsInside(Vec2 localPoint, out LPDData? data)
        {
            data = null;

            var closestLD = Edge.CalculateDistance(Vertices[Vertices.Length-1], Vertices[0], localPoint);
            for (int i = 1; i < Vertices.Length; i++)
            {
                var temp = Edge.CalculateDistance(Vertices[i-1], Vertices[i], localPoint);
                if(temp.Distance < closestLD.Distance) closestLD = temp;
            }

            if (!closestLD.isPerpendicular) return false;
            var lineSurfaceNormal = (closestLD.LineEnd - closestLD.LineStart).FastSurfaceNormal;

            double outsideDistance = (closestLD.ClosestLinePoint + lineSurfaceNormal - localPoint).Magnitude;
            double insideDistance  = (closestLD.ClosestLinePoint - lineSurfaceNormal - localPoint).Magnitude;

            MessageBox.Show($"line : {(closestLD.LineEnd - closestLD.LineStart)} | surfaceNormal : {lineSurfaceNormal}");
            MessageBox.Show($"outside : {outsideDistance} | inside : {insideDistance} ");

            if (outsideDistance < insideDistance) return false;

            data = closestLD;
            return true;
        }

        public void InvertNormals()
        {
            Array.Reverse(Vertices);
        }
    }
}
