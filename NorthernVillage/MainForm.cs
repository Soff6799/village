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

        private readonly List<SnowFlake> flakes;
        private readonly System.Windows.Forms.Timer timer;
        private readonly Image snowflakeImage;
        private readonly Random rnd = new();

        /// <summary>
        /// инициализирует новый экземпляр класса MainForm и настраивает логику со снежинками.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            BackgroundImage = NorthernVillage.Resources1.villageSNOW;
            BackgroundImageLayout = ImageLayout.Stretch;
            snowflakeImage = NorthernVillage.Resources1.snowFlake;

            flakes = new List<SnowFlake>(FlakesCount);

            var minSize = MinSnowFlakeSize;
            var maxSize = MaxSnowflakeSize;
            for (var i = 0; i < FlakesCount; i++)
            {
                var sizeVal = rnd.Next(minSize, maxSize + 1);
                var flakeSize = new Size(sizeVal, sizeVal);
                var fallSpeed = Math.Max(MinFallSpeed, sizeVal / SpeedDivider);
                var x = rnd.Next(0, Math.Max(1, ClientSize.Width - flakeSize.Width));
                var y = rnd.Next(-ClientSize.Height, ClientSize.Height);
                flakes.Add(new SnowFlake(new Point(x, y), flakeSize, fallSpeed));
            }

            timer = new System.Windows.Forms.Timer();
            timer.Interval = TimerIntervalMs;
            timer.Tick += TimerTick;
            timer.Start();

            KeyPreview = true;
            KeyDown += MainFormKeyDown;
        }

        private void RenderScene(Graphics g)
        {
            if (BackgroundImage != null)
            {
                g.DrawImage(BackgroundImage, ClientRectangle);
            }

            foreach (var flake in flakes)
            {
                flake.Draw(g, snowflakeImage);
            }
        }

        private void TimerTick(object? sender, EventArgs e)
        {
            var formHeight = ClientRectangle.Height;
            var formWidth = ClientRectangle.Width;
            for (var i = 0; i < flakes.Count; i++)
            {
                flakes[i].UpdatePosition(formHeight);
                if (flakes[i].Position.Y <= -flakes[i].Size.Height + flakes[i].FallSpeed &&
                    flakes[i].Position.Y >= -flakes[i].Size.Height)
                {
                    Point currentPosition = flakes[i].Position;
                    currentPosition.X = rnd.Next(0, formWidth);
                    flakes[i].Position = currentPosition;
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

        private void MainFormKeyDown(object? sender, KeyEventArgs e)
        {
            Close();
        }
    }
}