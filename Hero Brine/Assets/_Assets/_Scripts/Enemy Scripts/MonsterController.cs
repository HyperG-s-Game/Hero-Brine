using UnityEngine;

namespace HeroBrine{
    public class MonsterController : MonoBehaviour {

        [SerializeField] private PlayerController playerController;
        [SerializeField] private float playerCatchMoveSpeed;
        [SerializeField] private float followSpeedOnScreen = 5f,followSpeedOffScreen = 2f;
        [SerializeField] private float onScreenTimerMax = 5f;
        [SerializeField] private float rotaionSmoothSpeed = 2f;

        [SerializeField] private Transform playerPoint;
        [SerializeField] private Transform onScreenPoint;
        [SerializeField] private Transform offScreenPoint;
        [SerializeField] private EnemyAnimationHandler monsterAnimationHandler;
        [SerializeField] private bool catchPlayer;
        [SerializeField] private bool moveCloseBy;



        private float currentYrot;
        private float rotaionSmoothTime;
        private float currentScreenTime;
        private bool alreadyCaughtPlayer;
        private void Start(){
            currentScreenTime = onScreenTimerMax;
            playerController.OnPlayerBump += PlayerBumpEvent;
        }
        private void PlayerBumpEvent(){
            moveCloseBy = true;
            // currentScreenTime = onScreenTimerMax;
        }
        

        private void LateUpdate(){
            if(!catchPlayer){
                transform.eulerAngles = new Vector3(transform.eulerAngles.x,currentYrot,transform.eulerAngles.z);
            }
        }
        private void Update(){
            if(!catchPlayer){
                alreadyCaughtPlayer = false;
                monsterAnimationHandler.StartMove(true);
                if(moveCloseBy){
                    
                    currentYrot = Mathf.SmoothDamp(currentYrot,onScreenPoint.eulerAngles.y,ref rotaionSmoothTime,rotaionSmoothSpeed * Time.deltaTime);
                    monsterAnimationHandler.gameObject.SetActive(true);
                    if(Vector3.Distance(transform.position,onScreenPoint.position) > 0.01f){
                        transform.position = Vector3.MoveTowards(transform.position,onScreenPoint.position, followSpeedOnScreen * Time.deltaTime);
                    }
                    if(currentScreenTime <= 0f){
                        if(moveCloseBy){
                            currentScreenTime = onScreenTimerMax;
                            moveCloseBy = false;
                            playerController.RestBumpAmount();
                        }
                    }else{
                        currentScreenTime -= Time.deltaTime;
                    }
                }else{
                    currentYrot = Mathf.SmoothDamp(currentYrot,offScreenPoint.eulerAngles.y,ref rotaionSmoothTime,rotaionSmoothSpeed * Time.deltaTime);
                    if(Vector3.Distance(transform.position,offScreenPoint.position) > 0.1f){
                        
                        transform.position = Vector3.MoveTowards(transform.position,offScreenPoint.position, followSpeedOffScreen * Time.deltaTime);
                    }
                    if(Vector3.Distance(transform.position,offScreenPoint.position) >= 4f){
                        monsterAnimationHandler.gameObject.SetActive(true);
                    }else{
                        monsterAnimationHandler.gameObject.SetActive(false);
                    }
                    
                }
            }else{
                monsterAnimationHandler.gameObject.SetActive(true);
                if(Vector3.Distance(transform.position,playerPoint.position) >= 0.1f){
                    transform.position = Vector3.MoveTowards(transform.position,playerPoint.position,playerCatchMoveSpeed * Time.deltaTime);
                }else{
                    monsterAnimationHandler.transform.localEulerAngles = new Vector3(monsterAnimationHandler.transform.localEulerAngles.x,180f,monsterAnimationHandler.transform.localEulerAngles.z);
                }

            }
        }
        
        public void CatchPlayer(bool value){
            catchPlayer = value;
            if(catchPlayer){
                monsterAnimationHandler.StartMove(false);
                monsterAnimationHandler.Victory();
            }
        }



        
    }

}