using UnityEngine;
using System.Collections.Generic;

public class FightCoordinator : MonoBehaviour
{

    private List<string> _enemies;
    private List<string> _battleEvents;


    private Spawner _spawner;

    public void Start()
    {
        _spawner = FindObjectOfType<Spawner>();

        InitFight(new List<string> { "one", "two" }, new List<string>{ "one", "two" });
    }

    public void InitFight(List<string> enemies, List<string> battleEvents)
    {
        _enemies = enemies;
        _battleEvents = battleEvents;

        // Spawn first 5 enemies off screen
    }

    public void SpawnEnemies(List<string> enemies)
    {
        enemies.ForEach((s) => {});
    }

    public void StartFight()
    {
        // Walk enemies on screen
        // Do any intro events
        // Start first attack
    }

    public void DoAttack()
    {
        // Pick an enemy
        // Pick an attack type
        // Start it off. Wait for it to finish.
    }
	
	void Update () 
    {
	
	}
}
