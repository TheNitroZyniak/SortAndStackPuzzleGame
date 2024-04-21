using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using DG.Tweening;


public class UIManager : MonoBehaviour {

    [SerializeField] private BallCell[] ballCells;
    [SerializeField] private GameObject victoryPopup, lostPopup;


    public void OpenLosePopup() {
        lostPopup.SetActive(true);
    }

    public void OpenVictoryPopup() {
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        currentLevel++;
        if (currentLevel == 4) currentLevel = 3;
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        StartCoroutine(OpenVictory());
    }

    IEnumerator OpenVictory() {
        yield return new WaitForSeconds(0.5f);
        victoryPopup.SetActive(true);
    }

    public void SetScene(int scene) {
        SceneManager.LoadScene(scene);
    }
}

