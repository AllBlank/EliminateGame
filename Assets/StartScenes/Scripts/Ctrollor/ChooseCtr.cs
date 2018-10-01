using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseCtr : UIbase
{

    public class Ctro
    {

        GameObject tmpUI = GameObject.Find("UI");


        /// <summary>
        /// 返回
        /// </summary>
        public void OnCloseClick()
        {
            tmpUI.transform.Find("Choose").gameObject.SetActive(false);

        }

        /// <summary>
        /// 进入游戏界面
        /// </summary>
        public void OnGoClick()
        {
            SceneManager.LoadScene("GameScene" + LevelsManager.Instance.currentChooseLevelNum);
        }
        /// <summary>
        /// 使用技能1 
        /// </summary>
        public void OnBombClc()
        {

        }
        /// <summary>
        /// 使用技能2
        /// </summary>
        public void OnStepClc()
        {

        }
        /// <summary>
        /// 使用技能3
        /// </summary>
        public void OnFlashCli()
        {

        }
        /// <summary>
        /// 使用技能4
        /// </summary>
        public void OnAddTimeCli()
        {

        }
        /// <summary>
        /// 购买技能
        /// </summary>
        public void OnMoreClc()
        {
            tmpUI.transform.Find("Store").gameObject.SetActive(true );

        }

    }


    Image[] starts;

    Text targetScore;

    Text score;

    Text level;

    Text boomCount;

    Text specialCount;

    Text resetCount;

    Text addTimeCount;


    private void Awake()
    {
        starts = new Image[3];
        for (int i = 0; i < starts.Length; i++)
        {
            starts[i] = transform.GetChild(0).GetChild(2 + i).GetChild(0).GetComponent<Image>();
        }
        level = transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<Text>();
        targetScore = transform.GetChild(0).GetChild(7).GetChild(0).GetComponent<Text>();
        score = transform.GetChild(0).GetChild(6).GetChild(0).GetComponent<Text>();
        boomCount = transform.GetChild(0).GetChild(9).GetChild(3).GetChild(0).GetChild(0).GetComponent<Text>();
        specialCount = transform.GetChild(0).GetChild(9).GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>();
        resetCount = transform.GetChild(0).GetChild(9).GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>();
        addTimeCount = transform.GetChild(0).GetChild(9).GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>();
    }

    private void OnEnable()
    {

        

        level.text = LevelsManager.Instance.currentChooseLevelNum.ToString();

        targetScore.text = SqliteLevelsDataManager.Instance.SelectSingleData("OneStarScores", "LevelsInfoTable", "LevelsNum", LevelsManager.Instance.currentChooseLevelNum).ToString();

        if (SqlitePlayerDataManager.Instance.CheckExistData(LevelsManager.Instance.currentChooseLevelNum))
        {
            int starNum = System.Convert.ToInt32(SqlitePlayerDataManager.Instance.SelectSingleData("LevelStar", "PlayerDataTable", "LevelNum", LevelsManager.Instance.currentChooseLevelNum).ToString());

            for (int i = 0; i < starNum; i++)
            {
                starts[i].enabled = true;
            }

            for (int i = starNum; i < starts.Length; i++)
            {
                starts[i].enabled = false;
            }
            score.text = SqlitePlayerDataManager.Instance.SelectSingleData("LevelScore", "PlayerDataTable", "LevelNum", LevelsManager.Instance.currentChooseLevelNum).ToString();
        }
        else
        {
            for (int i = 0; i < starts.Length; i++)
            {
                starts[i].enabled = false;
            }
            score.text = "0";
        }

        List<ArrayList> tempList = SqlitePropDataManager.Instance.AllDataExcuteASC("PlayerPropTable");

        boomCount.text = tempList[0][1].ToString();

        specialCount.text = tempList[0][3].ToString();

        resetCount.text = tempList[0][4].ToString();

        addTimeCount.text = tempList[0][2].ToString();
    }

    private void Start()
    {
        Ctro c = new Ctro();
        AddButtonListen("close_n", c.OnCloseClick);
        AddButtonListen("Button_n", c.OnGoClick);
        AddButtonListen("AddTime_n", c.OnAddTimeCli);
        AddButtonListen("Flash _n", c.OnFlashCli);
        AddButtonListen("AddSteps _n", c.OnStepClc);
        AddButtonListen("Bomb _n", c.OnBombClc);

    }

}