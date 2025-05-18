using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject settingMenuUI;
    public GameObject gazeBrightnessUI;

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        //Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OpenSettings()
    {
        pauseMenuUI.SetActive(false);
        settingMenuUI.SetActive(true);
    }

    public void CloseSettings()
    {
        settingMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    public void OpenBrightnessUI()
    {
        settingMenuUI.SetActive(false);
        gazeBrightnessUI.SetActive(true);
        //if (pauseMenuUI.activeSelf)
        //    Time.timeScale = 0f;
       // else if (!settingMenuUI.activeSelf && !gazeBrightnessUI.activeSelf)
        //    Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    public void CloseBrightnessUI()
    {
        gazeBrightnessUI.SetActive(false);
        settingMenuUI.SetActive(true);
    }
    public void QuitGame()
    {
        Debug.Log("게임 종료!");
        Application.Quit();
    }
    void Start()
    {
        pauseMenuUI.SetActive(false);
        settingMenuUI.SetActive(false);
        gazeBrightnessUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool isActive = pauseMenuUI.activeSelf;
            pauseMenuUI.SetActive(!isActive);
            //Time.timeScale = isActive ? 1f : 0f;
            Cursor.lockState = isActive ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !isActive;
        }

    }
}
