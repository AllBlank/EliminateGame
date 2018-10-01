/**
 * 玩家信息数据库管理类 继承SqliteFramework类
 * 具体化父类数据库操作语句的实现
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SqlitePlayerDataManager : SqliteFramework {

    #region 单例
    private static SqlitePlayerDataManager instance;
    public static SqlitePlayerDataManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new SqlitePlayerDataManager();
            }
            return instance;
        }
    }
    private SqlitePlayerDataManager() { }
    #endregion

    /// <summary>
    /// 往表中插入单行数据
    /// </summary>
    /// <param name="tableName">表名</param>
    /// <param name="Levels">插入关卡</param>
    /// <param name="Scores">插入分数</param>
    /// <param name="Stars">插入星星数</param>
    public void InsertData(string tableName, int levels, int scores, int stars)
    {
        //if (!IsTableExist(tableName))
        //{
        //    CreatLevelsTable(tableName);
        //}
        string sqlQuery = string.Format("INSERT INTO {0} Values({1},{2},{3})", tableName, levels, scores, stars);
        base.NullReturnExcute(sqlQuery);
    }

    /// <summary>
    /// 根据关卡来更新数据
    /// </summary>
    /// <param name="tableName">表名</param>
    /// <param name="scores">要更新的分数</param>
    /// <param name="stars">要更新的星星数</param>
    /// <param name="whereLevel">限定条件：关卡</param>
    public void UpdateData(string tableName, int updateScores, int updateStars, int whereLevel)
    {
        //if (!IsTableExist(tableName))
        //{
        //    CreatLevelsTable(tableName);
        //}
        string sqlQuery = string.Format("UPDATE {0} SET LevelScore = {1},LevelStar = {2} WHERE LevelNum = {3}", tableName, updateScores, updateStars, whereLevel);
        base.NullReturnExcute(sqlQuery);
    }

    /// <summary>
    /// 查询单行数据
    /// </summary>
    /// <param name="selectColumn">要查询的字段</param>
    /// <param name="tableName">表名</param>
    /// <param name="whereColumn">限定查询条件</param>
    /// <param name="whereValue">限定查询条件字段值</param>
    /// <returns></returns>
    public object SelectSingleData(string selectColumn, string tableName, string whereColumn, int whereValue)
    {
        //if (!IsTableExist(tableName))
        //{
        //    CreatLevelsTable(tableName);
        //}
        string selectQuery = string.Format("Select {0} From {1} where {2} = '{3}'", selectColumn, tableName, whereColumn, whereValue);
        object currenrObj = base.SingleDataSearch(selectQuery);
        return currenrObj;
    }

    /// <summary>
	/// 判断数据是否存在
	/// </summary>
	/// <returns><c>true</c>, if exist data was checked, <c>false</c> otherwise.</returns>
	/// <param name="levelNumber">关卡编号.</param>
	public bool CheckExistData(int levelnum)
    {
        string query = "Select * From PlayerDataTable Where LevelNum = " + levelnum;
        object result = base.SingleDataSearch(query);

        if (result != null)
            return true;
        else
            return false;
    }

    /// <summary>
	/// 根据表名查询所有数据，并以一个字段从小到大排序
	/// </summary>
	/// <returns>The data excute AS.</returns>
	/// <param name="tableName">Table name.</param>
	/// <param name="orderByColomnName">Order by colomn name.</param>
	public List<ArrayList> AllDataExcuteASC(string tableName, string orderByColomnName)
    {
        string sqlQuery = "Select * From " + tableName + " Order By " + orderByColomnName + " ASC";
        return base.MultipleDataSearch(sqlQuery);
    }

    ///// <summary>
    ///// 创建玩家信息管理表
    ///// 表中有三列数据：关卡，分数，星星数
    ///// </summary>
    ///// <param name="tableName"></param>
    //private void CreatLevelsTable(string tableName)
    //{
    //    //创建表
    //    //表中有三列数据：关卡，分数，星星数
    //    string query = "CREATE TABLE " + tableName + "(LevelNum int,LevelScore int,LevelStar int)";
    //    base.NullReturnExcute(query);
    //}

}
