using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using TMPro;
using DG.Tweening;
using UnityEditor.Rendering;

public class BottomCells : MonoBehaviour{
    [Inject] BoxesManager _boxesManager;
    [Inject] MainGameManager _mainGameManager;
    [SerializeField] ObjectCell[] cells;

    private Dictionary<string, Stack<SelectableObject>> selectedObjects = new Dictionary<string, Stack<SelectableObject>>();

    private int selectedObjectCounter = -1;

    public static int currentCoroutine;


    public void CreateDictionary(string key) {
        Stack<SelectableObject> objectQueue = new Stack<SelectableObject>();
        selectedObjects.Add(key, objectQueue);
    }

    public void ClearDictionary() {
        selectedObjects.Clear();
        selectedObjectCounter = -1;
        currentCoroutine = 0;

        objectsToRemove1.Clear();
        objectsToRemove2.Clear();

        foreach(ObjectCell cell in cells) {
            cell.ClearCell(false);
        }

    }

    List<SelectableObject> objectsToRemove1 = new List<SelectableObject>();
    List<SelectableObject> objectsToRemove2 = new List<SelectableObject>();

    Coroutine previosCoroutine;

    public bool CheckIfAllow(SelectableObject newObject) {
        if (currentCoroutine >= 2 && selectedObjects[newObject.objectType].Count == 2) {
            return false;
        }
        else
            return true;
    }

    public Vector3 UpdateBefore(SelectableObject newObject, int id) {
        
        selectedObjects[newObject.objectType].Push(newObject);

        if (selectedObjects[newObject.objectType].Count == 3) {
            if (objectsToRemove1.Count == 0) {
                for (int i = 0; i < 3; i++) {
                    SelectableObject ourObject = selectedObjects[newObject.objectType].Pop();
                    //selectedObjects[newObject.objectType]
                    ourObject.removeList = 1;
                    objectsToRemove1.Add(ourObject);
                }
                objectsToRemove1.Reverse();
            } 
            else {
                //foreach (SelectableObject ourObject in selectedObjects[newObject.objectType]) {
                for (int i = 0; i < 3; i++) {
                    SelectableObject ourObject = selectedObjects[newObject.objectType].Pop();
                    ourObject.removeList = 2;
                    objectsToRemove2.Add(ourObject);
                }
                objectsToRemove2.Reverse();
            }
            
            
            selectedObjects[newObject.objectType].Clear();
        }

        int insertIndex = selectedObjectCounter;
        bool move = false;

        for (int i = 0; i < 5; i++) {
            if (cells[i].ObjectString == newObject.objectType) {
                if (cells[i].ObjectString != cells[i + 1].ObjectString && !cells[i + 1].IsEmpty()) {
                    move = true;
                    insertIndex = i + 1;
                }
            }
        }

        if (move) {
            Queue<SelectableObject> objectsToMove = new Queue<SelectableObject>();

            for (int i = insertIndex; i < 6; i++) {
                if (cells[i].currentObject != null)
                    objectsToMove.Enqueue(cells[i].currentObject);
            }
            if (objectsToMove.Count > 0) {
                for (int i = insertIndex; i < 6; i++) {
                    if (objectsToMove.Count > 0) {
                        cells[i + 1].SetCell(objectsToMove.Dequeue(), 0, true);
                        selectedObjectCounter = i + 1;
                    }
                }
                cells[insertIndex].SetCell(newObject, id, false);
                if (selectedObjectCounter == 6) _mainGameManager.BlockTouch();
                return cells[insertIndex].transform.position;
            } 
            else {
                print("Check me!");
                if (selectedObjectCounter == 6) _mainGameManager.BlockTouch();
                return cells[selectedObjectCounter].transform.position;
            }
        }
        else {
            selectedObjectCounter++;
            if (selectedObjectCounter == 6) _mainGameManager.BlockTouch();
            cells[selectedObjectCounter].SetCell(newObject, id, false);
            return cells[selectedObjectCounter].transform.position;
        }
    }


    [SerializeField] MeshRenderer[] indikators;
    [SerializeField] TextMeshProUGUI currentCoroutText;


    public void UpdateAfterUndo(string typeId) {
        selectedObjects[typeId].Pop();
    }


    private void Update() {

        //currentCoroutText.text = currentCoroutine.ToString();

        //for (int i = 0; i < cells.Length; i++) {
        //    if (cells[i].IsEmpty())
        //        indikators[i].material.color = Color.green;
        //    else
        //        indikators[i].material.color = Color.red;
        //}
        //if (selectedObjectCounter + 1 < 7) {
        //    indikators[selectedObjectCounter + 1].material.color = Color.yellow;
        //}
        //for (int i = 0; i < cells.Length; i++) {
        //    if (cells[i].IsEmpty()) {
        //        selectedObjectCounter = i - 1;
        //        break;
        //    }
        //}


        foreach (SelectableObject obj in _mainGameManager.moveHistory) {
            Check3After(obj);
        }
    }

    public Vector3 GetCurrentCell() {
        return cells[selectedObjectCounter].transform.position;
    }


    public int GetCurrentCellId() {
        return selectedObjectCounter;
    }

    public void UpdateSelectedBallsDisplay(SelectableObject newObject, int id) {
        CheckFor3(newObject.objectType, newObject.removeList);
    }

