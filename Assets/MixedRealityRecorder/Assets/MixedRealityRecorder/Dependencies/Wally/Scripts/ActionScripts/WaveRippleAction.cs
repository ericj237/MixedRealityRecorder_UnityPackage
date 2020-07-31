using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveRippleAction : MonoBehaviour, IAction {

	#region Inspector configurable variables
	[Tooltip("WaveRippleActionHelper to retrieve wave data from")]
	public WaveRippleActionHelper waveRippleActionHelper;
	public bool findByNameInstead = true;
	public string waveRippleActionHelperName = "Wave Ripple Action Helper";
	#endregion

	#region Private variable declarations
	private TileScript ts;
	#endregion

	#region Unity methods (Start, Update, ...)
	void Awake() {
		if (findByNameInstead) {
			WaveRippleActionHelper[] wrahs = FindObjectsOfType<WaveRippleActionHelper>();
			foreach (WaveRippleActionHelper wrah in wrahs) {
				if (wrah.name.Equals(waveRippleActionHelperName)) {
					waveRippleActionHelper = wrah;
					break;
				}
			}
		}
		if (waveRippleActionHelper == null) {
			if (findByNameInstead)
				Debug.Log($"Could not find WaveRippleActionHelper by name '{waveRippleActionHelperName}' and variable 'waveRippleActionHelper' is null. Removing Action.");
			else
				Debug.Log("Could not find WaveRippleActionHelper 'waveRippleActionHelper'. Removing Action.");
			Destroy(this);
		}

		// Get a reference to the TileScript to avoid frequent GetComponent calls
		ts = transform.GetComponent<TileScript>();
	}
	#endregion

	public float GetPercentage() {
		return waveRippleActionHelper.GetPercentageForPosition(new Vector2(ts.x, ts.y));
	}
}
