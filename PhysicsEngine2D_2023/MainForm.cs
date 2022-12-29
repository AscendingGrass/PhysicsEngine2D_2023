namespace PhysicsEngine2D_2023;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
        var env = new EnvironmentVisualizer(new PhysicsEnvironment2D(500,500));
        
        this.Controls.Add(env);
        env.Environment.AddObject2D(new Box2D(Vec2.Zero, new Vec2(100,100),.5,.5, new Vec2(3,0)));
        env.Environment.Run();
    }
}