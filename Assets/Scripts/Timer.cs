using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    private float elapsedTime = 30f;
    private float initialTime = 30f; // Store the initial timer value for resetting
    private bool isColorChanging = false;
    private Coroutine colorChangeCoroutine;
    private Coroutine resetCoroutine;

    public System.Action OnTimerFinished;

    void Start()
    {
        // Make sure the timer starts hidden
        timerText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (elapsedTime > 0f)
        {
            // Decrease elapsed time
            elapsedTime -= Time.deltaTime;

            // Update timer text
            timerText.text = Math.Round(elapsedTime, 0).ToString();

            // Color change logic (for when timer runs low)
            if (elapsedTime <= 11f && !isColorChanging)
            {
                isColorChanging = true;
                if (colorChangeCoroutine != null)
                {
                    StopCoroutine(colorChangeCoroutine);
                }
                colorChangeCoroutine = StartCoroutine(ChangeColorWithDelay(Color.red, 0.5f));
            }

            if (elapsedTime > 11f && timerText.color != Color.white)
            {
                if (colorChangeCoroutine != null)
                {
                    StopCoroutine(colorChangeCoroutine);
                }
                colorChangeCoroutine = StartCoroutine(ChangeColorWithDelay(Color.white, 0.5f));
            }

            // If the timer reaches 0, reset and hide the text
            if (elapsedTime <= 0f)
            {
                elapsedTime = 0f;
                if (resetCoroutine == null)
                {
                    resetCoroutine = StartCoroutine(ResetTimerAndHideText());
                }
            }
        }
    }

    // Method to start the timer with a specific duration
    public void StartTimer(float duration)
    {
        elapsedTime = duration;
        timerText.color = Color.white;       // reset to white
        isColorChanging = false;             // allow color to change again
        timerText.gameObject.SetActive(true);
    }

    // Coroutine to change color with delay
    private IEnumerator ChangeColorWithDelay(Color targetColor, float delay)
    {
        yield return new WaitForSeconds(delay);
        timerText.color = targetColor;
    }

    // Coroutine to reset timer and hide text
    private IEnumerator ResetTimerAndHideText()
    {
        timerText.color = Color.white;       
        isColorChanging = false;
        timerText.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        elapsedTime = initialTime;
        resetCoroutine = null;
    }

    // Optional: Method to manually show the timer again if needed
    public void ShowTimer()
    {
        timerText.gameObject.SetActive(true); // Make the text visible again
    }
}
