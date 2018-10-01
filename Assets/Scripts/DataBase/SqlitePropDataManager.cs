/**
 * 玩家信息数据库管理类 继承SqliteFramework类
 * 具体化父类数据库操作语句的实现
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SqlitePropDataManager : SqliteFramework {

    #region 单例
    private static SqlitePropDataManager instance;
    public static SqlitePropDataManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new SqlitePropDataManager();
            }
            return instance;
        }
    }
    private SqlitePropDataManager() { }
    #endregion
   
    /// <summary>
	/// 根据表名查询所有数据
	/// </summary>
	/// <returns>The data excute AS.</returns>
	/// <param name="tableName">Table name.</param>
	/// <param name="orderByColomnName">Order by colomn name.</param>
	public List<ArrayList> AllDataExcuteASC(string tableName)
    {
        string sqlQuery = "Select * From " + tableName ;
        return base.MultipleDataSearch(sqlQuery);
    }

    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="tableName">表名</param>
    /// <param name="updateCoins">更新的金币数</param>
    /// <param name="updateBoom">更新的炸弹道具个数</param>
    /// <param name="updateTime">更新的时间道具个数</param>
    /// <param name="updateSpecial">更新的特殊道具个数</param>
    /// <param name="updateRefresh">更新的刷新道具个数</param>
    public void UpdateData(string tableName, int updateCoins, int updateBoom, int updateTime, int updateSpecial, int updateRefresh)
    {
        //if (!IsTableExist(tableName))
        //{
        //    CreatLevelsTable(tableName);
        //}
        
        string sqlQuery = string.Format("UPDATE {0} SET Coins = {1},Boom = {2},Time = {3},Special = {4},Refresh = {5} WHERE FixedValue = 1", 
                                    tableName, updateCoins, updateBoom, updateTime, updateSpecial, updateRefresh);
        base.NullReturnExcute(sqlQuery);
    }

    public void UpdateCoinData(string tableName, int updateCoins)
    {
        //if (!IsTableExist(tableName))
        //{
        //    CreatLevelsTable(tableName);
        //}
        string sqlQuery = string.Format("UPDATE {0} SET Coins = {1} WHERE FixedValue = 1",
                                    tableName, updateCoins);
        base.NullReturnExcute(sqlQuery);
    }

    public void UpdateBoomData(string tableName, int updateBoom)
    {
        //if (!IsTableExist(tableName))
        //{
        //    CreatLevelsTable(tableName);
        //}
        string sqlQuery = string.Format("UPDATE {0} SET Boom = {1} WHERE FixedValue = 1",
                                    tableName,  updateBoom);
        base.NullReturnExcute(sqlQuery);
    }

    public void UpdateTimeData(string tableName, int updateTime)
    {
        //if (!IsTableExist(tableName))
        //{
        //    CreatLevelsTable(tableName);
        //}
        string sqlQuery = string.Format("UPDATE {0} SET Time = {1} WHERE FixedValue = 1",
                                    tableName, updateTime);
        base.NullReturnExcute(sqlQuery);
    }

    public void UpdateSpecialData(string tableName, int updateSpecial)
    {
        //if (!IsTableExist(tableName))
        //{
        //    CreatLevelsTable(tableName);
        //}
        string sqlQuery = string.Format("UPDATE {0} SET Special = {1} WHERE FixedValue = 1",
                                    tableName, updateSpecial);
        base.NullReturnExcute(sqlQuery);
    }

    public void UpdateRefreshData(string tableName,int updateRefresh)
    {
        //if (!IsTableExist(tableName))
        //{
        //    CreatLevelsTable(tableName);
        //}
        string sqlQuery = string.Format("UPDATE {0} SET Refresh = {1} WHERE FixedValue = 1",
                                    tableName, updateRefresh);
        base.NullReturnExcute(sqlQuery);
    }

    ///// <summary>
    ///// 创建玩家道具管理表
    ///// 表中有五列数据：金币，炸弹道具个数，炸弹道具个数，时间道具个数，特殊道具个数，刷新道具个数
    ///// </summary>
    ///// <param name="tableName"></param>
    //private void CreatLevelsTable(string tableName)
    //{
    //    //创建表
    //    //表中有五列数据：金币，炸弹道具个数，炸弹道具个数，时间道具个数，特殊道具个数，刷新道具个数,更新限制条件
    //    string query = "CREATE TABLE " + tableName + "(Coins int,Boom int,Time int,Special int,Refresh int,FixedValue int)";
    //    base.NullReturnExcute(query);
    //}

}
