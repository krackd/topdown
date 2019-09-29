using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spell : MonoBehaviour
{
	public Image Icon;
	public Text ChargesLabel;
	public Ability Ability;

	// Start is called before the first frame update
	void Start()
    {
        if (Icon == null)
		{
			Debug.LogError("Missing icon on spell " + gameObject.name);
		}

		if (ChargesLabel == null)
		{
			Debug.LogError("Missing charges label on spell " + gameObject.name);
		}

		if (Ability == null)
		{
			Debug.LogError("Missing ability on spell " + gameObject.name);
		}

		ChargesLabel.text = Ability.Charges.ToString();
		Ability.OnChargesChanged.AddListener(updateCharges);
	}

	private void updateCharges(int charges)
	{
		ChargesLabel.text = charges.ToString();
	}
}
