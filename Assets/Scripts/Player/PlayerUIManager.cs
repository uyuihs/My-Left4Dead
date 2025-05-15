using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour {
   public static PlayerUIManager Singleton;
   [HideInInspector] public PlayerUIHUDManager playerUIHUDManager;
   
   private void Awake() {
       if (Singleton == null) {
           Singleton = this;
       } else {
           Destroy(gameObject);
       }
       playerUIHUDManager = GetComponentInChildren<PlayerUIHUDManager>();
   }

    private void Start(){
        DontDestroyOnLoad(gameObject);
    }

    

}
