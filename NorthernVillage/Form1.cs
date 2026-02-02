using System.Drawing.Drawing2D;
namespace NorthernVillage;
using System.Windows.Forms;
public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
        this.Paint += Form1_Paint;
    }
    private void Form1_Paint(object sender, PaintEventArgs e)
    {
        e.Graphics.DrawImage(NorthernVillage.Resources1.villageSNOW, new Rectangle(0, 0, this.ClientSize.Width, this.ClientSize.Height)
    );

    }
    private void button1_Click(object sender, EventArgs e)
    {
        var dc = CreateGraphics();
        dc.FillEllipse(Brushes.Blue, 100, 100, 100, 100);
    }
}