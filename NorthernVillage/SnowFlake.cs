namespace NorthernVillage;

/// <summary>
/// Представляет одну снежинку на экране с её позицией, размером и скоростью падения.
/// </summary>
public class SnowFlake
{
    /// <summary>
    /// Получает или устанавливает позицию снежинки на экране.
    /// </summary>
    public Point Position { get; set; }

    /// <summary>
    /// Получает размер снежинки.
    /// </summary>
    public Size Size { get; }

    /// <summary>
    /// Получает скорость падения снежинки (пикселей за один такт таймера).
    /// </summary>
    public int FallSpeed { get; }

    public SnowFlake(Point initialPosition, Size size, int fallSpeed)
    {
        Position = initialPosition;
        Size = size;
        FallSpeed = fallSpeed;
    }
    
    /// <summary>
    /// Обновляет позицию снежинки при попадании за границу экрана
    /// </summary>
    /// <param name="formHeight">Высота формы для определения нижней границы.</param>
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