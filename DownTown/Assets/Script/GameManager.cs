using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // 시간 설정 - 시작 시간(오전 6시) ~ 종료 시간(오전 2시)
    private int hour = 6;
    private int minute;
    private float timer = 0f;
    private bool pmORam = false;        // 기본 pm(오전)
    public TextMeshProUGUI time_text;
    public float time_rate = 0.5f;      // 높으면 빠름 / 낮으면 느림
    public static GameManager instance;
    public bool playerState = true;            // 취침 상태인지, 활동 상태인지
    private string nowTime;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }
    }
    void Update()
    {
        // 시간 설정
        Time_Setting();
        // 오전 2시면 강제 취침
        if (hour == 2 && minute == 0 && pmORam == false)
        {
            playerState = false;
            Debug.Log(playerState + "강제 취침");
        }
    }
    void Time_Setting()
    {
        // 시간을 느리게 하기 위해
        timer += Time.deltaTime * time_rate;
        if (minute >= 60)
        {
            hour += 1;
            minute = 0;
            timer = 0f;
            Debug.Log("분 초기화");
        }
        // 오전 -> 오후 / 오후 -> 오전
        if(hour > 12)
        {
            pmORam = !pmORam;
            hour = 1;
            Debug.Log($"pmORam : {pmORam} + 시간 초기화 + 오전오후 변경");
        }
        minute = (int)timer;
        time_text.text = hour + " : " + minute;
    }
}
