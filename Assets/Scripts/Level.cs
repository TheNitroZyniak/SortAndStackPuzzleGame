using UnityEditor;
using UnityEngine;

[CreateAssetMenu (fileName = "New Level", menuName = "ScriptableObjects/Levels")]
public class Level : ScriptableObject{
    public int levelIndex;
    public int secondsToComplete;
    public int amountOfCubes;
    public int amountOfSpheres;
    public int amountOfCapsules;
}
