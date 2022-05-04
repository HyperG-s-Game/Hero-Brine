using UnityEngine;
using GamerWolf.Utils;

namespace HeroBrine {
    public class PlayerCollision : MonoBehaviour {

        [SerializeField] private int obstacleLayerNumber = 9;
        [SerializeField] private PlayerController playerController;
        private void OnCollisionEnter(Collision coli){
            if(coli.gameObject.layer == obstacleLayerNumber){
                playerController.CheckBump();
                // CinemachineScreenShakeManager.current.Shake(5f,0.1f);
                return;
            }
            if(coli.transform.CompareTag("Obstacle")){
                GameHandler.current.GameOver();
                // ToggleInput (false);
            }
        }
        private void OnCollisionStay(Collision coli){

            if(coli.transform.CompareTag("Obstacle")){
                GameHandler.current.GameOver();
                // CinemachineScreenShakeManager.current.Shake(5f,0.1f);
                // ToggleInput (false);
            }
        }
        private void OnTriggerStay(Collider coli){
            if(coli.transform.CompareTag("Obstacle")){
                GameHandler.current.GameOver();
                CinemachineScreenShakeManager.current.Shake(5f,0.1f);
            }

            if(coli.transform.CompareTag("Hazard")){
                GameHandler.current.GameOver();
                return;
            }
            LevelVariations variations = coli.GetComponent<LevelVariations>();
            if(variations != null){
                playerController.SetCurrentLevelVariation(variations);
                
            }
            if(coli.transform.CompareTag("Turn")){
                if(coli.TryGetComponent<LevelTurn>(out LevelTurn turn)){
                    playerController.SetCurrentTurn(turn);
                }
            }
        }
        private void OnTriggerEnter(Collider coli){
            if(coli.transform.CompareTag("Hazard")){
                GameHandler.current.GameOver();
                return;
            }
            LevelVariations variations = coli.GetComponent<LevelVariations>();
            if(variations != null){
                playerController.SetCurrentLevelVariation(variations);
                
            }
            if(coli.transform.CompareTag("Turn")){
                if(coli.TryGetComponent<LevelTurn>(out LevelTurn turn)){
                    playerController.SetCurrentTurn(turn);
                }
            }
            
        }
        private void OnTriggerExit(Collider coli){
            if(coli.transform.GetComponent<LevelVariations>()){
                playerController.SetCurrentLevelVariation(null);
            }
            if(coli.transform.CompareTag("Turn")){
                playerController.SetCurrentTurn(null);
                LevelManager.current.OnSegmentCrossed();
            }
            

        }
        public void Bump(){
            playerController.CheckBump();
        }
        
    }

}