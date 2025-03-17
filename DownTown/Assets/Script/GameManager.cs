using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    // 시간 설정 - 시작 시간(오전 6시) ~ 종료 시간(오전 2시)
    private DateTime gameTime;
    private float deltaTime = 0f;
    private int hour12;                 // 기본 pm(오전)
    // 영어 요일 -> 한글 한 글자 요일 매핑
    private string[] KoreanDays = { "일", "월", "화", "수", "목", "금", "토" };

    [Header("기본 정보")]
    public float time_rate = 0.5f;      // 높으면 빠름 / 낮으면 느림
    public bool playerState;            // 취침 상태인지, 활동 상태인지

    [Header("날짜/시간")]
    public TextMeshProUGUI time_text;
    public TextMeshProUGUI date_text;

    /*
     맑음 : 45
     흐림 : 15
     비   : 30
     번개 : 10
     */
    [Header("날씨 아이콘 ")]
    public Image icon;              // 아이콘 위치
    public Image serenityIcon;      // 맑음
    public Image cloudyIcon;        // 흐림
    public Image rainIcon;          // 비
    public Image lightningIcon;     // 번개

    private void Awake()
    {
        // 초기화
        gameTime = new DateTime(2024, 1, 1, 6, 0, 0);
        playerState = true;
        Weather();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }
    }
    void Update()
    {
        if (playerState)
        {
            // 시간 설정
            Time_Setting();
        }
        // 오전 2시면 강제 취침
        if (gameTime.Hour == 2)
        {
            playerState = false;
            gameTime = gameTime.AddHours(6 - gameTime.Hour).AddMinutes(-gameTime.Minute);
            Debug.Log("강제 취침 및 다음날로 이동");
            // 새로운 날의 시작과 함께 playerState 활성화
            Invoke(nameof(StartNewDay), 1f); // 약간의 딜레이 후 새날 시작
        }
    }
    void StartNewDay()
    {
        playerState = true; // 플레이어 활동 상태로 전환
        Debug.Log("새로운 날이 시작되었습니다!");
        Weather();
    }

    void Time_Setting()
    {
        // 시간을 느리게 하기 위해
        deltaTime = Time.deltaTime * time_rate;
        gameTime = gameTime.AddMinutes(deltaTime);   // 시간을 직접 추가
        
        // 오전 -> 오후 / 오후 -> 오전
        hour12 = gameTime.Hour % 12;
        if (hour12 == 0) hour12 = 12; // 자정과 정오 처리
        string str_pmORam = gameTime.Hour >= 12 ? "PM" : "AM";

        // 한글 요일 가져오기
        string koreanDay = KoreanDays[(int)gameTime.DayOfWeek];

        time_text.text = $"{str_pmORam} {hour12} : {gameTime.Minute}";
        date_text.text = $"{gameTime.Month}월 {gameTime.Day}일 ({koreanDay})";
    }
    void Weather()
    {
        Debug.Log("날씨 초기화");
        int random = UnityEngine.Random.Range(0, 100);

        if (random <= 45)
        {
            //icon = serenityIcon;
            Debug.Log("맑음");
        }
        else if(random <= 50)
        {
            //icon = cloudyIcon;
            Debug.Log("흐림");
        }
        else if (random <= 80)
        {
            //icon = rainIcon;
            Debug.Log("비");
        }
        else
        {
            //icon = lightningIcon;
            Debug.Log("번개");
        }
    }
}
