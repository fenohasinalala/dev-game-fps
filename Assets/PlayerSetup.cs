using UnityEngine;
using Mirror;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;
    [SerializeField]
    private string remoteLayerName = "RemotePlayer";
    Camera sceneCamera;

    private void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponents();
            AssingeRemotePlayer();
        }
        else
        {
            sceneCamera = Camera.main;
            if (sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }
            GetComponent<Player>().Setup();
        }        
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        string netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player player = GetComponent<Player>();
        GameManager.RegisteurPlayer(netID, player);
    }

    private void AssingeRemotePlayer(){
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName); 
    }
    private void DisableComponents(){
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }

    private void OnDisable()
    {
        if (isLocalPlayer)
        {
            if (sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(true);
            }
        }

        GameManager.UnregisterPlayer(transform.name);
    }
}


