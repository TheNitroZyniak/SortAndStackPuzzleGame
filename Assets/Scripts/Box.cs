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


    private void OnMouseDown() {
        //if (!_mainGameManager.IsTouchBoxBlocked()) {
            Select();
        //}
    }


    public void Select() {
        _bottomCells.RemoveFromCells(this);
    }

    public void Add(SelectableObject newObject, float factor) {
        list.Add(newObject);

               

        Vector3 posToPlace = transform.position;
        //newObject.transform.DOScale(newObject.transform.localScale / 2f, 0.25f);

        Vector3 startPos = newObject.transform.position;
        Vector3 endPos = flyPoint.position;

        Vector3 midPoint = (startPos + endPos) * 0.5f;
        //Vector3 point3 = (2 * (startPos + endPos) / 3f);

        //point2 = point2 + Vector3.back * 5;
        //point3 = point3 + Vector3.back * 5;
        Vector3 arcHeight = midPoint + Vector3.back * 8;

        Vector3 startRot = newObject.transform.rotation.eulerAngles;
        startRot = new Vector3(startRot.x - 360, startRot.y, startRot.z);

        newObject.transform.DORotate(startRot, 0.6f, RotateMode.FastBeyond360).SetLoops(2, LoopType.Restart);
        
        newObject.transform.DOScale(newObject.transform.localScale * 0.75f, 1.2f);

        newObject.transform.DOPath(new Vector3[] { startPos, arcHeight, flyPoint.position }, 1.2f, PathType.CatmullRom).
            SetEase(Ease.Linear).
            OnComplete(() => {
            newObject.transform.SetParent(transform);
            newObject.ToBox();
            newObject.StopRotate();
            //newObject.transform.DOMove(posToPlace, 0.25f).OnComplete(() => {
            //    newObject.transform.SetParent(transform);
            //});
        });

        //newObject.transform.DOMove(flyPoint.position, 0.25f).OnComplete(() => {
        //    newObject.transform.SetParent(transform);
        //    newObject.ToBox();
        //    //newObject.transform.DOMove(posToPlace, 0.25f).OnComplete(() => {
        //    //    newObject.transform.SetParent(transform);
        //    //});
        //});
        //Vector3 endRotation = newObject.endRotation;

        //if (!newObject.rotateZ)
        //    endRotation = new Vector3(endRotation.x + 90, endRotation.y, endRotation.z);
        //else
        //    endRotation = new Vector3(endRotation.x, endRotation.y, endRotation.z + 90);

        //newObject.transform.DORotate(endRotation, 0.5f).OnComplete(() => {
        //    newObject.Block();
        //});

        //Vector3 currentScale = newObject.transform.localScale;
        //newObject.transform.DOScale(currentScale / factor, 0.5f);
    }

    public void MakeAllKinematic(bool doKin) {
        foreach (SelectableObject newObject in list) {
            newObject.MakeKinematic(doKin);
        }
    }
}
