using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DataProducerConsumerLibrary;

namespace RGB_ProducerConsumer
{
    public partial class Form1 : Form
    {
        private CommonData _data = new CommonData();
        private DataProducer _producerRed, _producerGreen, _producerBlue;
        private DataReceiver _receiver;
        private Graphics _graphics;
        private Rectangle _trapRectangle;
        private SoundManager _soundManager = new("C:/Users/danil/source/repos/RGB_ProducerConsumer/Properties/debussy.wav");

        public Form1()
        {
            InitializeComponent();
            
            _soundManager.Play();

            //todo: may issue bugs
            // this.SetStyle(ControlStyles.UserPaint, true);
            // this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            // this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            DataProducer.BackColor = this.panel1.BackColor;
            
            this._graphics = panel1.CreateGraphics();
            this._receiver = new DataReceiver(_data);
            this._receiver.DataReceived += r_DataReceived;
            this._receiver.Start();

            int edgeWidth = 350;
            var loc = new Point(panel1.Location.X + panel1.Width / 2 - edgeWidth / 2,
                panel1.Location.Y + panel1.Height / 2 - edgeWidth / 2);
            this._trapRectangle = new Rectangle(loc.X, loc.Y, edgeWidth, edgeWidth);
        }

        private void r_DataReceived(object sender, DataReceiver.DataReceivedEventArgs e)
        {
            new Thread((s) => //todo: understand properly
            {
                this.panel1.BeginInvoke((MethodInvoker)(() => this.panel1.BackColor = e.Color));
            }).Start();

            this._producerRed = this._producerBlue = this._producerGreen = null;

            // this.panel1.Refresh();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            DataProducer.BackColor = this.panel1.BackColor;
            
            if (_producerRed is null)
            {
                _producerRed = new DataProducer(DataType.Red, _data, e.Location,
                    panel1.ClientRectangle, _trapRectangle, _graphics);
                _producerRed.Start();
                return;
            }


            if (_producerGreen is null)
            {
                _producerGreen = new DataProducer(DataType.Green, _data, e.Location,
                    panel1.ClientRectangle, _trapRectangle, _graphics);
                _producerGreen.Start();
                return;
            }


            if (_producerBlue is null)
            {
                _producerBlue = new DataProducer(DataType.Blue, _data, e.Location,
                    panel1.ClientRectangle, _trapRectangle, _graphics);
                _producerBlue.Start();
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            //this._graphics = e.Graphics;
            _graphics.DrawRectangle(new Pen(Color.Black, 2), _trapRectangle);
            _graphics.FillRectangle(new SolidBrush(Color.White), _trapRectangle);
        }
    }
}