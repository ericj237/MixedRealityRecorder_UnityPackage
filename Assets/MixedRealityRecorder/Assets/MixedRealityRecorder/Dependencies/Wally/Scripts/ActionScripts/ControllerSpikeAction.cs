using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerSpikeAction : MonoBehaviour, IAction {

	#region Inspector configurable variables
	public string trackedGameObjectName;
	public float minFrontDist = 0.6f;
	public float maxFrontDist = 2f;
	public float minSideDist = 0.075f;
	public float maxSideDist = 0.5f;
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
		float depthFactor = Mathf.InverseLerp(maxFrontDist, minFrontDist, dist.z);
		float sideFactor = Mathf.InverseLerp(maxSideDist, minSideDist, Mathf.Abs(dist.x));
		sideFactor *= Mathf.InverseLerp(maxSideDist, minSideDist, Mathf.Abs(dist.y));
		return depthFactor * sideFactor;
	}
}
