using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour{

    [System.Serializable]
    public class Pool {
        public ObjectType tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;

    public Dictionary<ObjectType, Queue<GameObject>> poolDictionary;

    void Start(){
        poolDictionary = new Dictionary<ObjectType, Queue<GameObject>>();

        foreach(Pool pool in pools) { 
            Queue<GameObject> objectQueue = new Queue<GameObject>();
            for(int i = 0; i < pool.size; i++) {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectQueue.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectQueue);
        }
    }

    public GameObject SpawnFromPool(ObjectType tag, Vector3 position, Quaternion rotation) {

        if (!poolDictionary.ContainsKey(tag)) {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist");       
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(objectToSpawn);
 
        return objectToSpawn;
    }
}
