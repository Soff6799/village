namespace NorthernVillage
{
    internal class SnowFlake
    {
        /// <summary>
        /// Позиция снежинки на экране.
        /// </summary>
        public Point Position; 
        
        /// <summary>
        /// Размер снежинки.
        /// </summary>
        public Size Size;
        
        /// <summary>
        /// Скорость даления (пикселей за один такт таймера).
        /// </summary>
        public readonly int FallSpeed;

        public SnowFlake(Point initialPosition, Size size, int fallSpeed)
        {
            Position = initialPosition;
            Size = size;
            FallSpeed = fallSpeed;
        }
        
        public void UpdatePosition(int formHeight)
        {
            Position = new Point(Position.X, Position.Y + FallSpeed);
            if (Position.Y > formHeight)
            {
                Position = new Point(Position.X, -Size.Height);
            }
        }

        public void Draw(Graphics g, Image snowflakeImage)
        { 
            g.DrawImage(snowflakeImage, new Rectangle(Position.X, Position.Y, Size.Width, Size.Height));
        }
    }

    public partial class Form1 : Form
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
        
        public Form1()
        {
            InitializeComponent();
            this.BackgroundImage = NorthernVillage.Resources1.villageSNOW;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            _snowflakeImage = NorthernVillage.Resources1.snowFlake;
            
            _flakes = new List<SnowFlake>(FlakesCount);
            
            var minSize = MinSnowFlakeSize;
            var maxSize = MaxSnowflakeSize;
            for (var i = 0; i < FlakesCount; i++)
            {
                var sizeVal = _rnd.Next(minSize, maxSize+1);
                var flakeSize = new Size(sizeVal, sizeVal);
                var fallSpeed = Math.Max(MinFallSpeed, sizeVal / SpeedDivider);
                var x = _rnd.Next(0, Math.Max(1, this.ClientSize.Width - flakeSize.Width));
                var y = _rnd.Next(-this.ClientSize.Height, this.ClientSize.Height);
                _flakes.Add(new SnowFlake(new Point(x, y), flakeSize, fallSpeed));
            }
            
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = TimerIntervalMs;
            _timer.Tick += Timer_Tick;
            _timer.Start();

            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;
        }

        private void RenderScene(Graphics g)
        {
            if (this.BackgroundImage != null)
            {
                g.DrawImage(this.BackgroundImage, this.ClientRectangle);
            }

            foreach (var flake in _flakes)
            {
                flake.Draw(g, _snowflakeImage);
            }
        }

        private void Timer_Tick(object? sender, EventArgs e) 
        {
            var formHeight = this.ClientRectangle.Height; 
            var formWidth = this.ClientRectangle.Width;
            for (var i = 0; i < _flakes.Count; i++)
            {
                _flakes[i].UpdatePosition(formHeight);
                if (_flakes[i].Position.Y <= -_flakes[i].Size.Height + _flakes[i].FallSpeed && _flakes[i].Position.Y >= -_flakes[i].Size.Height)
                {
                    _flakes[i].Position.X = _rnd.Next(0, formWidth);
                }
            }
            using (Graphics g = this.CreateGraphics())
            {
                using (BufferedGraphics buffer = BufferedGraphicsManager.Current.Allocate(g, this.ClientRectangle))
                {
                    RenderScene(buffer.Graphics);
                    buffer.Render(g);
                }
            }
        }

        private void Form1_KeyDown(object? sender, KeyEventArgs e)
        {
            this.Close();
        }
    }
}