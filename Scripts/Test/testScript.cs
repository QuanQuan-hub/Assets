﻿using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class testScript : MonoBehaviour
{
    Dictionary<ActionEnum, KeyCode> Action2Key;
    // Start is called before the first frame update
    void Start()
    {
        //EventManager.AddListener(EventID.initJson, InitJson);
        //InitJson();
        InputManager.Instance.ModifyKey(ActionEnum.attack, KeyCode.A);
        InputManager.Instance.SaveActionKey();
    }

    void Update()
    {
        if (InputManager.Instance.IsKeyDown(ActionEnum.forward))
        {
            print("前进");
        }
    }
    #region 采样键位
    private void InitJson()
    {
        Action2Key = new Dictionary<ActionEnum, KeyCode>();
        StartCoroutine(inputKey());
    }
    IEnumerator inputKey()
    {
        foreach (ActionEnum action in Enum.GetValues(typeof(ActionEnum)))
        {
            print(action + ":");
            yield return inputAnyKey(action);
        }
        string jsonString = JsonConvert.SerializeObject(Action2Key, Formatting.None);

        string jsonPath = Application.streamingAssetsPath + "/Action2KeyConfig.json";
        if (!File.Exists(jsonPath))
        {
            File.CreateText(jsonPath).Dispose();
        }
        File.WriteAllText(jsonPath, jsonString, System.Text.Encoding.UTF8);

        print(jsonString);
    }
    IEnumerator inputAnyKey(ActionEnum action)
    {
        while (true)
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keyCode))
                    {
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