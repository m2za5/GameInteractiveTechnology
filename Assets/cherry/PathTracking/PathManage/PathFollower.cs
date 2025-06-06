using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    public float moveSpeed = 2f;

    private List<Vector3> path;
    private int currentIndex = 0;
    private int direction = 1; // 👉 +1: 정방향, -1: 역방향
    private bool following = false;

    public void BeginFollow()
    {
        path = PathManager.Instance.saver.LoadPath();
        currentIndex = 0;
        following = true;

        Vector3 pathVec = new Vector3(path[0].x, path[0].y, transform.position.z);
        Debug.Log(pathVec);
        transform.position = pathVec;
    }

    public void ResetPath()
    {
        following = false;
        currentIndex = 0;
        path = null;
    }

    void Update()
    {
        if (!following || path == null || currentIndex >= path.Count) return;

        Vector3 pathVec = new Vector3(path[currentIndex].x, path[currentIndex].y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, pathVec, moveSpeed * Time.deltaTime);
        
        
        if (Vector3.Distance(transform.position, pathVec) < 0.1f)
        {
            currentIndex += direction;

            // ✅ 방향 전환 조건
            if (currentIndex >= path.Count)
            {
                currentIndex = path.Count - 2;
                direction = -1;
            }
            else if (currentIndex < 0)
            {
                currentIndex = 1;
                direction = 1;
            }
        }
    }
}