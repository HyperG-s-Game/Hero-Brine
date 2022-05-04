using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HeroBrine {
    public class LevelDestroyer : MonoBehaviour {
        

        [SerializeField] private LevelVariations variations;

        private void OnTriggerExit(Collider coli){
            if(coli.TryGetComponent<PlayerCollision>(out PlayerCollision player)){
                variations.DestroyMySelfWithDelay();
            }
        }
    }

}