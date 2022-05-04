using UnityEngine;
using GamerWolf.Utils;
using System.Collections;
using System.Collections.Generic;

namespace HeroBrine {
    public class Coins : MonoBehaviour {
        private void OnTriggerEnter(Collider coli){
            if(coli.transform.TryGetComponent<PlayerInventory>(out PlayerInventory player)){
                ObjectPoolingManager.current.SpawnFromPool(PoolObjectTag.coin_Effect,transform.position,Quaternion.identity);
                player.AddCoinAmount();
                gameObject.SetActive(false);
            }
        }
        
    }

}