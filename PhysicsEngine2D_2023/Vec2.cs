using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsEngine2D_2023;

public struct Vec2
{
    public readonly static Vec2 Zero = new Vec2(0, 0);

    public double X, Y;

    public double Length => Math.Sqrt(X*X + Y*Y);
    public Vec2 Normal => new Vec2(1, -(X/Y) ).Unit;
    public Vec2 Unit => this / Length;

    #region constructors
    public Vec2(double x = 0, double y = 0) =>
        (X, Y) = (x, y);
    #endregion

    public double Dot(Vec2 v)
    {
        return X*v.X + Y*v.Y;
    }

    public void SetY(double y) => this.Y = y;
    public void SetX(double x) => this.X = x;

    #region operator overloads
    public static Vec2 operator +(Vec2 c1, Vec2 c2) =>
        new Vec2(c1.X + c2.X, c1.Y + c2.Y);
    public static Vec2 operator -(Vec2 c1, Vec2 c2) =>
        new Vec2(c1.X - c2.X, c1.Y - c2.Y);
    public static Vec2 operator -(Vec2 c1) =>
       new Vec2(-c1.X, -c1.Y);
    public static Vec2 operator *(Vec2 c1, Vec2 c2) =>
        new Vec2(c1.X * c2.X, c1.Y * c2.Y);
    public static Vec2 operator *(Vec2 c1, double mult) =>
        new Vec2(c1.X * mult, c1.Y * mult);
    public static Vec2 operator *(double mult, Vec2 c1) =>
        new Vec2(c1.X * mult, c1.Y * mult);
    public static Vec2 operator /(Vec2 c1, Vec2 c2) =>
        new Vec2(c1.X / c2.X, c1.Y / c2.Y);
    public static Vec2 operator /(Vec2 c1, double div) =>
        new Vec2(c1.X / div, c1.Y / div);

    #endregion

    public override string ToString() =>
        $"Coord({X},{Y})";

    public void Deconstruct(out double x, out double y) =>
        (x, y) = (X, Y);
}

