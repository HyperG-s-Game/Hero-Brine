using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HeroBrine {
    public class TurningDirection : MonoBehaviour {
        [SerializeField] private TurnType type;


        public TurnType GetVariationsType(){
            return type;
        }
    }

}