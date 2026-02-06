namespace NorthernVillage
{
    public partial class MainForm : Form
    {
        //Маг числа
        private const int MinSnowFlakeSize = 8;
        private const int MaxSnowflakeSize = 50;
        private const int TimerIntervalMs = 40;
        private const int SpeedDivider = 8;
        private const int MinFallSpeed = 1;
        private const int FlakesCount = 150;

        private readonly List<SnowFlake> _flakes;
        private readonly System.Windows.Forms.Timer _timer;
        private readonly Image _snowflakeImage;
        private readonly Random _rnd = new Random();

        public MainForm()
        {
            InitializeComponent();
            BackgroundImage = NorthernVillage.Resources1.villageSNOW;
            BackgroundImageLayout = ImageLayout.Stretch;
            _snowflakeImage = NorthernVillage.Resources1.snowFlake;

            _flakes = new List<SnowFlake>(FlakesCount);

            var minSize = MinSnowFlakeSize;
            var maxSize = MaxSnowflakeSize;
            for (var i = 0; i < FlakesCount; i++)
            {
                var sizeVal = _rnd.Next(minSize, maxSize + 1);
                var flakeSize = new Size(sizeVal, sizeVal);
                var fallSpeed = Math.Max(MinFallSpeed, sizeVal / SpeedDivider);
                var x = _rnd.Next(0, Math.Max(1, ClientSize.Width - flakeSize.Width));
                var y = _rnd.Next(-ClientSize.Height, ClientSize.Height);
                _flakes.Add(new SnowFlake(new Point(x, y), flakeSize, fallSpeed));
            }

            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = TimerIntervalMs;
            _timer.Tick += Timer_Tick;
            _timer.Start();

            KeyPreview = true;
            KeyDown += MainForm_KeyDown;
        }

        private void RenderScene(Graphics g)
        {
            if (BackgroundImage != null)
            {
                g.DrawImage(BackgroundImage, ClientRectangle);
            }

            foreach (var flake in _flakes)
            {
                flake.Draw(g, _snowflakeImage);
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            var formHeight = ClientRectangle.Height;
            var formWidth = ClientRectangle.Width;
            for (var i = 0; i < _flakes.Count; i++)
            {
                _flakes[i].UpdatePosition(formHeight);
                if (_flakes[i].Position.Y <= -_flakes[i].Size.Height + _flakes[i].FallSpeed &&
                    _flakes[i].Position.Y >= -_flakes[i].Size.Height)
                {
                    Point currentPosition = _flakes[i].Position;
                    currentPosition.X = _rnd.Next(0, formWidth);
                    _flakes[i].Position = currentPosition;
                }
            }

            using (Graphics g = CreateGraphics())
            {
                using (BufferedGraphics buffer = BufferedGraphicsManager.Current.Allocate(g, ClientRectangle))
                {
                    RenderScene(buffer.Graphics);
                    buffer.Render(g);
                }
            }
        }

        private void MainForm_KeyDown(object? sender, KeyEventArgs e)
        {
            Close();
        }
    }
}