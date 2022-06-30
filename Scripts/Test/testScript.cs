using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class testScript : MonoBehaviour
{
    public Text actionText;
    public Text keyText;
    public Text reslutText;

    Dictionary<ActionEnum, KeyCode> Action2Key;
    // Start is called before the first frame update
    void Start()
    {
        reslutText.text = "未生成采样";
        //EventManager.AddListener(EventID.initJson, InitJson);
        //InitJson();
        //InputManager.Instance.ModifyKey(ActionEnum.attack, KeyCode.A);
        //InputManager.Instance.SaveActionKey();
    }

    KeyCode currentKey = KeyCode.None;
    void Update()
    {
        
    }
    #region 采样键位
    public void InitJson()
    {
        Action2Key = new Dictionary<ActionEnum, KeyCode>();
        Action2Key[ActionEnum.None] = 0;
        StartCoroutine(inputKey());
    }
    IEnumerator inputKey()
    {
        foreach (ActionEnum action in Enum.GetValues(typeof(ActionEnum)))
        {
            if (action == ActionEnum.None)
            {
                continue;
            }
            print(action + ":");
            yield return inputAnyKey(action);
        }
        string jsonString = JsonConvert.SerializeObject(Action2Key, Formatting.Indented);

        string jsonPath = Application.streamingAssetsPath + "/Action2KeyConfig.json";
        if (!File.Exists(jsonPath))
        {
            File.CreateText(jsonPath).Dispose();
        }
        File.WriteAllText(jsonPath, jsonString, System.Text.Encoding.UTF8);

        reslutText.text = "";
        foreach (KeyValuePair < ActionEnum, KeyCode > item in Action2Key)
        {
            reslutText.text += item.Key.ToString() + ":" + item.Value.ToString() + "\n";
        }
        print(jsonString);
    }
    IEnumerator inputAnyKey(ActionEnum action)
    {
        while (true)
        {
            actionText.text = action.ToString();
            if (Input.anyKeyDown)
            {
                foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
                {
                    
                    if (Input.GetKeyDown(keyCode))
                    {
                        keyText.text = keyCode.ToString();
                        Action2Key[action] = keyCode;
                        print(action + "---" + keyCode);
                        break;
                    }
                }
                break;
            }
            yield return 0;
        }
        yield return 0;
    }
    #endregion
}
