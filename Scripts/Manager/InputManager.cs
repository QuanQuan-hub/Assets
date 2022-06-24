using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager 
{
    //单例模式
    private static InputManager instance;
    public static InputManager GetInstance()
    {
        if (instance == null)
            instance = new InputManager();
        return instance;
    }

    public InputManager()
    {
        Init();
    }

    Dictionary<ActionEnum, KeyCode> Action2Key;
    Dictionary<KeyCode, ActionEnum> Key2Action;

    private void Init()
    {
        Action2Key = new Dictionary<ActionEnum, KeyCode>();
        Key2Action = new Dictionary<KeyCode, ActionEnum>();
        //TODO:读json表
    }
}