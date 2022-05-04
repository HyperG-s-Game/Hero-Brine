using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HeroBrine {
    public class BumpManager : MonoBehaviour {
        


        private void OnTriggerEnter(Collider coli){
            if(coli.transform.TryGetComponent<PlayerCollision>(out PlayerCollision player)){
                player.Bump();
                GetComponent<Collider>().enabled = false;
            }
        }
    }
}
