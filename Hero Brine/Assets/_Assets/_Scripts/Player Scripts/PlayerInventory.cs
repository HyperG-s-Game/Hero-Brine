using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HeroBrine {
    public class PlayerInventory : MonoBehaviour {
        

        [SerializeField] private int totalCoinAmount;

        public void AddCoinAmount(){
            totalCoinAmount++;
        }
        
    }
}
