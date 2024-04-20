using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BottomCells : MonoBehaviour{
    [Inject] StackManager _stackManager;
    [SerializeField] ObjectCell[] cells;
    

    private int selectedObjectCounter = 0;
    int temp;

    public void UpdateBefore(SelectableObject newObject, int id) {
        int insertIndex = selectedObjectCounter;
        for (int i = 0; i < selectedObjectCounter; i++) {
            if (cells[i].ObjectId == id) {
                insertIndex = i + 1;
            }
        }

        for (int i = selectedObjectCounter - 1; i >= insertIndex; i--) {
            cells[i + 1].SetCell(cells[i].currentObject, cells[i].ObjectId, true);
        }

        cells[insertIndex].ClearCell(false);
        temp = selectedObjectCounter;
        selectedObjectCounter = insertIndex;
        

    }

    public Vector3 GetCurrentCell() {
        return cells[selectedObjectCounter].transform.position;
    }

    public void UpdateSelectedBallsDisplay(SelectableObject newObject, int id) {
        cells[selectedObjectCounter].SetCell(newObject, id, false);
        selectedObjectCounter = temp;
        selectedObjectCounter++;
        CheckFor3(id);
    }

    private void CheckFor3(int id) {
        int count = 0;
        for (int i = 0; i < selectedObjectCounter; i++) {
            if (cells[i].ObjectId == id) count++;
        }

        if (count == 3) {
            RemoveAndShiftCells(id);
        } 
        else if (selectedObjectCounter == cells.Length) {
            //GameController.Instance.SetActiveAllBalls(false);

            //LevelFailed levelFailed = new LevelFailed();
            //int currentLvl = PlayerPrefs.GetInt("Level");
            //levelFailed.level = currentLvl;
            //string json = JsonUtility.ToJson(levelFailed);
            //AppMetrica.Instance.ReportEvent("Level_Failed", json);
        }
    }


    private void RemoveAndShiftCells(int id) {
        int first = -1;
        int removeCount = 0;
        for (int i = 0; i < selectedObjectCounter; i++) {
            if (cells[i].ObjectId == id) {
                if (first < 0) first = i;

                _stackManager.AddToStack(cells[i].currentObject);

                cells[i].ClearCell(true);
                removeCount++;
            }
        }

        int index = first;
        for (int i = 0; i < cells.Length - 3; i++) {
            if (cells[i].IsEmpty()) {
                if (!cells[i + 3].IsEmpty()) {
                    cells[i].SetCell(cells[i + 3].currentObject, cells[i + 3].ObjectId, true);
                    index = i + 1;
                }
            }
        }

        for (int i = index; i < cells.Length; i++) cells[i].ClearCell(false);
        selectedObjectCounter = index;
    }
}
