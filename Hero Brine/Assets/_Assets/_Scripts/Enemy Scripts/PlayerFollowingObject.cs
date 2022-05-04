using UnityEngine;

namespace HeroBrine {
    public class PlayerFollowingObject : MonoBehaviour{
        [SerializeField] private Transform followObject;
        private void Update(){
            // transform.position = followObject.position;
            // transform.rotation = followObject.rotation;
        }
    }

}