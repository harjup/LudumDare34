public enum Target
{
    Unknown,
    Circle,
    Box
}

public class EnemyData  : IStoryItem
{
    public Pattern Pattern { get; set; }

    public Target Target { get; set; }

    public float RunTime { get; set; }

    public GameContext.Difficulty Difficulty { get; set; }

    public EnemyData(Pattern pattern, Target target, float runTime = 2f)
    {
        Target = target;
        Pattern = pattern;
        RunTime = runTime;
    }
}
