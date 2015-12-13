using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EndingChoice : MonoBehaviour
{

    private GameObject ChoiceImageObject;
    private GameObject BombEndObject;
    private GameObject NothingEndObject;
    private GameObject EndingObject;

    private bool _choiceMade;

    private List<Cue> _bombEnding = new List<Cue> 
    {
        new Portraits(null, null,null),
        new Confab("Adder", "(;°Д°)", StageDirection.Left),
        new Confab("Sqoji", "|・□・；|", StageDirection.Left),
        new Confab("Gerty", "We did it. We are heroes for potential bomb victims everywhere.", StageDirection.Left),
        new Confab("Haswel", "But not these bomb victims. Or miners.", StageDirection.Left),
        new Confab("Gerty", "Yeah I guess. Let's leave before things heat up.", StageDirection.Left),
    };

    /*  
    But not these bomb victims. Or miners.
    Yeah I guess. Let's leave before things heat up.*/

    private List<Cue> _doNothingEnding = new List<Cue>
    {
        new Portraits(null, null,null),
        new Confab("Gerty", "Why is this even here?", StageDirection.Left),
        new Confab("Haswel", "Every minute of every day, you have to choose to not blow up. Maybe we are the monsters.", StageDirection.Left),
        new Confab("Adder", "(;°Д°)", StageDirection.Left),
        new Confab("Sqoji", ":I", StageDirection.Left),
        new Confab("Gerty", "Oh. Ok let's leave.", StageDirection.Left),
    };


    // Use this for initialization
    void Start()
    {
        ChoiceImageObject = GameObject.Find("ChoiceImage");
        BombEndObject = GameObject.Find("BombEnd");
        NothingEndObject = GameObject.Find("NothingEnd");
        EndingObject = GameObject.Find("EndingText");

        _choiceMade = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_choiceMade)
        {
            WaitForChoiceInput();
        }
    }

    private void WaitForChoiceInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Play Right Choice
            // Show Doing Nothing
            ChoiceImageObject.SetActive(false);
            NothingEndObject.SetActive(false);

            StartCoroutine(PlayFinalLines(true));

            _choiceMade = true;
            

            // Show last dialog
            // END
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // Play Left Choice 
            // Show Explosion Card
            ChoiceImageObject.SetActive(false);
            BombEndObject.SetActive(false);

            StartCoroutine(PlayFinalLines(false));

            _choiceMade = true;
            // Show last dialog
            // END
        }
        
    }

    private IEnumerator PlayFinalLines(bool bomb)
    {
        var dialogGui = FindObjectOfType<DialogGui>();
        dialogGui.Init();

        dialogGui.transform.localPosition = Vector3.zero;

        if (bomb)
        {
            yield return StartCoroutine(dialogGui.DisplayConfabs(_bombEnding));
        }
        else
        {
            yield return StartCoroutine(dialogGui.DisplayConfabs(_doNothingEnding));
        }

        dialogGui.transform.localPosition = new Vector3(-1000, 0, 0);
        EndingObject.transform.localPosition = new Vector3(0,0,0);
    }

}
