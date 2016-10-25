using System;
using System.Windows.Forms;
using System.Drawing;
class Paper
{
    public Single[] single = new Single[30];
    public Multi[] multi = new Multi[10];
    Random rand = new Random();

    public Paper()
    {
        Book book = Window.book;
        for (int i = 0; i < 30; i++)
        {
            int chapter = rand.Next(0, book.length - 1);
            int q = rand.Next(0, book.chapter[chapter].singleSize - 1);
            single[i] = book.chapter[chapter].single[q];
            for (int j = 0; j < i; j++)
            {
                if (single[j] == single[i])
                {
                    i--; break;
                }
            }
        }
        for (int i = 0; i < 10; i++)
        {
            int chapter = rand.Next(0, book.length - 1);
            int q = rand.Next(0, book.chapter[chapter].multiSize - 1);
            multi[i] = book.chapter[chapter].multi[q];
            for (int j = 0; j < i; j++)
            {
                if (multi[j] == multi[i])
                {
                    i--; break;
                }
            }
        }
    }
}
class Unsure
{
    public bool single;
    public int num;
    public override string ToString()
    {
        string type = single ? "[单选]" : "[多选]";
        string name = single ? Exam.paper.single[num].name : Exam.paper.multi[num].name;
        return type + " " + (1 + num) + ". " + name;
    }
    public Unsure(bool single, int num)
    {
        this.num = num;
        this.single = single;
    }
}
class Exam : Form
{
    public static Paper paper;
    bool single = true;
    int q = 0;
    bool[,] multiAns = new bool[10, 4];
    int[] singleAns = new int[30];
    int count = 2400;
    Choice[] question = new Choice[5];
    ListBox singleList = new ListBox();
    ListBox multiList = new ListBox();
    ListBox unSureList = new ListBox();
    Label time = new Label();
    Timer timer = new Timer();

