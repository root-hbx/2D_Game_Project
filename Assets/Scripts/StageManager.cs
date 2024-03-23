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
    public Vector3 enemyPosition = new(0, 0, 0);

    List<List<InputKey>> enemyActions = new();
    List<List<InputKey>> heroActions = new();

    void Awake()
    {
        stageSwitchUI = FindObjectOfType<StageSwitchUI>();
        Assert.IsNotNull(stageSwitchUI, "StageSwitchUI not found");
    }

    void Start()
    {
        recordAction = gameObject.AddComponent<RecordAction>();
        LoadStage();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartStage();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            BackStage();
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
            stageSwitchUI.NextLevel();
            return;
        }

        currentStage++;
        StageCompleted();
    }

    public void GameOver()
    {
        if (iterationCompleted)
        {
            return;
        }

        StageCompleted();
        stageSwitchUI.GameOver();
    }
    #endregion

    void StageCompleted()
    {
        iterationCompleted = true;
        UpdateCharacterMoveState(false);
        RemoveObjects();
        LoadStage();
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
    }

    void LoadStage()
    {
        Debug.Log("Stage " + currentStage);

        if (levelCompleted)
        {
            var aiHero = Instantiate(Resources.Load<GameObject>("Prefabs/Hero"), heroPosition, Quaternion.identity);
            aiHero.GetComponent<MoveBehaviour>().input = new RecordInput(heroActions[heroActions.Count - 1]);
            aiHero.GetComponent<ShootBehaviour>().input = new RecordInput(heroActions[heroActions.Count - 1]);
            Debug.Log("Instantiate ai hero");
        }
        else if (IsHeroStage)
        {
            var hero = Instantiate(Resources.Load<GameObject>("Prefabs/Hero"), heroPosition, Quaternion.identity);
            Debug.Log("Instantiate hero");
            hero.GetComponent<MoveBehaviour>().AddIndicator();
        }
        else
        {
            var enemy = Instantiate(Resources.Load<GameObject>("Prefabs/Enemy"), enemyPosition, Quaternion.identity);
            Debug.Log("Instantiate enemy");
            enemy.GetComponent<MoveBehaviour>().AddIndicator();
            var aiHero = Instantiate(Resources.Load<GameObject>("Prefabs/Hero"), heroPosition, Quaternion.identity);
            aiHero.GetComponent<MoveBehaviour>().input = new RecordInput(heroActions[heroActions.Count - 1]);
            aiHero.GetComponent<ShootBehaviour>().input = new RecordInput(heroActions[heroActions.Count - 1]);
            Debug.Log("Instantiate ai hero");
        }

        for (int i = 0; i < (currentStage + (levelCompleted ? 1 : 0) - 1) / 2; i++)
        {
            var aiEnemy = Instantiate(Resources.Load<GameObject>("Prefabs/Enemy"), enemyPosition, Quaternion.identity);
            aiEnemy.GetComponent<MoveBehaviour>().input = new RecordInput(enemyActions[i]);
            aiEnemy.GetComponent<ShootBehaviour>().input = new RecordInput(enemyActions[i]);
            Debug.Log("Instantiate ai enemy");
        }
        UpdateCharacterMoveState(false);
        stageSwitchUI.ShowStartIndicator(true);
        started = false;
    }

    void StartStage()
    {
        if (started)
        {
            return;
        }

        stageSwitchUI.ShowStartIndicator(false);
        UpdateCharacterMoveState(true);
        recordAction.TakeActions();
        iterationCompleted = false;
        started = true;
    }

    void BackStage()
    {
        if (started)
        {
            StageCompleted();
            if (levelCompleted)
            {
                stageSwitchUI.NextLevel();
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
        StageCompleted();
    }
}
