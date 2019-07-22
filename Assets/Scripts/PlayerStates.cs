using UnityEngine;
using UnityEngine.Events;

public class PlayerStates : MonoBehaviour {

	[Header("Weapons")]
	public GameObject WeaponSlot;
	public GameObject SwordPrefab;
	public GameObject PistolPrefab;
	public GameObject RiflePrefab;

	[Header("Events")]
	public UnityEvent OnSwitchRifle;
	public UnityEvent OnSwitchPistol;
	public UnityEvent OnSwitchSword;

	private GameObject currentWeapon;

	void Start()
	{
		OnSwitchSword.Invoke();
		SwitchWeapon(Instantiate(SwordPrefab, WeaponSlot.transform));
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Alpha1))
		{
			SwitchWeapon(Instantiate(SwordPrefab, WeaponSlot.transform));
			OnSwitchSword.Invoke();
		}
		else if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Alpha2))
		{
			SwitchWeapon(Instantiate(PistolPrefab, WeaponSlot.transform));
			OnSwitchPistol.Invoke();
		}
		//else if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Alpha3))
		//{
		//	OnSwitchRifle.Invoke();
		//}
	}

	private void SwitchWeapon(GameObject ne)
	{
		UnequipWeapon();
		Equip(ne);
	}

	private void UnequipWeapon()
	{
		Destroy(currentWeapon);
		currentWeapon = null;
	}

	private void Equip(GameObject ne)
	{
		currentWeapon = ne;
	}
}
