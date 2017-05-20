using System;
using UnityEngine;

public static class AssertExtension
{
    /// <summary>
    /// Asserts that a T value is within the boundaries of min and max
    /// </summary>
    public static bool IsWithin<T>(this T value, T min, T max) where T : IComparable
    {
        return value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0;
    }
}
