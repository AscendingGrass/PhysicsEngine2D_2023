using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsEngine2D_2023
{
    public class Shape : ICloneable
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

        public void Offset(Vec2 offset)
        {
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i] += offset;
            }
        }

        public LPDData[] IntersectingVertices(Shape shape)
        {
            var list = new List<LPDData>(Vertices.Length/2);
            foreach (var item in Vertices)
            {
                if(shape.Contains(item, out LPDData? data))
                {
                    
                    list.Add(data.Value);
                }
            }
            return list.ToArray();
        }

        public bool Contains(Vec2 globalPoint, Vec2 shapeLocation, out LPDData? data)
        {
            return Contains(globalPoint-shapeLocation, out data);
        }
        
        public virtual bool Contains(Vec2 localPoint, out LPDData? data)
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

            if (outsideDistance < insideDistance) return false;

            data = closestLD;
            return true;
        }

        public void InvertNormals()
        {
            Array.Reverse(Vertices);
        }

        public virtual object Clone()
        {
            return new Shape((Vec2[])Vertices.Clone());
        }
    }

    public class Rectangle : Shape
    {
        public Rectangle(Vec2 size) 
            : base(new[] { Vec2.Zero, new Vec2(0, size.Y), new Vec2(size.X, size.Y), new Vec2(size.X, 0) }) 
        { }
    }

    public class Circle : Shape
    {
        public double Radius { get; init; }
        public Circle(double radius) : base(null)
        {
            Radius = radius;
        }

        public override bool Contains(Vec2 localPoint, out LPDData? data)
        {
            throw new NotImplementedException();
            return base.Contains(localPoint, out data);
        }

        public override object Clone()
        {
            return new Circle(Radius);
        }
    }
}
