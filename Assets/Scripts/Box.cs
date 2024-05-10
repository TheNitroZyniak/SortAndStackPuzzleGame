using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class Box : MonoBehaviour{
    [Inject] MainGameManager _mainGameManager;
    [Inject] BottomCells _bottomCells;

    public List<SelectableObject> list = new List<SelectableObject>();

    public string type;

    private bool isSelected;

    public TextMeshPro binText;
    public Transform flyPoint;

    public int boxObjectId;

    public Vector3 endPosition;

    public int amountOfObjects;

    public bool boxIsMoving;

    public void Add(SelectableObject newObject, float factor) {

        list.Add(newObject);

        Vector3 startPos = newObject.transform.position;

        //Vector3 endPos = endPosition;
        Vector3 endPos = transform.position;


        Vector3 midPoint = (startPos + endPos) * 0.5f;
        Vector3 arcHeight = midPoint + Vector3.back * 8;
        Vector3 startRot = newObject.transform.rotation.eulerAngles;

        startRot = new Vector3(startRot.x - 360, startRot.y, startRot.z);

        newObject.transform.DORotate(startRot, 0.4f, RotateMode.FastBeyond360).SetLoops(2, LoopType.Restart);
        
        newObject.transform.DOScale(newObject.transform.localScale * 0.75f, 0.8f);

        //newObject.transform.DOPath(new Vector3[] { startPos, arcHeight, endPosition }, 0.8f, PathType.CatmullRom).
        newObject.transform.DOPath(new Vector3[] { startPos, arcHeight, transform.position }, 0.8f, PathType.CatmullRom).
            SetEase(Ease.Linear).
            OnComplete(() => {
                newObject.transform.SetParent(transform);
                newObject.ToBox();
                newObject.StopRotate();

                

        });
    }

    public void MakeAllKinematic(bool doKin) {
        foreach (SelectableObject newObject in list) {
            newObject.MakeKinematic(doKin);
        }
    }

    public bool isMovingForward;

    public IEnumerator MoveBoxForward() {
        transform.parent = null;
        yield return new WaitForSeconds(1);
        MakeAllKinematic(true);
        transform.DOMoveY(50, 10);
        isMovingForward = true;
    }
}
