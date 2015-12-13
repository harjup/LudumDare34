using UnityEngine;
using System.Collections;

public class Hittable : MonoBehaviour
{
    private Jumpable _jumpable;
    public void Start()
    {
        _jumpable = gameObject.GetComponentInParent<Jumpable>();
    }

    public void OnTriggerEnter(Collider other)
    {
        _jumpable.GotHit();
    }

    
}
