using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour 
{
    private static Manager instance = null;
    public static Manager Instance
    {
        get
        {
            if (!instance)
                instance = FindObjectOfType<Manager>();
            return instance;
        }
    }
}
