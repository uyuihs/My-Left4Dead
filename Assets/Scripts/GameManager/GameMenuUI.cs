using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class GameMenuUI : NetworkBehaviour {
    [SerializeField] private Button ClientBtn;
    [SerializeField] private Button HostBtn;
    [SerializeField] private Button ServerBtn;
    [SerializeField] private Camera cam; 
    [SerializeField] private GameObject playerCamera;

    private void Start() {
        ClientBtn.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
            BottomHalfButtons();
        });
        HostBtn.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
            BottomHalfButtons();
        });
        ServerBtn.onClick.AddListener(() => {
            NetworkManager.Singleton.StartServer();
            BottomHalfButtons();
        });
    }

    private void BottomHalfButtons(){
        Cursor.lockState = CursorLockMode.Locked;
        Destroy(cam.gameObject);
        playerCamera.SetActive(true);
        Destroy(ClientBtn.gameObject);
        Destroy(HostBtn.gameObject);
        Destroy(ServerBtn.gameObject);
    }
}
