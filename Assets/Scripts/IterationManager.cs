using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class IterationManager : IManualBehaviour
{
    IterationSwitchUI iterationSwitchUI;
    RecordAction recordAction;
    TimeLimit timeLimitBar;
    BlockManager blockManager;

    public AudioManager audioManager;

    bool IterStarted;
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

    private bool isWaitingDelayBeforeRemoveObjects = false;

    List<List<InputKey>> enemyActions = new();
    List<List<InputKey>> heroActions = new();

    new void Awake()
    {
        base.Awake();

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
        audioManager = gameObject.AddComponent<AudioManager>();
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
        audioManager.Play(AudioManager.AudioList.bgmForPlaying, true);
    }

    public override void ManualUpdate()
    {
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
            FindObjectOfType<SpeedrunRecorder>().Complete();
            levelCompleted = true;
            IterationCompleted();
            iterationSwitchUI.ShowContent(IterationSwitchUI.MessageType.NextLevel);
            audioManager.Play(AudioManager.AudioList.bgmWin, false);
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

    IEnumerator DelayBeforeRemoveObjects(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!isWaitingDelayBeforeRemoveObjects)
            yield break;

        timeLimitBar.Reset();
        foreach (var spikeBlock in GameObject.FindGameObjectsWithTag("SpikeBlock"))
        {
            spikeBlock.GetComponent<SpikeBlockBehaviour>().Reset();
        }
        if (useBlockManager)
            blockManager.Reset();
        RemoveObjects();
        LoadIteration();
        isWaitingDelayBeforeRemoveObjects = false;
    }

    void IterationCompleted(bool isUndo = false)
    {
        iterationCompleted = true;
        UpdateCharacterMoveState(false);
        StopRunningAnim();
        isWaitingDelayBeforeRemoveObjects = true;
        StartCoroutine(DelayBeforeRemoveObjects(isUndo ? 0f : 1f));
    }

    void StopRunningAnim()
    {
        var heroes = GameObject.FindGameObjectsWithTag("Hero");
        foreach (var hero in heroes)
        {
            hero.GetComponent<Rigidbody2D>().velocity = new Vector2(0, hero.GetComponent<Rigidbody2D>().velocity.y);
            hero.GetComponent<Animator>().SetBool("isRunning", false);
        }

        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<Rigidbody2D>().velocity = new Vector2(0, enemy.GetComponent<Rigidbody2D>().velocity.y);
            enemy.GetComponent<Animator>().SetBool("isRunning", false);
        }
    }

    void UpdateCharacterMoveState(bool ableMove)
    {
        var heroes = GameObject.FindGameObjectsWithTag("Hero");
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var hero in heroes)
        {
            hero.GetComponent<MoveBehaviour>().enabled = ableMove;
            if (allowAliceShoot)
                hero.GetComponent<ShootBehaviour>().enabled = ableMove;
            else
                hero.GetComponent<ShootBehaviour>().enabled = false;
        }

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
            aiHero.GetComponentInChildren<MoveBehaviour>().input = new RecordInput(heroActions[heroActions.Count - 1]);
            aiHero.GetComponentInChildren<ShootBehaviour>().input = new RecordInput(heroActions[heroActions.Count - 1]);
            audioManager.Play(AudioManager.AudioList.bgmForPlaying, true);
        }
        else if (IsHeroIteration)
        {
            var hero = Instantiate(Resources.Load<GameObject>("Prefabs/Hero"), heroPosition, Quaternion.identity);
            hero.GetComponentInChildren<MoveBehaviour>().AddIndicator();
            iterationSwitchUI.ShowGoal(IterationSwitchUI.GoalType.ReachDestination);
        }
        else
        {
            var enemy = Instantiate(Resources.Load<GameObject>("Prefabs/Enemy"), enemyPosition[currentIteration / 2 - 1], Quaternion.identity);
            enemy.GetComponentInChildren<MoveBehaviour>().AddIndicator();
            var aiHero = Instantiate(Resources.Load<GameObject>("Prefabs/Hero"), heroPosition, Quaternion.identity);
            aiHero.GetComponentInChildren<MoveBehaviour>().input = new RecordInput(heroActions[heroActions.Count - 1]);
            aiHero.GetComponentInChildren<ShootBehaviour>().input = new RecordInput(heroActions[heroActions.Count - 1]);
            iterationSwitchUI.ShowGoal(IterationSwitchUI.GoalType.KillHero);
        }

        for (int i = 0; i < (currentIteration + (levelCompleted ? 1 : 0) - 1) / 2; i++)
        {
            var aiEnemy = Instantiate(Resources.Load<GameObject>("Prefabs/Enemy"), enemyPosition[i], Quaternion.identity);
            aiEnemy.GetComponentInChildren<MoveBehaviour>().input = new RecordInput(enemyActions[i]);
            aiEnemy.GetComponentInChildren<ShootBehaviour>().input = new RecordInput(enemyActions[i]);
        }
        UpdateCharacterMoveState(false);

        Instantiate(Resources.Load<GameObject>("Prefabs/Destination"), destinationPosition, Quaternion.identity);
        IterStarted = false;

        if (!levelCompleted)
            iterationSwitchUI.ShowContent(IterationSwitchUI.MessageType.Start);
    }

    void StartIteration()
    {
        if (IterStarted)
        {
            return;
        }
        timeLimitBar.Run();
        iterationSwitchUI.StopShowContent();
        UpdateCharacterMoveState(true);
        recordAction.TakeActions();
        iterationCompleted = false;
        IterStarted = true;
        Random.InitState(42);
    }

    void BackIteration()
    {
        if (levelCompleted)
        {
            return;
        }
        if (IterStarted)
        {
            IterationCompleted(true);
            if (levelCompleted)
            {
                iterationSwitchUI.ShowContent(IterationSwitchUI.MessageType.NextLevel);
            }
            else
            {
                iterationSwitchUI.ShowContent(IterationSwitchUI.MessageType.Undo);
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
        IterationCompleted(true);
    }
}
