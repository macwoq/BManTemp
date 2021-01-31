using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuiButt : MonoBehaviour {

    public GameObject panel;

    void Awake()
    {
        Time.timeScale = 0f;        
    }


    public void AppQuit()
    {
        Debug.Log("Quit");
        Application.Quit();

    }


    public void ResumeGame()
    {
        panel.GetComponent<Animator>().Play("FadeOut");
        StartCoroutine(UnloadSceneWait());
    }

    IEnumerator UnloadSceneWait()
    {
        yield return new WaitForSecondsRealtime(1f);

        SceneManager.UnloadSceneAsync("Quit");

        Time.timeScale = 1f;
    }
}
