using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public void ExitButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
