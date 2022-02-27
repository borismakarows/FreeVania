using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scenePersist : MonoBehaviour
{
    void Awake() 
    {
        int numScenePersists = FindObjectsOfType<scenePersist>().Length;
        if (numScenePersists > 1)
        {
            Destroy(gameObject);
        }        
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    public void ResetTheScene()
    {
        Destroy(gameObject);
    }
}
