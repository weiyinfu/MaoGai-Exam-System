using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

class Question
{
    public string name;
    public string[] choice = new string[4];
}
class Single : Question
{
    public int ans;
}
class Multi : Question
{
    public bool[] ans = new bool[4];
}
class Chapter
{
    public string name;
    public Single[] single = new Single[100];
    public Multi[] multi = new Multi[100];
    public int singleSize;
    public int multiSize;
}
class Book
{
    public Chapter[] chapter = new Chapter[20];
    public int length;
    string getAll()
    {
        FileStream file = new FileStream("../../haha.txt", FileMode.Open);
        byte[] b = new byte[file.Length];
        file.Read(b, 0, (int)file.Length);
        string s = System.Text.Encoding.Default.GetString(b);
        s = s.Replace('、', '.');
        s = s.Replace('．', '.');
        file.Close();
        return s;
    }
    string[] split(string s)
    {
        string[] str = new string[5000];
        int i = 0;
        int j = 0;
        while (i < s.Length)
        {
            while (i < s.Length && (s[i] == ' ' || s[i] == '\n' || s[i] == '\r'))
                i++;
            int last = i;
            while (i < s.Length && s[i] != ' ' && s[i] != '\n' && s[i] != '\r')
                i++;
            if (i > last)
                str[j++] = s.Substring(last, i - last);
        }
        string[] ss = new string[j];
        for (i = 0; i < j; i++)
            ss[i] = str[i];
        return ss;
    }
    int readSingle(string[] str, int i, Chapter cha)
    {
        int singleSize = 0;
        while (i < str.Length)
        {
            Single sin = new Single();
            sin.name = str[i];
            if (!Char.IsDigit(sin.name[0]))
            {
                cha.singleSize = singleSize;
                return i;
            }
            int where = sin.name.IndexOf('.');
            where++;
            sin.name = sin.name.Substring(where);
            Console.WriteLine(singleSize + " " + sin.name);
            sin.choice[0] = str[i + 1].Substring(str[i + 1].IndexOf('.') + 1);
            sin.choice[1] = str[i + 2].Substring(str[i + 2].IndexOf('.') + 1);
            sin.choice[2] = str[i + 3].Substring(str[i + 3].IndexOf('.') + 1);
            sin.choice[3] = str[i + 4].Substring(str[i + 4].IndexOf('.') + 1);
            i += 5;
            cha.single[singleSize] = sin;
            singleSize++;
        }
        throw new Exception("read single error");
    }
    int readMulti(string[] str, int i, Chapter cha)
    {
        int multiSize = 0;
        while (i < str.Length)
        {
            Multi sin = new Multi();
            sin.name = str[i];
            if (!Char.IsDigit(str[i][0]))
            {
                cha.multiSize = multiSize;
                return i;
            }
            int where = sin.name.IndexOf('.');
            where++;
            sin.name = sin.name.Substring(where);
            Console.WriteLine(multiSize + " " + sin.name);
            sin.choice[0] = str[i + 1].Substring(str[i + 1].IndexOf('.') + 1);
            sin.choice[1] = str[i + 2].Substring(str[i + 2].IndexOf('.') + 1);
            sin.choice[2] = str[i + 3].Substring(str[i + 3].IndexOf('.') + 1);
            sin.choice[3] = str[i + 4].Substring(str[i + 4].IndexOf('.') + 1);
            i += 5;
            cha.multi[multiSize] = sin;
            multiSize++;
        }
        throw new Exception("read single error");
    }
    public Book()
    {
        string s = getAll(); 
        string[] str = split(s);
        int i = 0;//which string
        int j = 0;//which chapter
        int len = str.Length;
        while (i < len)
        {
            Console.WriteLine("第" + j + "章");
            chapter[j] = new Chapter();
            i++;//the first chapter
            chapter[j].name = str[i];
            Console.WriteLine(str[i]);
            i++;
            i++;//"single choice"
            i = readSingle(str, i, chapter[j]);
            i++;
            i = readMulti(str, i, chapter[j]);
            i++;//answer
            i++;//single ans
            for (int k = 0; chapter[j].single[k] != null; k++)
            {
                int where = str[i].IndexOf('.') + 1;
                if (str[i][where] == 'A') chapter[j].single[k].ans = 0;
                else if (str[i][where] == 'B') chapter[j].single[k].ans = 1;
                else if (str[i][where] == 'C') chapter[j].single[k].ans = 2;
                else if (str[i][where] == 'D') chapter[j].single[k].ans = 3;
                else Console.WriteLine("第" + j + "章 " + "第 " + k + " 题 " + str[i]);
                i++;
            }
            i++;//multi ans
            for (int k = 0; chapter[j].multi[k] != null; k++)
            {
                int where = str[i].IndexOf('.') + 1;
                for (int t = where; t < str[i].Length; t++)
                {
                    if (str[i][t] == 'A') chapter[j].multi[k].ans[0] = true;
                    else if (str[i][t] == 'B') chapter[j].multi[k].ans[1] = true;
                    else if (str[i][t] == 'C') chapter[j].multi[k].ans[2] = true;
                    else if (str[i][t] == 'D') chapter[j].multi[k].ans[3] = true;
                    else Console.WriteLine("第" + j + "章 " + "第 " + k + " 题" + " mlulti ");
                }
                Console.WriteLine(k + " " + str[i]);
                i++;
            }
            j++;
        }
        length = j;
    }
}
