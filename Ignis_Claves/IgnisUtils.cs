using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text.RegularExpressions;
using ProtoBuf;

namespace IgnisClaves;

internal static class IgnisUtils
{
    internal static string GetUsername()
    {
        return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? WindowsIdentity.GetCurrent().Name : "default";
    }

    public static void SaveToFile<T>(T data, string filePath)
    {
        using FileStream fileStream = File.Create(filePath);
        Serializer.Serialize(fileStream, data);
    }

    public static T LoadFromFile<T>(string filePath)
    {
        using FileStream fileStream = File.OpenRead(filePath);
        return Serializer.Deserialize<T>(fileStream);
    }

    public static string ToBeautyString<T>(this IEnumerable<T> list)
    {
        string result = "[";

        foreach (T obj in list) result += $"{obj}, ";

        if (result.Length > 2) result = result[..^2];

        return result + "]";
    }

    public static string ConvertFileName(string fileName, char replacementChar = '_')
    {
        // Паттерн для удаления запрещенных символов и пробелов в начале и конце строки
        string pattern = @"^[^a-zA-Z0-9]+|[^a-zA-Z0-9_]+(?=.*[^_])|[^a-zA-Z0-9_]+$";

        // Удаляем запрещенные символы и пробелы
        string sanitizedFileName = Regex.Replace(fileName, pattern, $"{replacementChar}");

        // Если получившаяся строка пустая, заменяем на один знак подчеркивания
        if (string.IsNullOrEmpty(sanitizedFileName))
            sanitizedFileName = "_";
        else
            // Удаляем знак подчеркивания в конце строки
            sanitizedFileName = sanitizedFileName.TrimEnd('_').TrimStart('_');

        return sanitizedFileName;
    }

    /// <summary>
    ///     Если папка не существует, то создаёт, иначе ничего не делает
    /// </summary>
    public static void CreateFolderIfNotExists(string folderPath)
    {
        // Проверяем существование папки
        if (!Directory.Exists(folderPath))
            // Создаем папку, если она не существует
            Directory.CreateDirectory(folderPath);
    }
}

public class FrameCounter
{
    public const int MaximumSamples = 100;

    private readonly Queue<float> _sampleBuffer = new();
    public long TotalFrames { get; private set; }
    public float TotalSeconds { get; private set; }
    public float AverageFramesPerSecond { get; private set; }
    public float CurrentFramesPerSecond { get; private set; }

    public void Update(float deltaTime)
    {
        CurrentFramesPerSecond = 1.0f / deltaTime;

        _sampleBuffer.Enqueue(CurrentFramesPerSecond);

        if (_sampleBuffer.Count > MaximumSamples)
        {
            _sampleBuffer.Dequeue();
            AverageFramesPerSecond = _sampleBuffer.Average(i => i);
        }
        else
        {
            AverageFramesPerSecond = CurrentFramesPerSecond;
        }

        TotalFrames++;
        TotalSeconds += deltaTime;
    }
}