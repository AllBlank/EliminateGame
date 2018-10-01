using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelector : MonoBehaviour {

    public static LevelSelector instance;

    public int selectLevel = 1;

    private void Awake()
    {
        instance = this;
    }

}
