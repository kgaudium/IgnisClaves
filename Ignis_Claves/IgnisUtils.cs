using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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

    public static void Serialize<T>(T data, string absoluteFilePath)
    {
        using FileStream fileStream = File.Create(absoluteFilePath);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(fileStream, data);
        fileStream.Close();

        //Serializer.Serialize(fileStream, data);
    }

    public static T Deserialize<T>(string absoluteFilePath)
    {
        using FileStream fileStream = File.OpenRead(absoluteFilePath);
        BinaryFormatter formatter = new BinaryFormatter();
        return (T)formatter.Deserialize(fileStream);

        //return Serializer.Deserialize<T>(fileStream);
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

    private readonly Queue<float> sampleBuffer = new();
    public long TotalFrames { get; private set; }
    public float TotalSeconds { get; private set; }
    public float AverageFramesPerSecond { get; private set; }
    public float CurrentFramesPerSecond { get; private set; }

    public void Update(float deltaTime)
    {
        CurrentFramesPerSecond = 1.0f / deltaTime;

        sampleBuffer.Enqueue(CurrentFramesPerSecond);

        if (sampleBuffer.Count > MaximumSamples)
        {
            sampleBuffer.Dequeue();
            AverageFramesPerSecond = sampleBuffer.Average(i => i);
        }
        else
        {
            AverageFramesPerSecond = CurrentFramesPerSecond;
        }

        TotalFrames++;
        TotalSeconds += deltaTime;
    }
}