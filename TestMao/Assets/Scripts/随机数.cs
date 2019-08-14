using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 随机数 : MonoBehaviour {

    string testStr;

    private void Start()
    {
        for (int i = 0; i < 16; i++)
        {
            int r = Random.Range(0, 16);

            Debug.Log(r);
        }
    }
    void OnGUI()
    {

    
        if (GUILayout.Button("产生随机数"))
        {
            testStr = "";

            Hashtable hashtable = new Hashtable();
            System.Random rm = new System.Random();
            int RmNum = 15;
            for (int i = 0; hashtable.Count < RmNum; i++)
            {
                int nValue = rm.Next(1,16);
                if (!hashtable.ContainsValue(nValue))
                {
                    hashtable.Add(nValue, nValue);    //Add(key,value)
                    testStr += nValue + " ";
                }
            }
        }
        GUILayout.Label(testStr);
    }
}
