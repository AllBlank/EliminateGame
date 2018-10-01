using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase : MonoBehaviour {

    private void Awake()
    {
        Texture2D cursorTexture = Resources.Load<Texture2D>("Hand");
        Screen.SetResolution(720, 1080, false);
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }

    private void OnEnable()
    {
        //开启数据库
        SqlitePlayerDataManager.Instance.OpenDataBase("XiaoxiaoleDataBase");
        SqlitePropDataManager.Instance.OpenDataBase("XiaoxiaoleDataBase");
        SqliteLevelsDataManager.Instance.OpenDataBase("XiaoxiaoleDataBase");
    }

    private void OnDisable()
    {
        //关闭数据库
        SqlitePlayerDataManager.Instance.CloseDataBase();
        SqlitePropDataManager.Instance.CloseDataBase();
        SqliteLevelsDataManager.Instance.CloseDataBase();
    }
}
