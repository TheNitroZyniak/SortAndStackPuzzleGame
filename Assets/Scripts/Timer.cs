using System.Collections;
using UnityEngine;
using TMPro;
using Zenject;

public class Timer : MonoBehaviour{
    [Inject] AudioManager _audioManager;
    [Inject] MainGameManager _mainGameManager;

    [SerializeField] TextMeshProUGUI text;
    int sec;

    public void ResumeTimer() {
        StartCoroutine(Cdown(sec));
    }

    public void StartTimer(int seconds) {
        StartCoroutine(Cdown(seconds));
    }

    public void StopTimer() {
        StopAllCoroutines();
    }


    IEnumerator Cdown(int seconds) {
        int count = seconds;
        while (count > 0) {
            text.text = count.ToString();
            yield return new WaitForSeconds(1);
            count--;
            sec = count;
        }
        text.text = "0";

        yield return new WaitForSeconds(1);
        //_audioManager.PlaySound(0);
        _mainGameManager.GameLost();
        text.text = "";
    }

}
