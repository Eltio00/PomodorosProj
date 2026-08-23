using System;
using System.IO;
using UnityEngine;

public static class FileLogger
{
    private static readonly string LogsFolder = Path.Combine(Application.persistentDataPath, "Logs");

    private const string SuccessLogFile = "extraction_success.log";
    private const string GarbageLogFile = "extraction_garbage.log";
    private const string ErrorLogFile = "extraction_errors.log";
    private const string UnsupportedLogFile = "extraction_unsupported.log";

    static FileLogger()
    {
        if (!Directory.Exists(LogsFolder))
            Directory.CreateDirectory(LogsFolder);
    }

    public static void LogSuccess(string filePath, int charCount)
    {
        WriteLine(SuccessLogFile, $"OK | {filePath} | {charCount} chars extracted");
    }

    public static void LogGarbage(string filePath, int charCount)
    {
        WriteLine(GarbageLogFile, $"GARBAGE | {filePath} | {charCount} chars, failed quality check");
    }

    public static void LogError(string filePath, string exceptionMessage)
    {
        WriteLine(ErrorLogFile, $"ERROR | {filePath} | {exceptionMessage}");
    }

    public static void LogUnsupportedFormat(string filePath)
    {
        WriteLine(UnsupportedLogFile, $"UNSUPPORTED | {filePath}");
    }

    private static void WriteLine(string fileName, string message)
    {
        string fullPath = Path.Combine(LogsFolder, fileName);
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string line = $"[{timestamp}] {message}";

        try
        {
            File.AppendAllText(fullPath, line + Environment.NewLine);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to write to log file '{fileName}': {e.Message}");
        }
    }
}