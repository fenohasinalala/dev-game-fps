using UnityEngine;
using Mirror;

public class PlayerShoot : NetworkBehaviour 
{

    public PlayerWeapon weapon;
    [SerializeField]
    private LayerMask mask;
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private LineRenderer line;
    void Start()
    {
        if (cam == null)
        {
            Debug.LogError("not have camera");
            this.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            shoot();
        }
    }
    [Client]
    private void shoot (){
        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, weapon.range, mask)){
            if (hit.collider.tag == "Player")
            {
                CmdPlayerShoot(hit.collider.name, weapon.damage);
            }
        }
        LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
        lineRenderer.enabled = true;
        Invoke("ViewFire",0.2f);
        //lineRenderer.enabled = false;
    }

    private void ViewFire (){
        LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    [Command]
    private void CmdPlayerShoot(string playerId, float damage){
        Debug.Log(playerId + "a été toucher");
        Player player = GameManager.GetPlayer(playerId);
        player.RpcTakeDamage(damage);

    }
}