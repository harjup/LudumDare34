using UnityEngine;
using System.Collections;

public class Portraits : Cue
{

    public Portraits()
    {
        
    }

    public Portraits(string leftPortrait01, string leftPortrait02, string rightPortrait)
    {
        LeftPortrait01 = leftPortrait01;
        LeftPortrait02 = leftPortrait02;
        RightPortrait = rightPortrait;
    }

    public string LeftPortrait01 { get; set; }

    public string LeftPortrait02 { get; set; }

    public string RightPortrait { get; set; }
}
