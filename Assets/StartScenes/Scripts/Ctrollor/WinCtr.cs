using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinCtr : UIbase  {

    public class Ctro
    {

        GameObject tmpUI = GameObject.Find("UI");


        /// <summary>
        /// 返回选择关卡界面
        /// </summary>
        public void OnMenuClick()
        {
            SceneManager.LoadScene("MenuScene");

        }
        /// <summary>
        /// 进入下一关
        /// </summary>
        public void OnFastWardClick()
        {
            LevelsManager.Instance.currentChooseLevelNum++;
            SceneManager.LoadScene("GameScene" + LevelsManager.Instance.currentChooseLevelNum);
        }
        /// <summary>
        /// 再玩一次
        /// </summary>
        public void OnRepeatClick()
        {
            SceneManager.LoadScene("GameScene" + LevelsManager.Instance.currentChooseLevelNum);
        }
           
    }


    public Image[] stars;

    Text levelText;

    Text targetScore;

    Text score;

    
    private void Awake()
    {
        stars = new Image[3];

        stars[0] = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
        stars[1] = transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>();
        stars[2] = transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetComponent<Image>();

        levelText = transform.GetChild(0).GetChild(0).GetChild(3).GetChild(1).GetComponent<Text>();

        targetScore = transform.GetChild(0).GetChild(0).GetChild(5).GetChild(0).GetComponent<Text>();

        score = transform.GetChild(0).GetChild(0).GetChild(6).GetChild(0).GetComponent<Text>();
    }


    void Start()
    {

        if (SqlitePlayerDataManager.Instance.CheckExistData(PlayerManager.instance.currentlevel))
        {
            int oldScore = System.Convert.ToInt32(SqlitePlayerDataManager.Instance.SelectSingleData("LevelScore", "PlayerDataTable", "LevelNum", LevelsManager.Instance.currentChooseLevelNum));
            if (LevelsManager.Instance.currentChooseLevelScore > oldScore)
            {
                SqlitePlayerDataManager.Instance.UpdateData("PlayerDataTable", LevelsManager.Instance.currentChooseLevelScore, LevelsManager.Instance.currentChooseLevelStars, LevelsManager.Instance.currentChooseLevelNum);
            }          
        }
        else
        {
            SqlitePlayerDataManager.Instance.InsertData("PlayerDataTable", LevelsManager.Instance.currentChooseLevelNum, LevelsManager.Instance.currentChooseLevelScore, LevelsManager.Instance.currentChooseLevelStars);
        }


        levelText.text = PlayerManager.instance.currentlevel.ToString();

        targetScore.text = PlayerManager.instance.levelInfo.OneStarScore.ToString();

        score.text = PlayerManager.instance.playerScore.ToString();

        for (int i = 0; i < LevelsManager.Instance.currentChooseLevelStars; i++)
        {
            stars[i].enabled = true;
        }

        for (int i = LevelsManager.Instance.currentChooseLevelStars; i < stars.Length; i++)
        {
            stars[i].enabled = false;
        }


        Ctro c = new Ctro();
        AddButtonListen("menu_n", c.OnMenuClick);
        AddButtonListen("FastFoward_n", c.OnFastWardClick);
        AddButtonListen("Repeat_n", c.OnRepeatClick);

    }


    // Update is called once per frame
    void Update()
    {

    }
}
