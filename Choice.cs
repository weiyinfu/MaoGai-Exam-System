using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing; 
class Choice : Label
{
    int Num;
    public int num
    {
        get
        {
            return Num;
        }
        set
        {
            Num = value;
        }
    }
    bool Chosen;
    public bool chosen
    {
        get
        {
            return Chosen;
        }
        set 
        {
            Chosen = value;
            Invalidate();
        }
    }
    public Choice(int num, string s, Rectangle rec,MouseEventHandler click)
    {
        this.num = num;
        this.Location = new Point(rec.Left, rec.Top);
        this.Size = new Size(rec.Width, rec.Height);
        this.Font = new Font("Consolas", 15);
        this.Text = s;

        MouseClick += click;
    }
    protected override void OnClick(EventArgs e)
    {
        if (chosen) chosen = false;
        else chosen = true;
    }
    protected override void OnMouseHover(EventArgs e)
    {
        CreateGraphics().DrawRectangle(new Pen(Color.Tomato, 3), new Rectangle(5, 5, this.Width - 10, this.Height - 10));
    }
    protected override void OnPaint(PaintEventArgs e)
    {
        if (chosen)
            e.Graphics.DrawRectangle(new Pen(Color.Tomato, 3), new Rectangle(5, 5, this.Width - 10, this.Height - 10));
        e.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), new Rectangle(10,10,this.Width-20,this.Height-20));
    }
    protected override void OnMouseLeave(EventArgs e)
    {
        if (chosen) return;
        chosen = false;
    }
}