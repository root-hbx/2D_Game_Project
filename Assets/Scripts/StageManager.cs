using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

/// <summary>
/// LoadStage() -> NextStage() or GameOver() being called -> Call LoadStage() again
/// Callable functions: NextStage(), GameOver()
/// </summary>
public class StageManager : MonoBehaviour
{
    StageSwitchUI stageSwitchUI;
    RecordAction recordAction;

    bool completed;
    bool started;

    public int stages = 1;
    public int currentStage = 1;
    public bool IsHeroStage => currentStage % 2 == 1;

    public Vector3 heroPosition = new(0, 0, 0);
    public Vector3 enemyPosition = new(0, 0, 0);

    List<List<InputKey>> enemyActions = new();
    List<InputKey> heroActions;

    void Awake()
    {
        stageSwitchUI = FindObjectOfType<StageSwitchUI>();
    }

    void Start()
    {
        LoadStage();
        recordAction = gameObject.AddComponent<RecordAction>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartStage();
        }
    }

    #region Stage Complete Functions
    public void NextStage()
    {
        if (completed)
        {
            return;
        }

        Assert.IsTrue(currentStage <= stages);
        if (currentStage == stages)
        {
            StageCompleted();
            stageSwitchUI.NextLevel();
            return;
        }

        if (IsHeroStage)
        {
            heroActions = recordAction.TakeActions();
        }
        else
        {
            enemyActions.Add(recordAction.TakeActions());
        }

        currentStage++;
        StageCompleted();
    }

    public void GameOver()
    {
        if (completed)
        {
            return;
        }

        StageCompleted();
        stageSwitchUI.GameOver();
    }
    #endregion

    void StageCompleted()
    {
        completed = true;
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
    }

    void LoadStage()
    {
        Debug.Log("Stage " + currentStage);
        if (IsHeroStage)
        {
            Instantiate(Resources.Load("Prefabs/Hero"), heroPosition, Quaternion.identity);
            Debug.Log("Instantiate hero");
        }
        else
        {
            Instantiate(Resources.Load("Prefabs/Enemy"), enemyPosition, Quaternion.identity);
            Debug.Log("Instantiate enemy");
            var aiHero = Instantiate(Resources.Load<GameObject>("Prefabs/Hero"), heroPosition, Quaternion.identity);
            aiHero.GetComponent<MoveBehaviour>().input = new RecordInput(heroActions);
            aiHero.GetComponent<ShootBehaviour>().input = new RecordInput(heroActions);
            Debug.Log("Instantiate ai hero");
        }

        for (int i = 0; i < (currentStage - 1) / 2; i++)
        {
            var aiEnemy = Instantiate(Resources.Load<GameObject>("Prefabs/Enemy"), enemyPosition, Quaternion.identity);
            aiEnemy.GetComponent<MoveBehaviour>().input = new RecordInput(enemyActions[i]);
            aiEnemy.GetComponent<ShootBehaviour>().input = new RecordInput(enemyActions[i]);
            Debug.Log("Instantiate ai enemy");
        }
        UpdateCharacterMoveState(false);
        stageSwitchUI.ShowStartIndicator(true);
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
        completed = false;
    }
}
