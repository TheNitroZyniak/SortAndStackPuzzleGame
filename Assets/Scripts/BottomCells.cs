using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BottomCells : MonoBehaviour{
    [Inject] BoxesManager _boxesManager;
    [Inject] MainGameManager _mainGameManager;
    [SerializeField] ObjectCell[] cells;

    private Dictionary<string, Queue<SelectableObject>> selectedObjects = new Dictionary<string, Queue<SelectableObject>>();

    private int selectedObjectCounter = -1;

    public void CreateDictionary(string key) {
        Queue<SelectableObject> objectQueue = new Queue<SelectableObject>();
        selectedObjects.Add(key, objectQueue);
    }

    public void ClearDictionary() {
        selectedObjects.Clear();
    }

    List<SelectableObject> objectsToRemove1 = new List<SelectableObject>();
    List<SelectableObject> objectsToRemove2 = new List<SelectableObject>();

    Coroutine previosCoroutine;


    public Vector3 UpdateBefore(SelectableObject newObject, int id) {
        
        selectedObjects[newObject.objectType].Enqueue(newObject);

        if (selectedObjects[newObject.objectType].Count == 3) {
            if (objectsToRemove1.Count == 0) {
                //objectsToRemove1 = new List<SelectableObject>();
                foreach (SelectableObject ourObject in selectedObjects[newObject.objectType]) {
                    ourObject.removeList = 1;
                    objectsToRemove1.Add(ourObject);
                }
            } 
            else {
                //objectsToRemove2 = new List<SelectableObject>();
                foreach (SelectableObject ourObject in selectedObjects[newObject.objectType]) {
                    ourObject.removeList = 2;
                    objectsToRemove2.Add(ourObject);
                }
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
            } else {
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
        //if(move) selectedObjectCounter--;

        
        
    }




    [SerializeField] MeshRenderer[] indikators;
    private void Update() {
        //for (int i = 0; i < cells.Length; i++) {
        //    if (cells[i].IsEmpty())
        //        indikators[i].material.color = Color.green;
        //    else
        //        indikators[i].material.color = Color.red;
        //}
        //if (selectedObjectCounter + 1 < 7) {
        //    indikators[selectedObjectCounter + 1].material.color = Color.yellow;
        //}



        //foreach (SelectableObject obj in _mainGameManager.moveHistory) {
        //    Check3After(obj);
        //}
    }

    public void UpdateSelectedCounter() {
        //for (int i = selectedObjectCounter + 1; i < cells.Length; i++) {
        //    if (!cells[i].IsEmpty()) selectedObjectCounter++;
        //}
    }

    public Vector3 GetCurrentCell() {
        return cells[selectedObjectCounter].transform.position;
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
                        if (block)
                            _mainGameManager.BlockTouch();

                        List<SelectableObject> takenObjects = new List<SelectableObject>();
                        RemoveAndShiftCells(selObjects, box);
                        //selObjects.Clear();
                    }
                }
            } 
            //else if (selectedObjectCounter + 1 == cells.Length) {
            //    _mainGameManager.GameLost();
            //}
        } 
        else if (selectedObjectCounter + 1 == cells.Length) {
            _mainGameManager.GameLost();
        }
    }




    public void Check3After(SelectableObject thisObject) {
        int c = thisObject.currentCell;

        if(thisObject.transform.position != cells[c].transform.position)
            thisObject.transform.position = cells[c].transform.position;

    }


    public void RemoveAndShiftCells(List<SelectableObject> removeObjects, Box boxToPlace) {
        foreach(SelectableObject obj in removeObjects) 
            _mainGameManager.RemoveFromHistory(obj);

        StartCoroutine(DoIEnum(removeObjects, boxToPlace));
        
    }

    IEnumerator DoIEnum(List<SelectableObject> removeObjects, Box boxToPlace) {
        if (previosCoroutine != null) yield return previosCoroutine;
        previosCoroutine = StartCoroutine(Remove3(removeObjects, boxToPlace));
    }

    IEnumerator Remove3(List<SelectableObject> removeObjects, Box boxToPlace) {

        

        _boxesManager.MoveBoxes(boxToPlace);
        int c = selectedObjectCounter + 1;

        int first = removeObjects[0].currentCell;
        int counter = first;
        foreach (SelectableObject obj in removeObjects) {
            obj.removeList = 0;
        }

        //yield return new WaitForSeconds(0.5f);

        
        foreach (SelectableObject obj in removeObjects) {       
            _boxesManager.AddToBoxes(obj, boxToPlace);  
            yield return new WaitForSeconds(0.25f);
        }
        foreach (SelectableObject obj in removeObjects) {         
            cells[counter].ClearCell(true);
            counter++;
        }
        removeObjects.Clear();

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

        _mainGameManager.Unblock();
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
