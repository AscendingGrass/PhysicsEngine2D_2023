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
        //return;
        var env = new EnvironmentVisualizer(new PhysicsEnvironment2D());
        env.Dock = DockStyle.Fill;
        this.Controls.Add(env);
        env.Environment.AddObject2D(new Box2D(new Vec2(0, 0), new Vec2(100,100),.5,.05, new Vec2(900,0)));
        env.Environment.AddObject2D(new Box2D(new Vec2(300,400), new Vec2(100,100),.3,.5, new Vec2(-20,600)));
        env.Environment.Run();
    }
}