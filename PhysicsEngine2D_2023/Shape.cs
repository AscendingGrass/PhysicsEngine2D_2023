using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        public virtual Vec2[] Vertices { get; private set; }

        public Shape(Vec2[] vertices)
        {
            Vertices = vertices;
        }

        public Shape(Shape s)
        {
            Vertices = (Vec2[])s.Vertices.Clone();
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

        public BoundingBox GetBoundingBox()
        {
            double xMin = double.MaxValue, 
                   xMax = double.MinValue, 
                   yMin = double.MaxValue, 
                   yMax = double.MinValue;
            foreach (var item in Vertices)
            {
                if (item.X < xMin) xMin = item.X;
                if (item.X > xMax) xMax = item.X;
                if (item.Y < yMin) yMin = item.Y;
                if (item.Y > yMax) yMax = item.Y;
            }

            var start = new Vec2(xMin, yMin);
            var end   = new Vec2(xMax, yMax);
            return new BoundingBox(start, end-start);
        }

        public LPDData[] IntersectingEdges(Vec2 lineStart, Vec2 lineEnd, double lineBuffer = 0)
        {
            var list = new List<LPDData>(2);
            for (int i = 0; i < Vertices.Length; i++)
            {
                var intersection = Edge.IntersectingPoint(Vertices[i], Vertices[(i + 1) % Vertices.Length], lineStart, lineEnd, lineBuffer);
                if (intersection != Vec2.Infinity)
                {
                    list.Add(new LPDData(Vertices[i], Vertices[(i + 1) % Vertices.Length], lineStart, intersection, (intersection-lineStart).Magnitude, true));
                }
            }
            return list.ToArray();
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

    public class BoundingBox : Shape
    {
        public double Bottom => base.Vertices[0].Y;
        public double Left => base.Vertices[0].X;
        public double Top => base.Vertices[2].Y;
        public double Right => base.Vertices[2].X;
        public override Vec2[] Vertices 
        {
            get => (Vec2[])base.Vertices.Clone();
        }
        public BoundingBox(Shape s) : base(s.GetBoundingBox()) { }
        public BoundingBox(Vec2 offset, Vec2 size) : base(Shape.Rectangle(offset, size)) { }
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

                data = new LPDData(Vec2.Infinity, Vec2.Infinity, localPoint, (new Vec2(localClosestX, localClosestY) + Center), Radius-magnitude, true);
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
