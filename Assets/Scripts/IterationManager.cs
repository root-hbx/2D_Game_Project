using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class IterationManager : MonoBehaviour
{
    IterationSwitchUI iterationSwitchUI;
    RecordAction recordAction;
    TimeLimit timeLimitBar;
    BlockManager blockManager;

    bool started;
    bool iterationCompleted;
    bool levelCompleted;

    public int iterations = 1;
    [HideInInspector]
    public int currentIteration = 1;
    public bool IsHeroIteration => currentIteration % 2 == 1;
    public bool useBlockManager = false;

    public Vector3 heroPosition = new(0, 0, 0);
    public List<Vector3> enemyPosition = new();
    public Vector3 destinationPosition = new(0, 0, 0);

    public string levelName;
    public bool allowAliceShoot = true;

    List<List<InputKey>> enemyActions = new();
    List<List<InputKey>> heroActions = new();

    void Awake()
    {
        iterationSwitchUI = FindObjectOfType<IterationSwitchUI>();
        Assert.IsNotNull(iterationSwitchUI, "IterationSwitchUI not found");
        Assert.IsTrue(enemyPosition.Count == iterations / 2, "Enemy positions not set correctly");

        timeLimitBar = GetComponent<TimeLimit>();
        Assert.IsNotNull(timeLimitBar, "TimeLimitBar not found");

        if (useBlockManager)
        {
            blockManager = GetComponent<BlockManager>();
            Assert.IsNotNull(blockManager, "BlockManager not found");
        }
    }

    void Start()
    {
        recordAction = gameObject.AddComponent<RecordAction>();
        iterationSwitchUI.ShowContent(IterationSwitchUI.MessageType.Start);
        if (levelName == null || levelName == "")
        {
            levelName = "Level ???";
        }
        iterationSwitchUI.ShowLevelName(levelName);
        LoadIteration();
    }

    void Update()
    {
        if (Input.anyKeyDown && !(Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.JoystickButton3) || Input.GetKeyDown(KeyCode.B)))
        {
            StartIteration();
        }

        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            BackIteration();
        }
    }

    #region Iteration Complete API
    public void NextIteration()
    {
        if (iterationCompleted)
        {
            return;
        }

        Assert.IsTrue(currentIteration <= iterations);

        if (levelCompleted) { }
        else if (IsHeroIteration)
        {
            heroActions.Add(recordAction.TakeActions());
        }
        else
        {
            enemyActions.Add(recordAction.TakeActions());
        }

        if (currentIteration == iterations)
        {
            levelCompleted = true;
            IterationCompleted();
            iterationSwitchUI.ShowContent(IterationSwitchUI.MessageType.NextLevel);
            return;
        }

        currentIteration++;
        IterationCompleted();
        iterationSwitchUI.ShowContent(IterationSwitchUI.MessageType.Pass);
    }

    public void GameOver()
    {
        if (iterationCompleted)
        {
            return;
        }

        IterationCompleted();
        iterationSwitchUI.ShowContent(IterationSwitchUI.MessageType.GameOver);
    }
    #endregion

    IEnumerator DelayBeforeRemoveObjects()
    {
        yield return new WaitForSeconds(0.3f);
        timeLimitBar.Reset();
        foreach (var spikeBlock in GameObject.FindGameObjectsWithTag("SpikeBlock"))
        {
            spikeBlock.GetComponent<SpikeBlockBehaviour>().Reset();
        }
        if (useBlockManager)
            blockManager.Reset();
        RemoveObjects();
        LoadIteration();
    }

    void IterationCompleted()
    {
        iterationCompleted = true;
        UpdateCharacterMoveState(false);
        PlayDisappearingAnimation();
        StartCoroutine(DelayBeforeRemoveObjects());
    }

    void PlayDisappearingAnimation()
    {
        var heroes = GameObject.FindGameObjectsWithTag("Hero");
        foreach (var hero in heroes)
        {
            hero.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            hero.GetComponent<Animator>().SetTrigger("Disappear");
        }

        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            enemy.GetComponent<Animator>().SetTrigger("Disappear");
        }
    }

    void UpdateCharacterMoveState(bool ableMove)
    {
        var heroes = GameObject.FindGameObjectsWithTag("Hero");
        foreach (var hero in heroes)
        {
            hero.GetComponent<MoveBehaviour>().enabled = ableMove;
            if(allowAliceShoot)
                hero.GetComponent<ShootBehaviour>().enabled = ableMove;
            else
                hero.GetComponent<ShootBehaviour>().enabled = false;
        }

        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<MoveBehaviour>().enabled = ableMove;
            enemy.GetComponent<ShootBehaviour>().enabled = ableMove;
        }
    }

    void RemoveObjects()
    {
        var heroes = GameObject.FindGameObjectsWithTag("Hero");
        foreach (var hero in heroes)
        {
            Destroy(hero);
        }

        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            Destroy(enemy);
        }

        var bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (var bullet in bullets)
        {
            Destroy(bullet);
        }

        var dest = GameObject.FindGameObjectsWithTag("Destination");
        foreach (var d in dest)
        {

            Destroy(d);
        }
    }

    void LoadIteration()
    {
        if (levelCompleted)
        {
            var aiHero = Instantiate(Resources.Load<GameObject>("Prefabs/Hero"), heroPosition, Quaternion.identity);
            aiHero.GetComponent<MoveBehaviour>().input = new RecordInput(heroActions[heroActions.Count - 1]);
            aiHero.GetComponent<ShootBehaviour>().input = new RecordInput(heroActions[heroActions.Count - 1]);
        }
        else if (IsHeroIteration)
        {
            var hero = Instantiate(Resources.Load<GameObject>("Prefabs/Hero"), heroPosition, Quaternion.identity);
            hero.GetComponent<MoveBehaviour>().AddIndicator();
        }
        else
        {
            var enemy = Instantiate(Resources.Load<GameObject>("Prefabs/Enemy"), enemyPosition[currentIteration / 2 - 1], Quaternion.identity);
            enemy.GetComponent<MoveBehaviour>().AddIndicator();
            var aiHero = Instantiate(Resources.Load<GameObject>("Prefabs/Hero"), heroPosition, Quaternion.identity);
            aiHero.GetComponent<MoveBehaviour>().input = new RecordInput(heroActions[heroActions.Count - 1]);
            aiHero.GetComponent<ShootBehaviour>().input = new RecordInput(heroActions[heroActions.Count - 1]);
        }

        for (int i = 0; i < (currentIteration + (levelCompleted ? 1 : 0) - 1) / 2; i++)
        {
            var aiEnemy = Instantiate(Resources.Load<GameObject>("Prefabs/Enemy"), enemyPosition[i], Quaternion.identity);
            aiEnemy.GetComponent<MoveBehaviour>().input = new RecordInput(enemyActions[i]);
            aiEnemy.GetComponent<ShootBehaviour>().input = new RecordInput(enemyActions[i]);
        }
        UpdateCharacterMoveState(false);

        Instantiate(Resources.Load<GameObject>("Prefabs/Destination"), destinationPosition, Quaternion.identity);

        started = false;
    }

    void StartIteration()
    {
        if (started)
        {
            return;
        }
        timeLimitBar.Run();
        iterationSwitchUI.StopShowContent();
        UpdateCharacterMoveState(true);
        recordAction.TakeActions();
        iterationCompleted = false;
        started = true;
        Random.InitState(42);
    }

    void BackIteration()
    {
        if (started)
        {
            IterationCompleted();
            if (levelCompleted)
            {
                iterationSwitchUI.ShowContent(IterationSwitchUI.MessageType.NextLevel);
            }
            else
            {
                iterationSwitchUI.ShowContent(IterationSwitchUI.MessageType.Start);
            }
            return;
        }

        if (currentIteration == 1)
        {
            return;
        }

        if (!levelCompleted)
        {
            currentIteration--;
        }
        levelCompleted = false;
        if (IsHeroIteration)
        {
            heroActions.RemoveAt(heroActions.Count - 1);
        }
        else
        {
            enemyActions.RemoveAt(enemyActions.Count - 1);
        }
        iterationSwitchUI.ShowContent(IterationSwitchUI.MessageType.Undo);
        IterationCompleted();
    }
}
