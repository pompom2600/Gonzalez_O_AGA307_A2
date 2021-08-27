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
    private Image sliderImg;

    [SerializeField] private Color fullPowerColour;
    [SerializeField] private Color lowBatteryColour;

    void Start()
    {
        torchLight = GetComponentInChildren<Light2D>();
        torchLight.enabled = isTurnedOn;
        originalIntensity = torchLight.intensity;
        originalRadius = torchLight.pointLightOuterRadius;
        timer = batteryLife;
        originalSlider = torchSlider.value;
        mainCam = Camera.main;
        sliderImg = torchSlider.fillRect.GetComponent<Image>();
    }

    
    void Update()
    {
        Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(Input.mousePosition); //Light follows the mouse position
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
            if (timer >= 0) 
            {
                float displayT = timer / batteryLife;
                float t = Mathf.Min( timer / fullPowerTime, 1);
                torchSlider.value = displayT;
                torchLight.pointLightOuterRadius = Mathf.Lerp(0, originalRadius, t);
                torchLight.intensity = Mathf.Lerp(0, originalIntensity, t);

                sliderImg.color = (timer > fullPowerTime) ? fullPowerColour : lowBatteryColour;

            }
        }
    }
    // Recharge the Battery--------------------------------
    public void RechargeBattery() 
    {
        torchLight.intensity = originalIntensity;
        torchLight.pointLightOuterRadius = originalRadius;
        timer = batteryLife;
        torchSlider.value = originalSlider;
    }
}