    private void CheckFor3(string id, int removeList) {

        if(removeList == 1) {
            RemoveBefore(objectsToRemove1, id);
        }
        else
            RemoveBefore(objectsToRemove2, id);
    }


    [SerializeField] TextMeshProUGUI objectsToRemove;

    public void RemoveBefore(List<SelectableObject> selObjects, string id) {
        int count = 0;
        string objType = String.Empty;

        for (int i = 0; i < selectedObjectCounter + 1; i++) {
            if (cells[i].ObjectString == id) {
                count++;
                objType = cells[i].currentObject.objectType;
            }
        }

        if (selObjects != null) {

            if (selObjects.Count == 3) {

                foreach (Box box in _boxesManager.boxesList) {
                    if (box.type == objType) {
                        bool block = true;
                        foreach (ObjectCell cell in cells) {
                            if (cell.IsEmpty()) {
                                block = false;
                            }
                        }

                        objectsToRemove.text = selObjects[0].name + " " + selObjects[1].name + " " + selObjects[2].name;
                        RemoveAndShiftCells(selObjects, box);
                    }
                }

            } 
            else if (selectedObjectCounter + 1 == cells.Length) {
                _mainGameManager.GameLost();
            }
        } 
        else if (selectedObjectCounter + 1 == cells.Length) {
            _mainGameManager.GameLost();
        }
    }

    public void Check3After(SelectableObject thisObject) {
        int c = thisObject.currentCell;
        thisObject.transform.position = cells[c].transform.position;
    }


    public void RemoveAndShiftCells(List<SelectableObject> removeObjects, Box boxToPlace) {
        foreach (SelectableObject obj in removeObjects)       
            _mainGameManager.RemoveFromHistory(obj);
        
        StartCoroutine(DoIEnum(removeObjects, boxToPlace));
    }

    IEnumerator DoIEnum(List<SelectableObject> removeObjects, Box boxToPlace) {
        currentCoroutine++;

        if (previosCoroutine != null) yield return previosCoroutine;
        previosCoroutine = StartCoroutine(Remove3(removeObjects, boxToPlace));
    }

    IEnumerator Remove3(List<SelectableObject> removeObjects, Box boxToPlace) {

        _boxesManager.MoveBoxes(boxToPlace);
        int c = selectedObjectCounter + 1;

        if (removeObjects.Count == 0) yield return null;

        int first = removeObjects[0].currentCell;
        int counter = first;
        foreach (SelectableObject obj in removeObjects) {
            obj.removeList = 0;
        }

        foreach (SelectableObject obj in removeObjects) {
            cells[counter].ClearCell(true);
            counter++;
        }

        int index = first;
        bool removeLast = index == 0 ? true : false;

        for (int i = 0; i < cells.Length - 3; i++) {
            if (cells[i].IsEmpty()) {
                if (!cells[i + 3].IsEmpty()) {
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
                index++;
            }
        }

        for (int i = index; i < cells.Length; i++) cells[i].ClearCell(false);
        
        selectedObjectCounter--;
        _mainGameManager.Unblock();

        List<SelectableObject> finalObjects = new List<SelectableObject>();
        for(int i = 0; i < removeObjects.Count; i++) 
            finalObjects.Add(removeObjects[i]);

        removeObjects.Clear();

        foreach (SelectableObject obj in finalObjects) {
            Vector3 pos = obj.transform.position;
            obj.transform.DOMove(new Vector3(pos.x, pos.y, pos.z - 2), 0.1f);

            Vector3 scale = obj.transform.localScale;

            obj.transform.DOScale(scale * 1.25f, 0.2f);
        }

        yield return new WaitForSeconds(0.1f);

        foreach (SelectableObject obj in finalObjects) {
            _boxesManager.AddToBoxes(obj, boxToPlace);
            yield return new WaitForSeconds(0.15f);
        }
        finalObjects.Clear();
        yield return new WaitForSeconds(0.2f);
        currentCoroutine--;
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

    public void CheckAllCells() {
        for (int i = 0; i < cells.Length; i++) {
            if (cells[i].IsEmpty()) {
                int nearestIndex = FindNearestNonEmptyCellIndex(i);
                if (nearestIndex != -1) {
                    cells[i].SetCell(cells[nearestIndex].currentObject, 0, true);
                    cells[nearestIndex].ClearCell(false);
                }
            }
        }
    }


    int FindNearestNonEmptyCellIndex(int currentIndex) {
        int nearestIndex = -1;
        float minDistance = float.MaxValue;
        for (int i = currentIndex; i < cells.Length; i++) {
            if (!cells[i].IsEmpty() && i != currentIndex) {
                float distance = Vector3.Distance(cells[currentIndex].transform.position, cells[i].transform.position);
                if (distance < minDistance) {
                    minDistance = distance;
                    nearestIndex = i;
                }
            }
        }
        return nearestIndex;
    }


    public void MoveLeft(int currentCell) {
        cells[currentCell].ClearCell(false);
        int c = currentCell;
        for (int i = currentCell; i < 6; i++) {
            if (!cells[i + 1].IsEmpty()) {
                cells[i].SetCell(cells[i + 1].currentObject, 0, true);
                c = i + 1;
            }
        }
        for(int i = c; i < 6; i++)
            cells[i].ClearCell(false);

        selectedObjectCounter--;
    }
}
