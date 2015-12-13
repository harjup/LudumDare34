using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class FightCoordinator : MonoBehaviour
{
    private List<EnemyData> _enemies;
    
    private List<string> _battleEvents;

    private GameObject _enemyPrefab;

    private GuiManager _guiManager;
    
    private Spawner _spawner;

    private List<Enemy> _activeEnemies;

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

        _guiManager = FindObjectOfType<GuiManager>();

        Debug.Log(_enemyPrefab);
        InitFight(GenerateListOfEnemies(5));
    }

    public void InitFight(List<EnemyData> enemies)
    {
        _enemies = enemies;
        var firstWave = _enemies;

        var characters = FindObjectsOfType<Jumpable>().ToList();

        DOTween.Sequence()
            .AppendInterval(.5f)
            //Walk two heros in at the same time!!!
            .Append(GetLeftHeroIntoPosition(characters, 1f))
            .Join(GetRightHeroIntoPosition(characters, 1f))
            .AppendCallback(() => { _activeEnemies = SpawnEnemies(firstWave); })
            .AppendInterval(.1f)
            .AppendCallback(() =>
            {
                var start = GetTarget(TargetSpot.TargetType.EnemyStart).transform.position;
                var target = GetTarget(TargetSpot.TargetType.EnemyTarget).transform.position;

                Func<float> targetTime = () => (1f + UnityEngine.Random.Range(0f, .5f));

                _activeEnemies.Select(w => w.WalkTo(start, target, targetTime(), _activeEnemies.IndexOf(w)))
                    .ToList()
                    .ForEach(t => t.Play());
            })
            .AppendInterval(2f)
            .AppendCallback(() => { _guiManager.ShowIntroText();})
            .AppendInterval(1.5f)
            .AppendCallback(() => { _guiManager.ShowBattleGui(); })
            .AppendCallback(StartFight)
            .Play();

        // When the current wave finishes, start a new one.
        // If we have no more waves left, start end sequence

        
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

    public List<Enemy> SpawnEnemies(List<EnemyData> enemies)
    {
        var start = GetTarget(TargetSpot.TargetType.EnemyStart).transform.position;
        return enemies.Select(e =>
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
        StartCoroutine(FightingSeq());
    }


    public IEnumerator FightingSeq()
    {
        var routines = 
            _activeEnemies
            .AsRandom()
            .ToList()
            .Select(e => e.DoAttack());

        List<Coroutine> attackCoroutines = new List<Coroutine>();

        // Kick each one off one second after the other
        foreach (var r in routines)
        {
            yield return new WaitForSeconds(1f);
            attackCoroutines.Add(StartCoroutine(r));
        }

        // Wait for all to finish
        foreach (var coroutine in attackCoroutines)
        {
            yield return coroutine;
        }

        // Done!!!
        Debug.Log("Done!!!");
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
