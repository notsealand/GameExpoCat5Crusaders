using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

public class Melee : NetworkComponent
{
    public GameObject melee;
    public GameObject parentPlayer;

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
                //parentPlayer = melee.GetComponentInParent<Transform>();
                //Debug.Log(parentPlayer.ToString);
                if(IsDirty)
                {
                    IsDirty = false;
                }
            }
        }
        yield return new WaitForSeconds(.1f);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && IsServer && other.gameObject != parentPlayer)
        {
            Debug.Log("Collision With Player");
            SendUpdate("GOTAHIT", "-1");
            //melee.SetActive(false);
            yield return new WaitForSeconds(20);
            melee.SetActive(true);
        }
    }
}
