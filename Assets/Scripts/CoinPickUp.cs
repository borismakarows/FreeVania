using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickUp : MonoBehaviour
{
    [SerializeField] AudioClip coinPickUpSFX;
    GameSession gameSession;
    bool wasCollected;
    
    void Start() 
    {
        gameSession = FindObjectOfType<GameSession>();    
    }


    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player" && wasCollected == false)
        {
            wasCollected = true;
            gameSession.CoinPicked();
            AudioSource.PlayClipAtPoint(coinPickUpSFX,Camera.main.transform.position);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else if (wasCollected)
        {
            Destroy(gameObject);
        }
    }
    
}
