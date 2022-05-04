using UnityEngine;
using Cinemachine;

namespace GamerWolf.Utils {
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CinemachineScreenShakeManager : MonoBehaviour {

        [SerializeField] private CinemachineScreenShakePropertiensSO propertiensSO;
        private CinemachineVirtualCamera virtualCamera;
        private float startingIntensity,shakeTimeTotal;
        private float shakeTimer;

        #region Singleton......
        public static CinemachineScreenShakeManager current{get;private set;}
        private void Awake(){
            if(current == null){
                current = this;
            }else
            {
                Destroy(current.gameObject);
            }
            virtualCamera = GetComponent<CinemachineVirtualCamera>();
        }
        #endregion

        [ContextMenu("Shake Camera")]
        public void Shake(float intensity,float time){
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
            shakeTimer = time;
            shakeTimeTotal = time;
            startingIntensity = intensity;

        }
        public void Shake(CinemachineScreenShakePropertiensSO _propertiensSO){
            Shake(_propertiensSO.intensity,_propertiensSO.time);
        }

        private void Update(){
            if(shakeTimer > 0){
                shakeTimer -= Time.deltaTime;
                if(shakeTimer <= 0){
                    CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                    cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity,0f,(1 - (shakeTimer / shakeTimeTotal)));
                }
            }
        }
    }

}