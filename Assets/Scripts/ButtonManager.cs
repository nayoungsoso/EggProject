using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public ButtonType currentType;
    public GameObject panel;
    public string sceneName;

    public void OnBtnClick()
    {
        switch (currentType)
        {
            case ButtonType.ChangeScene:
                SceneManager.LoadScene(sceneName);
                Debug.Log("씬 전환");
                break;

            case ButtonType.OpenPanel:
                panel.SetActive(true);
                Debug.Log("패널 활성화");
                break;

            case ButtonType.ClosePanel:
                panel.SetActive(false);
                Debug.Log("패널 비활성화");
                break;

            case ButtonType.QuitApp:
                Application.Quit();
                Debug.Log("게임종료");
                break;
        }
    }
}