using System.Collections.Generic;

public class ConfabLoader
{
    public List<Confab> GetScript(string sceneName)
    {
        return new List<Confab>
        {
            new Confab("Enemy", "I want to <b>steal bombs</b>!!! Ah!!!", "BoxGuy", "CircleGuy", "EnemyGuy"),
            new Confab("Ibb", ":I", "BoxGuy", "CircleGuy", "EnemyGuy"),
            new Confab("Enemy", "GET OVER HERE", "BoxGuy", "CircleGuy", "EnemyGuy")
        };
    }
}
