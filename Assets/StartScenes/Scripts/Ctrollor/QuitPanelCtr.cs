using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitPanelCtr : UIbase  {

    public class Ctro
    {

        GameObject tmpUI = GameObject.Find("UI");


        /// <summary>
        /// 返回
        /// </summary>
        public void OnCloseClick()
        {
            tmpUI.transform.Find("QuitPanel").gameObject.SetActive(false);

        }

        public void OnCloseaClick()
        {
            tmpUI.transform.Find("QuitPanel").gameObject.SetActive(false);

        }
        /// <summary>
        /// 退出游戏
        /// </summary>
        public void OnQuitClick()
        {
            Application.Quit();

        }

    }
    // Use this for initialization
    void Start()
    {
        Ctro c = new Ctro();
        AddButtonListen("Close_n", c.OnCloseClick);

        AddButtonListen("Yes_n", c.OnQuitClick);

        AddButtonListen("No_n", c.OnCloseaClick);


    }
    private void Awake()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }
    
}
