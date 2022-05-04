using System;
using UnityEngine;
using GamerWolf.Utils;
namespace HeroBrine {
    public enum HorizontalPosition{Right,Mid,Left}
    public class PlayerController : MonoBehaviour {

        [Header("Testing")]
        [SerializeField] private bool enableInputs;
        
        [Header("Movement")]
        [SerializeField] private float forwardMoveSpeed = 20f;
        [SerializeField] private HorizontalPosition currentHorizontalPositon;
        [SerializeField] private float direcitonMoveThreshold = 0.1f;
        
        [Header("Component Reference")]
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private PlayerMover mover;
        [SerializeField] private PlayerSwipeController swipeController;
        [SerializeField] private PlayerAnimationController animationController;
        [SerializeField] private PlayerCollision playerCollision;

        #region Private Variables.......................................
        private LevelTurn currentTurn;
        private LevelVariations currentLevelVariation;
        private Vector3 smoothMoveReference;
        private Vector2 swipeStartPostion,swipeEndPositon;
        private float currentRot = 0f;
        public int bumpCount = 1;
        private bool canCheckBump;
        public Action OnPlayerBump;
        public event EventHandler OnPlayerTurn;
        

        #endregion


        #region Initializing Input Scripts....................
        
        private void OnEnable(){
            swipeController.onStartTouch += SwipeStart;
            swipeController.onEndTouch += SwipeEnd;
        }

        private void OnDisable(){
            swipeController.onStartTouch -= SwipeStart;
            swipeController.onEndTouch -= SwipeEnd;
        }
        #endregion


        private void Start(){
            GameHandler.current.OnGameOver += OnGameOverEvent;
            currentRot = transform.eulerAngles.y;
            currentHorizontalPositon = HorizontalPosition.Mid;
            mover.ChangeLane();
            mover.SetLanePosition(currentHorizontalPositon);
            RotateCorrectly(Vector3.zero);
        }
        private void Update(){
            mover.ApplyGraivity();
            if(enableInputs){
                if(Input.GetKeyDown(KeyCode.UpArrow)){
                    mover.JumpWithController();
                }else if(Input.GetKeyDown(KeyCode.DownArrow)){
                    mover.Slide();
                }
                
                // Move();
                mover.ChangeLane();
                animationController.Slide(mover.GetIsSliding());
                animationController.SetGroundedValue(mover.IsGrounded());
                animationController.Jump(mover.GetIsjumping());

            }
        }
        private void FixedUpdate(){
            if(enableInputs){
            // mover.ApplyGraivity();
                Move();
            }
        }

        private void OnGameOverEvent(object sender,EventArgs args){
            animationController.Run(false);
            animationController.Caught();
        }
        private void Move(){
            transform.Translate(Vector3.forward * forwardMoveSpeed * Time.deltaTime);
        }
        private void SwipeLeft(){

            mover.TurnInvoke(false);
            Debug.Log("Swipedleft");
            if(currentTurn == null){
                if(currentHorizontalPositon == HorizontalPosition.Left){
                    CheckBump();
                    return;
                }
            }

            if(currentLevelVariation != null && currentTurn != null){

                if(currentTurn.GetTurnType() == TurnType.LeftTurnOnly || 
                    currentTurn.GetTurnType() == TurnType.T_Section){
                    if(currentTurn.GetTurnType() == TurnType.T_Section){
                        levelManager.OnTSectionTurn?.Invoke(currentLevelVariation.GetPlayerSpawnPointLeft());
                    }
                    if(currentHorizontalPositon == HorizontalPosition.Left){
                        
                        Turn90Left();
                        InvokeOnTurn();
                    }
                }

            }
            if(currentHorizontalPositon == HorizontalPosition.Right){
                currentHorizontalPositon = HorizontalPosition.Mid;
            }
            else if(currentHorizontalPositon == HorizontalPosition.Mid){
                currentHorizontalPositon = HorizontalPosition.Left;
            }

        }
        private void InvokeOnTurn(){
            OnPlayerTurn?.Invoke(this,EventArgs.Empty);
        }
        
