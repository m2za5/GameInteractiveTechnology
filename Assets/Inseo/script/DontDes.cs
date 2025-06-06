using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDes : MonoBehaviour
{
    private static DontDes instance;
    void Awake()
    {
        // �ߺ� ����: �̹� �����ϴ� �ν��Ͻ��� �ִٸ� ����
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // �� ������Ʈ�� �� ��ȯ �� �ı����� ����
    }
}
