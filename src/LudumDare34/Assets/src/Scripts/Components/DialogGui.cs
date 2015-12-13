using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class DialogGui : MonoBehaviour
{
    private Text Name;
    private Text Content;

    private Image LeftCharacter;
    private Image LeftCharacter2;
    private Image RightCharacter;

    private TextCrawler _textCrawler;

    private Dictionary<string, string> characterSpriteMap = new Dictionary<string, string>
    {
        {"BoxGuy", "Art/BoxGuy/BoxGuyIdle/BoxGuyIdle-001"},
        {"CircleGuy", "Art/CircleGuy/Cicle Guy Idle/CicleGuyIdle-001"},
        {"EnemyGuy", "Art/EnemyGuy/Walk/EnemyGuyWalk-001"}
    }; 


	void Start ()
	{
	    _textCrawler = gameObject.AddComponent<TextCrawler>();

	    var textComponents = GetComponentsInChildren<Text>();
        var imageComponents = GetComponentsInChildren<Image>();

	    Content = textComponents.First(c => c.gameObject.name == "TextContent");
        Name = textComponents.First(c => c.gameObject.name == "SpeakerName");

	    LeftCharacter = imageComponents.First(c => c.gameObject.name.Contains("Left-01-Character"));
        LeftCharacter2 = imageComponents.First(c => c.gameObject.name.Contains("Left-02-Character"));
        RightCharacter = imageComponents.First(c => c.gameObject.name.Contains("RightCharacter"));

	    Name.text = "The Boss";

	    LeftCharacter.color = Color.white;
        LeftCharacter2.color = Color.white;
        RightCharacter.color = Color.white;

	}

    public IEnumerator DisplayConfabs(List<Confab> confabs)
    {
        foreach (var confab in confabs)
        {
            Name.text = confab.Name;

            LeftCharacter.overrideSprite = Resources.Load<Sprite>(characterSpriteMap[confab.LeftPortrait01]);
            LeftCharacter2.overrideSprite = Resources.Load<Sprite>(characterSpriteMap[confab.LeftPortrait02]);
            RightCharacter.overrideSprite = Resources.Load<Sprite>(characterSpriteMap[confab.RightPortrait]);

            yield return StartCoroutine(_textCrawler.TextCrawl(confab.Content, t =>  Content.text = t));
            // Wait for input to continue...
            yield return StartCoroutine(WaitForInput());
        }
    }

    public void Update()
    {
        ListenForSkipInput();
    }


    public void ListenForSkipInput()
    {
        if (   Input.GetKeyDown(KeyCode.LeftArrow) 
            || Input.GetKeyDown(KeyCode.RightArrow))
        {
            _textCrawler.SkipToEnd();
        }
    }


    public IEnumerator WaitForInput()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)
                || Input.GetKeyDown(KeyCode.RightArrow))
            {
                yield break;
            }

            yield return null;
        }
    }
}