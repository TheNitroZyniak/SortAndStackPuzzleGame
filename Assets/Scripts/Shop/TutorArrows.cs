using DG.Tweening;
using UnityEngine;

public class TutorArrows : MonoBehaviour{

    [SerializeField] GameObject[] Arrows;
    public static bool activateArrow;

    int tutorShowed;

    private void Start() {
   
        tutorShowed = PlayerPrefs.GetInt("Tut");

        if (tutorShowed == 1) gameObject.SetActive(false);
        else {
            ActivateArrow(0);
        }
    }


    int counter;

    public void ActivateArrow(int i) {
        Arrows[i].SetActive(true);
        Vector3 endPos = Arrows[i].transform.position;
        endPos = new Vector3(endPos.x, endPos.y, endPos.z + 1.5f);
        counter++;
        Arrows[i].transform.DOMove(endPos, 1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    public void Update() {
        if (activateArrow && tutorShowed == 0) {
            Arrows[counter-1].SetActive(false);
            if(counter < 3)
                ActivateArrow(counter);
            else
                counter++;

            activateArrow = false;
            if(counter == Arrows.Length + 1) {
                PlayerPrefs.SetInt("Tut", 1);
                tutorShowed = 1;
                foreach (var item in Arrows) item.SetActive(false);
                gameObject.SetActive(false);
            }
        }
    }
}
