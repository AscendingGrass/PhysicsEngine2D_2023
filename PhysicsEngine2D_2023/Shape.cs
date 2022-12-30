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

        //the outside should be on the right side of the edges
        //ex. {(0,0), (0,1), (1,1), (1,0)} has the correct normals
        //while this has inverted normals {(0,0), (1,0), (1,1), (0,1)} therefore InvertNormals() should be called
        public Vec2[] Vertices { get; private set; }

        public Shape(Vec2[] vertices)
        {
            Vertices = vertices;
        }

        public bool IsInside(Vec2 point, out LPDData? data)
        {
            data = null;

            var closestLD = Edge.CalculateDistance(Vertices[Vertices.Length-1], Vertices[0], point);
            for (int i = 1; i < Vertices.Length; i++)
            {
                var temp = Edge.CalculateDistance(Vertices[i-1], Vertices[i], point);
                if(temp.Distance < closestLD.Distance) closestLD = temp;
            }

            return false;
        }

        public void InvertNormals()
        {
            Array.Reverse(Vertices);
        }
    }
}
