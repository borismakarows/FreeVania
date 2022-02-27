using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class nextScene : MonoBehaviour
{
    [SerializeField] float delayToOtherScene = 2.2f;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(NextLevel());
        }
    }
    
    IEnumerator NextLevel()
    {
        yield return new WaitForSecondsRealtime(delayToOtherScene);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentSceneIndex +1;
        if (nextScene == SceneManager.sceneCountInBuildSettings)
        {
            nextScene = 0;
        }
        FindObjectOfType<scenePersist>().ResetTheScene();
        SceneManager.LoadScene(nextScene);
    }



}
