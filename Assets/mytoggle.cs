using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class mytoggle : MonoBehaviour {
    private Toggle toggle;
    Transform son1;
    Transform son2;
   
	// Use this for initialization
	void Start () {
        toggle = GetComponent<Toggle>();
        son1 = transform.Find("Open");
        son2 = transform.Find("Close");
        OnValueChange(toggle .isOn  );
	}
	public void OnValueChange(bool isOn )
    {
        son1.gameObject.SetActive(isOn);
        son2.gameObject.SetActive(!isOn);
       
    }
	// Update is called once per frame
	void Update () {
		
	}
}
