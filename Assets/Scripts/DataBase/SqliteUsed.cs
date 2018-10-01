using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * 数据库使用举例类
 * 
 */
public class SqliteUsed : MonoBehaviour {
    
    //数据库名
    public string databaseName = "XiaoxiaoleDataBase";
    //表名
    public string playerDataTableName = "PlayerDataTable";
    public string levelInfoTableName = "LevelsInfoTable";

    //private void OnEnable()
    //{
    //    //开启数据库
    //    SqlitePlayerDataManager.Instance.OpenDataBase(databaseName);
        
    //}

    //private void OnDisable()
    //{   
    //    //关闭数据库
    //    SqlitePlayerDataManager.Instance.CloseDataBase();
    //}

    /// <summary>
    /// 获取数据库中数据（外界同步数据库数据）
    /// </summary>
    /// <returns>返回获得的数据List<ArrayList>  按关卡顺序排序</returns>
    private List<ArrayList> DataSynchronism()
    {    
        //根据关卡顺序获取玩家关卡数据
        List<ArrayList> result = SqlitePlayerDataManager.Instance.AllDataExcuteASC(playerDataTableName,"LevelNum");       
        return result;
        //根据关卡顺序获取关卡过关条件基本数据
        List<ArrayList> result1 = SqliteLevelsDataManager.Instance.AllDataExcuteASC(levelInfoTableName, "LevelsNum");
        return result1;
    }
    /// <summary>
    /// 往数据库中插入或者更新数据(数据库数据同步)
    /// 只允许更新玩家信息数据表
    /// </summary>
    private void InsertUpdateData()
    {
        LevelsManager.Instance.SynPlayData(playerDataTableName);
    }
}
