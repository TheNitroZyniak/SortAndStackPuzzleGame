using UnityEngine;

public class ObjectCell : MonoBehaviour{
    [SerializeField] private int cellId;
    //public int ObjectId { get; private set; }

    public string ObjectString { get; private set; }

    public SelectableObject currentObject;

    private void Start() {
        //ObjectId = -1;
        ObjectString = "";
    }

    public void ClearCell(bool deactivate) {
        //ObjectId = -1;
        ObjectString = "";
        if (currentObject != null) {
            if (deactivate) {
                //currentObject.gameObject.SetActive(false);
            }
            currentObject = null;
        }
    }

    public void SetCell(SelectableObject newObject, int id, bool doMove) {
        //ObjectId = id;

        ObjectString = newObject.objectType;

        currentObject = newObject;
        currentObject.currentCell = cellId;

        if (doMove) {
            currentObject.transform.position = transform.position;
            currentObject.twitchingPosition = transform.position;
        }
    }
    public bool IsEmpty() {
        //return ObjectId == -1 ? true : false;
        return ObjectString == "" ? true : false;
    }
}
