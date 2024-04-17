using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Zenject;

public class SelectableObject : MonoBehaviour {
    [Inject] MainGameManager _mainGameManager;

    [SerializeField] int id;
    [SerializeField] Sprite ballImage;

    private bool isSelected;
    public float rotationSpeed = -20.0f;

    public GameObject ballUIImagePrefab;
    public RectTransform uiTarget;

    SphereCollider col;


    private void Start() {
        col = GetComponent<SphereCollider>();
    }

    void Update() {
        //transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }


    private void OnMouseDown() {
        if (!isSelected) Select();
    }

    public void Select() {
        isSelected = true;
        //AudioManager.Instance.PlaySound(0, 1);
        //UIManager.Instance.DeselectTutor();
        _mainGameManager.RemoveFromList(this);

        Disappear();
    }

    public void Disappear() {
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        Vector3 inputPos = Camera.main.WorldToScreenPoint(transform.position);

        //GameObject uiImageObj = Instantiate(ballUIImagePrefab, new Vector3(inputPos.x - Screen.width / 2f, inputPos.y - Screen.height / 2f, 0), Quaternion.identity);
        //uiImageObj.transform.SetParent(GameObject.Find("Canvas").transform, false); // Установите Canvas как родителя UI-изображения
        //uiImageObj.GetComponent<Image>().sprite = ballImage;



        ////UIManager.Instance.UpdateBefore(ballImage, id);




        //Vector3 targetPosition = UIManager.Instance.GetCurrentImagePos();
        //uiImageObj.GetComponent<RectTransform>().position = targetPosition;

        ////UIManager.Instance.UpdateSelectedBallsDisplay(ballImage, id);
        ////GameController.Instance.RemoveFromList(this);
        //StartCoroutine(MoveToUI(uiImageObj.GetComponent<RectTransform>()));
    }

    IEnumerator MoveToUI(RectTransform uiElement) {
        float duration = 0.25f; // Продолжительность анимации
        float elapsedTime = 0;

        Vector3 startPosition = uiElement.transform.position;
        //UIManager.Instance.UpdateBefore(ballImage, id);
        //Vector3 targetPosition = UIManager.Instance.GetCurrentImagePos();

        while (elapsedTime < duration) {
            //uiElement.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //uiElement.position = targetPosition;
        Destroy(uiElement.gameObject);
        //UIManager.Instance.UpdateSelectedBallsDisplay(ballImage, id);
        //GameController.Instance.RemoveFromList(this);
    }

    public void DisableCol() {
        col.enabled = false;
    }
}
