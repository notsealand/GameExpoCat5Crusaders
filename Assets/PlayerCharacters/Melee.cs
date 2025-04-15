using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

public class Melee : NetworkComponent
{
    public GameObject melee;
    public GameObject myPlayer;
    public Rigidbody MyRig;
    public GameObject gameMaster;

    public override void HandleMessage(string flag, string value)
    {
        switch(flag)
        {
            case "GOTAHIT":
                if(IsServer)
                {

                }
                if(IsClient)
                {

                }
                break;
        }
    }

    public override void NetworkedStart()
    {
        
    }

    public override IEnumerator SlowUpdate()
    {
        while(IsConnected)
        {

            if(IsServer)
            {
                //myPlayer = melee.GetComponentInParent<Transform>();
                //Debug.Log(myPlayer.ToString);
                if(IsDirty)
                {
                    IsDirty = false;
                }
            }
            yield return new WaitForSeconds(.1f);
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        MyRig = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(myPlayer != null)
        {
            MyRig.velocity = new Vector3(myPlayer.GetComponent<Player>().LastX, MyRig.velocity.y, myPlayer.GetComponent<Player>().LastY) * myPlayer.GetComponent<Player>().Speed;
            MyRig.position = new Vector3(myPlayer.GetComponent<Player>().PosX + myPlayer.GetComponent<Player>().LastX, 0, myPlayer.GetComponent<Player>().PosZ + myPlayer.GetComponent<Player>().LastY);
            myPlayer.GetComponent<Player>().meleeReload = melee;
        } else if (gameMaster.GetComponent<GameMaster>().elapsedTime > 2)
        {
            MyCore.NetDestroyObject(melee.GetComponent<NetworkID>().NetId);
        }
    }
        

    /*public IEnumerator OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && IsServer && other.gameObject != myPlayer)
        {
            Debug.Log("Collision With Player");
            SendUpdate("GOTAHIT", "-1");
            //private gameObject temp = myPlayer;
            melee.SetActive(false);
            yield return new WaitForSeconds(3);
            Debug.Log("Collision Finish");
            melee.SetActive(true);
            //myPlayer = temp;
        }
    }*/
}
