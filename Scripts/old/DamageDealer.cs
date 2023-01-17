using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DamageDealer : MonoBehaviour
{
    //This must be on child object, like emptry transform, so that it will only use that collider
    //This script is placed on weapons or anything that does damage. Just enable/disable colliders when necessary.
    //this script sends message with damage values to colliders with correct tag.

    //Flags enable picking multiple enums, but don't pick all.
    //Pick all tags that are targets for damage. NOTE! Doesn't work, if everything is selected. Not sure why.
    [System.Flags]
    public enum Targets
    {
        //!!! numbers must be powers of 2 !!!
        A = 1, //All
        B = 2, //None
        Player = 4,
        Enemy = 8,
        NPC = 16,
        Turned_good_guy = 32,
        Hostile = 64,
        Special = 128
    }
    public Targets targets;

    //different damage values
    public float rawDamage; //raw damage
    public float stunDamage; //stun damage
    public float virusDamage; //virus damage
    public float vaccineDamage; //vaccine damage

    float[] damageStorage = new float[4]; //array used to pass damage values to others with SendMessage
    public void Awake()
    {    
        if (GetComponent<Collider>() == null)
        {
            //if DamageDealer object is missing collider, alert
            Debug.Log("No collider found in " + this.name + ". DamageDealer script needs it.");
        }
        //saves damage values to float[] array
        damageStorage[0] = rawDamage;
        damageStorage[1] = stunDamage;
        damageStorage[2] = virusDamage;
        damageStorage[3] = vaccineDamage;
    }
    void OnTriggerEnter(Collider col)
    {
        //importnant script parts:
        //col.gameObject.SendMessage("ApplyDamage", damageStorage);
        //col.tag.Contains()
        //col.name.Contains()
        if (targets == Targets.A)
        {
            if (col.name.Contains("Attackable")) //All living creatures that can be attacked, should have this in name. Also destroyable objects.
            {
                //Attack all
                col.gameObject.SendMessage("ApplyDamage", damageStorage);
            }
        }
        if (targets == Targets.B)
        {
            if (col.name.Contains(Targets.B.ToString())) //All living creatures that can be attacked, should have this in name. Also destroyable objects.
            {
                //something
            }
        }
        if (targets == Targets.Player)
        {
            if (col.name.Contains(Targets.Player.ToString()))
            {
                col.gameObject.SendMessage("ApplyDamage", damageStorage);
            }
        }
        if (targets == Targets.Enemy)
        {            
            if (col.name.Contains(Targets.Enemy.ToString()))
            {
                col.gameObject.SendMessage("ApplyDamage", damageStorage);
            }
        }
        if (targets == Targets.NPC)
        {
            if (col.name.Contains(Targets.NPC.ToString()))
            {
                col.gameObject.SendMessage("ApplyDamage", damageStorage);
            }
        }
        if (targets == Targets.Turned_good_guy)
        {
            if (col.name.Contains(Targets.Turned_good_guy.ToString()))
            {
                col.gameObject.SendMessage("ApplyDamage", damageStorage);
            }
        }
        if (targets == Targets.Hostile)
        {
            if (col.name.Contains(Targets.Hostile.ToString()))
            {
                col.gameObject.SendMessage("ApplyDamage", damageStorage);
            }
        }
        if (targets == Targets.Special)
        {
            if (col.name.Contains(Targets.Special.ToString()))
            {
                col.gameObject.SendMessage("ApplyDamage", damageStorage);
            }
        }


    }
}

