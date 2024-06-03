using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditor.Rendering;
using UnityEngine;
using Zenject;

public class InputManager : MonoBehaviour{
    [Inject] MainGameManager _mainGameManager;
    [SerializeField] Camera mainCamera;
    SelectableObject selObject;

    

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray.origin, ray.direction, Mathf.Infinity);

            float minDist = float.MaxValue;
            RaycastHit ourHit = new RaycastHit();

            bool doIt = false;

            foreach (RaycastHit hit in hits) {
                if (hit.collider.gameObject.CompareTag("SelectableObject")) {
                    if(hit.distance < minDist) {
                        minDist = hit.distance;
                        ourHit = hit;
                        doIt = true;
                    }
                }
            }

            if (doIt) {
                if (!_mainGameManager.IsTouchBlocked()) {
                    selObject = ourHit.collider.gameObject.GetComponent<SelectableObject>();
                    Vector3 pos = selObject.transform.position;
                    selObject.moveTweener = selObject.transform.DOMove(new Vector3(pos.x, pos.y - 1.5f, pos.z - 3), 0.2f).OnComplete(() => {
                        selObject.MakeKinematic(true);
                    });
                    Vector3 scl = selObject.transform.localScale;
                    selObject.scaleTweener = selObject.transform.DOScale(scl * 1.2f, 0.2f);

                    //ourHit.collider.gameObject.GetComponent<SelectableObject>().Select(false);
                } 
            } 
            else {
                if (!_mainGameManager.IsTouchBlocked()) {
                    foreach (RaycastHit hit in hits) {
                        if (hit.collider.gameObject.layer == 6) {
                            SelectableObject closestObject = FindClosestObjectWithinRange(_mainGameManager.allObjects, hit.point);
                            if (closestObject != null) {
                                selObject = closestObject;

                                Vector3 pos = selObject.transform.position;
                                selObject.moveTweener = selObject.transform.DOMove(new Vector3(pos.x, pos.y - 1.5f, pos.z - 3), 0.2f).OnComplete(() => {
                                    selObject.MakeKinematic(true);
                                });
                                Vector3 scl = selObject.transform.localScale;
                                selObject.scaleTweener = selObject.transform.DOScale(scl * 1.2f, 0.2f);
                                //closestObject.Select(false);
                            }
                        }
                    }
                }
            }

        }

        if (Input.GetMouseButton(0)) {
            if (selObject != null) {

            }
        }

        if (Input.GetMouseButtonUp(0)) {
            if(selObject !=  null) {
                if (selObject.moveTweener != null && selObject.moveTweener.IsPlaying()) selObject.moveTweener.Kill();
                if (selObject.scaleTweener != null && selObject.scaleTweener.IsPlaying()) selObject.scaleTweener.Kill();
                
                selObject.Select(false);
                selObject = null;
            }
        }
    }


    public SelectableObject FindClosestObjectWithinRange(List<SelectableObject> objects, Vector3 point) {
        SelectableObject closestObject = null;
        float closestDistance = 1.5f;

        foreach (SelectableObject obj in objects) {
            float distance = Vector3.Distance(obj.transform.position, point);
            if (distance < closestDistance) {
                closestObject = obj;
                closestDistance = distance;
            }
        }

        return closestObject;
    }
}
