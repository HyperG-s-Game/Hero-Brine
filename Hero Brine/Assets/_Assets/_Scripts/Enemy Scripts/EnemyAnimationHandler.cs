using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HeroBrine {
    [RequireComponent(typeof(Animator))]
    public class EnemyAnimationHandler : MonoBehaviour {
        

        private Animator animator;
        private void Awake(){
            animator = GetComponent<Animator>();
        }

        public void StartMove(bool value){
            animator.SetBool("run",value);
        }
        public void Victory(){
            animator.SetTrigger("victory");
        }
    }

}