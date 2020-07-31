using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;

public class ImageAction : MonoBehaviour, IAction {

	#region Inspector configurable variables
	[Tooltip("Image to show on wally")]
	public Texture2D image;
	[Tooltip("Make white 0% and black 100% (default black 0% and white 100%")]
	public bool invertValues;
	#endregion

	#region Private variable declarations
	private TileScript ts;
	#endregion

	#region Unity methods (Start, Update, ...)
	void Awake() {
		if (image == null) {
			Debug.Log("Could not find Texture2D testImage. Removing Action.");
			Destroy(this);
		}

		// Get a reference to the TileScript to avoid frequent GetComponent calls
		ts = transform.GetComponent<TileScript>();
	}
	#endregion

	public float GetPercentage() {
		if (image == null)
			return 0;
		if (invertValues)
			return 1 - image.GetPixel(ts.x, ts.y).grayscale;
		else
			return image.GetPixel(ts.x, ts.y).grayscale;
	}
}
