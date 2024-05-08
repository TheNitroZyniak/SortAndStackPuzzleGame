using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BottomCells : MonoBehaviour{
    [Inject] BoxesManager _boxesManager;
    [Inject] MainGameManager _mainGameManager;
    [SerializeField] ObjectCell[] cells;
 
    private List<string> selectedObjectIds = new List<string>();

    private int selectedObjectCounter = -1;
    int temp;

    public void UpdateBefore(SelectableObject newObject, int id) {
        selectedObjectCounter++;
        int insertIndex = selectedObjectCounter;
        for (int i = 0; i < selectedObjectCounter; i++) {
            if (cells[i].ObjectString == newObject.objectType) {
                insertIndex = i + 1;
            }
        }
        print(selectedObjectCounter + "    " + insertIndex);
        for (int i = selectedObjectCounter - 1; i >= insertIndex; i--) {
            cells[i + 1].SetCell(cells[i].currentObject, 0, true);
        }

        cells[insertIndex].ClearCell(false);
        temp = selectedObjectCounter;
        selectedObjectCounter = insertIndex;

        
    }

    [SerializeField] MeshRenderer[] indikators;
    private void Update() {
        for (int i = 0; i < cells.Length; i++) {
            if (cells[i].IsEmpty())
                indikators[i].material.color = Color.green;
            else
                indikators[i].material.color = Color.red;
        }
        if (selectedObjectCounter + 1 < 7) {
            indikators[selectedObjectCounter + 1].material.color = Color.yellow;
        }



        foreach (SelectableObject obj in _mainGameManager.moveHistory) {
            Check3After(obj);
        }
    }


    public Vector3 GetCurrentCell() {
        return cells[selectedObjectCounter].transform.position;
    }

    public void UpdateSelectedBallsDisplay(SelectableObject newObject, int id) {
        cells[selectedObjectCounter].SetCell(newObject, id, false);
        selectedObjectCounter = temp;
        //selectedObjectCounter++;
        CheckFor3(newObject.objectType);
    }

    private void CheckFor3(string id) {
        int count = 0;

        string objType = String.Empty;

        for (int i = 0; i < selectedObjectCounter + 1; i++) {
            if (cells[i].ObjectString == id) {
                count++;
                objType = cells[i].currentObject.objectType;
            }
        }

        if (count == 3) {
            foreach (Box box in _boxesManager.boxesList) {
                if(box.type == objType)
                    RemoveAndShiftCells(id, box);
            }
        } 
        else if (selectedObjectCounter == cells.Length) {
            _mainGameManager.GameLost();
        }
    }


    public void Check3After(SelectableObject thisObject) {
        int c = thisObject.currentCell;

        bool doMove = false;

        for(int i = c - 1; i >= 0; i--) {
            if (cells[c].IsEmpty()) {
                doMove = true;
            }
        }

        if (doMove) {
            for (int i = c - 1; i > 0; i--) {
                if (!cells[c].IsEmpty()) {
                    cells[c].SetCell(thisObject, 0, true);
                }
            }
        }


    }


    private void MakeTwitch(string id) {
        for (int i = 0; i < selectedObjectCounter; i++) {
            if (cells[i].ObjectString == id) {
                cells[i].currentObject.UnblockTwitching();
            }
        }
    }

    public void RemoveFromCells(Box boxToPlace) {
        for (int i = 0; i < selectedObjectIds.Count; i++) {
            if (boxToPlace.type == selectedObjectIds[i]) {
                RemoveAndShiftCells(selectedObjectIds[i], boxToPlace);
                selectedObjectIds.RemoveAt(i);
            }
        }
    }

    public void RemoveAndShiftCells(string objType, Box boxToPlace) {

        for (int i = 0; i < selectedObjectCounter + 1; i++) {
            if (cells[i].currentObject != null && cells[i].currentObject.objectType == objType) {
                _mainGameManager.RemoveFromHistory(cells[i].currentObject);
            }
        }

        StartCoroutine(Remove3(objType, boxToPlace));
        
    }

    IEnumerator Remove3(string objType, Box boxToPlace) {

        int first = -1;
        int removeCount = 0;
        _boxesManager.MoveBoxes(boxToPlace);

        int c = selectedObjectCounter + 1;

        yield return new WaitForSeconds(0.5f);

        int counter = 0;

        for (int i = 0; i < c; i++) {
            if (cells[i].currentObject != null) {
                if (cells[i].currentObject.objectType == objType && counter < 3) {
                    if (first < 0) first = i;
                    _boxesManager.AddToBoxes(cells[i].currentObject, boxToPlace);
                    cells[i].ClearCell(true);
                    counter++;
                    yield return new WaitForSeconds(0.25f);

                    removeCount++;
                }
            }
        } 

        // Тут проблема!!!

        int index = first;
        bool removeLast = index == 0 ? true : false;


        for (int i = 0; i < cells.Length - 3; i++) {
            if (cells[i].IsEmpty()) {
                if (!cells[i + 3].IsEmpty()) {
                    //cells[i].SetCell(cells[i + 3].currentObject, cells[i + 3].ObjectId, true);
                    cells[i].SetCell(cells[i + 3].currentObject, 0, true);
                    index = i + 1;
                }
            }
        }

        selectedObjectCounter = index;

        if (removeLast) {
            if (!cells[6].IsEmpty()) {
                cells[3].SetCell(cells[6].currentObject, 0, true);
                selectedObjectCounter++;
            }
        }

        for (int i = index; i < cells.Length; i++) cells[i].ClearCell(false);
        selectedObjectCounter--;
    }

    public void ChangeCells(int pos) {
        int index = pos;
        cells[pos].ClearCell(false);

        for (int i = pos; i < cells.Length - 1; i++) {
            if (cells[i + 1].currentObject != null) {
                cells[i].SetCell(cells[i + 1].currentObject, 0, true);
                index = i + 1;
            }
        }

        for (int i = index; i < cells.Length; i++) cells[i].ClearCell(false);
        selectedObjectCounter--;
    }
}
