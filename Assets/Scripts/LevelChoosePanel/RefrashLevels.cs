using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RefrashLevels : MonoBehaviour {
    Level level;
    private List<GameObject> levelsList ;
    //数据库名
    private string databaseName = "XiaoxiaoleDataBase";
    //表名
    private string playerDataTableName = "PlayerDataTable";
    private string levelInfoTableName = "LevelsInfoTable";

    

    private void Start()
    {
        //level = new Level();
        levelsList = new List<GameObject>();
        foreach (var item in transform.GetComponentsInChildren<Level>())
        {
            levelsList.Add(item.gameObject);
        }
        SynchronizationData();
    }

    private void SynchronizationData()
    {
        try
        {
            //从数据库读取数据
            List<ArrayList> result =  SqlitePlayerDataManager.Instance.AllDataExcuteASC(playerDataTableName,"LevelNum");
            //同步UI显示
            int levelNum = 0;
            int i = 0;
            foreach (var item in result)
            {   
                levelsList[i].GetComponent<Level>().starNum =  int.Parse(item[2].ToString());
                levelsList[i].GetComponent<Level>().scores = int.Parse(item[1].ToString());
                //获取当前设置关卡
                int currnetLevelNum = ++levelNum;
                //将锁设为非激活
                levelsList[i].transform.GetChild(0).gameObject.SetActive(false);
                //将关卡未通过设置为不激活
                levelsList[i].transform.GetChild(2).gameObject.SetActive(false);
                //将关卡通过设置为激活
                levelsList[i].transform.GetChild(1).gameObject.SetActive(true);
                //获取激活关卡星星父物体的位置
                Transform stars = levelsList[i].transform.GetChild(1).GetChild(4).transform;
                //根据星星数量，显示相应的星星
                ShowStars(int.Parse(item[2].ToString()), stars);
                i++;
            }
            //设置下一关为可玩关卡
            transform.GetChild(levelNum).GetChild(2).gameObject.SetActive(true);
            //将锁设为非激活
            transform.GetChild(levelNum).GetChild(0).gameObject.SetActive(false);
            //将关卡通过设置为未激活
            transform.GetChild(levelNum).GetChild(1).gameObject.SetActive(false);

        }
        catch (NullReferenceException ex)
        {
            Debug.LogWarning("同步数据异常" + ex.ToString());          
        }
    }

    private void ShowStars(int starNum, Transform stars)
    {
        switch (starNum)
        {
            case 0:
            case 1:
            case 2:
            case 3:
                for (int i = 0; i < starNum; i++)
                {   //设置星星数目
                    stars.GetChild(i).gameObject.SetActive(true);
                }
                break;
            default:
                break;
        }
    }
}
