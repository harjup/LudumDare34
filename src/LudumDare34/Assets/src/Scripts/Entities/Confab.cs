public enum StageDirection
{
    Left, Right
}

public class Confab : Cue
{
    public Confab(){ }

    public Confab(string name, string content, StageDirection direction)
    {
        Name = name;
        Content = content;
        Direction = direction;
    }

    public string Name { get; set; }

    public string Content { get; set; }

    public StageDirection Direction { get; set; }

    public override string ToString()
    {
        return string.Format("{0}|{1}", Name, Content);
    }
}