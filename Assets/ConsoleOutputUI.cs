using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleOutputUI : MonoBehaviour
{
    public Text consoleText;
    public int maxLogCount = 10;

    private Queue<string> logQueue = new Queue<string>();

    void Start()
    {
        // Redirect the console output to a custom function
        Application.logMessageReceived += HandleLog;
    }

    void HandleLog(string logText, string stackTrace, LogType type)
    {
        // Add the log message to the queue
        logQueue.Enqueue(logText);

        // Keep only the last 'maxLogCount' messages in the queue
        while (logQueue.Count > maxLogCount)
        {
            logQueue.Dequeue();
        }

        // Update the UI Text with the messages in the queue
        UpdateConsoleText();
    }

    void UpdateConsoleText()
    {
        // Display the log messages on the UI Text component
        consoleText.text = string.Join("\n", logQueue.ToArray());
    }
}
