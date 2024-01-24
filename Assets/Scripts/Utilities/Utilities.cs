using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities 
{
    public static bool IsIndexValid<T>(List<T> list, int index)
    {
        return index >= 0 && index < list.Count;
    }
    public static bool IsIndexValid<T>(T[] array, int index)
    {
        return index >= 0 && index < array.Length;
    }
}
