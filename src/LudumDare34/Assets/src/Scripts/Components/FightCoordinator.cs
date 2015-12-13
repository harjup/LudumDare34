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
        _enemyPrefab = Resources.Load<GameObject>("Prefabs/EnemyRoot");
        Debug.Log(_enemyPrefab);
        InitFight(GenerateListOfEnemies(5));
    }

    public void InitFight(List<EnemyData> firstWave)
    {
        _enemies = firstWave;
        var wave = new List<Enemy>();

        var characters = FindObjectsOfType<Jumpable>().ToList();

        DOTween.Sequence()
            .AppendCallback(() => {/* Show start fight GUI*/ Debug.Log("Start Fight!!!"); })
            .AppendInterval(.5f)
            .AppendCallback(() => {/* Hide start fight GUI*/ })
            //Walk two heros in at the same time!!!
            .Append(GetLeftHeroIntoPosition(characters, 1f))
            .Join(GetRightHeroIntoPosition(characters, 1f))
            .AppendCallback(() => { wave = SpawnEnemies(); })
            .AppendInterval(.1f)
            .AppendCallback(() =>
            {
                Debug.Log(wave.Count);
                var start = GetTarget(TargetSpot.TargetType.EnemyStart).transform.position;
                var target = GetTarget(TargetSpot.TargetType.EnemyTarget).transform.position;

                wave.Select(w => w.WalkTo(start, target, 1f, wave.IndexOf(w)))
                    .ToList()
                    .ForEach(t => t.Play());
            })
            .AppendInterval(1f)
            .AppendCallback(() => { /* Show Good Luck!!! GUI */ Debug.Log("GoodLuck!!!"); })
            .AppendInterval(.5f)
            .AppendCallback(() => {/* Hide Good Luck!!! GUI*/ })
            .AppendCallback(() => {/* Kick off wave */})
            .Play();

        // When the current wave finishes, start a new one.
        // If we have no more waves left, start end sequence




        /*var _spawnedEnemies = 
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

    public List<Enemy> SpawnEnemies()
    {
        var start = GetTarget(TargetSpot.TargetType.EnemyStart).transform.position;
        Debug.Log(_enemies.Count);
        return _enemies.Select(e =>
        {
            var result = Instantiate(_enemyPrefab).GetComponent<Enemy>();
            result.transform.position = start;
            result.Pattern = e.Pattern;
            result.Target = e.Target;
            return result;
        }).ToList();
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
