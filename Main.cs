using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
class Window : Form
{
    static void Main(string[] args)
    {
        Window win = new Window();
        Application.Run(win);
    }
    public static Book book = new Book();
    MenuStrip menu = new MenuStrip();
    ToolStripMenuItem mode = new ToolStripMenuItem("模式");

    void chooseMode(object o, EventArgs e)
    {
        foreach (Form f in MdiChildren)
            f.Close();
        Form form = null;
        switch (o.ToString())
        {
            case "随机刷题":
                form = new RandomPractise();
                break;
            case "按序刷题":
                form = new OrderPractise();
                break;
            case "背题模式":
                form = new Recite();
                break;
            case "模拟考试":
                form = new Exam();
                break;
        } 
        form.MdiParent = this;
        form.Size = MaximumSize;
        form.WindowState = FormWindowState.Maximized;
        form.Show();
    }
    public Window()
    {
        Text = "毛概考试系统--made by weidiao.neu";
        WindowState = FormWindowState.Maximized;

        IsMdiContainer = true;

        ToolStripMenuItem randomTest = new ToolStripMenuItem("随机刷题", null, chooseMode);
        ToolStripMenuItem orderPractise = new ToolStripMenuItem("按序刷题", null, chooseMode);
        ToolStripMenuItem recite = new ToolStripMenuItem("背题模式", null, chooseMode);
        ToolStripMenuItem exam = new ToolStripMenuItem("模拟考试", null, chooseMode);
        mode.DropDownItems.AddRange(new ToolStripItem[] { randomTest, orderPractise, recite, exam });

        menu.Items.Add(mode);
        this.Controls.Add(menu);
    }
}