using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroBrine {
    
    public class LevelTurn : MonoBehaviour{
        [SerializeField] private TurnType variationsType;

        // [SerializeField] private Transform CurrentDirectionPoint;
        // public void SetDirectionPoint(Transform point){
        //     CurrentDirectionPoint = point;
        // }
        public TurnType GetTurnType(){
            return variationsType;
        }
        
    }
}
