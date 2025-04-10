using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NETWORK_ENGINE;

[RequireComponent(typeof(Rigidbody))]
public class NetworkRigidBody : NetworkComponent
{
    //Position, velocity, rotation, angular velocity
    public Vector3 LastPosition;
    public Vector3 LastRotation;
    public Vector3 LastVelocity;
    public Vector3 LastAngular;

    //This vector will provide extra velocity on the client to try to compensate for any error in position.
    public Vector3 OffsetVelocity;

    //Minimum and maximum thresholds 
    public float Threshold = .1f;
    public float EThreshold = 2.5f;

    //Rigid body variable.  
    public Rigidbody MyRig;
    public bool UseOffsetVelocity = true;

    public bool ClientRecv = false;

    public override void NetworkedStart()
    {

    }
    //This function will aprse out a vector from Unity's Vector3.ToString format.
    public static Vector3 VectorFromString(string value)
    {
        char[] temp = { '(', ')' };
        string[] args = value.Trim().Trim(temp).Split(',');
        return new Vector3(float.Parse(args[0].Trim()), float.Parse(args[1].Trim()), float.Parse(args[2].Trim()));
    }
    public override void HandleMessage(string flag, string value)
    {
        if(IsClient && !ClientRecv)
        {
            ClientRecv = true;
        }

        if (flag == "POS" && IsClient)  //If position is received.
        {
            LastPosition = VectorFromString(value);  //Parse values.
            float d = (MyRig.position - LastPosition).magnitude;
            //if difference between the two positions is greater than emergency thresholds, Or if offset velocity is disabled, or if velocity is 0.
            if (d > EThreshold || !UseOffsetVelocity  || LastVelocity.magnitude<.1)
            {
                //Clear offset velocity set the position.
                OffsetVelocity = Vector3.zero;
                MyRig.position = LastPosition;
            }
            else if(LastVelocity.magnitude > .1)
            {
               //Otherwise calculate offset velocity.
               OffsetVelocity = (LastPosition - MyRig.position);
            }

        }
        if(flag == "VEL" && IsClient)
        {
            //Update last Velocity -- notice it is not set here.
            LastVelocity = VectorFromString(value);
        }
        if(flag == "ROT" && IsClient)
        {
            //Update rotation
            LastRotation = VectorFromString(value);
            MyRig.rotation = Quaternion.Euler(LastRotation);
        }
        if(flag == "ANG" && IsClient)
        {
            //Update Last angular velocity -- notice it is not set here.
            LastAngular = VectorFromString(value);
        }
    }

    public override IEnumerator SlowUpdate()
    {
        if(IsClient)
        {
            //The client may come in conflict if gravity is applied.
            MyRig.useGravity = false;
            
        }

       while(true)
        {
            if(IsServer)
            {
                //if difference in position is greater than threshold send update.
                if ((LastPosition - MyRig.position).magnitude > Threshold)
                {
                    SendUpdate("POS", MyRig.position.ToString("F3"));
                    LastPosition = MyRig.position;
                }
                //If difference in velocity is greater than threshold send update.
                if ((LastVelocity - MyRig.velocity).magnitude > Threshold)
                {
                    SendUpdate("VEL", MyRig.velocity.ToString("F3"));
                    LastVelocity = MyRig.velocity;
                }
                //If difference in roation is greater than threshold send update.
                if ((LastRotation - MyRig.rotation.eulerAngles).magnitude > Threshold)
                {
                    SendUpdate("ROT", MyRig.rotation.eulerAngles.ToString("F3"));
                    LastRotation = MyRig.rotation.eulerAngles;
                }
                //if the difference in the angualar velocity is greater than threshold send udpate.
                if ((LastAngular - MyRig.angularVelocity).magnitude > Threshold)
                {
                    SendUpdate("ANG", MyRig.angularVelocity.ToString("F3"));
                    LastAngular = MyRig.angularVelocity;
                }
                //If game object is dirty send uall updates mark Is Dirty as false.
                if (IsDirty)
                {
                    SendUpdate("POS", MyRig.position.ToString("F3"));
                    SendUpdate("VEL", MyRig.velocity.ToString("F3"));
                    SendUpdate("ROT", MyRig.rotation.eulerAngles.ToString("F3"));
                    SendUpdate("ANG", MyRig.angularVelocity.ToString("F3"));
                    IsDirty = false;
                }
            }
            /*if(IsClient)
            {
                if( (MyRig.position- LastPosition).magnitude > EThreshold)
                {
                    MyId.NotifyDirty();
                }
            }*/
            yield return new WaitForSeconds(.1f);
        }
    }
    void Start()
    {
        MyRig = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if(IsClient && ClientRecv)
        {
            //Continously update velocity.
            if (LastVelocity.magnitude < .05f)
            {
                //Stop velocity so you do not get drift on the client.
                OffsetVelocity = Vector3.zero;
            }
            if (UseOffsetVelocity)
            {
                //continously set velocity with offset velocity
                MyRig.velocity = LastVelocity + OffsetVelocity;
            }
            else
            {
                //continously set velocity without velocity.
                MyRig.velocity = LastVelocity;
            }
            //Continously update angular velocity.
            MyRig.angularVelocity = LastAngular;
        }
    }
}
