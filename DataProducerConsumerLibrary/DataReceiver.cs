using System;
using System.Diagnostics;
using System.Threading;
using System.Drawing;
using System.Media;
using RGB_ProducerConsumer;

namespace DataProducerConsumerLibrary
{
    public class DataReceiver
    {
        private CommonData _data;
        private Thread? _thread;
        private bool _hasData = false;
        private Color _color;
        private SoundManager _soundManager = new("C:/Users/danil/source/repos/RGB_ProducerConsumer/Properties/fart.wav", 0.5f);

        public DataReceiver(CommonData data)
        {
            this._data = data;
        }

        public class DataReceivedEventArgs
        {
            public Color Color { get; set; }
        }

        public event EventHandler<DataReceivedEventArgs> DataReceived;

        protected virtual void OnDataReceived(DataReceivedEventArgs e)
        {
            EventHandler<DataReceivedEventArgs> handler = DataReceived;
            handler?.Invoke(this, e);
        }

        private void Boom()
        {
            _soundManager.Play();
        }

        public void Start()
        {
            if (!(_thread?.IsAlive ?? false))
            {
                _thread = new Thread(() =>
                {
                    while (true)
                    {
                        var fullData = _data.GetData();
                        var resultColor = Color.FromArgb(fullData[0], fullData[1], fullData[2]);
                        this._color = resultColor;
                        this._data.Animator.ClearBalls();
                        this.OnDataReceived(new DataReceivedEventArgs { Color = resultColor });
                        Boom();
                    }
                });
                _thread.IsBackground = true;
                _thread.Start();
            }
        }
    }
}