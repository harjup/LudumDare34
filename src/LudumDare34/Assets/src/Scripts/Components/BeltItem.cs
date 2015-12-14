using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class BeltItem : MonoBehaviour
{

    public List<Sprite> sprites;

    // Use this for initialization
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = sprites.AsRandom().First();

        transform.DOMoveX(-18, 12f)
            .SetEase(Ease.Linear)
            .OnComplete(() => {Destroy(this.gameObject);});
    }

    // Update is called once per frame
    void Update()
    {

    }
}
