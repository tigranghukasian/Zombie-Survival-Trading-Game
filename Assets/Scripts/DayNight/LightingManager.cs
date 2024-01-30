using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [SerializeField] private Light directionalLight;
    [SerializeField] private LightingPreset preset;
    [SerializeField, Range(0, 360)] private float yRotation;
    [SerializeField, Range(0, 24)] private float timeOfDay;
    [SerializeField] private float dayLength;

    private void Update()
    {
        if (preset == null)
        {
            return;
        }

        if (Application.isPlaying)
        {
            float dayFraction = Time.deltaTime / dayLength;
             // Adjust this value to control the speed of the day/night cycle
            timeOfDay += Time.deltaTime;
            timeOfDay %= dayLength;
            float normalizedTimeOfDay = timeOfDay / dayLength * 24f;
            UpdateLighting(normalizedTimeOfDay / 24f);
        }

        else
        {
            UpdateLighting(timeOfDay / 24f);
        }

    }

    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = preset.FogColor.Evaluate(timePercent);

        if (directionalLight != null)
        {
            directionalLight.color = preset.DirectionalColor.Evaluate(timePercent);
            directionalLight.transform.localRotation =
                Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, yRotation, 0));
        }
    }
    private void OnValidate()
    {
        if (directionalLight != null)
        {
            return;
        }

        if (RenderSettings.sun != null)
        {
            directionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    directionalLight = light;
                }
            }
        }
    }
}
