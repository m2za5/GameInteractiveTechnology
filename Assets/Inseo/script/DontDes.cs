using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDes : MonoBehaviour
{
    private static DontDes instance;
    void Awake()
    {
        // 중복 방지: 이미 존재하는 인스턴스가 있다면 제거
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // 이 오브젝트는 씬 전환 시 파괴되지 않음
    }
}
