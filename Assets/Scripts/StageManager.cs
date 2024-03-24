using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    StageSwitchUI stageSwitchUI;
    RecordAction recordAction;

    bool started;
    bool iterationCompleted;
    bool levelCompleted;

    public int stages = 1;
    public int currentStage = 1;
    public bool IsHeroStage => currentStage % 2 == 1;

    public Vector3 heroPosition = new(0, 0, 0);
    public List<Vector3> enemyPosition = new();
    public Vector3 destinationPosition = new(0, 0, 0);
    private CooldownBar cooldownBar;
    private BlockManager blockManager;


    List<List<InputKey>> enemyActions = new();
    List<List<InputKey>> heroActions = new();

    void Awake()
    {
        stageSwitchUI = FindObjectOfType<StageSwitchUI>();
        Assert.IsNotNull(stageSwitchUI, "StageSwitchUI not found");
        Assert.IsTrue(enemyPosition.Count == stages / 2, "Enemy positions not set correctly");

        cooldownBar = GetComponent<CooldownBar>();
        Assert.IsNotNull(cooldownBar, "CooldownBar not found");

        blockManager = GetComponent<BlockManager>();
        Assert.IsNotNull(blockManager, "BlockManager not found");
    }

    void Start()
    {
        recordAction = gameObject.AddComponent<RecordAction>();
        stageSwitchUI.ShowContent(StageSwitchUI.MessageType.Start);
        LoadStage();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton4))
        {
            StartStage();
        }

        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            BackStage();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            SceneManager.LoadSceneAsync("Directory");
        }
    }

    #region Stage Complete Functions
    public void NextStage()
    {
        if (iterationCompleted)
        {
            return;
        }

        Assert.IsTrue(currentStage <= stages);

        if (levelCompleted) { }
        else if (IsHeroStage)
        {
            heroActions.Add(recordAction.TakeActions());
        }
        else
        {
            enemyActions.Add(recordAction.TakeActions());
        }

        if (currentStage == stages)
        {
            levelCompleted = true;
            StageCompleted();
            stageSwitchUI.ShowContent(StageSwitchUI.MessageType.NextLevel);
            return;
        }

        currentStage++;
        StageCompleted();
        stageSwitchUI.ShowContent(StageSwitchUI.MessageType.Pass);
    }

    public void GameOver()
    {
        if (iterationCompleted)
        {
            return;
        }

        StageCompleted();
        stageSwitchUI.ShowContent(StageSwitchUI.MessageType.GameOver);
    }
    #endregion

    IEnumerator DelayBeforeRemoveObjects()
    {
        yield return new WaitForSeconds(0.3f);
        RemoveObjects();
        LoadStage();
    }

    void StageCompleted()
    {
        iterationCompleted = true;
        cooldownBar.Reset();
        blockManager.Reset();
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
            hero.GetComponent<ShootBehaviour>().enabled = ableMove;
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

    void LoadStage()
    {
        if (levelCompleted)
        {
            var aiHero = Instantiate(Resources.Load<GameObject>("Prefabs/Hero"), heroPosition, Quaternion.identity);
            aiHero.GetComponent<MoveBehaviour>().input = new RecordInput(heroActions[heroActions.Count - 1]);
            aiHero.GetComponent<ShootBehaviour>().input = new RecordInput(heroActions[heroActions.Count - 1]);
        }
        else if (IsHeroStage)
        {
            var hero = Instantiate(Resources.Load<GameObject>("Prefabs/Hero"), heroPosition, Quaternion.identity);
            hero.GetComponent<MoveBehaviour>().AddIndicator();
        }
        else
        {
            var enemy = Instantiate(Resources.Load<GameObject>("Prefabs/Enemy"), enemyPosition[currentStage / 2 - 1], Quaternion.identity);
            enemy.GetComponent<MoveBehaviour>().AddIndicator();
            var aiHero = Instantiate(Resources.Load<GameObject>("Prefabs/Hero"), heroPosition, Quaternion.identity);
            aiHero.GetComponent<MoveBehaviour>().input = new RecordInput(heroActions[heroActions.Count - 1]);
            aiHero.GetComponent<ShootBehaviour>().input = new RecordInput(heroActions[heroActions.Count - 1]);
        }

        for (int i = 0; i < (currentStage + (levelCompleted ? 1 : 0) - 1) / 2; i++)
        {
            var aiEnemy = Instantiate(Resources.Load<GameObject>("Prefabs/Enemy"), enemyPosition[i], Quaternion.identity);
            aiEnemy.GetComponent<MoveBehaviour>().input = new RecordInput(enemyActions[i]);
            aiEnemy.GetComponent<ShootBehaviour>().input = new RecordInput(enemyActions[i]);
        }
        UpdateCharacterMoveState(false);

        Instantiate(Resources.Load<GameObject>("Prefabs/Destination"), destinationPosition, Quaternion.identity);

        started = false;
    }

    void StartStage()
    {
        if (started)
        {
            return;
        }
        cooldownBar.StartCooldown();
        stageSwitchUI.StopShowContent();
        UpdateCharacterMoveState(true);
        recordAction.TakeActions();
        iterationCompleted = false;
        started = true;
        Random.InitState(42);
    }

    void BackStage()
    {
        if (started)
        {
            StageCompleted();
            if (levelCompleted)
            {
                stageSwitchUI.ShowContent(StageSwitchUI.MessageType.NextLevel);
            }
            else
            {
                stageSwitchUI.ShowContent(StageSwitchUI.MessageType.Start);
            }
            return;
        }

        if (currentStage == 1)
        {
            return;
        }

        if (!levelCompleted)
        {
            currentStage--;
        }
        levelCompleted = false;
        if (IsHeroStage)
        {
            heroActions.RemoveAt(heroActions.Count - 1);
        }
        else
        {
            enemyActions.RemoveAt(enemyActions.Count - 1);
        }
        stageSwitchUI.ShowContent(StageSwitchUI.MessageType.Undo);
        StageCompleted();
    }
}
