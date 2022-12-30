namespace PhysicsEngine2D_2023;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();

        MessageBox.Show(Edge.CalculateDistance(Vec2.Zero, new Vec2(10,-15), new Vec2(12,2)).ToString());
        return;
        var env = new EnvironmentVisualizer(new PhysicsEnvironment2D());
        env.Dock = DockStyle.Fill;
        this.Controls.Add(env);
        env.Environment.AddObject2D(new Box2D(Vec2.Zero, new Vec2(100,100),.5,.5, new Vec2(100,0)));
        env.Environment.AddObject2D(new Box2D(new Vec2(300,400), new Vec2(100,100),.5,.5, new Vec2(-20,600)));
        env.Environment.Run();
    }
}