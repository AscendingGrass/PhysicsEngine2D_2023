using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.DataFormats;

namespace PhysicsEngine2D_2023
{
    public abstract class Object2D
    {
        const double PositionalCorrectionPower = 0.2; // usually 20% to 80%
        const double PositionalCorrectionSlop = 0.01; // usually 0.01 to 0.1

        private Vec2 location;
        private Vec2 velocity;
        private bool isStatic;
        private double inverseMass;

        private readonly Shape objectShape;
        private readonly Shape positionalShape;

        public Shape ObjectShape
        {
            get { return objectShape; }
            init 
            {
                objectShape = value;
                var temp = (Shape)value.Clone();
                temp.Offset(location);
                PositionalShape = temp;
            }
        }

        public Shape PositionalShape
        {
            get { return positionalShape; }
            private init { positionalShape = value; }
        }

        public Vec2 Location
        { 
            get => location;
            set
            {
                var oldLocation = location;
                location = value;
                positionalShape.Offset(value-oldLocation);
                LocationUpdated?.Invoke(this, new LocationUpdateEventArgs(oldLocation, value));
            }
        }
        public Vec2 Velocity 
        {
            get => velocity; 
            set 
            {
                if (IsStatic) return;
                velocity = value;
            } 
        }
        public bool IsStatic 
        {
            get => isStatic;
            set
            {
                isStatic = value;
                if (value) velocity = Vec2.Zero;
            } 
        }



        public double Mass
        {
            get { return 1/inverseMass; }
            set { inverseMass = (value == 0) ? 0 : 1/value; }
        }

        public double Restitution { get; set; }
        public double Friction { get; set; }

        public event EventHandler<LocationUpdateEventArgs>? LocationUpdated;

        protected Object2D(Vec2 location, Shape shape, double mass , double restitution, double friction, Vec2 velocity)
        {
            this.location = location;
            this.velocity = velocity;
            isStatic = false;
            Mass = mass;
            ObjectShape = shape;
            Restitution = restitution;
            Friction = friction;
        }

        public static bool IsIntersected(Object2D o1, Object2D o2, out IntersectionData? data)
        {
            var i1 = o1.PositionalShape.IntersectingVertices(o2.PositionalShape);
            var i2 = o2.PositionalShape.IntersectingVertices(o1.PositionalShape);

            data = null;
            LPDData max;
            try
            {
                max = i1[0];
            }
            catch (Exception) { return false; }


            for (int i = 1; i < i1.Length; i++)
            {
                if (i1[i].Distance > max.Distance)
                {
                    max = i1[i];
                }
            }
            bool flag = false;
            for (int i = 0; i < i2.Length; i++)
            {
                if (i2[i].Distance > max.Distance)
                {
                    max = i2[i];
                    flag = true;
                }
            }

            var obj1 = o2;
            var obj2 = o1;
            if (flag)
            {
                (obj1, obj2) = (obj2, obj1);
            }
            Vec2 normal = (Vec2)(max.LineEnd - max.LineStart).FastSurfaceNormal;
            data = new IntersectionData(obj1, obj2, normal, max);
            return true;

            //if (o1 is Box2D b1 && o2 is Box2D b2)
            //{
            //    if (b1.TopLeft.X > b2.BottomRight.X || b1.BottomRight.X < b2.TopLeft.X) return false;
            //    if (b1.TopLeft.Y > b2.BottomRight.Y || b1.BottomRight.Y < b2.TopLeft.Y) return false;
            //    return true;
            //}

            //return false;
        }


        // Reference : https://gamedevelopment.tutsplus.com/tutorials/how-to-create-a-custom-2d-physics-engine-the-basics-and-impulse-resolution--gamedev-6331
        public static void ResolveCollision(IntersectionData? data)
        {
            if (!data.HasValue) return;
            var d = data.Value;
            var relativeVelocity = d.b.Velocity - d.a.Velocity;

            double velocityAlongNormal = relativeVelocity.Dot(d.faceNormalA);

            // Do not resolve if velocities are separating 
            if (velocityAlongNormal > 0) return;

            // Calculate restitution 
            double e = Math.Min(d.a.Restitution, d.b.Restitution);

            // Calculate impulse scalar
            double j = -(1 + e) * velocityAlongNormal;
            j /=  d.a.inverseMass + d.b.inverseMass;

            // Apply impulse

            var impulse = j * d.faceNormalA;
            d.a.velocity -= d.a.inverseMass * impulse;
            d.b.velocity += d.b.inverseMass * impulse;

            PositionalCorrection(d);
        }

        public static void PositionalCorrection(IntersectionData data)
        {
            Vec2 correction = (Math.Max(data.lpdData.Value.Distance - PositionalCorrectionSlop, 0) / (data.a.inverseMass + data.b.inverseMass)) * PositionalCorrectionPower * data.faceNormalA;
            data.a.Location -= data.a.inverseMass * correction;
            data.b.Location += data.b.inverseMass * correction;
        }

    }

    public class Box2D : Object2D
    {
        
        private Vec2 size;

        public Vec2 TopLeft => Location;
        public Vec2 TopRight => new Vec2(size.X + Location.X, Location.Y);
        public Vec2 BottomLeft => new Vec2(Location.X, size.Y + Location.Y);
        public Vec2 BottomRight => Location + size;

        public Vec2 Size => size;

        public Box2D(Vec2 location, Vec2 size, double mass , double restitution, double friction, Vec2 velocity) 
            : base(
                location, 
                new Rectangle(size), 
                mass,
                restitution, 
                friction, 
                velocity)
        {
            this.size = size;
        }

        public Box2D(Vec2 location, Vec2 size, double mass, double restitution, double friction) : this(location, size, mass, restitution, friction, Vec2.Zero) { }
    }
}
