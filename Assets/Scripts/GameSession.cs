using System;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    [SerializeField] GameObject[] placeForSkills = null;
    [SerializeField] GameObject[] skills = null;
    [SerializeField] Canvas skillCanvas = null;

    public static GameSession Instance;

    int score = 0;
    int currentScore = 0;
    int scoreGap = 3500;
    float slowDownFactor = 0.05f;
    float slowDownLength = 1f;

    Boolean slowDownTime = false;
    Boolean speedUpTime = false;

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
                Time.fixedDeltaTime = Time.timeScale * .2f;
            } else {
                Time.timeScale = 0;
                slowDownTime = false;
            }
        } else if (speedUpTime) {
            if (Time.timeScale > 1) {
                Time.timeScale = 1;
                Time.fixedDeltaTime = Time.timeScale * .2f;
                this.speedUpTime = false;
            } else {
                Time.timeScale += (1 / slowDownLength) * Time.unscaledDeltaTime;
                Time.fixedDeltaTime = Time.timeScale * .2f;
            }
        }
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
        currentScore += scoreToAdd;

        if(currentScore >= scoreGap)
        {
            SlowTimeForSkillSelection();
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
        foreach (GameObject objForPosition in placeForSkills)
        {
            Vector3 position = objForPosition.transform.position;
            GameObject skill = Instantiate(skills[0], position, Quaternion.identity);
            skill.transform.SetParent(skillCanvas.transform);
        }
    }

    public static void HandleSkillSelection(Abilty.AbilityType type)
    {
        switch (type)
        {
            case Abilty.AbilityType.Health:
                FindObjectOfType<Player>().IncreaseHealth(100);
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
    }
}
