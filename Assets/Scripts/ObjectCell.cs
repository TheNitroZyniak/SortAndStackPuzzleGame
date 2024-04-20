using UnityEngine;

public class ObjectCell : MonoBehaviour{

    public int ObjectId { get; private set; }
    public SelectableObject currentObject;

    private void Start() {
        ObjectId = -1;
    }

    public void ClearCell(bool deactivate) {
        ObjectId = -1;
        if (currentObject != null) {
            if (deactivate) {
                //currentObject.gameObject.SetActive(false);
            }
            currentObject = null;
        }
    }

    public void SetCell(SelectableObject newObject, int id, bool doMove) {
        ObjectId = id;
        currentObject = newObject;
        if(doMove)
            currentObject.transform.position = transform.position;
    }

    public bool IsEmpty() {
        return ObjectId == -1 ? true : false;
    }
}
