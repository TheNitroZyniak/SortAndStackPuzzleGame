using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Stack : MonoBehaviour{
    public List<SelectableObject> list = new List<SelectableObject>();

    [SerializeField] Transform bottomPoint;

    public string type;
    private float _yPos;

    

    private void Start() {
        _yPos = bottomPoint.transform.position.y;
        //_yPos += GetComponent<MeshRenderer>().bounds.size.y / 2f;
    }

    public void ResetPos() {
        _yPos = transform.position.y;
        _yPos += GetComponent<MeshRenderer>().bounds.size.y / 2f;
    }

    public void Add(SelectableObject newObject, float factor) {
        list.Add(newObject);
        _yPos += (newObject.GetComponent<MeshRenderer>().bounds.size.z / factor) / 2f;

        Vector3 posToPlace = new Vector3(transform.position.x, _yPos, transform.position.z + newObject.yOffset);
        newObject.transform.DOMove(posToPlace, 0.5f);
        Vector3 endRotation = newObject.endRotation;

        if(!newObject.rotateZ)
            endRotation = new Vector3(endRotation.x + 90, endRotation.y, endRotation.z);
        else
            endRotation = new Vector3(endRotation.x, endRotation.y, endRotation.z + 90);

        newObject.transform.DORotate(endRotation, 0.5f).OnComplete(() => {
            newObject.Block();
        });

        Vector3 currentScale = newObject.transform.localScale;
        newObject.transform.DOScale(currentScale/ factor, 0.5f);

        _yPos += (newObject.GetComponent<MeshRenderer>().bounds.size.z / factor) / 2f;


    }

}
