using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu (fileName = "New Level", menuName = "ScriptableObjects/Levels")]
public class Level : ScriptableObject{
    public int levelIndex;
    public int secondsToComplete;

    public List<LevelObjectData> objects = new List<LevelObjectData>();

    public List<LevelObjectData> Get12Objects() {

        List<LevelObjectData> selected12Objects = new List<LevelObjectData>();

        for (int i = 0; i < 12; i++) {
            int r = Random.Range(0, objects.Count);
            selected12Objects.Add(objects[r]);
            objects.RemoveAt(r);
        }
        //objects.Clear();
        for (int i = 0; i < 12; i++) {
            objects.Add(selected12Objects[i]);
        }
        return selected12Objects;
    }
}

[System.Serializable]
public class LevelObjectData {
    public string objectType;
    public int objectAmount;
}

//public enum ObjectType {
//    None,
//    Book,
//    Book_2,
//    Clock,
//    Coin,
//    Drum,
//    Exclamation_Mark,
//    Fan,
//    Fork,
//    Guitar,
//    Harp,
//    Hat,
//    IceCream,
//    Key,
//    Leaf,
//    Letter,
//    Magnet,
//    Monitor,
//    Newspaper,
//    Note,
//    Pizza,
//    Question_Mark,
//    Radio,
//    Rainbow,
//    Scissors,
//    Shield, 
//    Smartphone,
//    Tambourine,
//    Violin,
//    Waffle,
//    Watermelon,
//    Wheel,
//    Window,
//    Xylophone,
//    A,
//    B,
//    C,
//    D,
//    E,
//    F,
//    G,
//    H,
//    I,
//    J,
//    K,
//    L,
//    M,
//    N,
//    O,
//    P,
//    Q,
//    R,
//    S,
//    T,
//    U,
//    V,
//    W,
//    X,
//    Y,
//    Z,
//    Zero,
//    One,
//    Two,
//    Three,
//    Four,
//    Five,
//    Six,
//    Seven,
//    Eight,
//    Nine,
//    Spoon,
//    Ink,
//    Giftbox,
//    Mike
//}
