using UnityEngine;
using System.Collections;

public class Hittable : MonoBehaviour
{

    private PlayerStats _playerStats;
    public void Start()
    {
        _playerStats = gameObject.GetComponentInParent<PlayerStats>();
    }

    public void OnTriggerEnter(Collider other)
    {
        _playerStats.TakeHit(1);
    }

    
}
