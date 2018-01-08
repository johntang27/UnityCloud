using UnityEngine;
using System.Collections;

public class Utility : MonoBehaviour {

	public Transform testObject;
	public float testHeight = 1f;

	public static string FormatCurrency(float amt, bool useK = false)
	{
		if(useK)
		{
			amt = amt / 1000;
			if(amt >= 1000)
			{
				amt = amt / 1000;
				return amt.ToString("$#.###;-$#.###; 0") + " million";
			}
			return amt.ToString("$#,###;-$#,###; 0") + "k";
		}

        return amt.ToString("$#,###;-$#,###; 0");
    }
}
