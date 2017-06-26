using UnityEngine;
using System.Collections;

public class SystemProperties
{
    private static SystemProperties mInstance = new SystemProperties();
    private AndroidJavaClass jc;

    private SystemProperties()
    {
#if UNITY_EDITOR
#else
        jc = new AndroidJavaClass("android.os.SystemProperties");
#endif
    }

    private string getProperty(string key, string def)
    {
#if UNITY_EDITOR
        return def;
#else
        return jc.CallStatic<string> ("get", key, def);
#endif
    }

    public static string get(string key, string def)
    {
        return mInstance.getProperty(key, def);
    }
}

