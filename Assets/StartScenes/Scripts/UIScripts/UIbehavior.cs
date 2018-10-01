using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class UIbehavior : MonoBehaviour
{
    string panelName = "";
    private void Awake()
    {
        panelName = transform.GetComponentInParent<UIbase>().name;//
        UImanager.instance.RegistGameObj(panelName, transform.name, gameObject);
    }
    /// <summary>
    /// 所有的添加事件罗列出来
    /// </summary>
    /// <param name="tmpAction">回调</param>
    public void AddButtonListen(UnityAction tmpAction)
    {
        Button tmpBtn = GetComponent<Button>();
        if (tmpBtn != null)
        {
            tmpBtn.onClick.AddListener(tmpAction);//添加事件
        }
    }
    public void AddSliderListen(UnityAction<float> tmpAction)
    {
        Slider tmpSlr = gameObject.GetComponent<Slider>();
        if (tmpSlr != null)
        {
            tmpSlr.onValueChanged.AddListener(tmpAction);
        }


    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="tmpAction"></param>
    public void AddInputFieldEndListen(UnityAction<string > tmpAction)
    {
        InputField tmpInf = gameObject.GetComponent<InputField>();
        if (tmpInf != null)
        {
            tmpInf.onEndEdit.AddListener(tmpAction);
        }


    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="tmpAction"></param>
    public void AddInputValueChangeLieten(UnityAction<string> tmpAction)
    {
        InputField tmpInf = gameObject.GetComponent<InputField>();
        if (tmpInf != null)
        {
            tmpInf.onValueChanged.AddListener(tmpAction);
        }


    }



    /// <summary>
    /// 动态添加接口事件回调
    /// </summary>
    /// <param name="action"></param>
    public void AddPointClickListener(UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = gameObject.GetComponent<EventTrigger>();
        if (trigger==null )
        {
            trigger = gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;//可换成其他接口
            entry.callback = new EventTrigger.TriggerEvent();
            entry.callback.AddListener(action);
        }
    }


    public void AddMyButtonClick(UnityAction onClick)
    {
        ButtonClick tmpInf = gameObject.GetComponent<ButtonClick>();
        if (tmpInf != null)
        {
            tmpInf.OnClick += onClick;
        }
    }
}