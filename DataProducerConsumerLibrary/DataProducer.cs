using System;
using System.Drawing;
using System.Threading;
using animationTry2;

namespace DataProducerConsumerLibrary
{
    public enum DataType
    {
        Red,
        Green,
        Blue
    }

    public class DataProducer
    {
        private Thread? _thread;
        private Random _rand = new();
        private CommonData _data;
        private Point _ballPosition;
        private Rectangle _containerRectangle;
        
        private Rectangle _trapRectangle;

        //private Animator _animator;
        private Graphics _graphics;
        public DataType Type { get; }
        public static Color BackColor { get; set; }
        
        public DataProducer(DataType type, CommonData data, Point pos, Rectangle rect, Rectangle trap, Graphics g)
        {
            Type = type;
            this._data = data;
            this._ballPosition = pos;
            this._containerRectangle = rect;
            this._trapRectangle = trap;
            this._graphics = g;
        }

        public void Start()
        {
            if (!(_thread?.IsAlive ?? false))
            {
                _thread = new Thread(() =>
                {
                    Animator.BackColor = BackColor;
                    int colorComponent = _rand.Next(255);

                    Color color = new();
                    if (this.Type == DataType.Red) color = Color.FromArgb(colorComponent, 0, 0);
                    if (this.Type == DataType.Green) color = Color.FromArgb(0, colorComponent, 0);
                    if (this.Type == DataType.Blue) color = Color.FromArgb(0, 0, colorComponent);

                    var ball = new Ball(_ballPosition, _containerRectangle, color);

                    if (this._data.Animator is null)
                        this._data.Animator = new Animator(_graphics, _containerRectangle)
                            { TrapRectangle = _trapRectangle};
                    this._data.Animator.AddBallAndStart(ball);
                    
                    this._data.PushData(colorComponent,this.Type);
                });
                _thread.IsBackground = true;
                _thread.Start();
            }
        }
    }
}