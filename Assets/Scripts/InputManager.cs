using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InputManager : MonoBehaviour
{
    [Inject] MainGameManager _mainGameManager;
    [Inject] AudioManager _audioManager;
    private Camera mainCamera;

    void Start() {
        mainCamera = Camera.main;
    }

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
                    _audioManager.PlaySound(1);
                    ourHit.collider.gameObject.GetComponent<SelectableObject>().Select(false);
                    _mainGameManager.BlockTouch();
                }
            }
        }
    }
}
