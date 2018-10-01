using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 管理器，对外提供接口
/// </summary>
public class UIbase : MonoBehaviour {
    /// <summary>
    /// 给子类添加脚本UIbehaivor
    /// </summary>
    public  void Awake()
    {
        Transform[] allChild = transform.GetComponentsInChildren<Transform>();
        for (int i = 1; i < allChild.Length; i++)
        {
            if (allChild [i].transform.name .EndsWith("_n"))
           {
            allChild[i].gameObject.AddComponent<UIbehavior>();
              }
               

        }

    }
    /// <summary>
    /// 得到子控件
    /// </summary>
    /// <returns></returns>
    public GameObject getChildGameObj(string objName)
    {
        
      return   UImanager.instance.getGameobject(transform .name  ,objName);

    }
    /// <summary>
    /// 添加事件
    /// </summary>
    public void AddButtonListen(string objName,UnityAction action)
    {
       GameObject son= getChildGameObj(objName);//拿到子控件

        son.GetComponent<UIbehavior>().AddButtonListen(action);//

    }

    public void AddMyButtonListen(string objName, UnityAction action)
    {
        GameObject son = getChildGameObj(objName);//拿到子控件

        son.GetComponent<UIbehavior>().AddMyButtonClick(action);//

    }


    public void AddInputFieldEndEditor(string objName,UnityAction<string >action)
    {
        GameObject son = getChildGameObj(objName);//拿到子控件
                                                  // son.GetComponent<UIbehavior>().AddInputFieldEndLieten(action);//
        son.GetComponent<UIbehavior>().AddInputFieldEndListen(action);
    }

    //
    void Ondestory()
    {
        UImanager.instance.UnRegistPanel(transform.name);
    }
}
