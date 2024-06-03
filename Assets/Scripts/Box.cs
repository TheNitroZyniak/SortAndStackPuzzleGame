using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class Box : MonoBehaviour{
    [Inject] MainGameManager _mainGameManager;
    [Inject] BottomCells _bottomCells;

    [SerializeField] GameObject _boom;

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

        Vector3 endPos = endPosition;
        //Vector3 endPos = transform.position;


        //Vector3 midPoint = (startPos + endPos) * 0.5f;
        //Vector3 arcHeight = midPoint + Vector3.back * 8;
        //Vector3 startRot = newObject.transform.rotation.eulerAngles;

        //startRot = new Vector3(startRot.x - 360, startRot.y, startRot.z);

        //newObject.transform.DORotate(startRot, 0.4f, RotateMode.FastBeyond360).SetLoops(2, LoopType.Restart);
        //newObject.transform.DOPath(new Vector3[] { startPos, arcHeight, endPosition }, 0.8f, PathType.CatmullRom).
        ////newObject.transform.DOPath(new Vector3[] { startPos, arcHeight, transform.position }, 0.8f, PathType.CatmullRom).
        //    SetEase(Ease.Linear).
        //    OnComplete(() => {
        //        newObject.transform.SetParent(transform);
        //        newObject.ToBox();
        //        newObject.StopRotate();
        //        //newObject.transform.position = endPosition;
        //});

        Instantiate(_boom, newObject.transform.position, Quaternion.identity);

        //newObject.transform.position = new Vector3(Random.Range(endPosition.x - 0.5f, endPosition.x + 0.5f), Random.Range(endPosition.y - 0.5f, endPosition.y + 0.5f), endPosition.z - 4);

        Vector3 scl = newObject.transform.localScale / 1.25f;

        newObject.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => {
            Vector3 point = flyPoint.transform.position;
            newObject.transform.position = new Vector3(Random.Range(point.x - 0.75f, point.x + 0.75f), Random.Range(point.y - 0.75f, point.y + 0.75f), point.z);

            //newObject.transform.localScale = scl;
            Vector3 scl1 = new Vector3(newObject.endScales[newObject.endScaleID], newObject.endScales[newObject.endScaleID], newObject.endScales[newObject.endScaleID]);

            newObject.transform.localScale = scl1;


            newObject.ToBox();
            newObject.StopRotate();
            newObject.transform.SetParent(transform);
            Instantiate(_boom, newObject.transform.position, Quaternion.identity);
        });
        

        //newObject.transform.DOScale(Vector3.one / 2f, 0.2f).OnComplete(() => {
        //    newObject.transform.position = new Vector3(Random.Range(endPosition.x - 0.5f, endPosition.x + 0.5f), Random.Range(endPosition.y - 0.5f, endPosition.y + 0.5f), endPosition.z - 4);
        //    newObject.ToBox();
        //    newObject.StopRotate();

        //    //Instantiate(_boom, newObject.transform.position, Quaternion.identity);

        //    newObject.transform.DOScale(Vector3.one, 0.2f).OnComplete(() => {
        //        newObject.transform.SetParent(transform);
        //    }); 
            
        //});

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
