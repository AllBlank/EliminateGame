using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCtr : UIbase {
    Ctro c;
   
	// Use this for initialization
	void Start () {
        c = new Ctro();
        AddMyButtonListen("PlayButton_n",c.OnPlayClick );
      
        //AddButtonListen("Store_n", c.OnStoreClick);

        //AddButtonListen("QuitGames_n", c.OnQuitClick);
       
       
    }




    private void Awake()
    {
        
    }
    // Update is called once per frame
    void Update () {
		
	}
    public class Ctro
    {
       
        GameObject tmpUI= GameObject.Find("UI");
        /// <summary>
        /// Play场景加载
        /// </summary>
        public void OnPlayClick()
        {
            tmpUI.transform.Find("ChooseMenu").gameObject.SetActive(true);
        }
        /// <summary>
        /// 商店切换
        /// </summary>
        public void OnStoreClick()
        {
            tmpUI.transform.Find("Store").gameObject.SetActive(true );
        }
       
        /// <summary>
        /// 退出游戏
        /// </summary>
       public void OnQuitClick()
        {
            tmpUI.transform.Find("QuitPanel").gameObject.SetActive(true);
        }
    }
}
