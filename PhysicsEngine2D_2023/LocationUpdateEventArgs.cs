using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsEngine2D_2023;

public class LocationUpdateEventArgs:EventArgs
{
    public Vec2 StartLocation { get; set; }
    public Vec2 UpdatedLocation { get; set; }

    public LocationUpdateEventArgs(Vec2 startLocation, Vec2 updatedLocation)
    {
        StartLocation = startLocation;
        UpdatedLocation = updatedLocation;
    }


}
