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
        private const int MinimumValueForXСoordinate = 0;
        private const int MinimumPossibleWidth = 1;
        private const int RandomRangeUpperBoundOffset = 1;

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
            BackgroundImage = NorthernVillage.Resources.villageSNOW;
            BackgroundImageLayout = ImageLayout.Stretch;
            snowflakeImage = NorthernVillage.Resources.snowFlake;

            flakes = new List<SnowFlake>(FlakesCount);

            var minSize = MinSnowFlakeSize;
            var maxSize = MaxSnowflakeSize;
            for (var cycleCounter = 0; cycleCounter < FlakesCount; cycleCounter++)
            {
                var sizeVal = rnd.Next(minSize, maxSize + RandomRangeUpperBoundOffset);
                var flakeSize = new Size(sizeVal, sizeVal);
                var fallSpeed = Math.Max(MinFallSpeed, sizeVal / SpeedDivider);
                var x = rnd.Next(MinimumValueForXСoordinate, Math.Max(MinimumPossibleWidth, ClientSize.Width - flakeSize.Width));
                var y = rnd.Next(-ClientSize.Height, ClientSize.Height);
                flakes.Add(new SnowFlake(new Point(x, y), flakeSize, fallSpeed));
            }

            timer = new System.Windows.Forms.Timer();
            timer.Interval = TimerIntervalMs;
            timer.Tick += TimerTick;
            timer.Start();
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
            for (var cycleCounter = 0; cycleCounter < flakes.Count; cycleCounter++)
            {
                flakes[cycleCounter].UpdatePosition(formHeight);
                if (flakes[cycleCounter].Position.Y <= -flakes[cycleCounter].Size.Height + flakes[cycleCounter].FallSpeed &&
                    flakes[cycleCounter].Position.Y >= -flakes[cycleCounter].Size.Height)
                {
                    Point currentPosition = flakes[cycleCounter].Position;
                    currentPosition.X = rnd.Next(0, formWidth);
                    flakes[cycleCounter].Position = currentPosition;
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

        /// <summary>
        /// Инициализация закрытия формы.
        /// </summary>
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            Close();
        }
    }
}