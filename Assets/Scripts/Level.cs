using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu (fileName = "New Level", menuName = "ScriptableObjects/Levels")]
public class Level : ScriptableObject{
    public int levelIndex;
    public int secondsToComplete;

    public List<LevelObjectData> objects = new List<LevelObjectData>();
}

[System.Serializable]
public class LevelObjectData {
    public ObjectType objectType;
    public int objectAmount;
}

public enum ObjectType {
    None,
    Shirt_1,
    Shirt_2,
    Shirt_3,
    Shirt_4,
    Shirt_5,
    Shirt_6,
    Sandwich,
    Headphones,
    Pizza,
    Gym,
    Donut,
    Disko
}
