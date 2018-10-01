/**
 * 单局游戏管理类
 * 
 * 管理游戏的开始结束以及通关分数
 * 
 * 将玩家游戏数据存入LevelsManager单例中 方便做数据库数据的同步
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

/// <summary>
/// 关卡信息结构体
/// </summary>
public struct LevelInfo
{
    public int LevelNum;
    public int OneStarScore;
    public int TwoStarScore;
    public int ThreeStarScore;
    public int PassSteps;
    public int PassTimes;

    public LevelInfo(int num,int one,int two,int three,int steps,int times)
    {
        this.LevelNum = num;
        this.OneStarScore = one;
        this.TwoStarScore = two;
        this.ThreeStarScore = three;
        this.PassSteps = steps;
        this.PassTimes = times;
    }
}


public enum GameType
{
    TIme,
    Step
}

public class PlayerManager :MonoBehaviour {

    public static PlayerManager instance;

    #region 基本属性  若对以下字段的值进行了改变，应该及时写入
        //当前选择关卡
        public int currentlevel;
        //玩家分数
        public int playerScore;
        //玩家步数
        public int playerStep;
        //玩家时间
        public float playerTime;
        //关卡信息
        public LevelInfo levelInfo;
    #endregion


    public GameType gameType;

    public Text score;
    public GameObject stepObj;
    public Text step;
    public GameObject timeObj;
    public Text time;

    public GameObject winObj;

    public GameObject loseObj;

    private void Awake()
    {
        instance = this;
        stepObj = transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).gameObject;
        timeObj = transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).gameObject;
        step = stepObj.transform.GetChild(1).GetComponent<Text>();
        time = timeObj.transform.GetChild(1).GetComponent<Text>();
        score = transform.GetChild(1).GetChild(0).GetChild(0).GetChild(3).GetChild(1).GetComponent<Text>();
        winObj = transform.GetChild(3).gameObject;
        loseObj = transform.GetChild(2).gameObject;
    }

    private void Start()
    {
        SynData();
        if(levelInfo.PassSteps != -1)
        {
            gameType = GameType.Step;
            timeObj.SetActive(false);
            stepObj.SetActive(true);
        }
        else if(levelInfo.PassTimes != -1)
        {
            gameType = GameType.TIme;
            timeObj.SetActive(true);
            stepObj.SetActive(false);
        }
        
    }

    private void Update()
    {
        if(gameType == GameType.TIme)
        {
            playerTime += Time.deltaTime;
            int realTIme = (int)(levelInfo.PassTimes - playerTime);
            if(realTIme < 0)
            {
                realTIme = 0;
            }
            time.text = realTIme.ToString();
            if(levelInfo.PassTimes <= playerTime)
            {
                Debug.Log(111);
                PassLevel();
            }
        }

        if(gameType == GameType.Step)
        {
            step.text = (levelInfo.PassSteps - playerStep).ToString();
            if (playerStep >= levelInfo.PassSteps)
            {
                PassLevel();
            }
        }
        score.text = playerScore.ToString();
    }

    /// <summary>
    /// 玩家过关与失败
    /// 玩家一旦开始游戏就应该调用此方法 来进行过关是否成功或者失败的操作
    /// </summary>
    public void PassLevel()
    {
        
        //以步数为条件的过关判断
        if (levelInfo.PassSteps > 0 && playerStep == levelInfo.PassSteps)
        {   //满足步数要求 且满足过关要求 过关
            if (IsPassLevel())
            {
                GetScore();
                StartCoroutine(SuccessLevel());
            }
            else
            {
                //GetScore();
                StartCoroutine(LoseLevel());
            }
        }
        //以时间为条件的过关判断
        else if(playerTime >= levelInfo.PassTimes)
        {
            if (IsPassLevel())
            {
                GetScore();
                StartCoroutine(SuccessLevel());
            }
            else
            {
                // GetScore();
                StartCoroutine(LoseLevel());
            }
        }
    }

    /// <summary>
    /// 根据当前关卡读取数据
    /// </summary>
    /// <param name="curlevel"></param>
    private void SynData()
    {
        currentlevel = LevelsManager.Instance.currentChooseLevelNum;
        
        Debug.Log(currentlevel);
        levelInfo = new LevelInfo(currentlevel, 
                        Convert.ToInt32( SqliteLevelsDataManager.Instance.SelectSingleData("OneStarScores", "LevelsInfoTable", "LevelsNum", currentlevel)),
                        Convert.ToInt32(SqliteLevelsDataManager.Instance.SelectSingleData("TwoStarScores", "LevelsInfoTable", "LevelsNum", currentlevel)),
                        Convert.ToInt32(SqliteLevelsDataManager.Instance.SelectSingleData("ThreeStarScores", "LevelsInfoTable", "LevelsNum", currentlevel)),
                        Convert.ToInt32(SqliteLevelsDataManager.Instance.SelectSingleData("PassSteps", "LevelsInfoTable", "LevelsNum", currentlevel)),
                        Convert.ToInt32(SqliteLevelsDataManager.Instance.SelectSingleData("PassTimes", "LevelsInfoTable", "LevelsNum", currentlevel)));
    }

    /// <summary>
    /// 判断玩家是否过关
    /// </summary>
    /// <returns></returns>
    private bool IsPassLevel()
    {
        if (playerScore >= levelInfo.OneStarScore)
            return true;
        return false;
    }

    /// <summary>
    /// 玩家过关得到分数，星星
    /// </summary>
    /// <returns></returns>
    private void GetScore()
    {
        if (playerScore >= levelInfo.ThreeStarScore)
        {
            LevelsManager.Instance.currentChooseLevelStars = 3;
        }
        else if (playerScore >= levelInfo.TwoStarScore)
        {
            LevelsManager.Instance.currentChooseLevelStars = 2;
        }
        else if(playerScore >= levelInfo.OneStarScore)
        {
            LevelsManager.Instance.currentChooseLevelStars = 1;
        }

        LevelsManager.Instance.currentChooseLevelScore = playerScore;
    }

    /// <summary>
    /// 玩家通关调用通关UI界面（可以用委托来实现）
    /// </summary>
    public IEnumerator SuccessLevel()
    {
        while (!ItemsManager.instance.canChange)
        {
            yield return 0;
        }
        ItemsManager.instance.canChange = false;
        yield return new WaitForSeconds(2f);
        winObj.SetActive(true);
    }

    /// <summary>
    /// 玩家失败调用失败UI界面（可以用委托来实现）
    /// </summary>
    public IEnumerator LoseLevel()
    {
        while (!ItemsManager.instance.canChange)
        {
            yield return 0;
        }
        ItemsManager.instance.canChange = false;
        yield return new WaitForSeconds(2f);
        loseObj.SetActive(true);
    }
}
