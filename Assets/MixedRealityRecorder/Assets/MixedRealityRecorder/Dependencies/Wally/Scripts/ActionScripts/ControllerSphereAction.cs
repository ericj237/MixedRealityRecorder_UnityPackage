using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerSphereAction : MonoBehaviour {

	#region Inspector configurable variables
	public string trackedGameObjectName;
	#endregion

	#region Private variable declarations
	private TileScript ts;

	// Reference to a tracked GameObject
	private GameObject trackedGO;
	#endregion

	#region Unity methods (Start, Update, ...)
	void Awake() {
		trackedGO = GameObject.Find(trackedGameObjectName);
		if (trackedGO == null) {
			Debug.Log($"Could not find GameObject {trackedGameObjectName}. Removing Action {name}.");
			Destroy(this);
		}

		// Get a reference to the TileScript to avoid frequent GetComponent calls
		ts = transform.GetComponent<TileScript>();
	}
	#endregion

	public float GetPercentage() {
		if (trackedGO == null)
			return 0;

		// Get world position of tracked GameObject
		Vector3 pos = trackedGO.transform.position;

		// Calculate distances from this tile
		Vector3 dist = ts.GetDistanceFromTile(pos);
		return Mathf.InverseLerp(2, 1, dist.magnitude); ;
	}
}
