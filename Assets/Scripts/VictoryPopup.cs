using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Zenject;

public class VictoryPopup : MonoBehaviour{
    [Inject] Timer _timer;

    [SerializeField] Star[] stars;
    [SerializeField] TextMeshProUGUI time;

    private void OnEnable() {
        time.text = _timer.GetTime().ToString() + " s";

        StartCoroutine(OpenStars(0.5f, 0));
        StartCoroutine(OpenStars(1f, 1));
        StartCoroutine(OpenStars(1.5f, 2));
    }


    private IEnumerator OpenStars(float sec, int starId) {
        yield return new WaitForSeconds(sec);
        stars[starId].ActivateStar();
    }
}
