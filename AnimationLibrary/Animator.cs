using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace animationTry2
{
    public class Animator : IDisposable
    {
        private Graphics _graphics;
        private List<Ball> _balls = new();
        private Size _containerSize;
        private bool _isAnimating = true;
        private BufferedGraphics _bg;
        private object _obj = new();
        private Thread _thread;
        private const int Max = 3;
        private bool _resized = false;

        public List<Ball> Balls => _balls;
        public static Color BackColor { get; set; }
        public Rectangle TrapRectangle { get; set; }

        public Animator(Graphics g, Rectangle rectangle)
        {
            Update(g, rectangle);
        }

        public void Update(Graphics g, Rectangle rectangle) //todo: delete black blinks
        {
            this._resized = true;
            this._graphics = g;
            this._containerSize = new Size(rectangle.Width, rectangle.Height);

            lock (_graphics)
                this._bg = BufferedGraphicsManager.Current.Allocate(this._graphics,
                    new Rectangle(0, 0, _containerSize.Width, _containerSize.Height));


            lock (_balls)
                foreach (var b in _balls)
                    b.Update(rectangle);
        }


        public void AddBallAndStart(Ball ball)
        {
            if (_thread == null || !_thread.IsAlive)
            {
                _isAnimating = true;
                _thread = new(Animate);
                _thread.IsBackground = true;
                _thread.Start();
            }

            if (_balls.Count() <= Max)
            {
                this._balls.Add(ball);
                ball.Start();
            }
        }

        public void Animate()
        {
            while (_isAnimating)
            {
                lock (_balls)
                {
                    for (int i = 0; i < _balls.Count; i++)
                    {
                        var b = _balls[i];
                        var center = new PointF(b.Origin.X + b.Radius, b.Origin.Y + b.Radius);
                        if (TrapRectangle.Contains(Rectangle.Ceiling(
                                new RectangleF(b.Origin.X, b.Origin.Y, b.Radius * 2, b.Radius * 2))))
                        {
                            b.vX = b.vY = 0;
                            b.IsAlive = false;
                        }
                    }
                }

                bool isStop = true;
                    foreach (var b in _balls)
                        if (b.vX != 0 || b.vY != 0)
                        {
                            isStop = false;
                            break;
                        }

                    
                if (isStop && _balls.Count == 3)
                {
                    _isAnimating = false;
                    return;
                }

                Monitor.Enter(_obj);
                Graphics g = _bg.Graphics;
                Monitor.Exit(_obj);

                g.Clear(BackColor);
                g.DrawRectangle(new Pen(Color.Black), TrapRectangle);
                g.FillRectangle(new SolidBrush(Color.White), TrapRectangle);

                lock (_balls)
                    for (int i = 0; i < _balls.Count(); i++)
                        _balls[i].Paint(g);

                lock (_bg)
                {
                    lock (_graphics)
                        _bg.Render();
                }


                Thread.Sleep(5);
            }
        }


        public event EventHandler BallDied;

        protected virtual void OnBallDied(EventArgs e)
        {
            EventHandler handler = BallDied;
            handler?.Invoke(this, e);
        }

        public void ClearBalls()
        {
            this._balls.Clear();
        }

        public void Dispose()
        {
            this._graphics?.Dispose();
            this._bg?.Dispose();
            this._thread?.Abort();
        }
    }
}