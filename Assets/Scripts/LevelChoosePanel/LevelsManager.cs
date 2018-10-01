/**
 *  关卡管理单例
 *      
 *     存放   当前选择的关卡数
 *            当前选择的关卡星星数
 *            当前选择的关卡的分数
 *            
 *     方法：  同步当前数据进入数据库
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsManager  {
    #region 关卡管理单例
    private static LevelsManager instance;
    public static LevelsManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new LevelsManager();
            }
            return instance;
        }
        private set { }
    }
    private LevelsManager() { }

    #endregion

    //当前选择的关卡
    public int currentChooseLevelNum;
    //当前选择的关卡的星星数
    public int currentChooseLevelStars;
    //当前选择的关卡的分数
    public int currentChooseLevelScore;

    /// <summary>
    /// 同步玩家数据
    /// </summary>
    public void SynPlayData(string tablename)
    {
        if (SqlitePlayerDataManager.Instance.CheckExistData(currentChooseLevelNum))
        {   
            //如果数据库中存在关卡数据 则更新关卡数据
            SqlitePlayerDataManager.Instance.UpdateData(tablename, currentChooseLevelScore,currentChooseLevelStars,currentChooseLevelNum);
        }
        else
        {   //不存在则新增关卡数据
            SqlitePlayerDataManager.Instance.InsertData(tablename, currentChooseLevelNum, currentChooseLevelScore, currentChooseLevelStars);
        }
    }
}
