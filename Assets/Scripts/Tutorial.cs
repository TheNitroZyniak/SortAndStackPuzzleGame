using UnityEngine;
using DG.Tweening;
using System.Net;

public class Tutorial : MonoBehaviour{

    [SerializeField] Transform hand;
    Vector3 startPoint, endPoint;



    private void Start() {
        if(PlayerPrefs.GetInt("TutorShowed") == 1) gameObject.SetActive(false);

        endPoint = new Vector3(hand.transform.position.x, hand.transform.position.y + 100, hand.transform.position.z);

        hand.DOMove(endPoint, 1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }
    int counter;

    public void SetNextPoint() {
        counter++;
        DOTween.KillAll();
        hand.transform.position = new Vector3(hand.transform.position.x + 220, hand.transform.position.y, hand.transform.position.z);
        endPoint = new Vector3(hand.transform.position.x, hand.transform.position.y + 100, hand.transform.position.z);
        hand.DOMove(endPoint, 1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    
        if(counter == 3) {
            PlayerPrefs.SetInt("TutorShowed", 1);
            gameObject.SetActive(false);
        }
    }



}
