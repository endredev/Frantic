using System;
using UnityEngine;
using System.Linq;

public class GameSession : MonoBehaviour
{
    [SerializeField] GameObject[] placeForSkills = null;
    [SerializeField] GameObject[] skills = null;
    [SerializeField] Canvas skillCanvas = null;

    public static GameSession Instance;

    int score = 0;
    int scoreIncrease = 50;
    int currentScore = 0;
    int scoreGapStatic = 3000;
    int scoreGap = 3500;
    float lastScoreUpdate = 5;
    float slowDownLength = 1f;

    Boolean slowDownTime = false;
    Boolean speedUpTime = false;
    Boolean skillSelectionUp = false;
    static Boolean gameStarted = false;

    private void Awake()
    {
        Instance = this;
        SetUpSingleton();
    }

    private void SetUpSingleton()
    {
        int numberGameSessions = FindObjectsOfType<GameSession>().Length;
        if(numberGameSessions > 1)
        {
            Destroy(gameObject);
        } else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update()
    {
        if (slowDownTime)
        {
            float amount = (1 / slowDownLength) * Time.unscaledDeltaTime;
            if (Time.timeScale - amount >= 0)
            {
                Time.timeScale -= (1 / slowDownLength) * Time.unscaledDeltaTime;
            } else {
                Time.timeScale = 0;
                slowDownTime = false;
            }
        } else if (speedUpTime) {
                Time.timeScale = 1;
            this.speedUpTime = false;
        }

        /* Score increase */
        if (Time.time - lastScoreUpdate >= 5f)
        {
            score += scoreIncrease;
            lastScoreUpdate = Time.time;
        }
    }

    public static Boolean GetGameStarted()
    {
        return gameStarted;
    }

    public void StartGame()
    {
        gameStarted = true;
    }

    public int GetScore()
    {
        return score;
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;

        if (score >= scoreGap && !skillSelectionUp)
        {
            SlowTimeForSkillSelection();
            skillSelectionUp = true;
            scoreGap = score + scoreGapStatic;
        }
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }

    public void SlowTimeForSkillSelection()
    {
        slowDownTime = true;
        ShowSkills();
    }

    private void ShowSkills()
    {
        GameObject[] skillsToShow = skills;
        int selectedSkills = 0;
        foreach (GameObject objForPosition in placeForSkills)
        {
            if (selectedSkills <= 1)
            {
                int skillIndex = UnityEngine.Random.Range(0, skillsToShow.Length);
                Vector3 position = objForPosition.transform.position;
                GameObject skill = Instantiate(skillsToShow[skillIndex], position, Quaternion.identity);
                skill.transform.SetParent(skillCanvas.transform);
                skillsToShow = skillsToShow.Where((source, index) => index != skillIndex).ToArray();
                selectedSkills++;
            }
        }
    }

    public static void HandleSkillSelection(Abilty.AbilityType type)
    {
        switch (type)
        {
            case Abilty.AbilityType.Health:
                FindObjectOfType<Player>().IncreaseHealth(100);
                break;
            case Abilty.AbilityType.FireRate:
                FindObjectOfType<Player>().IncreaseFireRate(25);
                break;
            case Abilty.AbilityType.Explosion:
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject enemy in enemies)
                {
                    enemy.GetComponent<Enemy>().GetDamage(1000);
                }
                break;
            default:
                Console.WriteLine("Default case");
                break;
        }

        GameObject[] buttons = GameObject.FindGameObjectsWithTag("SkillBtn");
        foreach (GameObject button in buttons)
        {
            Destroy(button);
        }
        GameSession.Instance.speedUpTime = true; 
        GameSession.Instance.skillSelectionUp = false;
    }
}
