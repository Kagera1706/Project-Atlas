using UnityEngine;
using System;
using System.Collections;

public static class LayerMaskExtension 
{
	/// <summary>
	/// Sets the layer mask to contain the layers taken as parameter 
	/// </summary>
	public static LayerMask SetLayerMask(this LayerMask layerMask, params string[] layerNames)
	{
		layerMask = 0;

		foreach (string name in layerNames)
			layerMask = layerMask.AddLayer(name);

		return layerMask;
	}

	/// <summary>
	/// Adds a layer to the mask
	/// </summary>
	public static LayerMask AddLayer(this LayerMask layerMask, string layerName)
	{
		return layerMask |= LayerValue(layerName);
	}

	/// <summary>
	/// Remove a layer from the mask
	/// </summary>
	public static LayerMask RemoveLayer(this LayerMask layerMask, string layerName)
	{
		return layerMask &= ~(LayerValue(layerName));
	}

	/// <summary>
	/// Checks if a layer is included in the mask
	/// </summary>
	public static bool IsLayerIncluded(this LayerMask layerMask, string layerName)
	{
		return (layerMask & LayerValue(layerName)) != 0;
	}

	/// <summary>
	/// Returns the index value of the layer whose name is taken as parameter
	/// </summary>
	public static int LayerValue(string layerName)
	{
		return (1 << LayerMask.NameToLayer(layerName));
	}

}