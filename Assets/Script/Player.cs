using UnityEngine;
using Mirror;
using System.Collections;

public class Player : NetworkBehaviour
{
    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get {return _isDead;}
        protected set { _isDead = value;}
    }
    [SerializeField]
    private float maxHealth = 100f;
    [SyncVar]
    private float curantHealth;
    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnableOnStart;

    public void Setup() {
        wasEnableOnStart = new bool[disableOnDeath.Length];
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            wasEnableOnStart[i] = disableOnDeath[i].enabled;
        }
        setDefaults();
    }
    private void setDefaults(){
        isDead = false;
        curantHealth = maxHealth;
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnableOnStart[i];
        }
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = true;
        }
    }
    private IEnumerator Respawn(){
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTimer);
        setDefaults();
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
    }
    private void Update() {
        if (!isLocalPlayer)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            RpcTakeDamage(9999f);
        }
    }
    [ClientRpc]
    public void RpcTakeDamage(float amount){
        if (isDead)
        {
            return;
        }
        curantHealth -= amount;
        Debug.Log("la vie de " + transform.name + " est maitenant " + curantHealth + " poit de vie"); 
        if (curantHealth <= 0)
        {
            Die();
        }
    }

    private void Die(){
        isDead = true;
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }
                Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }
        Debug.Log(transform.name +  " a ete eliminer");
        StartCoroutine(Respawn());
    }
}
