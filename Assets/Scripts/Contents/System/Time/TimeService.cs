using System;
using UnityEngine;
public class TimeService
{
    readonly TimeSettings settings;
    private DateTime currentTime;
    readonly TimeSpan sunriseTime;
    readonly TimeSpan sunsetTime;

    public DateTime CurrentTime => currentTime;

    public event Action OnSunrise = delegate { };
    public event Action OnSunset = delegate { };
    public event Action<int> OnHourChange = delegate { };

    readonly Observable<bool> isDayTime;
    readonly Observable<int> currentHour;

    public TimeService(TimeSettings settings)
    {
        this.settings = settings;
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(settings.startHour);
        sunriseTime = TimeSpan.FromHours(settings.sunriseHour);
        sunsetTime = TimeSpan.FromHours(settings.sunsetHour);

        isDayTime = new Observable<bool>(IsDayTime());
        currentHour = new Observable<int>(currentTime.Hour);

        isDayTime.ValueChanged += day => (day ? OnSunrise : OnSunset)?.Invoke();
        currentHour.ValueChanged += _ => OnHourChange?.Invoke(currentHour);
    }

    public TimeService(TimeSettings settings, DateTime dataTime)
    {
        this.settings = settings;
        currentTime = dataTime;
        sunriseTime = TimeSpan.FromHours(settings.sunriseHour);
        sunsetTime = TimeSpan.FromHours(settings.sunsetHour);

        isDayTime = new Observable<bool>(IsDayTime());
        currentHour = new Observable<int>(currentTime.Hour);

        isDayTime.ValueChanged += day => (day ? OnSunrise : OnSunset)?.Invoke();
        currentHour.ValueChanged += _ => OnHourChange?.Invoke(currentHour);
    }
    public void UpdateTime(float deltaTime)
    {
        currentTime = currentTime.AddSeconds(deltaTime * settings.timeMultiplier);
        isDayTime.Value = IsDayTime();
        currentHour.Value = currentTime.Hour;
    }

    public DateTime GetCalculateTime(float deltaTime)
    {
        return currentTime.AddSeconds(deltaTime * settings.timeMultiplier);
    }

    public float CalculateSunAngle()
    {
        bool isDay = IsDayTime();
        float startDegree = isDay ? 0 : 180;
        TimeSpan start = isDay ? sunriseTime : sunsetTime;
        TimeSpan end = isDay ? sunsetTime : sunriseTime;

        TimeSpan totalTime = CalculateDifference(start, end);
        TimeSpan elapsedTime = CalculateDifference(start, currentTime.TimeOfDay);

        double percentage = elapsedTime.TotalMinutes / totalTime.TotalMinutes;
        return Mathf.Lerp(startDegree, startDegree + 180, (float)percentage);
    }

    //현재 시간이 낮인지 여부를 판단
    bool IsDayTime() => currentTime.TimeOfDay > sunriseTime && currentTime.TimeOfDay < sunsetTime;

    //두 시간 간의 차이를 계산하는 메서드
    private TimeSpan CalculateDifference(TimeSpan from, TimeSpan to)
    {
        TimeSpan difference = to - from;
        return difference.TotalHours < 0 ? difference + TimeSpan.FromHours(24) : difference;
    }

    public void SetCurrentTime(DateTime currentTime)
    {
        this.currentTime = currentTime;
    }
}