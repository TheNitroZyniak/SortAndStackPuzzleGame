using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Zenject;
using ModestTree;
//using Firebase.Analytics;

public class MainGameManager : MonoBehaviour {
    [Inject] UIManager _uiManager;
    [Inject] Timer _timer;
    [Inject] BottomCells _cells;
    [Inject] Tutorial _tutor;
    //[Inject] StackManager _stackManager;
    [Inject] BoxesManager _boxesManager;

    public List<SelectableObject> allObjects = new List<SelectableObject>();
    private bool touchBlocked, touchBoxBlocked;

    public List<SelectableObject> moveHistory = new List<SelectableObject>();
    private List<SelectableObject> sortedObjects = new List<SelectableObject>();
    int amountOfUndo = 3;

    int currentLevel;

    [SerializeField] Transform[] cameras;

    private void Start() {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        float mainRatio = 1920f / 1080f;
        float newRatio = (float) Screen.height / (float) Screen.width;

        float oneRatio = 240f / 1080f;

        float difference = newRatio - mainRatio;
        float camOffset = (float)difference / (float)oneRatio;
        
        for(int i = 0; i < cameras.Length; i++) {
            cameras[i].transform.position = new Vector3(cameras[i].transform.position.x, 
                cameras[i].transform.position.y + camOffset, 
                cameras[i].transform.position.z);
        }

    }

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
                _uiManager.OpenVictoryPopup();
                //StartCoroutine(_boxesManager.MoveFromStacksToCentre());
                _timer.StopTimer();
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

        //LevelFailed level = new LevelFailed();
        //level.level = currentLevel;
        //string json = JsonUtility.ToJson(level);
        //AppMetrica.Instance.ReportEvent("Level_Failed", json);

        //FirebaseAnalytics.LogEvent("Level_Failed", new Parameter("Level", currentLevel));

        _timer.StopTimer();

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


    public void UnblockTouch() {

        //_cells.Check3After(thisObject);

        touchBlocked = false;
    }


    public bool IsTouchBoxBlocked() {
        return touchBoxBlocked;
    }

    public void UnblockBoxTouch() {
        touchBoxBlocked = false;
    }

    bool magnetBlocked;

    public void UseMagnet() {

        if (_cells.GetCurrentCellId() > 3 || magnetBlocked) return;

        int r = allObjects[Random.Range(0, allObjects.Count)].id;
        int c = 0;
        magnetBlocked = true;

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
        BlockTouch();
        foreach (SelectableObject obj in takenObjects) {
            obj.Select(true);
            yield return new WaitForSeconds(0.25f);

        }
        UnblockTouch();
        magnetBlocked = false;
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
            _cells.UpdateAfterUndo(objectToPlace.objectType);
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

