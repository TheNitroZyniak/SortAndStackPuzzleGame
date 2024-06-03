using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Zenject;
using DG.Tweening;

public class VictoryPopup : MonoBehaviour{
    [Inject] Timer _timer;

    [SerializeField] Star[] stars;
    [SerializeField] TextMeshProUGUI time;

    private void OnEnable() {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.8f);

        if (stars.Length > 0) {

            time.text = _timer.GetTime().ToString() + " s";

            float ratio = (float)_timer.GetTime() / (float)_timer.levelTime;

            if (ratio > 0.5f) {
                StartCoroutine(OpenStars(1f, 0));
                StartCoroutine(OpenStars(1.5f, 1));
                StartCoroutine(OpenStars(2f, 2));
            } else if (ratio > 0.25f) {
                StartCoroutine(OpenStars(1f, 0));
                StartCoroutine(OpenStars(1.5f, 1));
            } else
                StartCoroutine(OpenStars(1f, 0));

        } 
    }

    private IEnumerator OpenStars(float sec, int starId) {
        yield return new WaitForSeconds(sec);
        stars[starId].ActivateStar();
    }
}
