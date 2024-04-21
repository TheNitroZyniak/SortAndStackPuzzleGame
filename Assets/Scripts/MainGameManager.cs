using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Zenject;

public class MainGameManager : MonoBehaviour {
    [Inject] UIManager _uiManager;
    [Inject] Timer _timer;

    private List<SelectableObject> allObjects = new List<SelectableObject>();
    private bool touchBlocked;

    private bool CheckAllObjects() {
        if(allObjects.Count > 0) return true;
        return false;
    }

    public void AddToList(SelectableObject newObject) => allObjects.Add(newObject);
    public void RemoveFromList(SelectableObject objectToRemove) {
        if (allObjects.Any(obj => obj == objectToRemove)) {
            allObjects.Remove(objectToRemove);
            if (!CheckAllObjects()) {
                _uiManager.OpenVictoryPopup();
                _timer.StopTimer();
            }
        }
    }


    public void GameLost() {
        _uiManager.OpenLosePopup();
    }

    public void BlockTouch() {
        touchBlocked = true;
    }

    public bool IsTouchBlocked() { 
        return touchBlocked; 
    }

    public void UnblockTouch() {
        touchBlocked = false;
    }
}
