using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class crossFader : MonoBehaviour {

    public AudioMixer mixer;
    private bool mel1Fading = false;
    private bool mel2Fading = false;
    private bool back1Fading = false;
    private bool back2Fading = false;

    void Start () {
        fade("Melody1", -5.0f, 1000.0f);
    }

    public void fade(string track, float volume, float msToGoal)
    {
        StartCoroutine(startFade(track, volume, msToGoal));
    }
    
    private IEnumerator startFade(string track, float volume, float msToGoal)
    {
        bool finishFade = false;
        switch (track)
        {
            case ("Melody1"):
                if (mel1Fading) finishFade = true;
                else mel1Fading = true;
                break;
            case ("Melody2"):
                if (mel2Fading) finishFade = true;
                else mel2Fading = true;
                break;
            case ("Background1"):
                if (back1Fading) finishFade = true;
                else back1Fading = true;
                break;
            case ("Background2"):
                if (back2Fading) finishFade = true;
                else back2Fading = true;
                break;
        }
        bool goingUp = false;
        double startTime = AudioSettings.dspTime * 1000;
        double endTime = startTime + msToGoal;
        float startGain;
        mixer.GetFloat(track, out startGain);
        Debug.Log("start gain = " + startGain);
        if (volume > startGain) goingUp = true;

        double gainDifference = volume - startGain;
        double gainPerMs = gainDifference / msToGoal;
        Debug.Log("Gain per ms = " + gainPerMs);
        
        while(AudioSettings.dspTime < endTime && !finishFade)
        {
            Debug.Log("Current gain = " + gainPerMs * ((AudioSettings.dspTime * 1000) - startTime));
            mixer.SetFloat(track, (float)(gainPerMs * ((AudioSettings.dspTime * 1000) - startTime)));
            float currentGain;
            mixer.GetFloat(track, out currentGain);
            if (goingUp)
            {
                if (currentGain > volume) finishFade = true;
            }
            else if (currentGain < volume) finishFade = true;

            yield return new WaitForSeconds(.1f);
        }
        checkForFades(track);
    }

    private void checkForFades(string track)
    {
        switch (track)
        {
            case ("Melody1"):
                mel1Fading = false;
                break;
            case ("Melody2"):
                mel2Fading = false;
                break;
            case ("Background1"):
                back1Fading = false;
                break;
            case ("Background2"):
                back2Fading = false;
                break;
        }
    }
}
