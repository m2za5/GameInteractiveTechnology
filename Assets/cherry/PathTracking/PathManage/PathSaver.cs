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

    //public void SavePath()
    //{
    //    PathData data = new PathData { points = drawer.GetPath() };
    //    string json = JsonUtility.ToJson(data, true);
    //    //File.WriteAllText(Application.dataPath + "/" + fileName, json);
    //    string fullPath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
    //}

    public void SavePath()
    {
        PathData data = new PathData { points = PathManager.Instance.drawer.GetPath() };
        string json = JsonUtility.ToJson(data, true);
        string fullPath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
        File.WriteAllText(fullPath, json);
        Debug.Log("��� ���� �Ϸ�: " + fullPath);
    }

    public List<Vector3> LoadPath()
    {
        string fullPath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
        if (!File.Exists(fullPath))
        {
            Debug.LogWarning("��� ������ �����ϴ�: " + fullPath);
            return new List<Vector3>();
        }

        string json = File.ReadAllText(fullPath);
        return JsonUtility.FromJson<PathData>(json).points;
    }
}