using System;
using System.Windows.Forms;
using System.Drawing;
class Node
{
    public int chapter;
    public int q;
    public bool single;
    public Node(int cha, int qq, bool type)
    {
        chapter = cha;
        q = qq;
        single = type;
    }
}
class RandomPractise : Form
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
    Node[] queue = new Node[100];
    int qSize = 0;

    public RandomPractise()
    {
        Text = "随机练习";

        chapterLabel.Location = new Point(950, 40);
        chapterLabel.Size = new Size(200, 40);
        singleLabel.Location = new Point(950, 80);
        questionLabel.Location = new Point(950, 120);
        question.Location = new Point(40, 40);
        question.Size = new Size(900, 200);

        question.Font = new Font("Consolas", 15, FontStyle.Bold);

        MenuStrip menu = new MenuStrip();
        ToolStripMenuItem type = new ToolStripMenuItem("题型选择");
        ToolStripMenuItem seeAns = new ToolStripMenuItem("看答案", null, this.seeAns);
        ToolStripMenuItem nextQuestion = new ToolStripMenuItem("下一题", null, delegate { this.nextQuestion(); showQuestion(); });
        ToolStripMenuItem lastQuestion = new ToolStripMenuItem("上一题", null, delegate { this.lastQuestion(); showQuestion(); });

        var multi = new ToolStripMenuItem("多选题", null, chooseType);
        var single = new ToolStripMenuItem("单选题", null, chooseType);
        type.DropDownItems.Add(single);
        type.DropDownItems.Add(multi);

        menu.Items.Add(type);
        menu.Items.Add(seeAns);
        menu.Items.Add(lastQuestion);
        menu.Items.Add(nextQuestion);

        Controls.Add(menu);
        Controls.Add(chapterLabel);
        Controls.Add(questionLabel);
        Controls.Add(singleLabel);
        Controls.Add(question);

        showQuestion();
    }
    void seeAns(object o, EventArgs e)
    {
        if (single)
        {
            foreach (Control c in Controls)
            {
                if (c is Choice)
                {
                    Choice cho = c as Choice;
                    if (cho.num == book.chapter[chapter].single[q].ans)
                    {
                        cho.chosen = true;
                        return;
                    }
                }
            }
        }
        else
        {
            foreach (Control c in Controls)
            {
                if (c is Choice)
                {
                    Choice cho = c as Choice;
                    if (book.chapter[chapter].multi[q].ans[cho.num])
                    { 
                        cho.chosen = true;
                        cho.Invalidate();
                    }
                }
            }
        }
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
        chapterLabel.Text = "第" + (1+chapter) + "章 " +book.chapter[chapter].name;
        questionLabel.Text = (1+q)+"";
        if (single)
        {
            singleLabel.Text = "单选题";
            question.Text = book.chapter[chapter].single[q].name;
            for (int i = 0; i < 4; i++)
            {
                var cho = new Choice(i, (char)(i + 'A') + ". " + book.chapter[chapter].single[q].choice[i], new Rectangle(40, 100 * i + 240, 900, 100), singleClick);
                Controls.Add(cho);
            }
        }
        else
        {
            singleLabel.Text = "多选题";
            question.Text = book.chapter[chapter].multi[q].name;
            for (int i = 0; i < 4; i++)
            {
                var cho = new Choice(i, (char)(i + 'A') + ". " + book.chapter[chapter].multi[q].choice[i], new Rectangle(40, 100 * i + 240, 900, 100), multiClick);
                Controls.Add(cho);
            }
        }
    }
    void singleClick(object o, EventArgs e)
    {
        Choice cho = o as Choice;
        if (cho.num == book.chapter[chapter].single[q].ans)
        {
            nextQuestion();
            showQuestion();
        }
        else
        {
            cho.chosen = false;
        }
    }
    void multiClick(object o, EventArgs e)
    {
        Choice cho = o as Choice;
        if (book.chapter[chapter].multi[q].ans[cho.num])
        {
            multiAnswer[cho.num] = true;
            for (int i = 0; i < 4; i++)
            {
                if (multiAnswer[i] != book.chapter[chapter].multi[q].ans[i])
                    return;
            }
        }
        else
        {
            cho.chosen = false;
        }
    }
    void lastQuestion()
    {
        queue[qSize] = null;
        qSize=(qSize + 98) % 99;
        Node node = queue[qSize];
        if (node == null) return;
        chapter = node.chapter;
        single = node.single;
        q = node.q;
    }
    void nextQuestion()
    {
        chapter = rand.Next (0, book.length-1);
        if (single)
        {
            q = rand.Next(0,book.chapter[chapter].singleSize-1);
        }
        else
        {
            q = rand.Next(0, book.chapter[chapter].multiSize - 1);
        }
        qSize = (qSize + 1) % 99;
        queue[qSize] = new Node(chapter, q, single);
    }
}