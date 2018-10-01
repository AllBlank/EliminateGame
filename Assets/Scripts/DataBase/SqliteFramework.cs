/**
 * 数据库管理基类
 * 
 * 包含基础打开、关闭数据库
 * 简单数据库操作语句的实现（增、删、改、查）
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mono.Data.Sqlite;
using System.IO;

public class SqliteFramework {

    #region 数据库基础对象
    protected SqliteConnection conn;
    protected SqliteCommand command;
    protected SqliteDataReader reader;
    #endregion

    //数据库连接路径
    private string connectionPath;

    /// <summary>
    /// 打开数据库
    /// </summary>
    /// <param name="databasePath">数据库连接路径</param>
    public void OpenDataBase(string databaseName)
    {
        //设置数据库路径（动态添加数据库路径后缀）
        if (!databaseName.Contains(".sqlite"))
        {
            databaseName += ".sqlite";
        }

        //// 【沙盒路径】  
        //string sandboxPath = Application.persistentDataPath + "/" + databaseName;
        //
        //// 【用于www下载数据库的路径】表示的就是unity工程中StreamingAssets中的数据库文件  
        //// 打包成APK安装包之后，就是下面的地址  
        //string downPath = "jar:file://" + Application.dataPath + "!/assets" + "/" + databaseName;
        //
        //// 【安卓端】判断沙盒路径是否存在数据库文件  
        //// 如果不存在，就从StreamingAssets文件夹中下载数据库  
        //if (!File.Exists(sandboxPath))
        //{
        //
        //    Debug.Log("执行到此，表示沙盒路径中不存在 Data0118.sqlite 文件");
        //
        //    // 不存在数据库文件的时候，有两种创建方式  
        //    // 1.使用sqlite代码，手动创建，如果数据量过大，不适合代码的书写  
        //    // 2.通过下载的方式，去其他的目录下载，然后保存到沙盒路径  
        //
        //    WWW www = new WWW(downPath);
        //
        //    // 如果数据没有下载完成，就不能继续执行后面的代码  
        //    while (!www.isDone)
        //    {
        //
        //    }
        //    // 将www下载得到的所有数据，都保存到sandboxPath目录下  
        //    File.WriteAllBytes(sandboxPath, www.bytes);
        //}
        //
        //// 链接沙盒路径中的数据库文件  
        //string dataSandboxPath = "URI = file:" + Application.persistentDataPath + "/" + databaseName;
        //SqliteConnection con = new SqliteConnection(dataSandboxPath);
        
        //设置数据库连接路径
        connectionPath = "Data Source = " + Application.streamingAssetsPath + "/" +databaseName;
        //实例化连接对象
        conn = new SqliteConnection(connectionPath);
        //创建指令对象
        command = conn.CreateCommand();
        //开启数据库
        try
        {
            conn.Open();
            if (!IsTableExist("LevelsInfoTable"))
            {
                CreatLevelsTable("LevelsInfoTable");
                CreatLevelsTable("LevelsInfoTable");
                NullReturnExcute("INSERT INTO LevelsInfoTable Values(1,800,1800,3000,20,-1)");
                NullReturnExcute("INSERT INTO LevelsInfoTable Values(2,800,2200,4200,20,-1)");
                NullReturnExcute("INSERT INTO LevelsInfoTable Values(3,800,1600,2500,10,-1)");
                NullReturnExcute("INSERT INTO LevelsInfoTable Values(4,800,2200,4200,15,-1)");
                NullReturnExcute("INSERT INTO LevelsInfoTable Values(5,800,3200,6200,-1,150)");
                NullReturnExcute("INSERT INTO LevelsInfoTable Values(6,800,3200,600,-1,120)");
                NullReturnExcute("INSERT INTO LevelsInfoTable Values(7,800,1500,3000,10,-1)");
                NullReturnExcute("INSERT INTO LevelsInfoTable Values(8,800,1500,2000,-1,60)");
                NullReturnExcute("INSERT INTO LevelsInfoTable Values(9,800,1500,3000,8,-1)");
                NullReturnExcute("INSERT INTO LevelsInfoTable Values(10,800,1500,3000,10,-1)");
            }
            if (!IsTableExist("PlayerDataTable"))
            {
                CreatPlayerTable("PlayerDataTable");
            }
            if (!IsTableExist("PlayerPropTable"))
            {
                CreatPropTable("PlayerPropTable");
                NullReturnExcute("INSERT INTO PlayerPropTable Values(10000,1,1,1,1,1)");
            }
        }
        catch (SqliteException ex)
        {
            Debug.LogWarning("数据库打开异常：" + ex.ToString());
        }
    }

    /// <summary>
    /// 关闭数据库
    /// </summary>
    public void CloseDataBase()
    {
        try
        {
            if (reader != null)
            {
                //读取连接关闭
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }          
            conn.Close();
        }
        catch (SqliteException ex)
        {
            Debug.LogWarning("数据库关闭异常！" + ex.ToString());
        }
    }

    /// <summary>
    /// 增删改无返回式的执行
    /// </summary>
    /// <param name="query">传入的数据库语句</param>
    public virtual void NullReturnExcute(string query)
    {
        try
        {
            command.CommandText = query;
            command.ExecuteNonQuery();
        }
        catch (SqliteException ex)
        {
            Debug.LogWarning("语句执行异常：" + ex.ToString());
        }
    }

    /// <summary>
    /// 查询单行数据
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    public virtual object SingleDataSearch(string query)
    {
        try
        {
            command.CommandText = query;
            object result = command.ExecuteScalar();
            return result;
        }
        catch (SqliteException ex)
        {
            Debug.LogWarning("数据库操作异常：" + ex.ToString());
            return null;
        }
    }

    /// <summary>
    /// 查询多个数据
    /// </summary>
    /// <param name="query">数据库语句</param>
    /// <returns>查询到的数据</returns>
    public virtual List<ArrayList> MultipleDataSearch(string query)
    {
        try
        {
            command.CommandText = query;
            reader = command.ExecuteReader();
            //结果List
            List<ArrayList> result = new List<ArrayList>();
            while (reader.Read())
            {
                //单行数据
                ArrayList row = new ArrayList();
                //循环存储每一列
                for (int i = 0; i < reader.FieldCount; i++)
                {   
                    //TODO: 单行值存储
                    row.Add(reader.GetValue(i));
                }
                //存储行数据
                result.Add(row);
            }
            reader.Close();
            return result;
        }
        catch (SqliteException ex)
        {
            Debug.LogWarning("语句执行异常：" + ex.ToString());
            return null;
        }
    }

    /// <summary>
    /// 判断数据库中表是否存在
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    public bool IsTableExist(string tableName)
    {
        bool isTabelExist = true;
        command.CommandText = "SELECT COUNT(*) FROM sqlite_master where type='table' and name='" + tableName + "';";
        if (0 == Convert.ToInt32(command.ExecuteScalar()))
        {
            isTabelExist = false;
        }
        return isTabelExist;
    }


    /// <summary>
    /// 创建玩家信息管理表
    /// 表中有六列数据：关卡，一颗星分数，二颗星分数，三颗星分数，过关步数条件，过关时间条件
    /// </summary>
    /// <param name="tableName"></param>
    private void CreatLevelsTable(string tableName)
    {
        //创建表
        //表中有六列数据：关卡，一颗星分数，二颗星分数，三颗星分数，过关步数条件，过关时间条件
        string query = "CREATE TABLE " + tableName +
            "(LevelsNum int,OneStarScores int,TwoStarScores int,ThreeStarScores int,PassSteps int,PassTimes int)";
        NullReturnExcute(query);
    }

    /// <summary>
    /// 创建玩家信息管理表
    /// 表中有三列数据：关卡，分数，星星数
    /// </summary>
    /// <param name="tableName"></param>
    private void CreatPlayerTable(string tableName)
    {
        //创建表
        //表中有三列数据：关卡，分数，星星数
        string query = "CREATE TABLE " + tableName + "(LevelNum int,LevelScore int,LevelStar int)";
        NullReturnExcute(query);
    }

    /// <summary>
    /// 创建玩家道具管理表
    /// 表中有五列数据：金币，炸弹道具个数，炸弹道具个数，时间道具个数，特殊道具个数，刷新道具个数
    /// </summary>
    /// <param name="tableName"></param>
    private void CreatPropTable(string tableName)
    {
        //创建表
        //表中有五列数据：金币，炸弹道具个数，炸弹道具个数，时间道具个数，特殊道具个数，刷新道具个数,更新限制条件
        string query = "CREATE TABLE " + tableName + "(Coins int,Boom int,Time int,Special int,Refresh int,FixedValue int)";
        NullReturnExcute(query);
    }
}
