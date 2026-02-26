using UnityEngine;
using UnityEngine.Rendering;             // Volume 사용
using UnityEngine.Rendering.Universal;   // URP 효과 사용

public class DamageVignette : MonoBehaviour
{
    [SerializeField]
    private Volume globalVolume;

    private Vignette vignette;

    [SerializeField]
    private float maxIntensity = 0.5f;

    [SerializeField, Header("이펙트 깜박임 속도")]
    private float pulseSpeed = 5.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bool isVignetteAvailable = globalVolume.profile.TryGet(out vignette);
        if (isVignetteAvailable) 
        {
            Debug.Log("비네팅 효과 있음");
        }
    }
    
    public void UpdateVignette(float healthPercent)
    {
        if (vignette == null)
        {
            return;
        }

        // 체력이 낮을수록 intensity는 높아짐
        float intensity = (1.0f - healthPercent) * maxIntensity;

        vignette.intensity.value = intensity;

        // 체력이 30% 이하면 빨간색으로 깜빡임 이펙트
        if (healthPercent <= 0.3f)
        {
            vignette.color.value = Color.red;

            vignette.intensity.value += Mathf.Sin(Time.time * pulseSpeed) * 0.1f;
        }
        else
        {
            vignette.color.value = Color.black;
        }
    }
}
