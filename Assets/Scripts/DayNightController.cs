using UnityEngine;

public class DayNightController : MonoBehaviour
{
    public static DayNightController Instance;

    [SerializeField] private float startHour = 12.0f;
    [SerializeField] private float SecondsPerGameHour = 10.0f;
    [SerializeField] private bool loop24Hours = true;

    [SerializeField] private Light sunLight;
    [SerializeField] private float sunYaw = 30.0f;

    [SerializeField] private Material skyboxMaterial;
    [SerializeField] private string skyboxExposurePropertyName = "_Exposure";
    [SerializeField] private float nightExposure = 0.6f;
    [SerializeField] private float dayExposure = 1.2f;

    private float currentHour;
    private Material runtimeSkybox;

    private void Awake()
    {
        Instance = this;

        currentHour = startHour;

        if (skyboxMaterial != null)
        {
            runtimeSkybox = new Material(skyboxMaterial);
            RenderSettings.skybox = runtimeSkybox;
        }
    }

    void Update()
    {
        AdvanceTime();
        UpdateSunRotation();
        UpdateSkybox();
    }

    public float GetCurrentHour()
    {
        return currentHour;
    }

    // 시간 값을 낮 비율로 바꿈.
    float ComputeDayFactor01(float hour)
    {
        if (hour < 6.0f || hour > 18.0f)
        {
            return 0.0f;
        }

        if (hour <= 12.0f)
        {
            float t = (hour - 6.0f) / 6.0f;
            return Mathf.Clamp01(t);
        }
        else
        {
            float t = (hour - 12.0f) / 6.0f;
            return Mathf.Clamp01(1 - t);
        }
    }

    void UpdateSunRotation()
    {
        if (sunLight == null)
        {
            return;
        }

        float t = currentHour / 24.0f;
        float pitch = t * 360.0f - 90.0f;

        sunLight.transform.rotation = Quaternion.Euler(pitch, sunYaw, 0.0f);
    }

    void AdvanceTime()
    {
        float gameHourPerSecond = 1.0f / SecondsPerGameHour;
        currentHour += gameHourPerSecond * Time.deltaTime;

        if (loop24Hours == true)
        {
            if (currentHour >= 24.0f)
            {
                currentHour = 0.0f;
            }

            if (currentHour < 0.0f)
            {
                currentHour += 24.0f;
            }
        }
        else
        {
            currentHour = Mathf.Clamp(currentHour, 0.0f, 24.0f);
        }
    }

    void UpdateSkybox()
    {
        if (runtimeSkybox == null)
        {
            return;
        }

        // 낮 비율 계산 : 6시 ~ 18시는 낮, 0~1 사이로 보간
        float day01 = ComputeDayFactor01(currentHour);

        float exposure = Mathf.Lerp(nightExposure, dayExposure, day01);
        if (runtimeSkybox.HasProperty(skyboxExposurePropertyName))
        {
            runtimeSkybox.SetFloat(skyboxExposurePropertyName, exposure);
        }
    }

    public float GetDaysFactor01()
    {
        return ComputeDayFactor01(currentHour);
    }
}