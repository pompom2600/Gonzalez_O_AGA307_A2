using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class Torch : MonoBehaviour
{
    private Light2D torchLight;
    private float timer = 0;
    public float batteryLife = 20;
    public float fullPowerTime = 10;
    private bool isTurnedOn = true;
    private float originalIntensity;
    private float originalRadius;
    private Camera mainCam;
    public Slider torchSlider;
    private float originalSlider;

    void Start()
    {
        torchLight = GetComponentInChildren<Light2D>();
        torchLight.enabled = isTurnedOn;
        originalIntensity = torchLight.intensity;
        originalRadius = torchLight.pointLightOuterRadius;
        timer = batteryLife;
        originalSlider = torchSlider.value;
        mainCam = Camera.main;
    }

    
    void Update()
    {
        Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
        transform.up = (mouseWorldPos - transform.position).normalized;


        if (Input.GetButtonDown("Fire1"))
        {
            isTurnedOn = !isTurnedOn;
            torchLight.enabled = isTurnedOn;
        }

        if (isTurnedOn)
        {
            timer -= Time.deltaTime;
            if (timer < fullPowerTime && timer >= 0)
            {
                float t = timer / fullPowerTime;
                torchSlider.value = t;
                torchLight.pointLightOuterRadius = Mathf.Lerp(0, originalRadius, t);
                torchLight.intensity = Mathf.Lerp(0, originalIntensity, t);   
            }
        }
    }
    public void RechargeBattery()
    {
        torchLight.intensity = originalIntensity;
        torchLight.pointLightOuterRadius = originalRadius;
        timer = batteryLife;
        torchSlider.value = originalSlider;
    }
}
