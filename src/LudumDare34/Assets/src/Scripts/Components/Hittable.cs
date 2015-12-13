using UnityEngine;
using System.Collections;

public class Hittable : MonoBehaviour
{

    private PlayerStats _playerStats;
    private Jumpable _jumpable;
    public void Start()
    {
        _playerStats = gameObject.GetComponentInParent<PlayerStats>();
        _jumpable = gameObject.GetComponentInParent<Jumpable>();
    }

    public void OnTriggerEnter(Collider other)
    {
        _playerStats.TakeHit(1);
        _jumpable.GotHit();
    }

    
}
