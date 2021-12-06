using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IngameUI : MonoBehaviour
{
    public Text TimeLap;
    public Text ClearTxt;
    public float lap;
    public static int min;
    public static int sec;
    public static int msec;
    public GameObject GameOverMsg;
    public GameObject ClearMsg;
    public GameObject Player;
    public GameObject SpawnPoint;

    void Update()
    {
        // 캐릭터 생존 and 게임 진행 중 클리어타임 랩 지속적으로 증가
        if (!PlayerFSM.IsDead && !PlayerInteract.Goal)
            lap += Time.deltaTime;

        min = (int)lap / 60;
        sec = (int)lap % 60;
        msec = (int)(lap * 100) % 100;

        TimeLap.text = string.Format("{0:D2}", min)
            + " : " + string.Format("{0:D2}", sec)
            + " : " + string.Format("{0:D2}", msec);

        ClearTxt.text = "Clear Time " + string.Format("{0:D2}", min)
            + " : " + string.Format("{0:D2}", sec)
            + " : " + string.Format("{0:D2}", msec);

        // 게임 진행 중 캐릭터 사망시
        if (PlayerFSM.IsDead && !PlayerInteract.Goal)
            GameOverMsg.SetActive(true); // 게임 오버 메시지 출력
        // 캐릭터 생존 중 게임 클리어시
        if (!PlayerFSM.IsDead && PlayerInteract.Goal)
            ClearMsg.SetActive(true); // 게임 클리어 메시지 출력
    }

    // 캐릭터 리스폰
    public void Respawn()
    {
        lap = 0.0f;
        PlayerInteract.CurrentHealth = 100.0f;
        PlayerInteract.IsBurned = false;
        PlayerInteract.Goal = false;
        PlayerFSM.IsDead = false;
        // Player.transform.position = SpawnPoint.transform.position;
        ClearMsg.SetActive(false);
        GameOverMsg.SetActive(false);
    }

    // 게임 재시작
    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}