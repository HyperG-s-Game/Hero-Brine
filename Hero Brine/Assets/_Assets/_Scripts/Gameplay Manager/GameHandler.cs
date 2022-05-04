using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using GamerWolf.Utils;

namespace HeroBrine {
    public class GameHandler : MonoBehaviour {

        [SerializeField] private UnityEvent onGameStart;
        [SerializeField] private UnityEvent onGamePlaying;
        [SerializeField] private UnityEvent onGameEnd,afterEndDelayEvent;

        [SerializeField] private bool isGamePlaying,isGameEnd;


        public event EventHandler OnGameOver;

        #region Singelton...........
        public static GameHandler current;
        private void Awake(){
            if(current == null){
                current = this;
            }
        }


        #endregion

        private void Start(){
            isGameEnd = false;
            isGamePlaying = false;
            StartCoroutine(nameof(GameStartRoutine));
        }
        private IEnumerator GameStartRoutine(){
            onGameStart?.Invoke();
            while(!isGamePlaying){
                yield return null;
            }
            yield return StartCoroutine(nameof(GamePlayingRoutine));
        }
        private IEnumerator GamePlayingRoutine(){
            onGamePlaying?.Invoke();

            while(!isGameEnd){
                Debug.Log("Game Playing");
                yield return null;
            }
            onGameEnd?.Invoke();
            
            yield return new WaitForSeconds(2f);
            afterEndDelayEvent?.Invoke();

        }


        public void LetsPlay(){
            isGamePlaying = true;
        }
        public void GameOver(){
            Debug.Log("Game Over");
            isGameEnd = true;
            OnGameOver?.Invoke(this,EventArgs.Empty);
        }
        public void Restart(){
            if(LevelLoader.instance != null){
                LevelLoader.instance.PlayNextLevel();
            }else{
                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            }
        }

    }
}
