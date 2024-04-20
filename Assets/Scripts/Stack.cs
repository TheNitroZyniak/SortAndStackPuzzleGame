using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Stack : MonoBehaviour{
    List<SelectableObject> list = new List<SelectableObject>();
    public ObjectType type;
    private float _yPos;

    

    private void Start() {
        _yPos = transform.position.y;
    }

    public void Add(SelectableObject newObject, float factor) {
        list.Add(newObject);
        _yPos += 0.2f / factor;

        Vector3 posToPlace = new Vector3(transform.position.x, _yPos, transform.position.z);
        newObject.transform.DOMove(posToPlace, 0.5f);
        Vector3 endRotation = newObject.endRotation;
        endRotation = new Vector3(endRotation.x + 90, endRotation.y, endRotation.z);
        newObject.transform.DORotate(endRotation, 0.5f).OnComplete(() => {
            newObject.Block();
        });

        Vector3 currentScale = newObject.transform.localScale;
        newObject.transform.DOScale(currentScale/ factor, 0.5f);

        
    }

}