    public Exam()
    {
        paper = new Paper();
        Text = "模拟考试";

        MenuStrip menu = new MenuStrip();
        ToolStripMenuItem handin = new ToolStripMenuItem("交卷", null, delegate { this.handin(); });
        ToolStripMenuItem lastQuestion = new ToolStripMenuItem("上一题", null, delegate { this.lastQuestion(); showQuestion(); });
        ToolStripMenuItem nextQuestion = new ToolStripMenuItem("下一题", null, delegate { this.nextQuestion(); showQuestion(); });

        menu.Items.Add(handin);
        menu.Items.Add(lastQuestion);
        menu.Items.Add(nextQuestion);

        question[4] = new Choice(4, "", new Rectangle(40, 40, 900, 200), delegate { mark(); });
        for (int i = 0; i < 4; i++)
        {
            question[i] = new Choice(i, "", new Rectangle(40, 100 * i + 240, 900, 100), click);
        }

        time.ForeColor = Color.Tomato;
        time.Font = new Font("Consolas", 20, FontStyle.Bold);
        time.Location = new Point(960, 30);
        time.Size = new Size(300, 50);


        Label singleLable = new Label();
        singleLable.Location = new Point(950, 100);
        singleLable.Text = "单选题";
        singleList.Location = new Point(950, 120);
        singleList.Font = new Font("Consolas", 10, FontStyle.Bold);
        singleList.Size = new Size(300, 150);

        Label multiLable = new Label();
        multiLable.Location = new Point(950, 270);
        multiLable.Text = "多选题";
        multiList.Font = new Font("Consolas", 10, FontStyle.Bold);
        multiList.Location = new Point(950, 290);
        multiList.Size = new Size(300, 150);
        Label unSureLabel = new Label();

        unSureLabel.Location = new Point(950, 445);
        unSureLabel.Text = "标记题";
        unSureList.Location = new Point(950, 460);
        unSureList.Size = new Size(300, 220);
        unSureList.Font = new Font("Consolas", 10, FontStyle.Bold);

        singleList.SelectedIndexChanged += delegate { q = singleList.SelectedIndex; single = true; showQuestion(); };
        multiList.SelectedIndexChanged += delegate { q = multiList.SelectedIndex; single = false; showQuestion(); };
        unSureList.SelectedIndexChanged += delegate
        {
            Unsure u = unSureList.SelectedItem as Unsure;
            if (u == null) return;
            q = u.num;
            single = u.single;
            showQuestion();
        };

        MainMenuStrip = menu;
        Controls.Add(menu);
        Controls.Add(singleList);
        Controls.Add(multiList);
        Controls.Add(unSureList);
        Controls.Add(multiLable);
        Controls.Add(singleLable);
        Controls.Add(unSureLabel);
        Controls.Add(time);

        foreach (Choice c in question)
            Controls.Add(c);

        for (int i = 0; i < 30; i++)
        {
            singleList.Items.Add((i + 1) + ". " + paper.single[i].name);
        }
        for (int i = 0; i < 10; i++)
        {
            multiList.Items.Add((i + 1) + ". " + paper.multi[i].name);
        }
        for (int i = 0; i < singleAns.Length; i++)
            singleAns[i] = -1;

        showQuestion();
        timer.Interval = 1000;
        timer.Tick += delegate { tick(); };
        timer.Start();
    }
    void handin()
    {
        timer.Stop();
        count = 0;
        MainMenuStrip.Items.RemoveAt(0);
        Controls.Remove(time);
        judge();
    }
    void click(object o, EventArgs e)
    {
        if (single) singleClick(o, null);
        else multiClick(o, null);
    }
    bool marked()
    {
        foreach (object o in unSureList.Items)
        {
            Unsure u = o as Unsure;
            if (u.single == single && u.num == q)
            {
                return true;
            }
        }
        return false;
    }
    void mark()
    {
        foreach (object o in unSureList.Items)
        {
            Unsure u = o as Unsure;
            if (u.single == single && u.num == q)
            {
                unSureList.Items.Remove(o);
                question[4].chosen = false;
                return;
            }
        }
        question[4].chosen = true;
        unSureList.Items.Add(new Unsure(single, q));
        return;
    }
    void judge()
    {
        int score = 0;
        int singleError = 0;
        int multiError = 0;
        unSureList.Items.Clear();
        for (int i = 0; i < 30; i++)
        {
            if (singleAns[i] != paper.single[i].ans)
            {
                singleError++;
                unSureList.Items.Add(new Unsure(true, i));
            }
        }
        for (int i = 0; i < 10; i++)
        {
            bool error = false;
            for (int j = 0; j < 4; j++)
            {
                if (multiAns[i, j] != paper.multi[i].ans[j])
                    error = true;
            }
            if (error)
            {
                multiError++;
                unSureList.Items.Add(new Unsure(false, i));
            }
        }
        score = 50;
        score -= multiError * 2;
        score -= singleError;
        if (score == 50)
        {
            MessageBox.Show("你太牛逼了！竟然全对了。");
        }
        else
        {
            MessageBox.Show("满分50，你考" + score + ".\n错了" + singleError + "道单选," + multiError + "道多选.\n错题在标记栏里");
        }
    }
    void tick()
    {
        count--;
        if (count == 0)
        {
            handin();
        }
        else
        {
            int minute = count / 60;
            int second = count % 60;
            time.Text = minute + " : " + second;
        }
    }
    void initChoice()
    {
        if (single)
        {
            for (int i = 0; i < 4; i++)
            {
                question[i].ForeColor = ForeColor;
                question[i].Font = new Font("Consolas", 15);
            }
            question[paper.single[q].ans].Font = new Font("隶书", 19, FontStyle.Italic | FontStyle.Bold);
            question[paper.single[q].ans].ForeColor = Color.Green;
        }
        else
        {
            for (int i = 0; i < 4; i++)
                if (paper.multi[q].ans[i])
                {
                    question[paper.single[q].ans].Font = new Font("隶书", 19, FontStyle.Italic | FontStyle.Bold);
                    question[paper.single[q].ans].ForeColor = Color.Green;
                }
                else
                {
                    question[i].ForeColor = ForeColor;
                    question[i].Font = new Font("Consolas", 15);
                }
        }
    }
    void showQuestion()
    {
        if(count==0)
            initChoice();
        if (single)
        {
            for (int i = 0; i < 4; i++)
            {
                question[i].Text = (char)(i + 'A') + ". " + paper.single[q].choice[i];
                question[i].num = i;
                if (singleAns[q] == i)
                    question[i].chosen = true;
                else
                    question[i].chosen = false;
            }
            if (marked()) question[4].chosen = true;
            else question[4].chosen = false;
            question[4].Text = (1 + q) + ". " + paper.single[q].name;
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                question[i].Text = (char)(i + 'A') + ". " + paper.multi[q].choice[i];
                question[i].num = i;
                if (multiAns[q, i])
                    question[i].chosen = true;
                else question[i].chosen = false;
            }
            if (marked()) question[4].chosen = true;
            else question[4].chosen = false;
            question[4].Text = (1 + q) + ". " + paper.multi[q].name;
            
        }
    }
    void singleClick(object o, EventArgs e)
    {
        Choice cho = o as Choice;
        if (cho.num == singleAns[q])
        {
            cho.chosen = false;
            singleAns[q] = -1;
        }
        else
        {
            if (singleAns[q] != -1)
                question[singleAns[q]].chosen = false;
            cho.chosen = true;
            question[cho.num].chosen = true;
            singleAns[q] = cho.num;
        }
    }
    void multiClick(object o, EventArgs e)
    {
        Choice cho = o as Choice;
        if (multiAns[q, cho.num])
        {
            multiAns[q, cho.num] = false;
            question[cho.num].chosen = false;
        }
        else
        {
            multiAns[q, cho.num] = true;
            cho.chosen = true;
        }
    }
    void lastQuestion()
    {
        if (q == 0) return;
        else q--;
    }
    void nextQuestion()
    {
        if (single)
        {
            if (q == 29) return;
            else q++;
        }
        else
        {
            if (q == 9) return;
            else q++;
        }
    }
}