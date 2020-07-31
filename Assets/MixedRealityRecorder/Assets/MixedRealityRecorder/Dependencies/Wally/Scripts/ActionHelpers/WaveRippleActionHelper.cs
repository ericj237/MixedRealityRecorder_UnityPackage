using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Wave {
	public Vector2 spawnPosition;
	public float spawnTime;
	[Range(0.001f, 1000f)]
	public float waveWidth;
	public float waveSpeed;
	public float falloff;
	public float timeOfDeath;
}

public class WaveRippleActionHelper : MonoBehaviour {

	#region Inspector configurable variables
	[Header("Wave Ripple settings")]
	[Tooltip("List of waves to spawn")]
	public List<Wave> waves;

	[Header("Wally specific settings")]
	[Tooltip("Make white 0% and black 100% (default black 0% and white 100%")]
	public bool invertValues;
	[Tooltip("Base tile value (from 0 to 1) to add wave to")]
	[Range(0, 1)]
	public float defaultValue = 0;
	#endregion

	#region Private variable declarations
	private int nthPixel;
	#endregion

	#region Unity methods (Start, Update, ...)
	void Update() {
		for (int i = waves.Count - 1; i >= 0; i--) {
			if (waves[i].timeOfDeath != 0 && waves[i].timeOfDeath < Time.time) {
				waves.RemoveAt(i);
			}
		}
	}
	#endregion

	public void AddWave(Wave wave) {
		waves.Add(wave);
	}

	public float GetPercentageForPosition(Vector2 pos) {
		float percentage = 0;

		foreach (Wave wave in waves) {
			percentage += GetPercentageForWave(wave, pos);
		}

		return percentage;
	}

	private float GetPercentageForWave(Wave wave, Vector2 pos) {
		// Vector from wave spawn towards tile position
		Vector2 diff = wave.spawnPosition - pos;

		// Time since wavestart, possibly sped up
		float wavePropagation = (Time.time - wave.spawnTime) * wave.waveSpeed;

		// Wave progress local to this tiles position
		float x = diff.magnitude - wavePropagation;

		// Amplitude for this tile
		float height = -(1 / wave.waveWidth) * (x * x) + 1;

		// Wave falloff
		height = height - (diff.magnitude * wave.falloff);

		height = Mathf.Clamp(height, 0f, 1f);

		return height;
	}
}
