using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex
{
    public readonly int Q; //column
    public readonly int R; //row
    public readonly int S; //spacing

    static readonly float WIDTH_MULTIPLIER = Mathf.Sqrt(3) / 2;
	
    public Hex(int q, int r)
    {
        Q = q;
        R = r;
        S = -(q + r);
    }

	public Vector3 Position()
    {
        float radius = 5f;
        float height = radius * 2;
        float width = height * WIDTH_MULTIPLIER;

        float posY = height * (R * 0.75f);
        float posX = width * (Q + R / 2f);

        return new Vector3(posX, 0, posY);
    }

    public override string ToString()
    {
        return "Hex_" + Q + "_" + R;
    }
}
