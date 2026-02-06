namespace NorthernVillage;

/// <summary>
/// Основной класс приложения, отвечающий за запуск программы.
/// </summary>
internal static class Program
{
    /// <summary>
    /// Главная точка входа для приложения, с которой начинается выполнение всей логики.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}