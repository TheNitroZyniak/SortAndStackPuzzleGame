using UnityEngine;
using Zenject;
using DG.Tweening;
using UnityEngine.EventSystems;

public class SelectableObject : MonoBehaviour {
    [Inject] MainGameManager _mainGameManager;
    [Inject] BottomCells _bottomCells;

    public ObjectType objectType;

    public int id;
    
    [SerializeField] Rigidbody _rb;
    private bool isSelected;

    public Vector3 cellScale;
    
    public Vector3 endRotation;
    public bool rotateZ;
    public int currentCell;

    Vector3 startScale;
    public float yOffset;

    private void Start() {
        startScale = transform.localScale;
    }

    private void OnMouseDown() {
        if (!_mainGameManager.IsTouchBlocked()) {
            if (!isSelected) {
                Select(false);
                _mainGameManager.BlockTouch();
            }
        }
    }

    public void Select(bool magnet) {
        //if (EventSystem.current.currentSelectedGameObject.layer == 5) {
        //    print(EventSystem.current.currentSelectedGameObject.name);
        //    return;
        //}
        isSelected = true;
        //AudioManager.Instance.PlaySound(0, 1);
        //UIManager.Instance.DeselectTutor();
        Disappear(magnet);
    }

    public void Disappear(bool magnet) {
        //GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        _rb.useGravity = false;
        Vector3 inputPos = Camera.main.WorldToScreenPoint(transform.position);
        //Vector3 pos = _uiManager.GetCurrentCellPos();


        //_uiManager.UpdateBefore(ballImage, id);
        //_uiManager.UpdateSelectedBallsDisplay(inputPos, ballImage, id);

        _bottomCells.UpdateBefore(this, id);

        Vector3 pos = _bottomCells.GetCurrentCell();
        pos = new Vector3(pos.x, pos.y + yOffset, pos.z);


        //Vector3 pos1 = worldToUISpace(FindObjectOfType<Canvas>(), pos);

        //print(pos);

        if (!magnet) {
            transform.DOMove(pos, 25).SetSpeedBased(true).OnComplete(() => {
                _mainGameManager.RemoveFromList(this);
                _bottomCells.UpdateSelectedBallsDisplay(this, id);
                
                _mainGameManager.UnblockTouch();
                Block();
            });
        } 
        else {
            transform.DOMove(pos, 0.2f).OnComplete(() => {
                _mainGameManager.RemoveFromList(this);
                _bottomCells.UpdateSelectedBallsDisplay(this, id);
                
                _mainGameManager.UnblockTouch();
                Block();
            });
        }

        transform.DOScale(cellScale, 0.2f);
        transform.DORotate(endRotation, 0.2f);

        //GameController.Instance.RemoveFromList(this);
        //StartCoroutine(MoveToUI(uiImageObj.GetComponent<RectTransform>()));


    }


    public void ToCentre() {
        transform.DOMove(new Vector3(0, 0, -2), 25).SetSpeedBased(true).OnComplete(() => {
            GetComponent<Collider>().enabled = true;
            _rb.useGravity = true;
            _rb.isKinematic = false;
        }); 
        transform.DOScale(startScale, 0.2f);
        isSelected = false;
    }

    public void Block() {
        _rb.isKinematic = true;
    }

}
