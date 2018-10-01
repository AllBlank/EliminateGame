using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 管理存储所有的子控件
/// </summary>
public class UImanager : MonoBehaviour {
    //表示PANEL,//panel下面子控件的名字、、//子控件的物体
    Dictionary <string ,Dictionary <string ,GameObject>>allChild;
    public static UImanager instance;
    public  void Awake()
    {
        
        allChild = new Dictionary<string, Dictionary<string, GameObject>>();
        instance = this;

    }

    /// <summary>
    /// 将自子控件注册进去
    /// </summary>
    /// <param name="panelName"></param>
    /// <param name="objName"></param>
    /// <param name="obj"></param>
    public void RegistGameObj(string panelName,string objName,GameObject  obj)
    {
       
        if (!allChild .ContainsKey (panelName))
        {
            Dictionary <string ,GameObject>tmpDict = new Dictionary<string, GameObject>();
            
            allChild.Add(panelName, tmpDict);
            
        }
        allChild[panelName].Add(objName, obj);
        Debug.Log(allChild[panelName][objName]);
            }
    /// <summary>
    /// 剔除
    /// </summary>
    /// <param name="panelName"></param>
    /// <param name="objName"></param>
    public void UnRegistGameObject(string panelName,string objName)
    {
        if (allChild .ContainsKey (panelName ))
        {
            if (allChild [panelName ].ContainsKey(objName ))
            {
                allChild[panelName].Remove(objName);//剔除
            }
        }
    }
    public void UnRegistPanel(string panelName)
    {
        if (allChild .ContainsKey (panelName))
        {
            allChild[panelName].Clear();
        }
    }
    /// <summary>
    /// 可以得到子控件
    /// </summary>
    /// <param name="panelName"></param>
    /// <param name="objName"></param>
    /// <returns></returns>
    public GameObject getGameobject(string panelName,string objName)
    {
        if (allChild .ContainsKey (panelName ))
        {
            
            return allChild[panelName][objName];
        }
        return null;
    }
}
