using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class StackManager : MonoBehaviour{

    [Inject] UIManager _uiManager;

    [SerializeField] Stack stackPrefab;
    [SerializeField] GameObject[] points;
    [SerializeField] Transform[] centrePoints;


    List<Stack> stackList = new List<Stack>();

    float factor = 1f;

    Transform ourStack;

    public void SetStackParams(int amount) {
        ourStack = points[amount - 3].transform;
        ourStack.gameObject.SetActive(true);

        switch (amount) {
            case 3: factor = 1f; break;
            case 4: factor = 1.1f; break;
            case 5: factor = 1.2f; break;
            case 6: factor = 1.3f; break;

        }
    }

    public void ResetStacks() {
        foreach (Stack stack in stackList)
            stack.ResetPos();
    }

    int counter = 0;

    public void CreateStacks(string stackType) {
        Stack stack = Instantiate(stackPrefab,
            ourStack.transform.GetChild(counter).transform.position,
            Quaternion.identity);

        stack.type = stackType;
        stackList.Add(stack);

        counter++;

    }


    public void AddToStack(SelectableObject newObject) {
        foreach (Stack stack in stackList) {
            if (stack.type == newObject.objectType){
                stack.Add(newObject, factor);
            }
        }
        //stacks[newObject.id].Add(newObject);
    }

    int c;

    public IEnumerator MoveFromStacksToCentre() {
        yield return new WaitForSeconds(1f);

        //foreach (Stack stack in stackList) {
        //    foreach(SelectableObject obj in stack.list) {
        //        obj.transform.DOMove(centrePoints[c].position, 0.5f);
        //        obj.transform.DOScale(obj.startScale/2f, 0.5f);
        //        obj.transform.DORotate(obj.endRotation, 0.5f);

        //        centrePoints[c].position = new Vector3(centrePoints[c].position.x, centrePoints[c].position.y + 0.75f, centrePoints[c].position.z);

        //        yield return new WaitForSeconds(0.25f);
        //    }
        //    yield return new WaitForSeconds(0.5f);
        //    c++;
        //}

        for(int i = stackList[0].list.Count - 1; i >= 0; i--) { 
            for(int j = 0; j < stackList.Count; j++) {

                SelectableObject obj = stackList[j].list[i];
                obj.transform.DOMove(centrePoints[j].position, 0.5f);
                obj.transform.DOScale(obj.startScale / 2f, 0.5f);
                obj.transform.DORotate(obj.endRotation, 0.5f);

                centrePoints[j].position = new Vector3(centrePoints[j].position.x, centrePoints[j].position.y + 0.75f, centrePoints[j].position.z);
            }

            yield return new WaitForSeconds(0.25f);
        }

        //foreach (Stack stack in stackList) {
        //    foreach (SelectableObject obj in stack.list) {
        //        obj.transform.DOMove(centrePoints[c].position, 0.5f);
        //        obj.transform.DOScale(obj.startScale / 2f, 0.5f);
        //        obj.transform.DORotate(obj.endRotation, 0.5f);

        //        centrePoints[c].position = new Vector3(centrePoints[c].position.x, centrePoints[c].position.y + 0.75f, centrePoints[c].position.z);

                
        //    }
        //    yield return new WaitForSeconds(0.25f);
        //    c++;
        //}



        _uiManager.OpenVictoryPopup();

    }

}
