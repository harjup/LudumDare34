using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public int MaxHitPoints;
    public int HitPoints;

    private Text _currentHitPoints;

    void Start()
    {
        HitPoints = MaxHitPoints;

        GameObject.Find("MaxHealth").GetComponent<Text>().text = MaxHitPoints.ToString();

        _currentHitPoints = GameObject.Find("CurrentHitPoints").GetComponent<Text>();
        _currentHitPoints.text = HitPoints.ToString();
    }

    public void TakeHit(int amount)
    {
        HitPoints -= amount;

        if (HitPoints < 0)
        {
            HitPoints = 0;
        }

        _currentHitPoints.text = HitPoints.ToString();
    }
}