        public void CheckBump(){
            Debug.Log("Bumped!");
            if(bumpCount >= 1){
                
                OnPlayerBump?.Invoke();
                bumpCount--;
            }else{
                GameHandler.current.GameOver();
            }
            CinemachineScreenShakeManager.current.Shake(5f,0.1f);
        }
        
        private void ResetBumpCheck(){

            canCheckBump = true;
        }
        private void SwipeRight(){

            Debug.Log("SwipedRight");
            mover.TurnInvoke(true);
            if(currentTurn == null){
                if(currentHorizontalPositon == HorizontalPosition.Right){
                    CheckBump();
                    return;
                }
            }
            if(currentLevelVariation != null && currentTurn != null){
                
                if(currentTurn.GetTurnType() == TurnType.RightTurnOnly || 
                    currentTurn.GetTurnType() == TurnType.T_Section){
                    if(currentTurn.GetTurnType() == TurnType.T_Section){
                        levelManager.OnTSectionTurn?.Invoke(currentLevelVariation.GetPlayerSpawnPointRight());
                    }
                    if(currentHorizontalPositon == HorizontalPosition.Right){
                        Turn90Right();
                        InvokeOnTurn();
                    }
                }

            }
            if(currentHorizontalPositon == HorizontalPosition.Left){
                currentHorizontalPositon = HorizontalPosition.Mid;
            }
            else if(currentHorizontalPositon == HorizontalPosition.Mid){
                currentHorizontalPositon = HorizontalPosition.Right;
            
            }
            
        }
        public void RestBumpAmount(){
            bumpCount = 1;
        }
        
        public void Turn90Right(){
            currentRot += 90f;
            
            RotateCorrectly(currentLevelVariation.GetPlayerSpawnPointRight().position);

        }
        public void Turn90Left(){
            currentRot -= 90;
            RotateCorrectly(currentLevelVariation.GetPlayerSpawnPointLeft().position);
        }

        private void RotateCorrectly(Vector3 point){
            transform.eulerAngles = new Vector3(transform.eulerAngles.x,currentRot,transform.eulerAngles.z);
            if(currentLevelVariation != null){
                transform.position = new Vector3(point.x,transform.position.y,point.z);
            }
            
        }
        private void SwipeStart(Vector3 positon,float time){
            swipeStartPostion = positon;
        }
        private void SwipeEnd(Vector3 position,float time){
            swipeEndPositon = position;
            DetectSwipe();
        }
        
        private void DetectSwipe(){
            if(enableInputs){
                SwipeDirection(swipeStartPostion,swipeEndPositon);
                
            }
        }

        private void SwipeDirection(Vector3 first,Vector3 end){
            if(Mathf.Abs(end.x - first.x) > direcitonMoveThreshold || Mathf.Abs(end.y - first.y) > direcitonMoveThreshold){
                if(Mathf.Abs(end.x - first.x) > Mathf.Abs(end.y - first.y)){
                    if(end.x > first.x){
                        
                        SwipeRight();
                    }else{
                        SwipeLeft();

                    }
                }else {
                    if(end.y > first.y){
                        mover.JumpWithPhysics();
                        // mover.JumpWithController();

                        animationController.Run(false);
                    }else{
                        mover.Slide();
                        animationController.Run(false);
                    }
                }
            }
            
            mover.SetLanePosition(currentHorizontalPositon);
        }
        public void ToggleInput(bool value){
            enableInputs = value;
            animationController.Run(value);
        }
        public bool GetInput(){
            return enableInputs;
        }
        public void SetCurrentLevelVariation(LevelVariations variations){
            currentLevelVariation = variations;
           
        }
        public void SetCurrentTurn(LevelTurn currentTurn){
            this.currentTurn = currentTurn;
        }
    }

}