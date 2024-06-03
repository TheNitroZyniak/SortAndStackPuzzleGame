using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class BoxesManager : MonoBehaviour {
    [Inject] UIManager _uiManager;
    //[Inject] ObjectPooler _objectPooler;
    [Inject] MainGameManager _mainGameManager;
    [Inject] BottomCells _bottomCells;

    [SerializeField] Transform[] centrePoints;
    [SerializeField] private Box boxPrefab;

    public List<Box> boxesList = new List<Box>();
    [SerializeField] GameObject[] points;
    Transform ourStack;
    int counter;


    private Vector2 startPosition;
    private Vector2 currentPosition;
    private float distance;

    float xPointToNotMove, xMaxPointToMove;
    
    int currentLevel;

    Coroutine previosCoroutine, previosCoroutine1;


    private readonly Queue<Task> _moveTasks = new Queue<Task>();

    bool blockThread;


    bool boxesAreMoving;

    int collectedBoxesConter;


    public async Task MoveBoxesAsync() {

        if (_moveTasks.Count > 0 && !blockThread) {
            blockThread = true;
            
            var task = _moveTasks.Dequeue();

            await task;
            
            blockThread = false;
        }
    }

    public void AddMoveTask(Task task) {
        _moveTasks.Enqueue(task);
    }

    void Update() {

        //MoveBoxesAsync();

        if (counter > 6) {
            foreach (Box box in boxesList) {
                if (box.transform.position.x >= xMaxPointToMove + 4.5f - 0.001f) {
                    Box objectWithMinX = boxesList.OrderBy(obj => obj.transform.position.x).First();
                    box.transform.position = new Vector3(objectWithMinX.transform.position.x - 4.5f, box.transform.position.y, box.transform.position.z);
                } else if (box.transform.position.x < -xMaxPointToMove - 4.5f + 0.001f) {
                    Box objectWithMinX = boxesList.OrderBy(obj => obj.transform.position.x).Last();
                    box.transform.position = new Vector3(objectWithMinX.transform.position.x + 4.5f, box.transform.position.y, box.transform.position.z);
                }
            }
        }
    }
    public void SetStackParams(int amount) {
        ourStack = points[amount - 3].transform;
        ourStack.gameObject.SetActive(true);
        boxesAreMoving = false;
        transform.position = new Vector3(0, transform.position.y, transform.position.z);

        index = 0;
        reset = false;

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

        //newBox.type = string.Empty;
        newBox.type = stackType;
        //newBox.binText.text = stackType;
        newBox.transform.parent = transform;


        boxesList.Add(newBox);

        counter++;
    }
    
    int amount;

    public void SetBorders() {
        xPointToNotMove = 6.75f;
        xMaxPointToMove = ourStack.transform.GetChild(counter - 1).transform.position.x;

        amount = boxesList.Count / 2;
        if (boxesList.Count == 3) amount++;
        if (amount > 5) amount = 5;

        amount = boxesList.Count;

        foreach (Box box in boxesList) 
            box.gameObject.SetActive(false);

        for (int i = 0; i < amount; i++) {
            boxesList[i].gameObject.SetActive(true);
            //amount = i;
        }

        

        float posX = 0;



        //switch (amount) {
        //    case 2: posX = 2.25f; break;
        //    case 3: posX = 6.75f; break;
        //    case 4: posX = 9; break;
        //    case 5: posX = 11.25f; break;
        //    //case 6: posX = xPointToNotMove; break;
        //}

        //transform.position = new Vector3(posX, transform.position.y, transform.position.z);

        currentLevel = PlayerPrefs.GetInt("CurrentLevel");
    }



    public void AddToOneOfBoxes(SelectableObject objectToAdd) {
        bool addToNotEmpty = false;

        foreach (Box box in boxesList) {
            if(box.type == objectToAdd.objectType) {

                AddToBoxes(objectToAdd, box);
                addToNotEmpty = true;
                _mainGameManager.RemoveFromList(objectToAdd, false);
                _mainGameManager.CheckWin();

                if(objectToAdd.currentCell != -1) {
                    _bottomCells.MoveLeft(objectToAdd.currentCell);
                    _mainGameManager.RemoveFromHistory(objectToAdd);
                }


                if (box.amountOfObjects == box.list.Count) {
                    int counter = boxesList.IndexOf(box);


                    if (!boxesAreMoving) {
                        box.boxIsMoving = true;
                        print("ZANas");
                        MoveBoxesAndOneCreate(box, counter);
                        boxesAreMoving = true;
                    } 
                    else {
                        box.boxIsMoving = true;
                        print("Reset");
                        reset = true;
                    }



                    //StartCoroutine(DoIt(box, counter));
                }

                return;
            }
        }

        if (!addToNotEmpty) {
            foreach (Box box in boxesList) {
                if (box.gameObject.activeInHierarchy) {
                    if (box.type == string.Empty) {

                        int amount = ObjectPooler.Instance.AmountOfAllEnabledObjectsByName(objectToAdd.objectType);
                        box.amountOfObjects = amount;

                        box.type = objectToAdd.objectType;
                        AddToBoxes(objectToAdd, box);

                        List<string> types = new List<string>();
                        foreach(Box b in boxesList) {
                            if(b.gameObject.activeInHierarchy) {
                                types.Add(b.type);
                            }
                        }

                        _mainGameManager.DeactivateObjectsInCellsAmongSelected(types);
                        _mainGameManager.RemoveFromList(objectToAdd, false);

                        if (objectToAdd.currentCell != -1) {
                            _bottomCells.MoveLeft(objectToAdd.currentCell);
                            _mainGameManager.RemoveFromHistory(objectToAdd);
                        }

                        _mainGameManager.CheckWin();


                        return;
                    }
                }
            }
        }


        objectToAdd.Select(false);

    }

    bool doOne;

    IEnumerator DoIt(Box box, int counter) {
        if (previosCoroutine != null) {
            if (previosCoroutine1 != null) {
                doOne = false;
                yield return previosCoroutine1;
                
            } 
            else {
                doOne = true;
                yield return previosCoroutine;
                //previosCoroutine = StartCoroutine(CreateAndMoveBoxes(box, counter));
            }
        }

        //if(doOne)
        //    previosCoroutine = StartCoroutine(CreateAndMoveBoxes(box, counter));
        //else
        //    previosCoroutine1 = StartCoroutine(CreateAndMoveBoxes(box, counter));
    }

    int index = 0;
    bool reset = false;

    async void MoveBoxesAndOneCreate(Box box, int count) {
        while (index < boxesList.Count) {       
            await CreateAndMoveBoxes(boxesList[index], count);
            index++;
            if (index == boxesList.Count) {
                index = 0;
                break;
            }
        }

        boxesAreMoving = false;
        if (reset) {
            index = 0;
            reset = false;
            MoveBoxesAndOneCreate(box, count);
        }
    }


    public void AddToBoxes(SelectableObject newObject, Box currentBox) {
        if (currentBox.type == newObject.objectType) {
            currentBox.Add(newObject, 1);
        }

    }

    async Task CreateAndMoveBoxes(Box box, int counter) {

        //if (previosCoroutine != null) {
        //    yield return StartCoroutine(previosCoroutine);
        //}
        //previosCoroutine = StartCoroutine(CreateAndMoveBoxes(box, counter));

        if (box.boxIsMoving) {

            box.boxIsMoving = false;

            StartCoroutine(box.MoveBoxForward());

            for (int i = 0; i < counter; i++) {
                boxesList[i].gameObject.transform.parent = null;
            }

            await Task.Delay(2000);

            MoveLeft();

            await Task.Delay(500);

            print(amount + " " + boxesList.Count);

            if (amount < boxesList.Count) {
                Vector3 pos = boxesList[amount].gameObject.transform.position;

                boxesList[amount].gameObject.transform.position = new Vector3(12, pos.y, pos.z);
                boxesList[amount].gameObject.SetActive(true);

                boxesList[amount].gameObject.transform.DOMoveX(pos.x, 0.5f);

                _mainGameManager.ActivateAllObjectsInCells();
            } else {

            }

            amount++;
        }
    }

    void MoveLeft() {
        transform.DOMoveX(transform.position.x - 4.5f, 0.5f).OnComplete(() => {
            foreach (Box box in boxesList) {
                if (box.gameObject.transform.parent == null && !box.isMovingForward) {
                    box.gameObject.transform.parent = transform;
                }
                box.MakeAllKinematic(false);
            }
            
        });
    }



    public void MoveBoxes(Box boxToPlace) {

        if (boxToPlace.transform.position.x > xPointToNotMove + 0.1f || boxToPlace.transform.position.x < -xPointToNotMove - 0.1f) {

            float offX;

            if (boxToPlace.transform.position.x > xPointToNotMove) {
                offX = -(boxToPlace.transform.position.x - xPointToNotMove);
                boxToPlace.endPosition = new Vector3(xPointToNotMove, boxToPlace.transform.position.y, boxToPlace.transform.position.z);
            } else {
                offX = -xPointToNotMove - boxToPlace.transform.position.x;
                boxToPlace.endPosition = new Vector3(-xPointToNotMove, boxToPlace.transform.position.y, boxToPlace.transform.position.z);
            }
            

            offX = transform.position.x + offX;

            foreach (Box box in boxesList) {
                box.MakeAllKinematic(true);
            }

            transform.DOMoveX(offX, 0.5f).OnComplete(() => {
                foreach (Box box in boxesList) {


                    box.MakeAllKinematic(false);


                }
            });
        } 
        else {        
            if (currentLevel != 0) {

                if (boxToPlace.transform.position.x > 0 && boxToPlace.transform.position.x < 3)
                    boxToPlace.endPosition = new Vector3(2.25f, boxToPlace.transform.position.y, boxToPlace.transform.position.z);
                else if(boxToPlace.transform.position.x < 0 && boxToPlace.transform.position.x >- 3)
                    boxToPlace.endPosition = new Vector3(-2.25f, boxToPlace.transform.position.y, boxToPlace.transform.position.z);
                else if (boxToPlace.transform.position.x > 3 && boxToPlace.transform.position.x < 7)
                    boxToPlace.endPosition = new Vector3(6.75f, boxToPlace.transform.position.y, boxToPlace.transform.position.z);
                else
                    boxToPlace.endPosition = new Vector3(-6.75f, boxToPlace.transform.position.y, boxToPlace.transform.position.z);

                
            } 

            else {
                if (boxToPlace.transform.position.x > 2)
                    boxToPlace.endPosition = new Vector3(4.5f, boxToPlace.transform.position.y, boxToPlace.transform.position.z);
                else if(boxToPlace.transform.position.x < -2)
                    boxToPlace.endPosition = new Vector3(-4.5f, boxToPlace.transform.position.y, boxToPlace.transform.position.z);
                else
                    boxToPlace.endPosition = new Vector3(0, boxToPlace.transform.position.y, boxToPlace.transform.position.z);

                
            }
        }

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
