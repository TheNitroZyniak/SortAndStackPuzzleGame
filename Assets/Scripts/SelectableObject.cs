using UnityEngine;
using Zenject;
using DG.Tweening;
using UnityEngine.EventSystems;

public class SelectableObject : MonoBehaviour {
    [Inject] MainGameManager _mainGameManager;
    [Inject] BottomCells _bottomCells;

    public string objectType;

    public int id;
    
    Rigidbody _rb;
    Collider _cd;
    private bool isSelected;

    public Vector3 cellScale;
    
    public Vector3 endRotation;
    public bool rotateZ;
    public int currentCell;

    public Vector3 startScale;
    public float yOffset;
    public bool isTwitching;

    public Vector3 twitchingPosition;

    private void Start() {
        _rb = GetComponent<Rigidbody>();
        _cd = GetComponent<Collider>();
        transform.localScale = transform.localScale * 2;

        startScale = transform.localScale;
    }

    bool allowRotate;

    private void Update() {
        Twitch();

        if (allowRotate) {
            //transform.RotateAround(new Vector3(50, 0, 0));
        }
    }

    public void StarRotate() {
        allowRotate = true;
    }

    public void StopRotate() {
        allowRotate = false;
    }

    public void Twitch() {
        if (isTwitching) {
            transform.position = new Vector3(twitchingPosition.x + Random.Range(-0.1f, 0.1f), 
                twitchingPosition.y + Random.Range(-0.1f, 0.1f), transform.position.z);
        }
    }

    public void UnblockTwitching() {
        twitchingPosition = transform.position;
        isTwitching = true; 
    }
    public void BlockTwitching() {
        transform.position = twitchingPosition;
        isTwitching = false;
    }
    private void OnMouseDown() {
        //if (!_mainGameManager.IsTouchBlocked()) {
            if (!isSelected) {
                Select(false);
                //_mainGameManager.BlockTouch();
            }
        //}
    }

    public void Select(bool magnet) {
        isSelected = true;
        //AudioManager.Instance.PlaySound(0, 1);
        //UIManager.Instance.DeselectTutor();
        Disappear(magnet);
    }

    public void ResetObject() {
        isSelected = false;
        _rb.useGravity = true;
        _rb.isKinematic = false;
        _cd.enabled = true;
        transform.localScale = startScale;
    }

    public void ToBox() {
        _rb.useGravity = true;
        _rb.isKinematic = false;
        _cd.enabled = true;
        _rb.velocity = Vector3.forward * 5;
    }

    public void Disappear(bool magnet) {
        _cd.enabled = false;
        _rb.useGravity = false;
        _rb.isKinematic = true;
        _rb.angularVelocity = Vector3.zero;
        _rb.velocity = Vector3.zero;
        Vector3 inputPos = Camera.main.WorldToScreenPoint(transform.position);

        _bottomCells.UpdateBefore(this, id);
        _mainGameManager.ChangeTutor();


        Vector3 pos = _bottomCells.GetCurrentCell();
        pos = new Vector3(pos.x, pos.y + yOffset, pos.z);

        if (!magnet) {
            transform.DOMove(pos, 35).SetSpeedBased(true).OnComplete(() => {
                _mainGameManager.RemoveFromList(this);
                _bottomCells.UpdateSelectedBallsDisplay(this, id);
                
                _mainGameManager.UnblockTouch(this);
                Block();
            });
        } 
        else {
            transform.DOMove(pos, 0.15f).OnComplete(() => {
                _mainGameManager.RemoveFromList(this);
                _bottomCells.UpdateSelectedBallsDisplay(this, id);
                
                _mainGameManager.UnblockTouch(this);
                Block();
            });
        }

        transform.DOScale(cellScale, 0.15f);
        transform.DORotate(endRotation, 0.15f);
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

    public void MakeKinematic(bool doKin) {
        _rb.isKinematic = doKin;
    }

}
