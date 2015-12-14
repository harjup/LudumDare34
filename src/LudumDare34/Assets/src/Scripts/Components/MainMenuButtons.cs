using UnityEngine;

public class MainMenuButtons : MonoBehaviour 
{
    public void OnStartButtonClick()
    {
        Application.LoadLevel("Cutscene");
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) 
            || Input.GetKeyDown(KeyCode.RightArrow))
        {
            Application.LoadLevel("Cutscene");
        }
    }

}
