using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreviewAndInfo : MonoBehaviour
{
    // Start is called before the first frame update
    public string scene;

    public void ExitMenu()
    {
        SceneManager.LoadScene(scene);
    }
}
