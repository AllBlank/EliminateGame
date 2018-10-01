using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseCtr : UIbase  {

    public class Ctro
    {

        GameObject tmpUI = GameObject.Find("Background");


        /// <summary>
        /// 返回选择关卡界面
        /// </summary>
        public void OnMenuClick()
        {
            SceneManager.LoadScene("MenuScene");

        }
       
        /// <summary>
        /// 再玩一次
        /// </summary>
        public void OnRepeatClick()
        {
            SceneManager.LoadScene("GameScene"+LevelsManager.Instance.currentChooseLevelNum);
        }

    }



    public Text level;

    public Text targetScore;

    // Use this for initialization
    void Start()
    {
        Ctro c = new Ctro();
        AddButtonListen("menu_n", c.OnMenuClick);
       
        AddButtonListen("Repeat_n", c.OnRepeatClick);


        level = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>();

        targetScore = transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>();

        level.text = PlayerManager.instance.currentlevel.ToString();

        targetScore.text = PlayerManager.instance.levelInfo.OneStarScore.ToString();

    }
    private void Awake()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }
}
