using UnityEngine;
using Cinemachine;
namespace HeroBrine {
    public class CameraManager : MonoBehaviour {
        
        [SerializeField] private float changeViewDelay = 1f;
        [SerializeField] private CinemachineVirtualCamera gameplayView,sideView;
        public void ChangeToGamePlayView(){
            Invoke(nameof(ChangeViewWithDelay),changeViewDelay);
        }
        private void ChangeViewWithDelay(){
            gameplayView.m_Priority = 5;
            sideView.m_Priority = 1;
        }
        public void StopFollow(){
            gameplayView.m_LookAt = null;
            gameplayView.m_Follow = null;
        }
    }

}