using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    [SerializeField] GameObject[] skillButtons = null;

    int score = 0;
    int currentScore = 0;
    int scoreGap = 1000;
    float slowDownFactor = 0.05f;
    float speedUpTime = 2f;
    bool timeSlowed = true;
 
    private void Awake()
    {    
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
        if (!timeSlowed && Time.timeScale <= 1)
        {
            Time.timeScale += (1f / speedUpTime) * Time.unscaledDeltaTime;
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
        foreach (GameObject button in skillButtons)
        {
            button.SetActive(true);
        }
        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * .2f;
        timeSlowed = true;
    }

    public void SkillSelected(string type)
    {
        HandleSkillSelection(type);
        timeSlowed = false;
        foreach (GameObject button in skillButtons)
        {
            button.SetActive(false);
            scoreGap = currentScore + 1000;
        }
    }

    private void HandleSkillSelection(string type)
    {
        switch (type)
        {
            case "health":
                FindObjectOfType<Player>().IncreaseHealth(100);
                break;
            case "explosion":
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
    }
}
