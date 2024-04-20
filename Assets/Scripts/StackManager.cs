using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StackManager : MonoBehaviour{

    [SerializeField] Stack stackPrefab;
    [SerializeField] GameObject[] points;

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

    

    int counter = 0;

    public void CreateStacks(ObjectType stackType) {
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

}
