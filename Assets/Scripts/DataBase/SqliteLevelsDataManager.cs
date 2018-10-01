using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SqliteLevelsDataManager : SqliteFramework {

    #region 单例
    private static SqliteLevelsDataManager instance;
    public static SqliteLevelsDataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SqliteLevelsDataManager();
            }
            return instance;
        }
    }
    private SqliteLevelsDataManager() { }
    #endregion

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
    ///// 表中有六列数据：关卡，一颗星分数，二颗星分数，三颗星分数，过关步数条件，过关时间条件
    ///// </summary>
    ///// <param name="tableName"></param>
    //private void CreatLevelsTable(string tableName)
    //{
    //    //创建表
    //    //表中有六列数据：关卡，一颗星分数，二颗星分数，三颗星分数，过关步数条件，过关时间条件
    //    string query = "CREATE TABLE " + tableName +
    //        "(LevelsNum int,OneStarScores int,TwoStarScores int,ThreeStarScores int,PassSteps int,PassTimes int)";
    //    base.NullReturnExcute(query);
    //}
}
