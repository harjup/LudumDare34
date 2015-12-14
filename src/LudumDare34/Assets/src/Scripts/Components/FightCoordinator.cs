using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine.UI;

public class FightCoordinator : MonoBehaviour
{
    private List<EnemyData> _enemies;
    
    private List<string> _battleEvents;

    private GameObject _enemyPrefab;
    private GameObject _bossPrefab;

    private GuiManager _guiManager;

    private bool _playerIsDead = false;

    private List<Enemy> _activeEnemies;

    public int WaveIndex = 0;
    public int WaveSize = 5;
    public int TotalCount = 10;

    private Action<bool> _finishedCallback;

    public void Start()
    {
        _enemyPrefab = Resources.Load<GameObject>("Prefabs/EnemyRoot");
        _bossPrefab = Resources.Load<GameObject>("Prefabs/BossRoot");

        _guiManager = FindObjectOfType<GuiManager>();

        //InitFight(GenerateListOfEnemies(10).ToList(), (a) => { });
    }

    public List<EnemyData> GenerateListOfEnemies(int count)
    {
        var result = new List<EnemyData>();
        //var patterns = new List<Pattern> { Pattern.StraightLine, Pattern.MakeDecision };
        var patterns = new List<Pattern> { Pattern.StraightLine };
        var targets = new List<Target> { Target.Box, Target.Circle };


        for (int i = 0; i < count; i++)
        {
            var data = new EnemyData(patterns.AsRandom().First(), targets.AsRandom().First());
            result.Add(data);
        }

        return result;
    }


    public void InitFight(List<EnemyData> enemies, Action<bool> finishedCallback)
    {
        _enemies = enemies;
        _finishedCallback = finishedCallback;

        WaveIndex = 0;
        var firstWave = _enemies.GetPage(WaveIndex, WaveSize);

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
            var prefab = _enemyPrefab;
            if (e.Difficulty == GameContext.Difficulty.Boss)
            {
                prefab = _bossPrefab;
            }

            var result = Instantiate(prefab).GetComponent<Enemy>();
            result.transform.position = start;
            result.Pattern = e.Pattern;
            result.Target = e.Target;
            result.RunTime = e.RunTime;
            result.Difficulty = e.Difficulty;
            return result;
        }).ToList();
    }

    private TargetSpot GetTarget(TargetSpot.TargetType target)
    {
        return FindObjectsOfType<TargetSpot>().FirstOrDefault(t => t.Target == target);
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

        var isHard = _activeEnemies.First().Difficulty == GameContext.Difficulty.Hard;

        // Kick each one off one second after the other
        foreach (var r in routines)
        {
            if (isHard)
            {
                yield return new WaitForSeconds(.7f + UnityEngine.Random.Range(0f, .5f));
            }
            else
            {
                yield return new WaitForSeconds(1f + UnityEngine.Random.Range(0f, .5f));
            }

            
            attackCoroutines.Add(StartCoroutine(r));
        }

        // Wait for all to finish
        foreach (var coroutine in attackCoroutines)
        {
            yield return coroutine;
        }

        NextWave();
    }

    public void NextWave()
    {

        if (_playerIsDead)
        {
            return;
        }

        WaveIndex++;
        var wave = _enemies.GetPage(WaveIndex, WaveSize);


        if (wave.Count == 0)
        {
            FightOver();
            return;
        }

        foreach (var activeEnemy in _activeEnemies)
        {
            activeEnemy.CleanUp();
        }


        DOTween.Sequence()
            .AppendCallback(() =>
            {
                _activeEnemies = SpawnEnemies(wave);
            })
            .AppendInterval(.25f)
            .AppendCallback(() =>
            {
                var start = GetTarget(TargetSpot.TargetType.EnemyStart).transform.position;
                var target = GetTarget(TargetSpot.TargetType.EnemyTarget).transform.position;

                Func<float> targetTime = () => (1f + UnityEngine.Random.Range(0f, .5f));

                _activeEnemies.Select(w => w.WalkTo(start, target, targetTime(), _activeEnemies.IndexOf(w)))
                    .ToList()
                    .ForEach(t => t.Play());
            })
            .AppendCallback(StartFight)
            .Play();
    }

    public void FightOver()
    {
        var characters = FindObjectsOfType<Jumpable>().ToList();

        DOTween.Sequence()
            .AppendInterval(.5f)
            .Append(characters.First().TweenWalkRight(2f))
            .Join(characters.Last().TweenWalkRight(2f))
            // TODO: Cameara Fade
            // TODO: Next area
            .AppendCallback(() => _finishedCallback(true))
            .Play();
    }

    public void StopWaves()
    {
        _playerIsDead = true;
    }

    public void ResetLevel()
    {
        //Debug.Log("ResetLevel ResetLevel");

        //TODO: Get callback working
        Application.LoadLevel("Scratch");
        //_finishedCallback(false);
    }
}

public static class EnumerableExtensions
{
    public static List<T> GetPage<T>(this List<T> data,  int page, int size)
    {
        return data.Skip(size * page).Take(size).ToList();
    } 
}