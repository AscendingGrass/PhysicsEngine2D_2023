using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsEngine2D_2023;

// Line-Point Distance Data (LPDData)
public readonly record struct LPDData(Vec2 LineStart, Vec2 LineEnd,Vec2 Point, Vec2 ClosestLinePoint, double Distance, bool isPerpendicular)
{
    public readonly static LPDData Empty = new LPDData(Vec2.Empty, Vec2.Empty, Vec2.Empty, Vec2.Empty, double.NaN, false);
}