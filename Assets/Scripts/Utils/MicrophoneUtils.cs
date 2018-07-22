using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MicrophoneUtils
{
    private AudioMixer audioMixer;
    private AudioSource aud;
    private float[] _samples;           		// Microphone samples
    private float[] _spectrum;          		// Spectrum of mic samples
    private CircularBuffer<float> _dbValues;	// Used to average recent volume.
    private CircularBuffer<float> _pitchValues; // Used to average recent pitch.    

    private float _rmsValue;            		// Volume in RMS
    private float _dbValue;             		// Volume in DB
    private float _pitchValue;                  // Pitch - Hz 
    private float _lowPassResults;      		// Low Pass Filter result
    private float _peakPowerForChannel;
    private int _blowingTime;                   // How long each blow has lasted

    private const int FREQUENCY = 48000;
    private const int SAMPLECOUNT = 1024;   	// Sample Count (must be power of 2)
    private const float REFVALUE = 0.1f;    	// RMS reference value for 0 dB.
    private const float FREQ_THRESHOLD = 0.02f; // Minimum amplitude to extract pitch (recieve anything)
    private const float ALPHA = 0.05f;
    private const int RecordedLength = 50;				// How many previous frames of sound are analyzed.
    private const int RequiedBlowTime = 4;             // How long a blow must last to be classified as a blow (and not a sigh for instance)


    public MicrophoneUtils(AudioSource aud, AudioMixer audioMixer)
    {
        this.aud = aud;
        this.audioMixer = audioMixer;
        _samples = new float[SAMPLECOUNT];
        _spectrum = new float[SAMPLECOUNT];
        _dbValues = new CircularBuffer<float>(RecordedLength);
        _pitchValues = new CircularBuffer<float>(RecordedLength);
    }

    /// <summary>
    /// Start microphone recording.
    /// </summary>
    public void StartMicListener(string audioMixerParamName)
    {
        audioMixer.SetFloat(audioMixerParamName, -80f);
        aud.clip = Microphone.Start(null, true, 1, FREQUENCY);
        aud.loop = true;
        while (!(Microphone.GetPosition(null) > 0)) { }
        aud.Play();
    }

    /// <summary>
    /// Stop the recording and close the microphone listener.
    /// </summary>
    public void StopMicListener()
    {
        Microphone.End(null);
        aud.Stop();
    }

    /// <summary>
    /// Determine wether user used a blow as input with enough blowing time. 
    /// Should be used in Monobehaviour.Update().
    /// </summary>
    public bool DetermineUserBlow()
    {
        // Analyze the sound, to get volume and pitch values.
        AnalyzeSound();
        DeriveBlow();

        return _blowingTime > RequiedBlowTime;
    }

    // refer to http://goo.gl/VGwKt
    private void AnalyzeSound()
    {
        #region AudioVolume
        // Populate float array with current audio buffer
        aud.GetOutputData(_samples, 0);

        // Sums squared samples
        float sum = 0;
        foreach (var sample in _samples)
            sum += Mathf.Pow(sample, 2);        

        // RMS value: is the square root of the average value of the squared samples.
        _rmsValue = Mathf.Sqrt(sum / SAMPLECOUNT);
        _dbValue = 20 * Mathf.Log10(_rmsValue / REFVALUE);
        #endregion

        #region AudioPitch
        // clamp audio to -160 db
        if (_dbValue < -160)
            _dbValue = -160;

        // Populate float list with frequency bars
        aud.GetSpectrumData(_spectrum, 0, FFTWindow.BlackmanHarris);

        // Search highest frequency and its index in the FFT spectrum
        float maxFreq = 0;
        int maxIdx = 0;
        for (int i = 0; i < SAMPLECOUNT; i++)
        {
            if (_spectrum[i] > maxFreq && _spectrum[i] > FREQ_THRESHOLD)
            {
                maxFreq = _spectrum[i];
                maxIdx = i;
            }
        }

        // Interpolation, as the highest frequency can lie between bands
        float freqN = maxIdx;
        // Incorporate and weigh in frequency neighbours
        if (maxIdx > 0 && maxIdx < SAMPLECOUNT - 1)
        {
            float dL = _spectrum[maxIdx - 1] / _spectrum[maxIdx];
            float dR = _spectrum[maxIdx + 1] / _spectrum[maxIdx];
            freqN += 0.5f * (dR * dR - dL * dL);
        }
        // Convert index to frequency
        _pitchValue = freqN * (FREQUENCY / 2) / SAMPLECOUNT;
        #endregion
    }

    private void DeriveBlow()
    {
        _dbValues.Add(_dbValue);
        _pitchValues.Add(_pitchValue);

        // Find the average pitch in our records (used to decipher against whistles, clicks, etc).
        float sumPitch = 0;
        foreach (float num in _pitchValues)
            sumPitch += num;

        sumPitch /= _pitchValues.Count;

        // Run low pass filter.
        _lowPassResults = ALPHA * _dbValue + (1.0f - ALPHA) * _lowPassResults;

        // Decides whether this instance of the result could be a blow or not.
        if (_lowPassResults > -30 && sumPitch == 0)
            _blowingTime += 1;
        else
            _blowingTime = 0;
    }

    #region Debugging
    public static void PrintDeviceNames()
    {
        foreach (string device in Microphone.devices)
        {
            Debug.Log("Name: " + device);
        }
    }

    public static void PrintDeviceCaps()
    {
        int i, j;
        Microphone.GetDeviceCaps(null, out i, out j);
        Debug.Log(i);
        Debug.Log(j);
    }
    #endregion
}