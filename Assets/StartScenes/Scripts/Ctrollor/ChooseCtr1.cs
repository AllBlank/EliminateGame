using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseCtr1 : UIbase {

    GameObject tmpUI;

    // Use this for initialization
    void Start () {
        tmpUI = GameObject.Find("UI");
        AddMyButtonListen("CancelButton_n", CancleClick);
        AddMyButtonListen("ShopButton_n", ShopClick);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void CancleClick()
    {
        tmpUI.transform.Find("ChooseMenu").gameObject.SetActive(false);
    }

    void ShopClick()
    {
        tmpUI.transform.Find("Store").gameObject.SetActive(true);
    }
}
