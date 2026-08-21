using UnityEngine;
using TMPro;

public class GameClock : MonoBehaviour
{
    [Header("날짜 설정")]
    [SerializeField] private int startYear = 852;
    [SerializeField] private int startMonth = 8;
    [SerializeField] private int startDay = 19;

    [Header("시작 시간 설정")]
    [SerializeField, Range(0, 23)] private int startHour = 6;
    [SerializeField, Range(0, 59)] private int startMinute = 0;

    [Header("종료 시간 설정")]
    [SerializeField, Range(0, 23)] private int endHour = 18;
    [SerializeField, Range(0, 59)] private int endMinute = 0;

    [Header("시간 배율")]
    [Tooltip("현실에서 몇 초가 지나야 게임 시간이 1분 흐를지 설정")]
    [SerializeField] private float secondsPerGameMinute = 1f;

    [Header("UI")]
    [SerializeField] private TMP_Text timeText;

    // 현재 게임 날짜
    private int year;
    private int month;
    private int day;

    // 현재 게임 시간
    private int hour;
    private int minute;

    // 시간 누적
    private float minuteTimer;

    // 종료 여부
    private bool isTimeOver;

    private void Start()
    {
        // 설정한 날짜/시간으로 초기화
        year = startYear;
        month = startMonth;
        day = startDay;

        hour = startHour;
        minute = startMinute;

        isTimeOver = false;

        UpdateUI();
    }

    private void Update()
    {
        // 종료 시간이 되었다면 시간 진행 중지
        if (isTimeOver)
        {
            return;
        }

        minuteTimer += Time.deltaTime;

        // 설정한 시간이 지나면 게임 시간 1분 증가
        if (minuteTimer >= secondsPerGameMinute)
        {
            minuteTimer -= secondsPerGameMinute;

            AddGameMinute();
        }
    }

    private void AddGameMinute()
    {
        minute++;

        // 60분 → 1시간
        if (minute >= 60)
        {
            minute = 0;
            hour++;
        }

        // 24시간 → 다음 날
        if (hour >= 24)
        {
            hour = 0;
            AddGameDay();
        }

        // 종료 시간에 도달했는지 확인
        if (hour > endHour || (hour == endHour && minute >= endMinute))
        {
            hour = endHour;
            minute = endMinute;

            isTimeOver = true;
        }

        UpdateUI();
    }

    private void AddGameDay()
    {
        day++;

        // 월의 마지막 날
        if (day > GetDaysInMonth(month))
        {
            day = 1;
            month++;
        }

        // 12월 → 다음 해
        if (month > 12)
        {
            month = 1;
            year++;
        }
    }

    private int GetDaysInMonth(int month)
    {
        switch (month)
        {
            case 2:
                return 28;

            case 4:
            case 6:
            case 9:
            case 11:
                return 30;

            default:
                return 31;
        }
    }

    private void UpdateUI()
    {
        timeText.text =
            $"왕국력 {year}년 {month}월 {day}일 {hour:00}:{minute:00}";
    }
}
