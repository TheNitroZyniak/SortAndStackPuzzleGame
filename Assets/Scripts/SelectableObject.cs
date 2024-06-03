using UnityEngine;
using Zenject;
using DG.Tweening;
using UnityEngine.EventSystems;

public class SelectableObject : MonoBehaviour {
    [Inject] MainGameManager _mainGameManager;
    [Inject] BottomCells _bottomCells;
    [Inject] BoxesManager _boxesManager;
    [Inject] AudioManager _audioManager;

    public Tweener moveTweener, scaleTweener;

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

    public bool isBeingRemoved;
    public Vector3 twitchingPosition;

    public int removeList;

    public float[] endScales;
    public int endScaleID;

    private void Start() {
        _rb = GetComponent<Rigidbody>();
        _cd = GetComponent<Collider>();

        transform.localScale = transform.localScale * 2;
        //transform.localScale = cellScale;
        currentCell = -1;
        startScale = transform.localScale;
    }

    private void Update() {
        if(transform.parent != null) {
            if(transform.position.y < 0) {
                transform.position = transform.parent.position;
            }
        }
    }

    public void StopRotate() {
    }

    public void Select(bool magnet) {
        //isSelected = true;

        //AudioManager.Instance.PlaySound(0, 1);
        //UIManager.Instance.DeselectTutor();
        Disappear(magnet);
    }



    public void SelectToBox() {
        _cd.enabled = false;
        _rb.useGravity = false;
        _rb.isKinematic = true;
        _rb.angularVelocity = Vector3.zero;
        _rb.velocity = Vector3.zero;

        _boxesManager.AddToOneOfBoxes(this);

    }




    public void ResetObject() {
        transform.parent = null;
        isSelected = false;
        _rb.useGravity = true;
        _rb.isKinematic = false;
        _cd.enabled = true;
        transform.localScale = startScale;
        isBeingRemoved = false;
        currentCell = -1;
    }

    public void ToBox() {
        _rb.useGravity = true;
        _rb.isKinematic = false;
        _cd.enabled = true;
        _rb.velocity = Vector3.forward * 5;
        gameObject.layer = 8;
    }

    


    public void Disappear(bool magnet) {

        if (!_bottomCells.CheckIfAllow(this)) {
            return;
        }
        _audioManager.PlaySound(1);
        gameObject.layer = 9;
        TutorArrows.activateArrow = true;
        _cd.enabled = false;
        _rb.useGravity = false;
        _rb.isKinematic = true;
        _rb.angularVelocity = Vector3.zero;
        _rb.velocity = Vector3.zero;

        Vector3 pos = _bottomCells.UpdateBefore(this, id);
        _mainGameManager.ChangeTutor();

        pos = new Vector3(pos.x, pos.y + yOffset, pos.z);


        if (!magnet) {
            transform.localScale = cellScale;
            //transform.DOScale(cellScale, 0.15f);
            transform.DORotate(endRotation, 0.15f);

            transform.DOMove(pos, 35).SetSpeedBased(true).OnComplete(() => {
                _mainGameManager.RemoveFromList(this, true);
                _bottomCells.UpdateSelectedBallsDisplay(this, id);
                Block();
            });
        } 
        else {
            transform.DOScale(cellScale, 0.15f);
            transform.DORotate(endRotation, 0.15f);

            transform.DOMove(pos, 0.15f).OnComplete(() => {
                _mainGameManager.RemoveFromList(this, true);
                _bottomCells.UpdateSelectedBallsDisplay(this, id);
                Block();
            });
        }

        
    }

    public void ToCentre() {
        gameObject.layer = 0;
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


    public void ActivateForSelectionToBox(bool activate) {
        _cd.enabled = activate;
    }

}
