using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetAction : MonoBehaviour, IAction {

	#region Inspector configurable variables
	public string trackedGameObjectName;
	[Tooltip("WaveRippleActionHelper to send wave data to")]
	public WaveRippleActionHelper waveRippleActionHelper;
	public bool findByNameInstead = true;
	public string waveRippleActionHelperName = "Wave Ripple Action Helper";
	public float minFrontDist = 0.4f;
	public float maxFrontDist = 2f;
	public float minSideDist = 0.075f;
	public float maxSideDist = 0.5f;
	#endregion

	#region Private variable declarations
	private TileScript ts;

	// Reference to a tracked GameObject
	private GameObject trackedGO;

	private bool canSendWave;
	#endregion

	#region Unity methods (Start, Update, ...)
	void Awake() {
		trackedGO = GameObject.Find(trackedGameObjectName);
		if (trackedGO == null) {
			Debug.Log($"Could not find GameObject {trackedGameObjectName}. Removing Action {name}.");
			Destroy(this);
		}

		if (findByNameInstead) {
			WaveRippleActionHelper[] wrahs = FindObjectsOfType<WaveRippleActionHelper>();
			foreach (WaveRippleActionHelper wrah in wrahs) {
				if (wrah.name.Equals(waveRippleActionHelperName)) {
					waveRippleActionHelper = wrah;
					break;
				}
			}
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
		float sideFactor = Mathf.InverseLerp(minSideDist + ((1 - depthFactor) * (maxSideDist - minSideDist)), minSideDist, Mathf.Abs(dist.x));
		sideFactor *= Mathf.InverseLerp(minSideDist + ((1 - depthFactor) * (maxSideDist - minSideDist)), minSideDist, Mathf.Abs(dist.y));

		if (waveRippleActionHelper != null) {
			// Only spawn wave for central tile
			if (Mathf.Abs(dist.x) < minSideDist && Mathf.Abs(dist.y) < minSideDist /*(ts.transform.localScale.y / 2)*/) {

				// Test conditions for wave spawning
				if (Mathf.Abs(dist.z) >= minFrontDist) {
					canSendWave = true;
				} else {
					if (canSendWave) {
						Wave wave = new Wave {
							spawnPosition = new Vector2(ts.x, ts.y),
							spawnTime = Time.time + 0.1f,
							waveWidth = 1.5f,
							waveSpeed = 10f,
							falloff = 0.01f,
							timeOfDeath = Time.time + 5f
						};

						waveRippleActionHelper.AddWave(wave);
						Debug.Log("Sent a wave");

						canSendWave = false;
					}
				}
			}
		}

		return depthFactor * sideFactor;
	}
}
