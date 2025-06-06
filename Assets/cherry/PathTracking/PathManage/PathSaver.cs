using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class PathData
{
    public List<Vector3> points = new();
}

public class PathSaver : MonoBehaviour
{
    public string fileName = "saved_path.json";

    string GetFullPath()
    {
        return Path.Combine(Application.streamingAssetsPath, fileName);
    }

    public void SavePath()
    {
        PathData data = new PathData { points = PathManager.Instance.drawer.GetPath() };
        string json = JsonUtility.ToJson(data, true);
        string fullPath = GetFullPath();
        File.WriteAllText(fullPath, json);
        Debug.Log("��� ���� �Ϸ�: " + fullPath);
    }

    public List<Vector3> LoadPath()
    {
        string fullPath = GetFullPath();
        if (!File.Exists(fullPath))
        {
            Debug.LogWarning("��� ������ �����ϴ�: " + fullPath);
            return new List<Vector3>();
        }

        string json = File.ReadAllText(fullPath);
        return JsonUtility.FromJson<PathData>(json).points;
    }
}