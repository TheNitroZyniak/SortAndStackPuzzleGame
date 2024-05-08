using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using Zenject;

public class BoxesManager : MonoBehaviour{
    [Inject] UIManager _uiManager;

    [SerializeField] Transform[] centrePoints;
    [SerializeField] private Box boxPrefab;

    public List<Box> boxesList = new List<Box>();
    [SerializeField] GameObject[] points;
    Transform ourStack;
    int counter;


    private Vector2 startPosition;
    private Vector2 currentPosition;
    private float distance;


    void Update() {
        //if (Input.GetMouseButtonDown(0)) {
        //    startPosition = Input.mousePosition;
        //} else if (Input.GetMouseButton(0)) {
        //    currentPosition = Input.mousePosition;
        //    distance = currentPosition.x - startPosition.x;

        //    Vector3 position = transform.position;
        //    position.x += distance * 0.01f; // Отрегулируйте значение 0,01f для желаемой скорости перемещения

        //    transform.position = position;
        //    startPosition = currentPosition;
        //}



        //if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
        //    startPosition = Input.GetTouch(0).position;
        //} 
        //else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
        //    currentPosition = Input.GetTouch(0).position;
        //    distance = currentPosition.x - startPosition.x;

        //    Vector3 position = transform.position;
        //    position.x += distance * 0.01f; // Отрегулируйте значение 0,01f для желаемой скорости перемещения

        //    transform.position = position;
        //    startPosition = currentPosition;
        //}
    }


    public void SetStackParams(int amount) {
        ourStack = points[amount - 3].transform;
        ourStack.gameObject.SetActive(true);

        counter = 0;
        foreach (Box box in boxesList) {
            Destroy(box.gameObject);
        }
        boxesList.Clear();
    }

    public void CreateStacks(string stackType) {
        Box newBox = Instantiate(boxPrefab,
            ourStack.transform.GetChild(counter).transform.position,
            Quaternion.Euler(180, 90, -90));

        newBox.type = stackType;
        newBox.binText.text = stackType;
        newBox.transform.parent = transform;


        boxesList.Add(newBox);

        counter++;

    }


    public void AddToBoxes(SelectableObject newObject, Box currentBox) {

        if (currentBox.type == newObject.objectType) {
            currentBox.Add(newObject, 1);

        }
        //stacks[newObject.id].Add(newObject);
    }


    public void MoveBoxes(Box boxToPlace) {

        float offX = boxToPlace.transform.position.x - transform.position.x;
        foreach (Box box in boxesList) {
            box.MakeAllKinematic(true);
        }

        
        transform.DOMoveX(-offX, 0.5f).OnComplete(() => {
            foreach (Box box in boxesList) {
                box.MakeAllKinematic(false);
            }
        });
    }

    public IEnumerator MoveFromStacksToCentre() {
        yield return new WaitForSeconds(3f);


        for (int i = boxesList[0].list.Count - 1; i >= 0; i--) {
            for (int j = 0; j < boxesList.Count; j++) {

                SelectableObject obj = boxesList[j].list[i];
                obj.transform.DOMove(centrePoints[j].position, 0.5f);
                obj.transform.DOScale(obj.startScale / 2f, 0.5f);
                obj.transform.DORotate(obj.endRotation, 0.5f);

                centrePoints[j].position = new Vector3(centrePoints[j].position.x, centrePoints[j].position.y + 0.75f, centrePoints[j].position.z);
            }

            yield return new WaitForSeconds(0.25f);
        }


        _uiManager.OpenVictoryPopup();

    }
}
