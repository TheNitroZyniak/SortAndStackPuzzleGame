using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using DG.Tweening;
using Zenject;
//using Firebase.Analytics;

public class UIManager : MonoBehaviour {
    [Inject] MainGameManager _mainGameManager;
    [Inject] Spawner _spawner;
    [Inject] AudioManager _audioManager;
    [Inject] Timer _timer;

    [SerializeField] private GameObject victoryPopup, lostPopup;
    [SerializeField] private Image freezeImage, blackImage;
    [SerializeField] private GameObject MenuScene, GameScene;
    [SerializeField] TextMeshProUGUI currentLevelText, gameLevelText;


    [SerializeField] Toggle[] mOrS;
    public static bool isMusicEnabled;
    public static bool isSoundsEnabled;

    // -45 -15 -14

    private void Start() {
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel") + 1;
        currentLevelText.text = currentLevel.ToString();

        int m = PlayerPrefs.GetInt("Music");
        int s = PlayerPrefs.GetInt("Sounds");

        if (m == 0) OnMusic(true);
        else OnMusic(false);

        if (s == 0) OnSounds(true);
        else OnSounds(false);
    }

    [SerializeField] TextMeshProUGUI FPSText;

    float poolingTime = 1f;
    float time;
    int frameCount;


    private void Update() {
        time += Time.deltaTime;
        frameCount++;
        if(time > poolingTime) {
            int frameRate = Mathf.RoundToInt(frameCount / time);
            FPSText.text = frameRate.ToString() + " FPS";
            time -= poolingTime;
            frameCount = 0;
        }

        
    }

    public void OpenLosePopup() {
        lostPopup.SetActive(true);
    }

    public void OpenVictoryPopup() {
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel");

        //LevelComplete level = new LevelComplete();
        //level.level = currentLevel;
        //string json = JsonUtility.ToJson(level);
        //AppMetrica.Instance.ReportEvent("Level_Completed", json);

        //FirebaseAnalytics.LogEvent("Level_Completed", new Parameter("Level", currentLevel));

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
        _timer.StopTimer();
        Color color = freezeImage.color;
        color.a = 50f/256f;
        freezeImage.DOColor(color, 0.5f);
        yield return new WaitForSeconds(10);
        color.a = 0;
        freezeImage.DOColor(color, 0.5f);
        _timer.ResumeTimer();
    }

    public void FromMenuToGame() {
        ToOne(true, false);
        _audioManager.PlaySound(2);
    }

    public void FromGameToGame() {
        ToOne(true, true);
    }

    public void FromGameToMenu() {
        ToOne(false, true);
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel") + 1;
        currentLevelText.text = currentLevel.ToString();
    }

    void ToOne(bool toGamefromMenu, bool showAds) {

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

                if (showAds) {
                    AdsManager.Instance.ShowInterstitial();
                }

                _spawner.CreateLevel();

                _audioManager.PlayMusic(Random.Range(0, 2));
            } 
            else {
                MenuScene.SetActive(true);
                GameScene.SetActive(false);
                print("Dfffg");
            }
        });

    }

    void ToZero() {
        Color color = blackImage.color;
        color.a = 0;
        blackImage.DOColor(color, 0.2f);

    }




    public void OnSounds(bool active) {
        if (active) {
            PlayerPrefs.SetInt("Sounds", 0);
            isSoundsEnabled = true;
            mOrS[1].isOn = true;

        } else {
            PlayerPrefs.SetInt("Sounds", 1);
            isSoundsEnabled = false;
            mOrS[1].isOn = false;
        }
    }


    public void OnMusic(bool active) {
        if (active) {
            PlayerPrefs.SetInt("Music", 0);
            isMusicEnabled = true;
            mOrS[0].isOn = true;
            //_audioManager.PlayMusic(0);

        } 
        else {
            _audioManager.StopMusic();
            PlayerPrefs.SetInt("Music", 1);
            isMusicEnabled = false;
            mOrS[0].isOn = false;
        }
    }

}

