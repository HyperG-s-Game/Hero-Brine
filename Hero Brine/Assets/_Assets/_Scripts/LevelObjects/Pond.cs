using UnityEngine;
using GamerWolf.Utils;
using System.Collections;
using System.Collections.Generic;
namespace HeroBrine {
    public class Pond : MonoBehaviour {
        
        
        private void OnTriggerEnter(Collider coli){
            if(coli.transform.TryGetComponent<PlayerCollision>(out PlayerCollision player)){
                player.Bump();
                GetComponent<Collider>().enabled = false;
                ObjectPoolingManager.current.SpawnFromPool(PoolObjectTag.puddel_Fall_Effect,player.transform.position,Quaternion.identity);
            }
            
        }
    }

}