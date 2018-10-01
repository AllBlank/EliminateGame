using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonClick : MonoBehaviour,IPointerClickHandler {
    private Animator ani;
    private AudioSource aud;

    public UnityAction OnClick = null;

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            ani.SetTrigger("Pressed");
            aud.Play();
            if(OnClick != null)
            {
                OnClick();
            }          
        }
    }

    private void Awake () {
        ani = GetComponent<Animator>();
        aud = GameObject.Find("Canvas").GetComponent<AudioSource>();
	}
	
}
