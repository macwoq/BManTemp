using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    public GameObject popBtn;

    private void Update() {
        if(Input.anyKeyDown){
            popBtn.SetActive(false);
        }
    }
}
