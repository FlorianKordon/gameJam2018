using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenController : MonoBehaviour {

    public ScreenOrientation screenOrientation= ScreenOrientation.Landscape;
    public bool autorotateToLandscapeLeft = true;
    public bool autorotateToLandscapeRight = true;
    public bool autorotateToPortrait = false;
    public bool autorotateToPortraitUpsideDown = false;

	private void Start () {
		Screen.orientation = ScreenOrientation.Landscape;
        Screen.autorotateToLandscapeLeft = autorotateToLandscapeLeft;
        Screen.autorotateToLandscapeRight = autorotateToLandscapeRight;
        Screen.autorotateToPortrait = autorotateToPortrait;
        Screen.autorotateToPortraitUpsideDown = autorotateToPortraitUpsideDown;
	}
}
