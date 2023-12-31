﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathmatchEndController : MonoBehaviour
{
    private bool canProceed = false;
    private int count = 0;
    public UnityEngine.UI.Text playerNameText;
    public UnityEngine.UI.Text standingsText;

    void Start()
    {
        playerNameText.text = GeneralScript.Player1.NumberOfLives > 0 ? "PLAYER 1" : "PLAYER 2";
        var startingLives = DifficultyRepository.GetNumberOfLives();
        standingsText.text = (startingLives - GeneralScript.Player2.NumberOfLives) + "-" + (startingLives - GeneralScript.Player1.NumberOfLives);

        StartCoroutine(FireFireworks());

        StartCoroutine(SetProceed());
    }

    IEnumerator FireFireworks()
    {
        if (count++ > 10)
        {
            yield break;
        }

        yield return new WaitForSeconds(0.5f);

        var fireworksInstance = Instantiate(Resources.Load<GameObject>(ResourceNames.Fireworks), this.transform.position, Quaternion.Euler(new Vector3(0, 0, 0))) as GameObject;
        fireworksInstance.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-2000, 2000), Random.Range(10000, 20000)));
        StartCoroutine(FireFireworks());
    }

    IEnumerator SetProceed()
    {
        yield return new WaitForSeconds(0.5f);
        this.canProceed = true;
    }

    public void GoToMenu()
    {
        if(!this.canProceed)
        {
            return;
        }

        SceneManager.LoadScene(SceneNames.Menu);
    }

    public void Restart()
    {
        if (!this.canProceed)
        {
            return;
        }

        LevelRepository.Randomize();
        PlayerSettingsRepository.PlayerOneSettings.LivesLeft = DifficultyRepository.GetNumberOfLives();
        PlayerSettingsRepository.PlayerTwoSettings.LivesLeft = DifficultyRepository.GetNumberOfLives();
        SceneManager.LoadScene(LevelRepository.NextRandomized().SceneName);
    }
}
