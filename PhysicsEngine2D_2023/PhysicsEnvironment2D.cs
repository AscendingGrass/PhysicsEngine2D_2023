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

    public PhysicsEnvironment2D(double width=100, double height=100, double gravity = 100)
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
            if(!item.IsStatic) item.Velocity += new Vec2(0, -Gravity * deltaTime);
            var temp = item.Location + (item.Velocity * deltaTime);
            if (item is Box2D b && (temp.Y + b.Size.Y > Height || temp.Y < 0))
            {
                item.Location = new Vec2(item.Location.X + (item.Velocity.X * deltaTime), (temp.Y < 0? 0 :  Height - b.Size.Y));
                item.Velocity -=  new Vec2(item.Velocity.X * item.Friction * deltaTime,  -item.Velocity.Y * item.Restitution);
                continue;
            }
            item.Location = temp;
        }
        
        for (int i = 0; i < objects.Count; i++)
        {
            for (int j = i+1; j < objects.Count; j++)
            {
                bool isIntersected = Object2D.IsIntersected(objects[i], objects[j], out IntersectionData data);
                if (isIntersected)
                    Object2D.ResolveCollision(data);
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
