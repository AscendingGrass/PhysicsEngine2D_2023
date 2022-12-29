using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsEngine2D_2023
{
    public abstract class Object2D
    {
        private Vec2 location;
        private Vec2 velocity;
        private bool isStatic;
        

        public Vec2 Location
        { 
            get => location;
            set
            {
                var oldLocation = location;
                location = value;
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

        public double Restitution { get; set; }
        public double Friction { get; set; }

        public event EventHandler<LocationUpdateEventArgs>? LocationUpdated;

        protected Object2D(Vec2 location,  double restitution, double friction, Vec2 velocity)
        {
            this.location = location;
            this.velocity = velocity;
            isStatic = false;
            Restitution = restitution;
            Friction = friction;
        }

        public static bool IsIntersected(Object2D o1, Object2D o2)
        {
            if (o1 is Box2D b1 && o2 is Box2D b2)
            {
                if (b1.TopLeft.X > b2.BottomRight.X || b1.BottomRight.X < b2.TopLeft.X) return false;
                if (b1.TopLeft.Y > b2.BottomRight.Y || b1.BottomRight.Y < b2.TopLeft.Y) return false;
                return true;
            }

            return false;
        }

        public static void ResolveCollision(Object2D o1, Object2D o2)
        {

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

        public Box2D(Vec2 location, Vec2 size,  double restitution, double friction, Vec2 velocity) : base(location, restitution, friction, velocity)
        {
            this.size = size;
        }

        public Box2D(Vec2 location, Vec2 size, double restitution, double friction) : this(location, size, restitution, friction, Vec2.Zero) { }
    }
}
