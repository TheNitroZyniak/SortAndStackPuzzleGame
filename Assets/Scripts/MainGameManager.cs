using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Zenject;
using ModestTree;
using static TMPro.Examples.TMP_ExampleScript_01;

public class MainGameManager : MonoBehaviour {
    [Inject] UIManager _uiManager;
    [Inject] Timer _timer;
    [Inject] BottomCells _cells;
    [Inject] Tutorial _tutor;
    //[Inject] StackManager _stackManager;
    [Inject] BoxesManager _boxesManager;

    private List<SelectableObject> allObjects = new List<SelectableObject>();
    private bool touchBlocked, touchBoxBlocked;

    public List<SelectableObject> moveHistory = new List<SelectableObject>();
    private List<SelectableObject> sortedObjects = new List<SelectableObject>();
    int amountOfUndo = 3;

    int currentLevel;

    private bool CheckAllObjects() {
        if(allObjects.Count > 0) return true;
        return false;
    }

    public void AddToList(SelectableObject newObject) => allObjects.Add(newObject);
    public void RemoveFromList(SelectableObject objectToRemove, bool save) {
        if (allObjects.Any(obj => obj == objectToRemove)) {
            
            if(save)
                SaveMove(objectToRemove);

            allObjects.Remove(objectToRemove);
            if (!CheckAllObjects()) {
                //_uiManager.OpenVictoryPopup();
                //StartCoroutine(_boxesManager.MoveFromStacksToCentre());
                //_timer.StopTimer();
            }
        }
    }

    public void ChangeTutor() {
        if (PlayerPrefs.GetInt("CurrentLevel") == 0) {
            _tutor.SetNextPoint();
        }
    }


    public void DisableAll() {
        foreach (var item in allObjects) {
            item.ResetObject();
            item.gameObject.SetActive(false);
        }
        foreach (var item in moveHistory) {
            item.ResetObject();
            item.gameObject.SetActive(false);
        }
        foreach (var item in sortedObjects) {
            item.ResetObject();
            item.gameObject.SetActive(false);
        }
        allObjects.Clear();
        moveHistory.Clear();
        sortedObjects.Clear();
    }

    public void Mix() {
        foreach (var item in allObjects) {
            item.transform.SetPositionAndRotation(
                new Vector3(Random.Range(-3.5f, 3.5f), Random.Range(-3, 6), Random.Range(0, 3)),
                Quaternion.Euler(0, Random.Range(0, 360), 0));
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

    public void Unblock() {
        touchBlocked = false;
    }


    public void UnblockTouch(SelectableObject thisObject) {

        _cells.Check3After(thisObject);

        touchBlocked = false;
    }


    public bool IsTouchBoxBlocked() {
        return touchBoxBlocked;
    }

    public void UnblockBoxTouch() {
        touchBoxBlocked = false;
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
        sortedObjects.Add(obj);
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

    public void ActivateAllObjectsInCells() {
        foreach (SelectableObject obj in moveHistory) {
            obj.ActivateForSelectionToBox(true);
        }
    }

    public void DeactivateObjectsInCellsAmongSelected(List<string> objectTypes) {

        foreach(SelectableObject obj in moveHistory) {
            bool isMatchingType = objectTypes.Any(type => type == obj.objectType);
            if (!isMatchingType) {
                obj.ActivateForSelectionToBox(false);
            }
        }
    }

    public void CheckWin() {
        if(allObjects.IsEmpty() && moveHistory.IsEmpty()) {
            _uiManager.OpenVictoryPopup();
        }
    }

}

