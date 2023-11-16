using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class LocalStorageExternal : MonoBehaviour
{
    [DllImport("__Internal")]
    public static extern void SetLocalStorageValue(string key, string value);

    [DllImport("__Internal")]
    public static extern string GetLocalStorageValue(string str);
    
    public static string GenerateUniqueID()
    {
        string uniqueID = Guid.NewGuid().ToString();
        return uniqueID;
    }
}
