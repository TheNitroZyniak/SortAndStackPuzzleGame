using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cells : MonoBehaviour {

    [SerializeField] private ObjectCell[] cellArray; // Массив ячеек
    private Dictionary<string, Queue<SelectableObject>> selectedObjects = new Dictionary<string, Queue<SelectableObject>>();



    public void CreateQueueForDictionary(string key) {
        Queue<SelectableObject> objectQueue = new Queue<SelectableObject>();
        selectedObjects.Add(key, objectQueue);
    }

    public void ClearSelectedObjects(string key) {
        if (selectedObjects.ContainsKey(key)) 
            selectedObjects[key].Clear();   
    }






    public Vector3 UpdateBefore(SelectableObject newObject, int id) {
        selectedObjects[newObject.objectType].Enqueue(newObject);

        //if (IsDefeated()) {
        //    GameLost();
        //    return Vector3.zero;
        //}

        //if (IsThreeSameObjectsCollected(newObject.objectType)) {
        //    GameWon();
        //    return Vector3.zero;
        //}

        //int insertIndex = GetInsertIndex(newObject.objectType);
        //if (insertIndex < 6 && !cellArray[insertIndex + 1].IsEmpty()) {
        //    ShiftObjectsRight(insertIndex + 1);
        //}

        //// Устанавливаем новый объект в соответствующую ячейку
        //cellArray[insertIndex].SetCell(newObject, id, false);
        //return cellArray[insertIndex].transform.position;
        return Vector3.zero;
    }





    public void SetObjectInCell(SelectableObject newObject, int cellIndex) {
        if (cellIndex >= 0 && cellIndex < cellArray.Length) {
            cellArray[cellIndex].SetCell(newObject, 0, false);
        } 
        else {
            Debug.LogWarning("Попытка установить объект в недопустимую ячейку.");
        }
    }


    public void ClearCell(int cellIndex) {
        if (cellIndex >= 0 && cellIndex < cellArray.Length) {
            cellArray[cellIndex].ClearCell(false);
        } 
        else {
            Debug.LogWarning("Попытка очистить недопустимую ячейку.");
        }
    }


    public SelectableObject GetObjectFromCell(int cellIndex) {
        if (cellIndex >= 0 && cellIndex < cellArray.Length) {
            return cellArray[cellIndex].currentObject;
        } else {
            Debug.LogWarning("Попытка получить объект из недопустимой ячейки.");
            return null;
        }
    }

    public bool IsCellEmpty(int cellIndex) {
        if (cellIndex >= 0 && cellIndex < cellArray.Length) {
            return cellArray[cellIndex].IsEmpty();
        } 
        else {
            Debug.LogWarning("Попытка проверить недопустимую ячейку.");
            return false;
        }
    }
}
