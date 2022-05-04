using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
[DefaultExecutionOrder(-1)]
public class PlayerSwipeController : MonoBehaviour {
    

    #region Evens.........

    public delegate void StartTouch(Vector3 postion,float time);
    public event StartTouch onStartTouch;
    public delegate void EndTouch(Vector3 postion,float time);
    public event EndTouch onEndTouch;
    
    #endregion
    
    [SerializeField] private Camera cam;
    private PlayerTouch playertouch;

    private void OnEnable(){
        playertouch.Enable();
    }
    private void OnDisable(){
        playertouch.Disable();
    }

    private void Awake(){
        playertouch = new PlayerTouch();
    }
    private void Start (){
        playertouch.Touch.PrimaryContact.started += ctx => StartTouchPrimary(ctx);
        playertouch.Touch.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);


    }

    

    private void StartTouchPrimary(InputAction.CallbackContext context){
        if(onStartTouch != null){
            onStartTouch(GetWorldScreenPoint(playertouch.Touch.primaryPostion.ReadValue<Vector2>()),(float)context.startTime);

        }
    }

    private void EndTouchPrimary(InputAction.CallbackContext context){
        if(onEndTouch != null){
            onEndTouch(GetWorldScreenPoint(playertouch.Touch.primaryPostion.ReadValue<Vector2>()),(float)context.time);
        }
    }

    

    private Vector3 GetWorldScreenPoint(Vector3 positon){
        Vector3 mousePos =  new Vector3(positon.x,positon.y,cam.nearClipPlane);
        return mousePos;
    }
    

}
