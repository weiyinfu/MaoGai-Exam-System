using System;
using System.Windows.Forms;
using System.Drawing;
class Recite : Form
{
    int chapter;
    int q;
    bool single = true;
    Random rand = new Random();
    Book book = Window.book;

    Label chapterLabel = new Label();
    Label questionLabel = new Label();
    Label singleLabel = new Label();
    Label question = new Label();
    bool[] multiAnswer = new bool[4];

    public Recite()
    {
        Text = "背题模式---按键盘任意键或者鼠标点击空白处";

        MouseDown += delegate
        {
            this.nextQuestion();
            showQuestion();
        };
        KeyDown += delegate
        {
            this.nextQuestion();
            showQuestion();
        }; 

        chapterLabel.Location = new Point(950, 40);
        chapterLabel.Size = new Size(200, 40);
        singleLabel.Location = new Point(950, 80);
        questionLabel.Location = new Point(950, 120);
        question.Location = new Point(40, 40);
        question.Size = new Size(900, 200);

        question.Font = new Font("Consolas", 15, FontStyle.Bold);

        MenuStrip menu = new MenuStrip();
        ToolStripMenuItem choose = new ToolStripMenuItem("章节选择");
        ToolStripMenuItem type = new ToolStripMenuItem("题型选择"); 
        ToolStripMenuItem nextQuestion = new ToolStripMenuItem("下一题", null, delegate { this.nextQuestion(); showQuestion(); });
        ToolStripMenuItem lastQuestion = new ToolStripMenuItem("上一题", null, delegate { this.lastQuestion(); showQuestion(); });

        for (int i = 1; i < 13; i++)
        {
            ToolStripMenuItem item = new ToolStripMenuItem("第" + i + "章 " + book.chapter[i - 1].name, null, chooseChapter);
            choose.DropDownItems.Add(item);
        }

        var multi = new ToolStripMenuItem("多选题", null, chooseType);
        var single = new ToolStripMenuItem("单选题", null, chooseType);
        type.DropDownItems.Add(single);
        type.DropDownItems.Add(multi);

        menu.Items.Add(choose);
        menu.Items.Add(type); 
        menu.Items.Add(lastQuestion);
        menu.Items.Add(nextQuestion);

        Controls.Add(menu);
        Controls.Add(chapterLabel);
        Controls.Add(questionLabel);
        Controls.Add(singleLabel);
        Controls.Add(question);

        showQuestion();
    }
    //keyDown 不管用，不知为什么
    void keyDown(object o,KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.Up:
            case Keys.Left: lastQuestion(); break;
            default: nextQuestion(); break;
        }
        showQuestion();
    }
    void chooseChapter(object o, EventArgs e)
    {
        string s = o.ToString();
        s = s.Substring(1, s.IndexOf('章') - 1);
        int num = Convert.ToInt32(s);
        chapter = num - 1;
        q = 0;
        showQuestion();
    }
    void chooseType(object o, EventArgs e)
    {
        if (o.ToString() == "单选题")
            single = true;
        else
            single = false;
        nextQuestion();
        showQuestion();
    }
    void showQuestion()
    {
        for (int i = Controls.Count - 1; i >= 0; i--)
        {
            if (Controls[i] is Choice)
            {
                Controls[i].Dispose();
            }
        }
        chapterLabel.Text = "第" + (chapter + 1) + "章 " + book.chapter[chapter].name;
        questionLabel.Text = q + 1 + "";
        if (single)
        {
            singleLabel.Text = "单选题";
            question.Text = book.chapter[chapter].single[q].name;
            for (int i = 0; i < 4; i++)
            {
                var cho = new Choice(i, (char)(i + 'A') + ". " + book.chapter[chapter].single[q].choice[i], new Rectangle(40, 100 * i + 240, 900, 100), null);
                if (i == book.chapter[chapter].single[q].ans)
                    cho.chosen = true;
                Controls.Add(cho);
            }
        }
        else
        {
            singleLabel.Text = "多选题";
            question.Text = book.chapter[chapter].multi[q].name;
            for (int i = 0; i < 4; i++)
            {
                var cho = new Choice(i, (char)(i + 'A') + ". " + book.chapter[chapter].multi[q].choice[i], new Rectangle(40, 100 * i + 240, 900, 100), null);
                if (book.chapter[chapter].multi[q].ans[i])
                    cho.chosen = true;
                Controls.Add(cho);
            }
        }
    } 
    void lastQuestion()
    {
        if (single)
        {
            if (q == 0)
            {
                chapter--;
                if (chapter == -1) chapter = book.length - 1;
                q = book.chapter[chapter].singleSize - 1;
            }
            else q--;
        }
        else
        {
            if (q == 0)
            {
                chapter--;
                if (chapter == -1) chapter = book.length - 1;
                q = book.chapter[chapter].singleSize - 1;
            }
            else q--;
        }
    }
    void nextQuestion()
    {
        if (single)
        {
            if (q == book.chapter[chapter].singleSize - 1)
            {
                chapter++;
                if (chapter == book.length) chapter = 0;
                q = 0;
            }
            else q++;
        }
        else
        {
            for (int i = 0; i < 4; i++)
                multiAnswer[i] = false;
            if (q == book.chapter[chapter].multiSize - 1)
            {
                chapter++;
                if (chapter == book.length) chapter = 0;
                q = 0;
            }
            else q++;
        }
    }
}