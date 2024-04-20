using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Zenject;
using DG.Tweening;
using UnityEditor.Rendering;


public class SelectableObject : MonoBehaviour {
    [Inject] MainGameManager _mainGameManager;
    [Inject] UIManager _uiManager;
    [Inject] BottomCells _bottomCells;

    public ObjectType objectType;

    public int id;
    [SerializeField] Sprite ballImage;
    [SerializeField] Rigidbody _rb;
    private bool isSelected;

    public GameObject ballUIImagePrefab;
    public RectTransform uiTarget;

    SphereCollider col;
    public Vector3 endRotation;


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
        //GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        _rb.useGravity = false;
        Vector3 inputPos = Camera.main.WorldToScreenPoint(transform.position);
        //Vector3 pos = _uiManager.GetCurrentCellPos();


        //_uiManager.UpdateBefore(ballImage, id);
        //_uiManager.UpdateSelectedBallsDisplay(inputPos, ballImage, id);

        _bottomCells.UpdateBefore(this, id);

        Vector3 pos = _bottomCells.GetCurrentCell();

        

        //Vector3 pos1 = worldToUISpace(FindObjectOfType<Canvas>(), pos);

        //print(pos);

        transform.DOMove(pos, 20).SetSpeedBased(true).OnComplete(() => {
            _bottomCells.UpdateSelectedBallsDisplay(this, id);
            _mainGameManager.RemoveFromList(this);
            Block();
        });
        transform.DORotate(endRotation, 0.25f);

        //GameController.Instance.RemoveFromList(this);
        //StartCoroutine(MoveToUI(uiImageObj.GetComponent<RectTransform>()));


    }
    public void Block() {
        _rb.isKinematic = true;
    }

}
