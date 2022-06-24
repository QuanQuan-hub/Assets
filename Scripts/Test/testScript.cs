using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.AddListener(EventID.test, DebugABanana);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            EventManager.ExecuteEvent(EventID.test);
        }
    }

    private void DebugABanana()
    {
        print("banana");
    }
}
