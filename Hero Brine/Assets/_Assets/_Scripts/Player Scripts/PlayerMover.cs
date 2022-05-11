using UnityEngine;
using System.Collections;
namespace HeroBrine {
    public class PlayerMover : MonoBehaviour{
        
        

        [Header("Movement")]
        [SerializeField] private float moveToPos = -2;
        [SerializeField] private float sideWaysMoveSpeed = 30f,rotationSmoothTime = 3f;
        
        [Space(20)]
        [Header("Ground Check")]
        // [SerializeField] private AnimationCurve jumpFallOff;
        // [SerializeField] private float jumpMultiplier;
        [SerializeField] private float fallMultiplier = 20f;
        [SerializeField] private float lowJumpMultiplier = 2f;
        [SerializeField] private float gravity = -9.8f;
        [SerializeField] private float jumpHeight = 120f;

        [SerializeField] private float groundcheckRadius = 0.5f;
        [SerializeField] private Vector3 groundCheckOffset;
        [SerializeField] private LayerMask groundLayer;

        [Header("Sliding")]
        [SerializeField] private float maxSlideTime = 2;
        // [SerializeField] private CapsuleCollider m_collider;
        [SerializeField] private Transform bodyGFX;
        [SerializeField] private Vector3 newSlideOffset,newColliderCenterSlideOffset;
        [SerializeField] private float newSlideColliderHeight,newColliderRadius;

        private Vector3 orignalColiOffset,orignalColliderCenterOffset;
        private float orgignalColiHeight,orignalColiRadius;
        private Vector3 currentPositon;
        private HorizontalPosition currentHorizontalPositon;
        private float currentX;
        private Vector3 velocity;
        private Rigidbody rb;
        private CapsuleCollider m_collider;
        // private CharacterController controller;
        
        private Coroutine SlideRoutine;
        private float newYRot;
        private Vector3 currentRot;
        private float rotSmoothTime;
        private bool isSliding;
        private bool isJumping;
        [SerializeField] private bool isMoveing;
        
        private void Awake(){
            rb = GetComponent<Rigidbody>();
            m_collider = GetComponent<CapsuleCollider>();
        }
        private void OnValidate(){
            if(gravity >= -1){
                gravity = -1f;
            }
        }

        private void Start(){
            orgignalColiHeight = m_collider.height;
            orignalColiOffset = m_collider.center;
            orignalColiRadius = m_collider.radius;
            currentX = 0f;
            currentPositon = transform.localPosition;
        }
        public bool IsGrounded(){
            return Physics.CheckSphere(transform.position + groundCheckOffset,groundcheckRadius,groundLayer);
        }

        private void LateUpdate(){
            if(newYRot != 0){
                newYRot = Mathf.SmoothDamp(newYRot,0f,ref rotSmoothTime,rotationSmoothTime * Time.deltaTime);
                bodyGFX.localEulerAngles = new Vector3(bodyGFX.localEulerAngles.x,newYRot,bodyGFX.localEulerAngles.z);
            }
            
        }
        
        public void ChangeLane(){
            switch(currentHorizontalPositon){
                case HorizontalPosition.Right:
                    currentX = moveToPos;
                break;

                case HorizontalPosition.Mid:
                    currentX = 0;
                break;

                case HorizontalPosition.Left:
                    currentX = -moveToPos;
                break;
            }
            if(Vector3.Distance(transform.localPosition,new Vector3(currentX,transform.localPosition.y,0f)) > 0.01f){
                isMoveing = true;
                transform.localPosition = Vector3.MoveTowards(transform.localPosition,new Vector3(currentX,transform.localPosition.y,0f) ,sideWaysMoveSpeed * Time.deltaTime);
            }else{
                isMoveing = false;
            }
        }
        public void SetLanePosition(HorizontalPosition _setPosition){
            currentHorizontalPositon = _setPosition;
        }
        public void TurnInvoke(bool right){
            if(right){
                newYRot = 30f;
            }else{
                newYRot = -30f;
            }
        }
        
        public void ApplyGraivity(){
            if(rb.velocity.y < 0f){
                rb.velocity += Vector3.up * gravity * (fallMultiplier - 1) * Time.deltaTime;

            }else if(rb.velocity.y > 0){
                rb.velocity += Vector3.up * gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }

        
        
        
        public void JumpWithPhysics(){
            if(IsGrounded()){
                isJumping = true;
                CancelInvoke(nameof(ResetJump));
                Invoke(nameof(ResetJump),1f);
                Debug.Log("Jump");
                rb.velocity = Vector3.zero;
                rb.AddForce(Vector3.up * jumpHeight,ForceMode.Impulse);
                if(SlideRoutine != null){
                    StopCoroutine(SlideRoutine);
                }
            }
        }
        public void JumpWithController(){
            if(!isJumping){
                isJumping = true;
                Debug.Log("Jump");
                CancelInvoke(nameof(ResetJump));
                Invoke(nameof(ResetJump),1f);
                if(IsGrounded()){
                    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                }
                if(SlideRoutine != null){
                    StopCoroutine(SlideRoutine);
                }
            }
        }
        // private IEnumerator JumpEventRoutine(){
        //     float timeInAir = 0.0f;

        //     do{
        //         float jumpForce = jumpFallOff.Evaluate(timeInAir);
        //         controller.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);
        //         timeInAir += Time.deltaTime;
        //         transform.position = new Vector3(transform.position.x,transform.position.y,0f);
        //         yield return null;

        //     } while (!IsGrounded() && controller.collisionFlags != CollisionFlags.Above);
        //     isJumping = false;

        // }
        private void ResetJump(){
            isJumping = false;
        }
        public bool GetIsjumping(){
            return isJumping;
        }

        
        public void Slide(){
            if(isJumping){
            
                Debug.Log("Hard Down");
                // controller.Move(Vector3.down * 50f * Time.deltaTime);
                rb.AddForce(Vector3.down * 20f,ForceMode.Impulse);
            }
            isSliding = true;
            m_collider.center = newColliderCenterSlideOffset;
            m_collider.height = newSlideColliderHeight;
            m_collider.radius = newColliderRadius;
            if(SlideRoutine == null){
                SlideRoutine = StartCoroutine(RestSlideRoutine());
            }else{
                StopCoroutine(SlideRoutine);
                SlideRoutine = StartCoroutine(RestSlideRoutine());
            }
            SlideRoutine = StartCoroutine(RestSlideRoutine());
            

        }
        private IEnumerator RestSlideRoutine(){
            yield return new WaitForSeconds(maxSlideTime);
            isSliding = false;
            m_collider.center = orignalColiOffset;
            m_collider.height = orgignalColiHeight;
            m_collider.radius = orignalColiRadius;
        }
        public bool GetIsSliding(){
            return isSliding;
        }
        public bool GetIsMoving(){
            return isMoveing;
        }
        private void OnDrawGizmos(){
            Gizmos.DrawWireSphere(transform.position + groundCheckOffset,groundcheckRadius);
        }

        
        
        
    }


    
}
