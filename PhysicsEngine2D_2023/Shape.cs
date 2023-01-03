using System;
using System.Collections.Generic;
using System.Drawing;
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

        public static Shape Rectangle(Vec2 size)
        {
            return Rectangle(Vec2.Zero, size);
        }

        public static Shape Rectangle(Vec2 offset, Vec2 size)
        {
            return new Shape(new[] { offset, new Vec2(offset.X, size.Y+offset.Y), new Vec2(size.X+offset.X, size.Y+offset.Y), new Vec2(size.X+offset.X, offset.Y) });
        }
        public virtual void Offset(Vec2 offset)
        {
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i] += offset;
            }
        }

        public LPDData[] IntersectingVertices(Shape shape)
        {
            var list = new List<LPDData>(Vertices.Length/2+1);
            foreach (var item in Vertices)
            {
                if(shape.Contains(item, out LPDData data))
                {
                    list.Add(data);
                }
            }
            return list.ToArray();
        }

        //public bool Contains(Vec2 globalPoint, Vec2 shapeLocation, out LPDData data)
        //{
        //    return Contains(globalPoint-shapeLocation, out data);
        //}
        
        public virtual bool Contains(Vec2 localPoint, out LPDData data)
        {
            var closestLD = Edge.CalculateDistance(Vertices[Vertices.Length-1], Vertices[0], localPoint);
            for (int i = 1; i < Vertices.Length; i++)
            {
                var temp = Edge.CalculateDistance(Vertices[i-1], Vertices[i], localPoint);
                if (temp == LPDData.Empty) continue;
                if (temp.Distance < closestLD.Distance) closestLD = temp;
            }
            data = closestLD;
            if (closestLD == LPDData.Empty) return false;

            var lineSurfaceNormal = (closestLD.LineEnd - closestLD.LineStart).FastSurfaceNormal;

            double outsideDistance = (closestLD.ClosestLinePoint + lineSurfaceNormal - localPoint).Magnitude;
            double insideDistance  = (closestLD.ClosestLinePoint - lineSurfaceNormal - localPoint).Magnitude;

            //if the outside distance is greater than the inside distance,
            //the point is located inside the shape
            return outsideDistance >= insideDistance;
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
        public Vec2 Center { get; private set; }
        public double Radius { get; init; }
        public Circle(Vec2 position, double radius) : base(new Vec2[0])
        {
            Center = position;
            Radius = radius;
        }

        public Circle(double radius) : this(Vec2.Zero, radius) { }

        public override void Offset(Vec2 offset)
        {
            Center += offset;
        }

        public override bool Contains(Vec2 localPoint, out LPDData data)
        {
            var direction = (localPoint - Center);
            double magnitude = direction.Magnitude;
            bool isInside = magnitude <= Radius;
            
            if (isInside)
            {
                //cos θ = (a · b) / (|a| |b|)
                //source : https://www.cuemath.com/geometry/angle-between-vectors/

                double cosValue = direction.Dot(new Vec2(1, 0)) / magnitude;
                double slope = direction.Y / direction.X;
                double localClosestX = Radius * cosValue;
                double localClosestY = double.IsInfinity(slope) ? 0 : (slope * localClosestX);

                data = new LPDData(Vec2.Empty, Vec2.Empty, localPoint, (new Vec2(localClosestX, localClosestY) + Center), Radius-magnitude, true);
                return true;
            }
            data = LPDData.Empty;
            return false;
        }

        public override object Clone()
        {
            return new Circle(Center, Radius);
        }
    }
}
