using UnityEngine;
using Zenject;
using DG.Tweening;



public class SelectableObject : MonoBehaviour {
    [Inject] MainGameManager _mainGameManager;
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
        if (!_mainGameManager.IsTouchBlocked()) {
            if (!isSelected) {
                Select();
                _mainGameManager.BlockTouch();
            }
        }
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

        transform.DOMove(pos, 25).SetSpeedBased(true).OnComplete(() => {
            _bottomCells.UpdateSelectedBallsDisplay(this, id);
            _mainGameManager.RemoveFromList(this);

            _mainGameManager.UnblockTouch();

            Block();
        });
        transform.DORotate(endRotation, 0.2f);

        //GameController.Instance.RemoveFromList(this);
        //StartCoroutine(MoveToUI(uiImageObj.GetComponent<RectTransform>()));


    }
    public void Block() {
        _rb.isKinematic = true;
    }

}
