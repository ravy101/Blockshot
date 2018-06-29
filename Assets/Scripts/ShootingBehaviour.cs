using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AI;

public class ShootingBehaviour : MonoBehaviour {

	public WeaponTypeData WeaponType;
    public LineRenderer ShotLine;
    public AudioSource ShotSound;
  
    public GameObject SpecifiedTarget;
	private int ShotsRemaining;
    private GameObject currentTarget;
    private float shotTimer = 0;
	private float reloadTimer = 0;
    private float effectsDisplayTime = 0.2f;
    private bool effectsEnabled = false;
	private bool reloading = false;


    // Use this for initialization
    void Start () {
		ShotsRemaining = WeaponType.ShotsBetweenReload;

	}

	// Update is called once per frame
	void Update () {
		if (!reloading) 
		{
			shotTimer += Time.deltaTime;
			//if there are valid targets and weapon ready
			if (shotTimer >= WeaponType.TimeBetweenBullets) 
			{
				currentTarget = FindTarget ();
				if (currentTarget != null) {
					//try shoot
					Shoot ();
				}
			}
		} else {
			reloadTimer += Time.deltaTime;
			if (reloadTimer >= WeaponType.ReloadTime) 
			{
				reloadTimer = 0;
				reloading = false;
			}
				
		}
        //clear effects after time
		if (effectsEnabled && (shotTimer + reloadTimer) >= effectsDisplayTime)
        {
            DisableEffects();
        }
	}

	public bool CanHit(GameObject targetObject, bool ignoreFieldOfFire = false)
	{
        RaycastHit hit;
        return (targetObject != null) //has a target
            && (Physics.Raycast(gameObject.transform.position, targetObject.transform.position - gameObject.transform.position, out hit, WeaponType.MaxRange)) //raycast toward target to max range of weapon                                                                                                                                                      //(Vector3.Distance(gameObject.transform.position, targetObject.transform.position) < WeaponType.MaxRange) 
            && (hit.collider.gameObject == targetObject)  //check that raycast reaches intended target
            && ((Quaternion.Angle(gameObject.transform.rotation, Quaternion.LookRotation(targetObject.transform.position - gameObject.transform.position)) < WeaponType.FieldOfFire) || ignoreFieldOfFire); // target within field of fire	or ignore flag true	
     }

    private GameObject FindTarget()
    {
        GameObject target;
        //check to see if specified target is valid
		if (CanHit(SpecifiedTarget))
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
		//update weapon
        shotTimer = 0;
		ShotsRemaining--;
		if (ShotsRemaining <= 0) 
		{
            //reload weapon
			ShotsRemaining = WeaponType.ShotsBetweenReload;
			reloading = true;
			Debug.Log ("Reloading");
		}
        //play effects
        EnableEffects();
        //resolve damage
        Debug.Log("Shooting at: " + currentTarget.ToString());
		currentTarget.GetComponent<Shootable>().TakeDamage(WeaponType.DamagePerShot);
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
