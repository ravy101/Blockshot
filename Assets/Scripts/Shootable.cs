using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Shootable : NetworkBehaviour {
    public float MaxHealth;
    public bool isDead = false;

    [SyncVar]
    private float currentHealth;

	// Use this for initialization
	void Start () {
        currentHealth = MaxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage(int amount)
    {
        Debug.Log("Taking damage: " + amount.ToString());
        if (!isDead)
        {
            currentHealth -= amount;

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        isDead = true;
        //play death effects
        gameObject.GetComponent<SelectableUnit>().enabled = false;
        gameObject.GetComponent<ControllableAgent>().enabled = false;
        Destroy(gameObject);
    }
}
