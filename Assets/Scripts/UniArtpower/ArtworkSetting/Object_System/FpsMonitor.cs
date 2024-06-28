using System.Collections.Generic;
using System.Text;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

public class FpsMonitor : MonoBehaviour
{
    public Text TXT_fps;
    
    //ProfilerRecorder mainThreadTimeRecorder;

    // static double GetRecorderFrameAverage(ProfilerRecorder recorder)
    // {
    //     var samplesCount = recorder.Capacity;
    //     if (samplesCount == 0)
    //         return 0;

    //     double r = 0;
    //     unsafe
    //     {
    //         var samples = stackalloc ProfilerRecorderSample[samplesCount];
    //         recorder.CopyTo(samples, samplesCount);
    //         for (var i = 0; i < samplesCount; ++i)
    //             r += samples[i].Value;
    //         r /= samplesCount;
    //     }

    //     return r;
    // }

    // void OnEnable()
    // {
    //     mainThreadTimeRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Internal, "Main Thread", 15);
    // }

    // void OnDisable()
    // {
    //     mainThreadTimeRecorder.Dispose();
    // }

    // void Update(){
    //     if(TXT_fps) TXT_fps.text = GetFPSString();
    // }

    public double GetFPS()
    {
        return 0;
    }

    public string GetFPSString()
    {
        return $"Frame Time: 0 ms";
    }
}