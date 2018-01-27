using UnityEngine;

public class Metronome : MonoBehaviour {

    public int BPM = 109;
    public int sampleOffset = 0;

    private double sampPerTick;
    private int sampleRate = 44100;
    private double accumulator;
    private bool hasTicked = false;
    private int numBuff;
    private int buffSize;

    private void Awake()
    {
        sampleRate = AudioSettings.outputSampleRate;
        sampPerTick = (sampleRate * 60) / (BPM);
        AudioSettings.GetDSPBufferSize(out buffSize, out numBuff);
        accumulator = sampleOffset;
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        for(int i = 0; i < buffSize; i++)
        {
            accumulator++;
            if (accumulator > sampPerTick)
            {
                hasTicked = true;
                accumulator -= sampPerTick;
            }
            data[i] = 0;
        }
    }

    private void tick()
    {
        //this is called on the closest frame to every metronome click.
        //Debug.Log("Metro tick");
    }

    private void Update()
    {
        if(hasTicked)
        {
            tick();
            hasTicked = false;
        }
    }
}
