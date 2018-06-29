using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "WeaponType", menuName = "Weapon Type Data")]
public class WeaponTypeData : ScriptableObject {
	public int DamagePerShot = 1;
	public float TimeBetweenBullets = 1f;
	public float ReloadTime = 2f;
	public float MaxRange = 100f;
	public int ShotsBetweenReload = 5;
	public float FieldOfFire = 45;
}
