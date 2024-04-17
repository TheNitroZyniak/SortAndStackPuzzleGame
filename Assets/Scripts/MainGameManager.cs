using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MainGameManager : MonoBehaviour {

    private List<SelectableObject> allObjects = new List<SelectableObject>();
    


    private bool CheckAllObjects() {
        if(allObjects.Count > 0) return true;
        return false;
    }

    public void AddToList(SelectableObject newObject) => allObjects.Add(newObject);
    public void RemoveFromList(SelectableObject objectToRemove) {
        if (allObjects.Any(obj => obj == objectToRemove)) {
            allObjects.Remove(objectToRemove);
            if (!CheckAllObjects()) {
                print("Victory");
            }
        }
    }

}
