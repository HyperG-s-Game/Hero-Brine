using UnityEngine;
using GamerWolf.Utils;
namespace HeroBrine{
    public enum VariationsType{
        Normal,Spawner,
    }
    public enum TurnType{
        Strainght,RightTurnOnly,LeftTurnOnly,T_Section,
    }
    public class LevelVariations : MonoBehaviour,IPooledObject{

        public bool isUsed;
        [SerializeField] private float lifeTime = 30f;
        [SerializeField] private VariationsType variationsType;
        [SerializeField] private LevelTurn levelTurn;
        [SerializeField] private float leveWidthSize;
        [SerializeField] private Vector3 offset;
        // [SerializeField] private Transform levelWidth;
        [SerializeField] private Transform[] newObstacleSpawnPointArray;
        
        [SerializeField] private Transform playerSpawnPointRight,playerSpawnPointLeft;
        [SerializeField] private int rand;
        // [SerializeField] private LevelTurn turn;
        public Transform GetNewObstacleSpawnPoint(){
            return newObstacleSpawnPointArray[rand];
        }
        public Transform GetPlayerSpawnPointRight(){
            return playerSpawnPointRight;
        }
        public Transform GetPlayerSpawnPointLeft(){
            return playerSpawnPointLeft;
        }
        
        public void DestroyMySelfWithDelay(float delay = 0f){
            Invoke(nameof(DestroyNow),delay);
        }
        
        public void DestroyNow(){
            gameObject.SetActive(false);
            isUsed = false;
        }
        
        public void OnObjectReuse(){
            isUsed = true;
            rand = Random.Range(0,newObstacleSpawnPointArray.Length);
            DestroyMySelfWithDelay(lifeTime);
        }
        public Transform[] GetObstacleSpawnPointArray(){
            return newObstacleSpawnPointArray;
        }
        public LevelTurn GetLevelTurn(){
            return levelTurn;
        }
        private void OnDrawGizmos(){
            Gizmos.color = Color.cyan;
            if(playerSpawnPointLeft != null){
                Gizmos.DrawRay(playerSpawnPointLeft.position + offset,playerSpawnPointLeft.right * leveWidthSize);
                Gizmos.DrawRay(playerSpawnPointLeft.position + offset,(playerSpawnPointLeft.right) * -leveWidthSize);
            }

            if(playerSpawnPointRight != null){
                Gizmos.DrawRay(playerSpawnPointRight.position + offset,playerSpawnPointRight.right * leveWidthSize);
                Gizmos.DrawRay(playerSpawnPointRight.position + offset,(playerSpawnPointRight.right) * -leveWidthSize);
            }
        }

        
    }
    

}