public class EnemyData 
{
    public string PrefabPath { get; set; }

    public string Name { get; set; }

    public EnemyData(string name, string prefabPath)
    {
        Name = name;
        PrefabPath = PrefabPath;
    }
}
