using System;
using System.Drawing;
using System.Threading;

namespace animationTry2
{
    public class Ball
    {
        public Color Color { get; set; }
        public float Radius { get; set; }
        public float vX { get; set; }
        public float vY { get; set; }
        public PointF Origin;
        public bool IsAlive;
        public Rectangle ContainerRect => _containerRect;
        public Size ContainerSize => _containerSize;


        private Rectangle _containerRect;
        private Size _containerSize;
        private Thread _thread;
        private Random _rand = new Random();

        public Ball(PointF origin, Rectangle rectangle, Color color)
        {
            this.Origin = origin;
            this._containerRect = rectangle;
            this._containerSize = new Size(rectangle.Width, rectangle.Height);
            this.Color = color;

            this.Radius = _rand.Next(5, 50);
            this.vX = _rand.Next(-20, 20);
            this.vY = _rand.Next(-20, 20);
            this.IsAlive = true;
        }

        public void Start()
        {
            while (Move())
                Thread.Sleep(5);
        }

        public void Paint(Graphics g)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //g.DrawEllipse(new Pen(Color.White, 2), Origin.X, Origin.Y, Radius * 2, Radius * 2);
            g.FillEllipse(new SolidBrush(Color), Origin.X, Origin.Y, Radius * 2, Radius * 2);
        }

        public void Update(Rectangle rectangle)
        {
            this._containerRect = rectangle;
            this._containerSize = new Size(rectangle.Width, rectangle.Height);
        }
        
        public bool Move()
        {
            if (this.IsAlive)
                return Physics.MoveBall(this);
            return false;
        }
        
    }
}