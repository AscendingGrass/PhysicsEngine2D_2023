using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsEngine2D_2023;

public readonly struct Vec2
{
    public readonly static Vec2 Zero  = new Vec2(0, 0);
    public readonly static Vec2 Infinity = new Vec2(double.PositiveInfinity, double.PositiveInfinity);

    public readonly double X, Y;

    public double Magnitude => Math.Sqrt(X*X + Y*Y);
    public Vec2 SurfaceNormal => new Vec2(-Y, X).UnitNormal;
    public Vec2 FastSurfaceNormal => new Vec2(-Y, X).FastUnitNormal;
    public Vec2 UnitNormal => this / Magnitude;
    public Vec2 FastUnitNormal
    {
        get
        {
            double aX = Math.Abs(X);
            double aY = Math.Abs(Y);
            double ratio = 1 / Math.Max(aX, aY);
            ratio *= (1.29289 - (aX + aY) * ratio * 0.29289);
            return new Vec2(X*ratio, Y*ratio);
        } 
    }



    #region constructors
    public Vec2(double x = 0, double y = 0) =>
        (X, Y) = (x, y);
    #endregion

    public double Dot(Vec2 v)
    {
        return X*v.X + Y*v.Y;
    }

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
    public static bool operator ==(Vec2 c1, Vec2 c2) =>
        c1.Equals(c2);
    public static bool operator !=(Vec2 c1, Vec2 c2) =>
        !c1.Equals(c2);
    #endregion



    public override string ToString() =>
        $"Coord({X},{Y})";

    public void Deconstruct(out double x, out double y) =>
        (x, y) = (X, Y);

    public override bool Equals(object? obj)
    {
        return obj is Vec2 vec && X == vec.X && Y == vec.Y;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}

