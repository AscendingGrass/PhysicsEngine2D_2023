namespace PhysicsEngine2D_2023;

public partial class MainForm : Form
{
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
        //return;
        var env = new EnvironmentVisualizer(new PhysicsEnvironment2D());
        env.Dock = DockStyle.Fill;
        this.Controls.Add(env);
        env.Environment.AddObject2D(new Box2D(new Vec2(0, 0), new Vec2(100, 100), 1, .7, .99, new Vec2(500, 0)));
        env.Environment.AddObject2D(new Box2D(new Vec2(500, 0), new Vec2(100, 50), 1, .7, .99, new Vec2(-500, 0)));
        env.Environment.Run();
    }
}