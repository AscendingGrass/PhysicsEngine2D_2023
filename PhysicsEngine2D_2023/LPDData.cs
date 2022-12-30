using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsEngine2D_2023;

// Line-Point Distance Data (LPDData)
public record LPDData(Vec2 LineStart, Vec2 LineEnd, Vec2 ClosestLinePoint, double Distance, bool isPerpendicular);
