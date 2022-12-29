using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timer = System.Windows.Forms.Timer;

namespace PhysicsEngine2D_2023;

public class PhysicsEnvironment2D
{
    public double Width  { get;}
    public double Height { get;}
    public double Gravity { get; set; }

    public int CountObjects => objects.Count;

    private bool isRunning;
    private Timer timer = new Timer() {Interval = 10};

    public bool IsRunning
    {
        get { return isRunning; }
    }

    private readonly List<Object2D> objects = new List<Object2D>();

    public event EventHandler? Updated;

    public PhysicsEnvironment2D(double width, double height, double gravity = 10)
    {
        Width = width;
        Height = height;
        Gravity = gravity;

        isRunning = false;
        timer.Tick += Update;
    }

    private void Update(object? sender, EventArgs e)
    {
        foreach (var item in objects)
        {
            item.Velocity += new Vec2(0, Gravity * timer.Interval/1000);
            item.Location += item.Velocity;
        }

        for (int i = 0; i < objects.Count; i++)
        {
            for (int j = i+1; j < objects.Count; j++)
            {
                if (Object2D.IsIntersected(objects[i], objects[j]))
                    Object2D.ResolveCollision(objects[i], objects[j]);
            }
        }

        Updated?.Invoke(this,EventArgs.Empty);
    }

    public Object2D this[int index] => objects[index]; 

    public void Run()
    {
        if (isRunning) return;
        isRunning = true;
        timer.Start();
    }

    public void Stop()
    {
        if (!isRunning) return;
        isRunning = false;
        timer.Stop();
    }

    public void AddObject2D(Object2D o)
    {
        foreach (var item in objects)
        {
            if(Object2D.IsIntersected(item,o)) Object2D.ResolveCollision(item,o);
        }
        objects.Add(o);
    }
}
