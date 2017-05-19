using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GOExtensions
{
    /// <summary>
    /// Returns the gameObject's material
    /// </summary>
    public static Material GetMaterial(this GameObject go)
    {
        Renderer rend = go.GetComponent<Renderer>();
        if (!rend)
            Debug.LogError(go.name + " : Renderer not found.");
        return rend ? rend.material : null;
    }

    /// <summary>
    /// Returns the gameObject's color
    /// </summary>
    public static Color GetColor(this GameObject go)
    {
        Material mat = go.GetMaterial();
        if (!mat)
            Debug.LogError(go.name + " : Material not found.");
        return mat ? mat.color : new Color();
    }
    
    /// <summary>
    /// Returns the gameObject's position
    /// </summary>
    public static Vector3 GetPosition(this GameObject go)
    {
        return go.transform.position;
    }

    /// <summary>
    /// Returns the gameObject's rotation
    /// </summary>
    public static Quaternion GetRotation(this GameObject go)
    {
        return go.transform.rotation;
    }


}
