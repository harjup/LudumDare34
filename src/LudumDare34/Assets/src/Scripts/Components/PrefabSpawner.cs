using UnityEngine;
using System.Collections;

public class PrefabSpawner : MonoBehaviour
{
    public GameObject _target;
    public float Min;
    public float Max;

    public void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    public IEnumerator SpawnObjects()
    {
        var nextDelay = Random.Range(Min, Max);

        while (true)
        {
            Instantiate(_target, transform.position, Quaternion.identity);

            yield return new WaitForSeconds(nextDelay);
        }
    }

}
