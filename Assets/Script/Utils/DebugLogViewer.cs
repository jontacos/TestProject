using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugLogViewer : MonoBehaviour
{

    public Text log = null;

    void Awake ()
    {
        Application.logMessageReceived += HandleLog;

    }

    private void OnDestroy()
    {
        Application.logMessageReceived -= HandleLog;
    }

    private void HandleLog(string logText, string stackTrace, LogType type)
    {
        log.text = logText;
    }
}

