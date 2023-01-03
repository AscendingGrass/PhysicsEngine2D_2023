using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsEngine2D_2023
{
    public readonly record struct IntersectionData(Object2D a, Object2D b, Vec2 faceNormalA, LPDData lpdData)
    {
        public static readonly IntersectionData Empty = new IntersectionData(null, null, Vec2.Empty, LPDData.Empty);
    }
}
