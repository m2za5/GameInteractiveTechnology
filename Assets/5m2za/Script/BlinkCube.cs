using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class BlinkCube : MonoBehaviour
{
    public GameObject[] Cubes;
    public float blinkSpeed = 0.1f;
    public float blinkDuration = 2f;

    private int currentCubeIndex = -1;

    void Start()
    {
        foreach (GameObject cube in Cubes)
        {
            if (cube != null)
            {
                cube.SetActive(false);
            }
        }

        StartCoroutine(ActivateNextCube());
    }

    IEnumerator ActivateNextCube()
    {
        while (true)
        {
            foreach (GameObject cube in Cubes)
            {
                if (cube != null)
                {
                    cube.SetActive(false);
                }
            }

            int randomIndex = Random.Range(0, Cubes.Length);
            if (Cubes[randomIndex] != null)
            {
                Cubes[randomIndex].SetActive(true);
            }
            yield return StartCoroutine(Blinking(Cubes[randomIndex]));
            yield return new WaitForSeconds(blinkDuration);
        }
    }
    IEnumerator Blinking(GameObject cube)
    {
        float elapsedTime = 0f;

        while (elapsedTime < blinkDuration)
        {
            cube.SetActive(!cube.activeSelf);
            elapsedTime += blinkSpeed;
            yield return new WaitForSeconds(blinkSpeed);
        }

        cube.SetActive(false);
    }
}