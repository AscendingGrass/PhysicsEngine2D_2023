using System.CodeDom;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;
using Timer = System.Windows.Forms.Timer;

namespace PhysicsEngine2D_2023;

public partial class MainForm : Form
{
    EnvironmentVisualizer visualizer;
    Timer t = new Timer {Interval = 100 };
    double totalDeltaTime = 0;
    long counter = 0;
    public MainForm()
    {
        InitializeComponent();

        //MessageBox.Show(Edge.CalculateDistance(Vec2.Zero, new Vec2(10,-15), new Vec2(12,2)).ToString());
        //Shape s = new Shape(new[] {Vec2.Zero, new Vec2(0,1), new Vec2(1,1), new Vec2(1,0) });
        //bool res = s.IsInside(new Vec2(.2, .5), out LPDData? data);

        //MessageBox.Show("inside : " + res + " | " + data);
        //var vel1 = new Vec2(0,0);
        //var vel2 = new Vec2(1,0);
        //var normal = new Vec2(1, 1).Normalized;
        //var diff = vel2 - vel1;
        //MessageBox.Show("normal : " + normal + " | velocity : " + diff.Dot(normal) + " | 1 Magnitude : " + new Vec2(1,1).Magnitude);

        //var r = new Shape(new[] { new Vec2(0, 0), new Vec2(0, 3), new Vec2(3, 3), new Vec2(3, 0) });
        //var r = new Circle(new Vec2(4,4),5);
        ////r.Offset(new Vec2(0, 4));
        //string str = "";
        //LPDData d = new LPDData();
        //for (int i = 9; i >= 0; i--)
        //{
        //    for (int j = 0; j < 10; j++)
        //    {
        //        str += r.Contains(new Vec2(j, i), out var data) ? "1" : "0";
        //        if ((j, i) == (3, 4)) d = (LPDData)data;

        //    }
        //    str += "\n";
        //}
        //MessageBox.Show(str);
        //MessageBox.Show(d.ToString());
        //MessageBox.Show(Edge.IntersectingPoint(new Vec2(1,6), new Vec2(5,6), new Vec2(1, 1), new Vec2(9,10)).ToString());
        //MessageBox.Show((Vec2.Empty == new Vec2(double.NaN, double.NaN)).ToString());
        //return;
        visualizer = new EnvironmentVisualizer(new PhysicsEnvironment2D(500,10));
        visualizer.Environment.FPSLimit = 60;
        visualizer.Environment.Updated += CountFPS;
        visualizer.Environment.Updated += UpdateFPSLabel;
        //t.Tick += UpdateFPSLabel;
        //t.Start();
        visualizer.Dock = DockStyle.Fill;
        this.Controls.Add(visualizer);
        Shape s = new Shape(new[] {new Vec2(0,100), new Vec2(50,150), new Vec2(100,0) });
        visualizer.Environment.AddObject2D(new Polygon2D(new Vec2(150,200), s, 2, .5, .5, Vec2.Zero));
        visualizer.Environment.AddObject2D(new Box2D(new Vec2(100,31), new Vec2(100, 100), 1, .7, .8, new Vec2(0, 100)));
        visualizer.Environment.AddObject2D(new Box2D(new Vec2(500, 100), new Vec2(100, 100), 1, .7, .8, new Vec2(-200, 0)));
        visualizer.Environment.AddObject2D(new Box2D(new Vec2(0, 0), new Vec2(4000, 30), 100, .7, .8, new Vec2(-200, 0)) { IsStatic = true});
        //visualizer.Environment.AddObject2D(new Sphere2D(new Vec2(270, 400), 100, 1, .7, .99, new Vec2(200, 0)));
        //env.Environment.AddObject2D(new Box2D(new Vec2(500, 0), new Vec2(100, 105), 1, .7, .99, new Vec2(0, 0)));
        visualizer.Environment.Run();
    }

    private void CountFPS(object? sender, EventArgs e)
    {
        UpdateEventArgs uea = (UpdateEventArgs)e;
        totalDeltaTime += uea.DeltaTime;
        counter++;
        
    }

    delegate void UpdateFPSLabelCallback(object? sender, EventArgs e);
    private void UpdateFPSLabel(object? sender, EventArgs e)
    {
        
        try
        {
            if (this.label1.InvokeRequired)
            {
                this.Invoke(new UpdateFPSLabelCallback(UpdateFPSLabel), new object[] { sender, e });
                return;
            }
            double deltaTime = Math.Round(totalDeltaTime / counter, 5);
            label1.Text = $"FPS : {(int)Math.Round(1/deltaTime)}";
            counter = 0;
            totalDeltaTime = 0;

        }
        catch (TargetInvocationException) { }
    }

    private void button2_Click(object sender, EventArgs e)
    {
        visualizer.Environment.Stop();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        visualizer.Environment.Run();
    }

    private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
    {
        
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        
    }
}