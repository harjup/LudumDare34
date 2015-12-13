public class Confab
{
    public Confab()
    {
        
    }

    public Confab(string name, string content, string leftPortrait01, string leftPortrait02, string rightPortrait)
    {
        Name = name;
        Content = content;
        LeftPortrait01 = leftPortrait01;
        LeftPortrait02 = leftPortrait02;
        RightPortrait = rightPortrait;

    }
    public string Name { get; set; }

    public string Content { get; set; }

    public string LeftPortrait01 { get; set; }

    public string LeftPortrait02 { get; set; }

    public string RightPortrait { get; set; }

    public override string ToString()
    {
        return string.Format("{0}|{1}|{2}|{3}|{4}", Name, Content, LeftPortrait01, LeftPortrait02, RightPortrait);
    }
}