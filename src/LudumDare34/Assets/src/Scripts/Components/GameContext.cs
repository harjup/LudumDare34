using UnityEngine;
using System.Collections;

public class GameContext : MonoBehaviour
{
    private ConfabLoader _loader;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        _loader = new ConfabLoader();

        StartCoroutine(DoThings());
    }

    public void OnLevelWasLoaded(int level)
    {
        StartCoroutine(DoThings());
    }

    public IEnumerator DoThings()
    {
        var dialogGui = FindObjectOfType<DialogGui>();

        if (Application.loadedLevelName == "Cutscene")
        {
            yield return StartCoroutine(dialogGui.DisplayConfabs(_loader.GetScript("asdf")));

            NextLevel();
        }
        else if (Application.loadedLevelName == "Scratch")
        {
            
        }

        yield break;
    }



    public void NextLevel()
    {
        Application.LoadLevel("Scratch");
    }


}
