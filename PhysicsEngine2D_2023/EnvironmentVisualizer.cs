﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhysicsEngine2D_2023
{
    public partial class EnvironmentVisualizer : UserControl
    {
        public PhysicsEnvironment2D Environment;

        //private Brush brush = new SolidBrush(Color.Black);
        public EnvironmentVisualizer(PhysicsEnvironment2D environment)
        {
            InitializeComponent();
            environment.Width  = Width;
            environment.Height = Height;
            environment.Updated += Environment_Updated;
            this.Environment = environment;
            
        }

        private void Environment_Updated(object? sender, EventArgs e) => this.Invalidate();

        private void EnvironmentVisualizer_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            
            for (int i = 0; i < Environment.CountObjects; i++)
            {
                if (Environment[i] is Sphere2D s)
                {
                    Circle c = ((Circle)s.PositionalShape);
                    using (var brush = new SolidBrush(Color.Black))
                    {
                        g.FillEllipse(brush, (int)Math.Round(c.Center.X), (int)Math.Round(Height - c.Center.Y), (int)Math.Round(c.Radius), (int)Math.Round(c.Radius));
                        continue;
                    }

                }
                using (var brush = new SolidBrush(Color.Black))
                {
                    g.FillPolygon(brush, Environment[i].PositionalShape.Vertices.Select(x => new Point((int)Math.Round(x.X), (int)Math.Round(Height - x.Y))).ToArray());
                }
            }
        }

        private void EnvironmentVisualizer_SizeChanged(object sender, EventArgs e)
        {
            Environment.Width = Width;
            Environment.Height = Height;
        }
    }
}
