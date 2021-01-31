using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsOut : MonoBehaviour
{
    public GameObject inGameOptions;
    public GameObject mainPanel;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void panelUp()
    {
        
        inGameOptions.GetComponent<Animator>().Play("outAnim");
        mainPanel.SetActive(true);
        
    }
}
