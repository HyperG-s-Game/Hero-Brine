using UnityEngine;
using System.Collections.Generic;


namespace GamerWolf.Utils{

    
    public enum PoolObjectTag{
        LevelStraightRoad_1 , 
        LevelStraightRoad_2,
        LevelStraightRoad_3,
        LevelStraightRoad_4,
        LevelStraightRoad_5,
        LevelStraightRoad_6,
        LevelStraightRoad_7,
        LevelStraightRoad_8,
        LevelStraightRoad_9,
        LevelStraightRoad_10,

        // Right turn Only........
        RightTurnOnly_1,
        RightTurnOnly_2,
        RightTurnOnly_3,
        RightTurnOnly_4,
        RightTurnOnly_5,
        RightTurnOnly_6,
        RightTurnOnly_7,
        RightTurnOnly_8,
        RightTurnOnly_9,
        RightTurnOnly_10,

        // Left Turn Only.............
        LeftTurnOnly_1,
        LeftTurnOnly_2,
        LeftTurnOnly_3,
        LeftTurnOnly_4,
        LeftTurnOnly_5,
        LeftTurnOnly_6,
        LeftTurnOnly_7,
        LeftTurnOnly_8,
        LeftTurnOnly_9,
        LeftTurnOnly_10,

        // T_Section..........
        T_Section_1,
        T_Section_2,
        T_Section_3,

        // Other
        puddel_Fall_Effect,
        coin_Effect,
    }
    public class ObjectPoolingManager : MonoBehaviour{

        #region Singelton.
        public static ObjectPoolingManager current{get;private set;}

        private void Awake(){
            if(current == null){
                current = this;
            }else{
                Debug.LogError(nameof(ObjectPoolingManager) + " is Found in the Scene");
                Destroy(current.gameObject);
            }
        }

        #endregion

        [System.Serializable]
        public class Pool{
            public PoolObjectTag tag;
            public GameObject prefabs;
            public int size;
        }

        [SerializeField] private List<Pool> pools = new List<Pool>();
        private Dictionary<PoolObjectTag , Queue<GameObject>> poolDictionary;

        private GameObject parentObj;
        private void Start(){
            poolDictionary = new Dictionary<PoolObjectTag, Queue<GameObject>>();
            CreatePool();
        }

        public void CreatePool(){
            foreach(Pool pool in pools){
                parentObj = new GameObject(pool.tag + " Pooled Object Parent");
                parentObj.transform.SetParent(transform);
                Queue<GameObject> objectPool = new Queue<GameObject>();
                for(int i = 0; i < pool.size; i++){
                    if(pool.prefabs != null){
                        GameObject obj = Instantiate(pool.prefabs) as GameObject;
                        obj.SetActive(false);
                        obj.transform.SetParent(parentObj.transform);
                        obj.name = string.Concat(pool.tag ," ",obj.transform.GetSiblingIndex().ToString());
                        objectPool.Enqueue(obj);
                    }
                    
                    
                }
                poolDictionary.Add(pool.tag,objectPool);
            }
        }
        private PoolObjectTag GetRandomTag(){
            int randomNum = Random.Range(0,pools.Count);
            return pools[randomNum].tag;
        }

        public GameObject SpawnRandomFromPool(Vector3 _spawnPoint,Quaternion _rotations){
            return SpawnFromPool(GetRandomTag(),_spawnPoint,_rotations);
        }
        
        public GameObject SpawnFromPool(PoolObjectTag tag,Vector3 _spawnPosition,Quaternion _rotation){
            if(!poolDictionary.ContainsKey(tag)){
                Debug.Log("Pool With the " + tag + " is not Found");
                return null;
            }
            GameObject objectToSpawn = poolDictionary[tag].Dequeue();
            objectToSpawn.transform.position = _spawnPosition;
            objectToSpawn.transform.rotation = _rotation;
            objectToSpawn.SetActive(true);
            IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();
            if(pooledObject != null){
                pooledObject.OnObjectReuse();
            }
            
            poolDictionary[tag].Enqueue(objectToSpawn);
            return objectToSpawn;
        }
    }

}
