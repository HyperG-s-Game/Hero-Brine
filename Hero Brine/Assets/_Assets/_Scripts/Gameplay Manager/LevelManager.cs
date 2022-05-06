using System;
using UnityEngine;
using GamerWolf.Utils;
using System.Collections.Generic;
namespace HeroBrine {
    public class LevelManager : MonoBehaviour{

        [SerializeField] private PlayerController playerController;
        [SerializeField] private Transform startingPoint;
        [SerializeField] private ObjectPoolingManager poolingManager;
        [SerializeField] private PoolObjectTag[] straingRoadTagArray,rightTurnOnlyTagArray,leftTurnOnlyTagArray,tSection;
        private LevelVariations newVariations;
        private bool canSpawn;

        public int levelSegmentCrossed;
        public static LevelManager current{get;private set;}
        
        private List<LevelVariations> previouslySpawnedVariationsList;
        public Action<Transform> OnTSectionTurn;
        private void Awake(){
            current = this;
        }
        private void Start(){
            OnTSectionTurn += (Transform dir) =>{
                SpawnRoadInDir(dir);
            };
            previouslySpawnedVariationsList = new List<LevelVariations>();
            playerController.OnPlayerTurn += (object sender,System.EventArgs e) => {
                startingPoint.gameObject.SetActive(false);
                int randAmountOfLevel = UnityEngine.Random.Range(5,30);
                SpawnNumberOfStraightRoad(randAmountOfLevel);
            };

            Invoke(nameof(SpawnInitialSegment),0.1f);
        }
        public void SpawnInitialSegment(){
            GameObject levelObject = poolingManager.SpawnFromPool(PoolObjectTag.LevelStraightRoad_1,startingPoint.position,Quaternion.identity);
            LevelVariations Obstaclelevel = levelObject.GetComponent<LevelVariations>();
            if(Obstaclelevel != null){
                newVariations = Obstaclelevel;
            }
            SpawnNumberOfStraightRoad(4);
        }
        public void SpawnNumberOfStraightRoad(int totalAmount){
            if(canSpawn){
                for (int i = 0; i < totalAmount; i++){
                    if(i == totalAmount - 1){
                        int rand = UnityEngine.Random.Range(0,10);
                        if(rand >= 2 && rand < 6){
                            SpawnLeftTurnRoad();
                        }else if(rand == 6 || rand == 7){
                            SpawnT_Section();
                        }else{
                            SpawnRightTurnRoad();
                        }
                    }else{
                        SpawnStraightRoad();
                    }

                }
            }
        }
        private void SpawnRoadInDir(Transform point){
            GameObject levelObject = poolingManager.SpawnFromPool(GetRandomStraightTag(),point.position,point.rotation);
            LevelVariations Obstaclelevel = levelObject.GetComponent<LevelVariations>();
            newVariations = Obstaclelevel;

            int randAmountOfLevel = UnityEngine.Random.Range(5,30);
            SpawnNumberOfStraightRoad(randAmountOfLevel);
            
        }
        private void SpawnStraightRoad(){
            GameObject levelObject = poolingManager.SpawnFromPool(GetRandomStraightTag(),newVariations.GetNewObstacleSpawnPoint().position,newVariations.GetNewObstacleSpawnPoint().rotation);
            newVariations = levelObject.GetComponent<LevelVariations>();
        }
        private void SpawnRightTurnRoad(){
            if(newVariations.GetLevelTurn().GetTurnType() == TurnType.T_Section){
                for (int i = 0; i < newVariations.GetObstacleSpawnPointArray().Length; i++){
                    GameObject levelObject = poolingManager.SpawnFromPool(GetRandomRightTurnOnlyTag(),newVariations.GetObstacleSpawnPointArray()[i].position,newVariations.GetNewObstacleSpawnPoint().rotation);
                    newVariations = levelObject.GetComponent<LevelVariations>();
                    
                }
            }else{

                GameObject levelObject = poolingManager.SpawnFromPool(GetRandomRightTurnOnlyTag(),newVariations.GetNewObstacleSpawnPoint().position,newVariations.GetNewObstacleSpawnPoint().rotation);
                LevelVariations Obstaclelevel = levelObject.GetComponent<LevelVariations>();
                newVariations = Obstaclelevel;
            }
        }
        private void SpawnLeftTurnRoad(){
            if(newVariations.GetLevelTurn().GetTurnType() == TurnType.T_Section){
                for (int i = 0; i < newVariations.GetObstacleSpawnPointArray().Length; i++){
                    GameObject levelObject = poolingManager.SpawnFromPool(GetRandomLeftTurnOnlyTag(),newVariations.GetObstacleSpawnPointArray()[i].position,newVariations.GetNewObstacleSpawnPoint().rotation);
                    LevelVariations Obstaclelevel = levelObject.GetComponent<LevelVariations>();
                    newVariations = Obstaclelevel;

                }
            }else{
                GameObject levelObject = poolingManager.SpawnFromPool(GetRandomLeftTurnOnlyTag(),newVariations.GetNewObstacleSpawnPoint().position,newVariations.GetNewObstacleSpawnPoint().rotation);
                LevelVariations Obstaclelevel = levelObject.GetComponent<LevelVariations>();
                newVariations = Obstaclelevel;
            }

        }
        private void SpawnT_Section(){
            int rand = UnityEngine.Random.Range(0,tSection.Length);
            GameObject levelObject = poolingManager.SpawnFromPool(tSection[rand],newVariations.GetNewObstacleSpawnPoint().position,newVariations.GetNewObstacleSpawnPoint().rotation);
            LevelVariations Obstaclelevel = levelObject.GetComponent<LevelVariations>();
            newVariations = Obstaclelevel;
        }
        

        public void ToggleCanSpawn(bool value){
            canSpawn = value;
        }

        public void OnSegmentCrossed(){
            levelSegmentCrossed++;
        }
        private PoolObjectTag GetRandomStraightTag(){
            int rand = UnityEngine.Random.Range(0,straingRoadTagArray.Length);
            return straingRoadTagArray[rand];
        }
        private PoolObjectTag GetRandomRightTurnOnlyTag(){
            int rand = UnityEngine.Random.Range(0,leftTurnOnlyTagArray.Length);
            return leftTurnOnlyTagArray[rand];
        }
        private PoolObjectTag GetRandomLeftTurnOnlyTag(){
            int rand = UnityEngine.Random.Range(0,rightTurnOnlyTagArray.Length);
            return rightTurnOnlyTagArray[rand];
        }
        

    }
}
