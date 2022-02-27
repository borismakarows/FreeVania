using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class GameSession : MonoBehaviour
{
    public int playerLives = 3;
    [SerializeField] float delayForAnimation = 2.5f;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI PointText;
    float oneCoinPoint = 10f;
    float sumCoin = 0f;
    void Start() 
    {
        livesText.text = playerLives.ToString();   
        PointText.text = sumCoin.ToString(); 
    }
    
    void Awake()
    {
        int numGameSession = FindObjectsOfType<GameSession>().Length;
        if (numGameSession > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }


    public void ProccessPlayerDeath()
    {
        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            sumCoin = 0f;
            PointText.text = sumCoin.ToString();
            FindObjectOfType<PlayerMovement>().isAlive = false;
            StartCoroutine(ResetGameSession());
        }
    }

    void TakeLife()
    {
        playerLives--;
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
        livesText.text = playerLives.ToString();
    }

    IEnumerator ResetGameSession()
    {
        FindObjectOfType<PlayerMovement>().DieAnimation();
        yield return new WaitForSecondsRealtime(delayForAnimation);
        FindObjectOfType<scenePersist>().ResetTheScene();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    public void CoinPicked()
    {
        sumCoin += oneCoinPoint;
        PointText.text = sumCoin.ToString();
    }
}
