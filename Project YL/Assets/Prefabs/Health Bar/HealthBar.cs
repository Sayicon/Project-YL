using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Canvas healthBarCanvas;
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public TextMeshProUGUI healthText;
    public string characterName;

    //Scaling health bar Scaling
    public float baseScale = 1f;
    public float scaleMultiplier;

    void Start()
    {
        if (healthText != null)
		{
            healthText.text = characterName;
		}
    }
    void LateUpdate()
    {
        if (Camera.main == null) return;
        if (healthBarCanvas == null) return;

        float dist = Vector3.Distance(Camera.main.transform.position, healthBarCanvas.transform.position);
        healthBarCanvas.transform.localScale = Vector3.one * dist * scaleMultiplier * baseScale;
        healthBarCanvas.transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                         Camera.main.transform.rotation * Vector3.up);
    }

    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;

        fill.color = gradient.Evaluate(1f);
    }
    public void SetHealth(int health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        SetHealthText(characterName, health, (int)slider.maxValue);
    }
    private void SetHealthText(String name, int health, int maxHealth)
    {
        if (healthText != null)
		{
            healthText.text = name + "\n" + health + " / " + maxHealth;
		}
    }
}
