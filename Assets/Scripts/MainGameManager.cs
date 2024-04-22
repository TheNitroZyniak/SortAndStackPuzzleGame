using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Zenject;

public class MainGameManager : MonoBehaviour {
    [Inject] UIManager _uiManager;
    [Inject] Timer _timer;
    [Inject] BottomCells _cells;

    private List<SelectableObject> allObjects = new List<SelectableObject>();
    private bool touchBlocked;

    private List<SelectableObject> moveHistory = new List<SelectableObject>();
    int amountOfUndo = 3;

    private bool CheckAllObjects() {
        if(allObjects.Count > 0) return true;
        return false;
    }

    public void AddToList(SelectableObject newObject) => allObjects.Add(newObject);
    public void RemoveFromList(SelectableObject objectToRemove) {
        if (allObjects.Any(obj => obj == objectToRemove)) {
            
            SaveMove(objectToRemove);

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


    public void UseMagnet() {
        int r = allObjects[Random.Range(0, allObjects.Count)].id;
        int c = 0;
        List<SelectableObject> takenObjects = new List<SelectableObject>();
        for(int i = 0; i < allObjects.Count; i++) {
            if(allObjects[i].id == r) {
                c++;
                takenObjects.Add(allObjects[i]);
                if (c == 3) break;
            }
        }

        StartCoroutine(SelectAll(takenObjects));

    }

    IEnumerator SelectAll(List<SelectableObject> takenObjects) {
        foreach (SelectableObject obj in takenObjects) {
            obj.Select(true);
            yield return new WaitForSeconds(0.25f);

        }
    }

    public void OnUndoButtonPress() => UndoLastMove();
    public void SaveMove(SelectableObject obj) => moveHistory.Add(obj);
    

    //public void MoveHistoryCount() => print(moveHistory.Count());

    public void RemoveFromHistory(SelectableObject obj) {
        moveHistory.Remove(obj);
    }

    public void UndoLastMove() {
        if (moveHistory.Count > 0 & amountOfUndo > 0) {

            SelectableObject objectToPlace = moveHistory[moveHistory.Count - 1];

            _cells.ChangeCells(objectToPlace.currentCell);
            objectToPlace.ToCentre();
            allObjects.Add(objectToPlace);
            moveHistory.Remove(objectToPlace);
        }
    }

}

