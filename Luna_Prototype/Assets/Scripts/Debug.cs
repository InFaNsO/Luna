using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Core
{
    public class Debug
    {
        public const string DEBUG_FLAG = "DEBUG_BUILD";
        
        [Conditional(DEBUG_FLAG)]
        static public void Log(string message)
        {
            string log = string.Format("<color=orange>{0}</color>", message);
            string log2 = $"<color=orange>{message}</color>";                                       // more effecient
            UnityEngine.Debug.Log(log);
        }

        [Conditional(DEBUG_FLAG)]
        static public void LogWarning(string message)
        {
            UnityEngine.Debug.LogWarning(message);
        }

        static public void LogError(string message)                                                 // Dont strip out your error log
        {
            UnityEngine.Debug.LogError(message);
        }
    }
}

