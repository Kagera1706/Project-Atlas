using System;

public class RNG
{
    private static Random rng = null;
    public static Random Instance
    {
        get
        {
            if (rng == null)
                rng = new Random();
            return rng;
        }
    }

    /// <summary>
    /// Generates a random integer between 0 and max
    /// </summary>
    public static int Generate(int max)
    {
        return Instance.Next(max);
    }

    /// <summary>
    /// Generates a random integer between min and max
    /// </summary>
    public static int Generate(int min, int max)
    {
        return Instance.Next(min, max);
    }

    /// <summary>
    /// Generates a random float between 0.0 and 1.0
    /// </summary>
    public static float Generate()
    {
        return (float)Instance.NextDouble();
    }

    /// <summary>
    /// Generates a random object of type T from a list
    /// </summary>
    public static T Generate<T>(T[] list)
    {
        return list[Generate(list.Length)];
    }
}
