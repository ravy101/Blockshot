using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBehaviour : MonoBehaviour {
    //Weapon deatails maybe should be classed up
    public int DamagePerShot = 20;
    public float TimeBetweenBullets = 0.4f;
    public float MaxRange = 100f;
    public LineRenderer ShotLine;
    public AudioSource ShotSound;
    public GameObject SpecifiedTarget;

    private GameObject currentTarget;
    private float shotTimer;
    private float effectsDisplayTime = 0.2f;
    private bool effectsEnabled = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        shotTimer += Time.deltaTime;
        //if there are valid targets and weapon ready
        if (shotTimer >= TimeBetweenBullets)
        {
            currentTarget = FindTarget();
            if (currentTarget != null)
            {
                //try shoot
                Shoot();
            }
        }

        //clear effects after time
        if (effectsEnabled && shotTimer >= effectsDisplayTime)
        {
            DisableEffects();
        }
	}

    private GameObject FindTarget()
    {
        GameObject target;
        //check to see if specified target is valid
        if ((SpecifiedTarget != null) && (Vector3.Distance(gameObject.transform.position, SpecifiedTarget.transform.position) < MaxRange))
        {
            target = SpecifiedTarget; 
        }
        else
        {
            //TODO look for targets of opportunity
            target = null;
        }
        return target;
    }

    private void Shoot()
    {
        shotTimer = 0;
        //play effects
        EnableEffects();
        //resolve damage
        Debug.Log("Shooting at: " + currentTarget.ToString());
        currentTarget.GetComponent<Shootable>().TakeDamage(DamagePerShot);
    }

    private void EnableEffects()
    {
        effectsEnabled = true;
        ShotLine.enabled = true;
        ShotLine.SetPosition(0, gameObject.transform.position);
        ShotLine.SetPosition(1, currentTarget.transform.position);
        ShotSound.Play();
            
    }

    private void DisableEffects()
    {
        effectsEnabled = false;
        ShotLine.enabled = false;
    }
}
