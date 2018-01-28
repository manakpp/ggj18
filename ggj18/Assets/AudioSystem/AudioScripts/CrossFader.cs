//Chris Wratt 2018
//Audio fader control

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

public class CrossFader : MonoBehaviour {

	public AudioMixer masterMixer;

	private Dictionary<string, bool> changingBools;

	void Start() {
		changingBools = new Dictionary<string, bool>();
		changingBools.Add("Melody1", false);
		changingBools.Add("SFX", false);
		changingBools.Add("Background1", false);
        changingBools.Add("Master", false);

    }

	public void CreateFade(string loopName, float endValue, float length) {
		if (changingBools.ContainsKey (loopName)) {
			StartCoroutine (VolumeFade (loopName, endValue, length));
		} else {
			Debug.Log ("Invalid loop name provided");
		}
	}

	//Fade in or out function. This creates and destroys new routines...
	private IEnumerator VolumeFade(string loopName, float endValue, float length) {

		float fadeStart = (float)AudioSettings.dspTime;
		float timeSinceStart = 0.0f;
		float myVolume;
		float startValue;

		//set starting value to current fader position
		masterMixer.GetFloat(loopName, out startValue);

		//this will cancel any fades occuring on this loop
		changingBools[loopName] = false;

		//delay
		yield return new WaitForSeconds (0.05f);

		changingBools[loopName] = true;

		//lerp fade loop with delay
		while(timeSinceStart < length){
			timeSinceStart = Mathf.Abs ((float)AudioSettings.dspTime - fadeStart);

			//this checks to see if a new fade has been called
			if (changingBools[loopName]) {
				yield return new WaitForSeconds (0.05f);
			} else {
				yield break;
			}

			myVolume = Mathf.Lerp (startValue, endValue, timeSinceStart / length);
			masterMixer.SetFloat(loopName, myVolume);

		}
		yield break;
	}
}
