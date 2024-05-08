using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using DG.Tweening;
using Zenject;

public class UIManager : MonoBehaviour {
    [Inject] MainGameManager _mainGameManager;
    [Inject] Spawner _spawner;
    [Inject] AudioManager _audioManager;

    [SerializeField] private GameObject victoryPopup, lostPopup;
    [SerializeField] private Image freezeImage, blackImage;

    [SerializeField] private GameObject MenuScene, GameScene;

    [SerializeField] TextMeshProUGUI currentLevelText, gameLevelText;

    private void Start() {
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel") + 1;
        currentLevelText.text = currentLevel.ToString();
    }

    public void OpenLosePopup() {
        lostPopup.SetActive(true);
    }

    public void OpenVictoryPopup() {
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        currentLevel++;
        //if (currentLevel == 4) currentLevel = 3;
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        StartCoroutine(OpenVictory());
    }

    IEnumerator OpenVictory() {
        yield return new WaitForSeconds(2f);
        _audioManager.PlaySound(0);
        victoryPopup.SetActive(true);
    }

    public void UseMagnet() {
        _mainGameManager.UseMagnet();
    }

    public void UseUndo() {
        _mainGameManager.OnUndoButtonPress();
    }

    public void UseMix() {
        _mainGameManager.Mix();
    }

    public void UseFreeze() {
        StartCoroutine(UseFreezeI());
    }

    IEnumerator UseFreezeI() {
        Color color = freezeImage.color;
        color.a = 50f/256f;
        freezeImage.DOColor(color, 0.5f);
        yield return new WaitForSeconds(10);
        color.a = 0;
        freezeImage.DOColor(color, 0.5f);
    }

    public void FromMenuToGame() {
        ToOne(true);
        _audioManager.PlaySound(2);
    }

    public void FromGameToGame() {
        ToOne(true);
    }

    public void FromGameToMenu() {
        ToOne(false);
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel") + 1;
        currentLevelText.text = currentLevel.ToString();
    }

    void ToOne(bool toGamefromMenu) {

        Color color = blackImage.color;
        color.a = 1;
        blackImage.DOColor(color, 0.2f).OnComplete(() => {
            ToZero();
            if (toGamefromMenu) {
                MenuScene.SetActive(false);
                GameScene.SetActive(true);

                int currentLevel = PlayerPrefs.GetInt("CurrentLevel");
                currentLevel++;
                gameLevelText.text = "Level " + currentLevel.ToString();

                _mainGameManager.DisableAll();
                victoryPopup.SetActive(false);
                lostPopup.SetActive(false);

                _spawner.CreateLevel();

                _audioManager.PlayMusic(Random.Range(0, 2));
            } 
            else {
                MenuScene.SetActive(true);
                GameScene.SetActive(false);

            }
        });

    }

    void ToZero() {
        Color color = blackImage.color;
        color.a = 0;
        blackImage.DOColor(color, 0.2f);

    }
}

