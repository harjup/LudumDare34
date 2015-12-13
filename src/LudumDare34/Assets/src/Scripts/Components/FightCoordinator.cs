using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class FightCoordinator : MonoBehaviour
{
    private List<EnemyData> _enemies;
    
    private List<string> _battleEvents;

    private GameObject _enemyPrefab;


    private Spawner _spawner;

    private List<EnemyData> GenerateListOfEnemies(int count)
    {
        var result = new List<EnemyData>();

        for (int i = 0; i < count; i++)
        {
            var data = new EnemyData(Pattern.MakeDecision, Target.Box);
            result.Add(data);
        }

        return result;
    }



    public void Start()
    {
        _spawner = FindObjectOfType<Spawner>();
        _enemyPrefab = Resources.Load<GameObject>("Prefab/EnemyRoot");

        InitFight(GenerateListOfEnemies(5));
    }

    public void InitFight(List<EnemyData> firstWave)
    {
        _enemies = firstWave;


        var characters = FindObjectsOfType<Jumpable>().ToList();

        DOTween.Sequence()
            .AppendCallback(() => {/* Show start fight GUI*/ Debug.Log("Start Fight!!!"); })
            .AppendInterval(.5f)
            .AppendCallback(() => {/* Hide start fight GUI*/ })
            //Walk two heros in at the same time!!!
            .Append(GetLeftHeroIntoPosition(characters, 1f))
            .Join(GetRightHeroIntoPosition(characters, 1f))
            .AppendCallback(() => {/* Spawn wave */ Debug.Log("SpawnWave");})
            .AppendCallback(() => {/* Tween wave in from right, wait for it to finish */})
            .AppendInterval(.5f)
            .AppendCallback(() => { /* Show Good Luck!!! GUI */ Debug.Log("GoodLuck!!!"); })
            .AppendInterval(.5f)
            .AppendCallback(() => {/* Hide Good Luck!!! GUI*/ })
            .AppendCallback(() => {/* Kick off wave */})
            .Play();

        // When the current wave finishes, start a new one.
        // If we have no more waves left, start end sequence




        /*var _spawnedEnemies = _enemies.Select(e =>
            {
                var result = GameObject.Instantiate(_enemyPrefab).GetComponent<Enemy>();
                result.Pattern = e.Pattern;
                result.Target = e.Target;
                return result;
            })
            .ToList();
*/
        // Spawn first 5 enemies off screen
    }

    // TODO: Should figure out how to condense nested tween sequences
    public Tween GetLeftHeroIntoPosition(List<Jumpable> characters, float time)
    {
        var start = GetTarget(TargetSpot.TargetType.LeftStart);
        var leftChar = characters.First(j => j.gameObject.name.Contains("Left"));
        var target = GetTarget(TargetSpot.TargetType.LeftTarget);
        
        return leftChar.TweenFromStartToFinish(start.transform.position, target.transform.position, time);
    }

    public Tween GetRightHeroIntoPosition(List<Jumpable> characters, float time)
    {
        var start = GetTarget(TargetSpot.TargetType.RightStart);
        var rightChar = characters.First(j => j.gameObject.name.Contains("Right"));
        var target = GetTarget(TargetSpot.TargetType.RightTarget);

        return rightChar.TweenFromStartToFinish(start.transform.position, target.transform.position, time);
    }

    private TargetSpot GetTarget(TargetSpot.TargetType target)
    {
        return FindObjectsOfType<TargetSpot>().FirstOrDefault(t => t.Target == target);
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
