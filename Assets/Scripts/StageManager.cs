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
    static StageManager instance;
    StageSwitchUI stageSwitchUI;
    RecordAction recordAction;

    bool completed;
    public int stages = 1;
    public int currentStage = 1;
    public bool IsHeroStage
    {
        get
        {
            return currentStage % 2 == 1;
        }
    }
    private bool readyToStart = false;

    public Vector3 heroPosition = new(0, 0, 0);
    public Vector3 enemyPosition = new(0, 0, 0);

    List<List<InputKey>> enemyActions = new();
    List<InputKey> heroActions;

    // void Awake()
    // {
    //     if (instance == null)
    //     {
    //         instance = this;
    //         DontDestroyOnLoad(gameObject);
    //         stageSwitchUI = FindObjectOfType<StageSwitchUI>();
    //         recordAction = FindObjectOfType<RecordAction>();
    //     }
    //     else
    //     {
    //         Destroy(gameObject);
    //         instance.LoadStage();
    //     }
    // }

    void Start()
    {
        LoadStage();
    }

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

        StageCompleted();
        currentStage++;
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

    void StageCompleted()
    {
        completed = true;
        DisableMove();
        removeObjects();
    }

    void DisableMove(bool isDisabling = false)
    {
        var heroes = GameObject.FindGameObjectsWithTag("Hero");
        foreach (var hero in heroes)
        {
            hero.GetComponent<MoveBehaviour>().enabled = isDisabling;
            hero.GetComponent<ShootBehaviour>().enabled = isDisabling;
        }

        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<MoveBehaviour>().enabled = isDisabling;
            enemy.GetComponent<ShootBehaviour>().enabled = isDisabling;
        }
    }
    void removeObjects()
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
        DisableMove();
        stageSwitchUI.ShowStartIndicator(true);
        readyToStart = true;
        // now wait for the return key to call StartStage()
    }
    void StartStage()
    {
        if (readyToStart)
        {
            stageSwitchUI.ShowStartIndicator(false);
            DisableMove(true);
            recordAction.TakeActions();
            completed = false;
        }
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartStage();
        }
    }
}
