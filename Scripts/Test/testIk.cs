using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testIk : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnAnimatorIK(int layerIndex)
    {
        Debug.Log("iklayer = " + layerIndex);
    }
}
