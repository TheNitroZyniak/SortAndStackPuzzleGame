using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Star : MonoBehaviour{

    [SerializeField] private Image goldStar;

    private void OnEnable() {
        goldStar.transform.localScale = Vector3.zero;
    }

    public void ActivateStar() {
        goldStar.gameObject.SetActive(true);

        goldStar.transform.DOScale(Vector3.one, 0.5f);

    }

}
