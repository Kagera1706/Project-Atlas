using System;
using UnityEngine;

public interface ISelectable
{
    bool IsSelected { get; set; }
    void Select();
    //void Select(GameObject selection);
}
