using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameContext : MonoBehaviour
{
    private ConfabLoader _loader;

    private int scriptIndex = 0;

    private List<List<IStoryItem>> story;

    private List<Cue> firstScene = new List<Cue>
    {
        new Portraits("CircleGuy", null, null),
        new Confab("Adder", ".(._.).", StageDirection.Left),
        new Portraits("CircleGuy", null, "BoxGuy"),
        new Confab("Squoji", ".['.']/", StageDirection.Right),
        new Confab("Both", @".(^_^)/\['.'].", StageDirection.Left),
        new Portraits("CircleGuy", "BoxGuy", null),
        new Confab("Intercom", "ATTENTION EMPLOYEES ... ATTENTION EMPLOYEES ... THIS IS THE BOSS.", StageDirection.Right),
        new Confab("Intercom", "OUR BELOVED BOMB FACTORY IS OVERRUN WITH THIEVES.", StageDirection.Right),
        new Confab("Intercom", "ALL PERSONAL ARE STRONGLY RECCOMENDED TO EVACUATE IMMEDIATELY.", StageDirection.Right),
        new Confab("Both", @"(」゜ロ゜)」 [；゜○゜]", StageDirection.Left),
        new Portraits("CircleGuy", "BoxGuy", "EnemyGuy"),
        new Confab("Bomb Theives", " Whoa, hey, some fresh Ops.", StageDirection.Right),
        new Confab("Bomb Theives", "We should steal them too. They could make even more bombs!!!", StageDirection.Right)
    };

    private List<Cue> secondScene = new List<Cue>
    {
        new Portraits("CircleGuy", null, null),
        new Confab("Adder", ".(._.).", StageDirection.Left),
        new Portraits("CircleGuy", null, "SquareGuy"),
        new Confab("Squoji", ".['.']/", StageDirection.Right),
        new Confab("Both", @".(^_^)/\['.'].", StageDirection.Left),
        new Portraits("CircleGuy", "SquareGuy", null),
        new Confab("Intercom", "ATTENTION EMPLOYEES ... ATTENTION EMPLOYEES ... THIS IS THE BOSS.", StageDirection.Right),
        new Confab("Intercom", "OUR BELOVED BOMB FACTORY IS OVERRUN WITH THIEVES.", StageDirection.Right),
        new Confab("Intercom", "ALL PERSONAL ARE STRONGLY RECCOMENDED TO EVACUATE IMMEDIATELY.", StageDirection.Right),
        new Confab("Both", @"(」゜ロ゜)」 [；゜○゜]", StageDirection.Left),
        new Confab("Bomb Theives", " Whoa, hey, some fresh Ops.", StageDirection.Right),
        new Confab("Bomb Theives", "We should steal them too. They could make even more bombs!!!", StageDirection.Right)
    };


    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        _loader = new ConfabLoader();


      story =  new List<List<IStoryItem>>
    {
        firstScene.Cast<IStoryItem>().ToList(),
        GenerateListOfEnemies(5).Cast<IStoryItem>().ToList(),
        new List<IStoryItem>
        {
            new Portraits("BoxGuy", null, "EnemyGuy"),
            new Confab("Adder Puff", "THIS IS THE SECOND CUTSCENE!!!!", StageDirection.Left),
            new Portraits("EnemyGuy", null, "BoxGuy"),
            new Confab("Adder Puff", "I Moved", StageDirection.Right),
            new Confab("Scoji", ":I", StageDirection.Left)
        },
        GenerateListOfEnemies(15).Cast<IStoryItem>().ToList()
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
            yield return StartCoroutine(dialogGui.DisplayConfabs(storyItems.Cast<Cue>().ToList()));

            NextLevel();
        }
        else if (Application.loadedLevelName == "Scratch")
        {
            var coord = FindObjectOfType<FightCoordinator>();


            coord.InitFight(storyItems.Cast<EnemyData>().ToList(), (win) =>
            {
                Debug.Log("Callback " + win.ToString());
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

        if (story.Count >= scriptIndex)
        {
            Application.LoadLevel("Ending");
        }

        var next = story[scriptIndex];
        if (next.First() is Cue)
        {
            Application.LoadLevel("Cutscene");
        }
        else if (next.First() is EnemyData)
        {
            Application.LoadLevel("Scratch");
        }
    }


}
