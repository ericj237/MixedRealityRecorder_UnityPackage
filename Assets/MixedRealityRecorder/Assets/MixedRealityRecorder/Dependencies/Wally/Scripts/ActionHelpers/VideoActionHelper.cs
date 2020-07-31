using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoActionHelper : MonoBehaviour {

	#region Inspector configurable variables
	[Header("Video Player settings")]
	[Tooltip("Video to show on wally")]
	public VideoClip videoClip;
	[Tooltip("Whether to play the audio of given video file")]
	public bool enableAudio = false;
	public float videoScale = 0.25f;
	public float playbackSpeed = 0.25f;

	[Header("Wally specific settings")]
	[Tooltip("Make white 0% and black 100% (default black 0% and white 100%")]
	public bool invertValues;
	[Tooltip("Tile value (from 0 to 1) to report when there is no data for a 'pixel'")]
	[Range(0, 1)]
	public float defaultValue = 0;
	#endregion

	#region Private variable declarations
	private VideoPlayer vp;
	private Texture2D vTex;
	private long lastFrame = -1;
	private int nthPixel;
	#endregion

	#region Unity methods (Start, Update, ...)
	void Awake() {
		if (videoClip == null) {
			Debug.Log("Could not find VideoClip 'videoClip'. Removing Action Helper.");
			Destroy(this);
		}

		// Set up VideoPlayer through which to access video data
		vp = gameObject.AddComponent<VideoPlayer>();
		vp.source = VideoSource.VideoClip;
		vp.clip = videoClip;
		vp.playOnAwake = true;
		vp.waitForFirstFrame = true;
		vp.isLooping = true;
		vp.skipOnDrop = true;
		vp.playbackSpeed = playbackSpeed;
		vp.renderMode = VideoRenderMode.RenderTexture;
		vp.targetTexture = new RenderTexture((int)vp.clip.width, (int)vp.clip.height, 24);
		vp.aspectRatio = VideoAspectRatio.NoScaling;
		vp.audioOutputMode = enableAudio ? VideoAudioOutputMode.Direct : VideoAudioOutputMode.None;

		nthPixel = (int)Mathf.Round(1f / videoScale);
	}
	#endregion

	public float GetPixel(int x, int y) {
		if (vp.isPlaying) {
			UpdateTexture();
			if (vTex != null)
				if (invertValues)
					return 1 - vTex.GetPixel(x * nthPixel, y * nthPixel).grayscale;
				else
					return vTex.GetPixel(x * nthPixel, y * nthPixel).grayscale;
			else
				return defaultValue;
		}
		return defaultValue;
	}

	private void UpdateTexture() {
		if (vp.frameCount == 0 || vp.frame == -1) {
			vTex = null;
			return;
		}
		// Skip if vTex is up to date already
		if (lastFrame == vp.frame)
			return;
		// Prepare vTex to load pixels from the video
		if (vTex == null || vTex.width != vp.targetTexture.width || vTex.height != vp.targetTexture.height)
			vTex = new Texture2D(vp.targetTexture.width, vp.targetTexture.height);
		// Make VideoPlayer targetTexture the active RenderTexture to access its contents
		RenderTexture.active = vp.targetTexture;
		// Read pixels from currently active RenderTexture into Texture2D 'vTex'
		vTex.ReadPixels(new Rect(0, 0, vp.targetTexture.width, vp.targetTexture.height), 0, 0);
		vTex.Apply();
		lastFrame = vp.frame;
	}
}
