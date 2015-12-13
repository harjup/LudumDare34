public enum Target
{
    Unknown,
    Circle,
    Box
}

public class EnemyData 
{
    public Pattern Pattern { get; set; }

    public Target Target { get; set; }

    public EnemyData(Pattern pattern, Target target)
    {
        Target = target;
        Pattern = pattern;
    }
}
