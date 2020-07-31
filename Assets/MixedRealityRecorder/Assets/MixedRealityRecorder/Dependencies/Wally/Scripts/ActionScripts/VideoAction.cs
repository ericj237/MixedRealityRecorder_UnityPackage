using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoAction : MonoBehaviour, IAction {

	#region Inspector configurable variables
	[Tooltip("VideoActionHelper to retrieve video data from")]
	public VideoActionHelper videoActionHelper;
	public bool findByNameInstead = true;
	public string videoActionHelperName = "Video Action Helper";
	#endregion

	#region Private variable declarations
	private TileScript ts;
	#endregion

	#region Unity methods (Start, Update, ...)
	void Awake() {
		if (findByNameInstead) {
			VideoActionHelper[] vahs = FindObjectsOfType<VideoActionHelper>();
			foreach (VideoActionHelper vah in vahs) {
				if (vah.name.Equals(videoActionHelperName)) {
					videoActionHelper = vah;
					break;
				}
			}
		}
		if (videoActionHelper == null) {
			if (findByNameInstead)
				Debug.Log($"Could not find VideoActionHelper by name '{videoActionHelperName}' and variable 'videoActionHelper' is null. Removing Action.");
			else
				Debug.Log("Could not find VideoActionHelper 'videoActionHelper'. Removing Action.");
			Destroy(this);
		}

		// Get a reference to the TileScript to avoid frequent GetComponent calls
		ts = transform.GetComponent<TileScript>();
	}
	#endregion

	public float GetPercentage() {
		if (videoActionHelper == null)
			return 0;
		else
			return videoActionHelper.GetPixel(ts.x, ts.y);
	}
}
