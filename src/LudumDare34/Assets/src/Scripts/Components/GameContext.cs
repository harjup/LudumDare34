﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameContext : MonoBehaviour
{
    private ConfabLoader _loader;

    private int scriptIndex = 0;

    private List<List<IStoryItem>> story;

    public enum Difficulty
    {
        Undefined, Simple, Moderate, Boss
    }

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
        new Portraits("CircleGuy", "BoxGuy", null),
        new Confab("Adder", "(＃´Ｏ`),", StageDirection.Left),
        new Confab("Squoji", "[v.v]", StageDirection.Right),
        new Portraits("CircleGuy", "BoxGuy", "TriangleGuy"),
        new Confab("Gerty", "This is bad, bad. Many thieves are out there trying to catch us.", StageDirection.Right),
        new Confab("Gerty", "Hello, Adder and Squoji.", StageDirection.Right),
        new Confab("Squoji", ".['.']/", StageDirection.Right),
        new Confab("Gerty", @"We should make our way to the control room. We can stop production there, I think.", StageDirection.Left),
        new Portraits("CircleGuy", "BoxGuy", "EnemyGuy"),
        new Confab("Bomb Theives", @"Whoa, no stopping the blow ups here.", StageDirection.Left),
        new Confab("Bomb Theives", @"That's what we're here for!", StageDirection.Left),
    };

    private List<Cue> thirdScene = new List<Cue>
    {
        new Portraits("CircleGuy", "BoxGuy", null),
        new Confab("Adder", "(＃´Ｏ`),", StageDirection.Left),
        new Confab("Squoji", "[v.v]", StageDirection.Right),
        new Portraits("CircleGuy", "BoxGuy", "TriangleGuy"),
        new Confab("Haswel", "Heading to the control room? I'll tag along.", StageDirection.Right),
        new Confab("Adder", ".(^_^).", StageDirection.Right),
        new Confab("Squoji", ".['.']/", StageDirection.Right),
        new Portraits("CircleGuy", "TriangleGuy", "TriangleGuy"),
        new Confab("Gerty", @"We should make our way to the control room. We can stop production there, I think.", StageDirection.Left),
        new Portraits("CircleGuy", "BoxGuy", "EnemyGuy"),
        new Confab("Bomb Theives", @"Whe boss won't like this!", StageDirection.Left),
        new Confab("Bomb Theives", @"He wants to blow up the whole world! There's no stopping it!!!", StageDirection.Left),
    };


    private List<Cue> fourthScene = new List<Cue>
    {
        new Portraits("CircleGuy", "BoxGuy", "Boss"),
        new Confab("Boss", "Hello, it is me. The boss.", StageDirection.Right),
        new Confab("Boss", "IT WAS ME! IT WAS ME ALL ALONG.", StageDirection.Right),
        new Confab("Boss", "I WAS MAKING BOMBS NOT TO SELL THEM, BUT TO USE THEM!", StageDirection.Right),
        new Confab("Boss", "NOW GET OUT OF MY WAY!!!!!!", StageDirection.Right),
        new Confab("Both", "(;°Д°) [;°Д°]", StageDirection.Left),
    };

    private List<Cue> fifthScene = new List<Cue>
    {
        new Portraits("CircleGuy", "BoxGuy", "Boss"),
        new Confab("Boss", "Hello, it is me. The boss.", StageDirection.Right),
        new Confab("Boss", "IT WAS ME! IT WAS ME ALL ALONG.", StageDirection.Right),
        new Confab("Boss", "I WAS MAKING BOMBS NOT TO SELL THEM, BUT TO USE THEM!", StageDirection.Right),
        new Confab("Boss", "NOW GET OUT OF MY WAY!!!!!!", StageDirection.Right),
        new Confab("Both", "(;°Д°) [;°Д°]", StageDirection.Left),
    };


    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        _loader = new ConfabLoader();


      story =  new List<List<IStoryItem>>
    {
        firstScene.Cast<IStoryItem>().ToList(),
        GenerateListOfEnemies(10, Difficulty.Simple).Cast<IStoryItem>().ToList(),
        secondScene.Cast<IStoryItem>().ToList(),
        GenerateListOfEnemies(15, Difficulty.Moderate).Cast<IStoryItem>().ToList(),
        thirdScene.Cast<IStoryItem>().ToList(),
        GenerateListOfEnemies(20, Difficulty.Moderate).Cast<IStoryItem>().ToList(),
        fourthScene.Cast<IStoryItem>().ToList(),
        GenerateListOfEnemies(1, Difficulty.Boss).Cast<IStoryItem>().ToList(),
        fifthScene.Cast<IStoryItem>().ToList(),
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


    public static List<EnemyData> GenerateListOfEnemies(int count, Difficulty hardness)
    {
        var result = new List<EnemyData>();


        var patterns = new List<Pattern> { Pattern.StraightLine, Pattern.MakeDecision };
        if (hardness == Difficulty.Simple)
        {
            patterns = new List<Pattern> { Pattern.StraightLine};
        }

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
