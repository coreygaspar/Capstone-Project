using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBoss : MonoBehaviour
{
    public TMP_Text healthBarText;
    public Slider healthSlider;
    Damageable bossDamageable;

    private void Awake()
    {
        GameObject boss = GameObject.FindGameObjectWithTag("Wizard");
        bossDamageable = boss.GetComponent<Damageable>();

        if(boss == null) 
        {
            Debug.Log("No boss found in the scene. Make sure it has tag 'Enemy'.");
        }
    }

    private void OnEnable()
    {
        bossDamageable.healthChanged.AddListener(OnBossHealthChanged);
    }

    private void OnDisable()
    {
        bossDamageable.healthChanged.RemoveListener(OnBossHealthChanged);
    }

    // Start is called before the first frame update
    void Start()
    {
        healthSlider.value = CalculateSliderPercentage(bossDamageable.Health, bossDamageable.MaxHealth);
        healthBarText.text = "Boss HP " + bossDamageable.Health + " / " + bossDamageable.MaxHealth;
    }

    private float CalculateSliderPercentage(float currentHealth, float maxHealth) {
        return currentHealth / maxHealth;
    }

    private void OnBossHealthChanged(int newHealth, int maxHealth)
    {
        healthSlider.value = CalculateSliderPercentage(newHealth, maxHealth);
        healthBarText.text = "Boss HP " + newHealth + " / " + maxHealth;
    }
}
