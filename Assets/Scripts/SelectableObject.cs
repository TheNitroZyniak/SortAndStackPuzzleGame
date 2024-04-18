using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Zenject;


public class SelectableObject : MonoBehaviour {
    [Inject] MainGameManager _mainGameManager;
    [Inject] UIManager _uiManager;
    [SerializeField] int id;
    [SerializeField] Sprite ballImage;
    [SerializeField] Rigidbody _rb;
    private bool isSelected;

    public GameObject ballUIImagePrefab;
    public RectTransform uiTarget;

    SphereCollider col;


    private void Start() {
        col = GetComponent<SphereCollider>();
        //_rb.velocity = new Vector3(Random.Range(-8, 8), -25, 0);
    }


    private void OnMouseDown() {
        if (!isSelected) Select();
    }

    public void Select() {
        isSelected = true;
        //AudioManager.Instance.PlaySound(0, 1);
        //UIManager.Instance.DeselectTutor();
        Disappear();
    }

    public void Disappear() {
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        Vector3 inputPos = Camera.main.WorldToScreenPoint(transform.position);

        _uiManager.UpdateBefore(ballImage, id);
        _uiManager.UpdateSelectedBallsDisplay(inputPos, ballImage, id);
        _mainGameManager.RemoveFromList(this);
        //GameController.Instance.RemoveFromList(this);
        //StartCoroutine(MoveToUI(uiImageObj.GetComponent<RectTransform>()));


    }

    public void DisableCol() {
        col.enabled = false;
    }
}
