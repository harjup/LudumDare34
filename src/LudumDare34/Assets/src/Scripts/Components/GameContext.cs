using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameContext : MonoBehaviour
{
    private ConfabLoader _loader;

    private int scriptIndex = 0;

    private List<List<IStoryItem>> story;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        _loader = new ConfabLoader();


      story =  new List<List<IStoryItem>>
    {
        new List<IStoryItem>
        {
            new Confab("Enemy", "I want to <b>steal bombs</b>!!! Ah!!!", "BoxGuy", "CircleGuy", "EnemyGuy"),
            new Confab("Ibb", ":I", "BoxGuy", "CircleGuy", "EnemyGuy"),
            new Confab("Enemy", "GET OVER HERE", "BoxGuy", "CircleGuy", "EnemyGuy")
        },
        GenerateListOfEnemies(5).Cast<IStoryItem>().ToList(),
        new List<IStoryItem>
        {
            new Confab("Enemy", "THIS IS THE SECOND CUTSCENE!!!!", "CircleGuy", null, "BoxGuy"),
            new Confab("Ibb", ":I",  "CircleGuy", null, "BoxGuy"),
            new Confab("Enemy", "GET OVER HERE", "CircleGuy", null, "BoxGuy")
        },
        GenerateListOfEnemies(10).Cast<IStoryItem>().ToList(),
        GenerateListOfEnemies(15).Cast<IStoryItem>().ToList(),
    };

    }

    public void OnLevelWasLoaded(int level)
    {
        var current = story[scriptIndex];
        StartCoroutine(DoThings(current));
    }

    public IEnumerator DoThings(List<IStoryItem> storyItems)
    {
        if (Application.loadedLevelName == "Cutscene")
        {
            var dialogGui = FindObjectOfType<DialogGui>();

            dialogGui.Init();
            yield return StartCoroutine(dialogGui.DisplayConfabs(storyItems.Cast<Confab>().ToList()));

            NextLevel();
        }
        else if (Application.loadedLevelName == "Scratch")
        {
            var coord = FindObjectOfType<FightCoordinator>();


            coord.InitFight(storyItems.Cast<EnemyData>().ToList(), (win) =>
            {
                if (win)
                {
                    NextLevel();
                }
                else
                {
                    ResetLevel();
                }
                
            });
        }

        yield break;
    }


    public static List<EnemyData> GenerateListOfEnemies(int count)
    {
        var result = new List<EnemyData>();
        var patterns = new List<Pattern> { Pattern.StraightLine, Pattern.MakeDecision };
        var targets = new List<Target> { Target.Box, Target.Circle };

        for (int i = 0; i < count; i++)
        {
            var data = new EnemyData(patterns.AsRandom().First(), targets.AsRandom().First());
            result.Add(data);
        }

        return result;
    }

    private void ResetLevel()
    {
        Application.LoadLevel("Scratch");
    }


    public void NextLevel()
    {
        scriptIndex++;

        var next = story[scriptIndex];
        if (next.First() is Confab)
        {
            Application.LoadLevel("Cutscene");
        }
        else if (next.First() is EnemyData)
        {
            Application.LoadLevel("Scratch");
        }
    }


}
