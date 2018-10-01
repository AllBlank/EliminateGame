using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 逻辑层
/// </summary>
public class LoadingCtrLogic
{
    public void Onclick()
    {

        Debug.Log("1111");
    }
}
/// <summary>
/// 数据层
/// </summary>
public class LoadModel
{
    public string useName;
    public string Password;

}
/// <summary>
/// 控制层
/// </summary>
public class LoadContro : UIbase  {
    LoadingCtrLogic load;

    // Use this for initialization
    void Start () {
        load = new LoadingCtrLogic();
        AddButtonListen("Image", load.Onclick );

    }

    private void Awake()
    {
        
    }
    // Update is called once per frame
    void Update () {
        
	}
}
