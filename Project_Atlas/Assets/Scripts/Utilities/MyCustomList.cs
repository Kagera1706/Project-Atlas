using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCustomList<T> : List<T>
{



    public new void Add(T elem)
    {
        if (!elem.ToString().Contains("Whatever"))
            base.Add(elem);
        else
            Debug.Log("elem contains 'Whatever' in its name : Cannot add !");
        
    }
}
