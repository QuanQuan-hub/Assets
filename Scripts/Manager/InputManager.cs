using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InputManager 
{
    //单例模式
    private static InputManager instance;
    public static InputManager Instance 
    { get
      {
          if (instance == null)
              instance = new InputManager();
          return instance;
      }
      set { instance = value; } 
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
        //读json表,通过Action2KeyConfig表初始化字典
        StreamReader sr = new StreamReader(CommonValue.Action2KeyConfig);
        Action2Key = JsonConvert.DeserializeObject<Dictionary<ActionEnum, KeyCode>>(sr.ReadToEnd());
    }
    public KeyCode GetActionKey(ActionEnum action) 
    {
        if (Action2Key.ContainsKey(action))
        {
            return Action2Key[action];
        }
        return KeyCode.None;
    }
    public bool IsKeyDown(ActionEnum action)
    {
        return Input.GetKeyDown(GetActionKey(ActionEnum.forward));
    }
    public bool IsKeyUp(ActionEnum action)
    {
        return Input.GetKeyUp(GetActionKey(ActionEnum.forward));
    }
    public bool IsKeyHold(ActionEnum action)
    {
        return Input.GetKey(GetActionKey(ActionEnum.forward));
    }
}