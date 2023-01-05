using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timer = System.Windows.Forms.Timer;

namespace PhysicsEngine2D_2023;

public class UpdateEventArgs : EventArgs
{
    public double DeltaTime;

    public UpdateEventArgs(double deltaTime)
    {
        DeltaTime = deltaTime;
    }
}

public class PhysicsEnvironment2D
{
    public double Width  { get; set; }
    public double Height { get; set; }
    public double Gravity { get; set; }
    
    public int CountObjects => objects.Count;

    private bool isRunning;
    private long lastRecordedTicks;
    private Task updateTask;
    private Stopwatch sw = new Stopwatch();
    private double minDeltaTime = .01;

    public int FPSLimit
    {
        get => (int)Math.Round(1 / minDeltaTime);
        set => minDeltaTime = 1.0/value;
    }

    public bool IsRunning
    {
        get { return isRunning; }
    }

    private readonly List<Object2D> objects = new List<Object2D>();

    public event EventHandler? Updated;

    public PhysicsEnvironment2D(double width=100, double height=100, double gravity = 1000)
    {
        Width = width;
        Height = height;
        Gravity = gravity;
        isRunning = false;
        updateTask = new Task(Update);
        //timer.Tick += Update;
    }

    private void ExecuteUpdates()
    {
        while (isRunning)
        {
            Update();
        }
    }

    private void Update()
    {
        double deltaTime = (sw.ElapsedTicks - lastRecordedTicks) / 10_000_000.0;
        if (deltaTime < minDeltaTime) return;
        lastRecordedTicks = sw.ElapsedTicks;
        //MessageBox.Show(deltaTime.ToString());
        foreach (var item in objects)
        {
            //double deltaTime = timer.Interval / 1000.0;
            
            item.Location = item.Location + (item.Velocity * deltaTime);
            

        }
        
        for (int i = 0; i < objects.Count; i++)
        {
            for (int j = i+1; j < objects.Count; j++)
            {
                if (objects[j].IsStatic) continue;
                bool isIntersected = Object2D.IsIntersected(objects[i], objects[j], out IntersectionData data);
                if (!isIntersected) continue;
                var velocity = data.b.Velocity;
                data = Object2D.FindCollisionNormal(data, data.lpdData.Point-(data.b.Velocity*deltaTime*2), 15);
                Object2D.ResolveCollision(data);
            }
            if (!objects[i].IsStatic) objects[i].Velocity += new Vec2(0, -Gravity * deltaTime);

            if (objects[i] is Box2D b && (b.Location.Y + b.Size.Y > Height || b.Location.Y < 0))
            {
                b.Location = new Vec2(b.Location.X, (b.Location.Y < 0 ? 0 : Height - b.Size.Y));
                b.Velocity -= new Vec2(b.Velocity.X * b.Friction * deltaTime, b.Velocity.Y * b.Restitution);

            }
        }
        
        Updated?.Invoke(this, new UpdateEventArgs(deltaTime));
    }

    public Object2D this[int index] => objects[index]; 

    public void Run()
    {
        if (isRunning) return;
        lastRecordedTicks = 0;
        sw.Restart();
        isRunning = true;
        updateTask = new Task(ExecuteUpdates);
        
        updateTask.Start();
        //timer.Start();
    }

    public async void Stop()
    {
        if (!isRunning) return;
        sw.Stop();
        isRunning = false;
        await updateTask;
        //timer.Stop();
    }

    public void AddObject2D(Object2D o)
    {
        objects.Add(o);
    }
}
