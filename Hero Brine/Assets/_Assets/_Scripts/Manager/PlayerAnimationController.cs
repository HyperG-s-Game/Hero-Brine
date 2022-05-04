using UnityEngine;


namespace HeroBrine {
    public class PlayerAnimationController : MonoBehaviour {

        private Animator animator;
        private void Awake(){
            animator = GetComponent<Animator>();
        }
        private void Start(){
            animator.applyRootMotion = true;
        }
        public void SetGroundedValue(bool value){
            animator.SetBool("isGrounded",value);
            // animator.applyRootMotion = false;
        }

        public void Run(bool value){
            animator.applyRootMotion = false;
            animator.SetBool("run",value);
        }
        public void Jump(bool value){
            animator.applyRootMotion = false;
            animator.SetBool("isJumping",value);
        }
        public void Slide(bool value){
            animator.applyRootMotion = false;
            animator.SetBool("Slide",value);
        }
        public void Caught(){
            Run(false);
            Jump(false);
            Slide(false);
            animator.applyRootMotion = false;
            animator.SetTrigger("Caught");
        }




    }

}