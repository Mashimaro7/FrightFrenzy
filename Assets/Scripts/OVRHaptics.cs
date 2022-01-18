// Decompiled with JetBrains decompiler
// Type: OVRHaptics
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public static class OVRHaptics
{
  public static readonly OVRHaptics.OVRHapticsChannel[] Channels;
  public static readonly OVRHaptics.OVRHapticsChannel LeftChannel;
  public static readonly OVRHaptics.OVRHapticsChannel RightChannel;
  private static readonly OVRHaptics.OVRHapticsOutput[] m_outputs;

  static OVRHaptics()
  {
    OVRHaptics.Config.Load();
    OVRHaptics.m_outputs = new OVRHaptics.OVRHapticsOutput[2]
    {
      new OVRHaptics.OVRHapticsOutput(1U),
      new OVRHaptics.OVRHapticsOutput(2U)
    };
    OVRHaptics.Channels = new OVRHaptics.OVRHapticsChannel[2]
    {
      OVRHaptics.LeftChannel = new OVRHaptics.OVRHapticsChannel(0U),
      OVRHaptics.RightChannel = new OVRHaptics.OVRHapticsChannel(1U)
    };
  }

  public static void Process()
  {
    OVRHaptics.Config.Load();
    for (int index = 0; index < OVRHaptics.m_outputs.Length; ++index)
      OVRHaptics.m_outputs[index].Process();
  }

  public static class Config
  {
    public static int SampleRateHz { get; private set; }

    public static int SampleSizeInBytes { get; private set; }

    public static int MinimumSafeSamplesQueued { get; private set; }

    public static int MinimumBufferSamplesCount { get; private set; }

    public static int OptimalBufferSamplesCount { get; private set; }

    public static int MaximumBufferSamplesCount { get; private set; }

    static Config() => OVRHaptics.Config.Load();

    public static void Load()
    {
      OVRPlugin.HapticsDesc controllerHapticsDesc = OVRPlugin.GetControllerHapticsDesc(2U);
      OVRHaptics.Config.SampleRateHz = controllerHapticsDesc.SampleRateHz;
      OVRHaptics.Config.SampleSizeInBytes = controllerHapticsDesc.SampleSizeInBytes;
      OVRHaptics.Config.MinimumSafeSamplesQueued = controllerHapticsDesc.MinimumSafeSamplesQueued;
      OVRHaptics.Config.MinimumBufferSamplesCount = controllerHapticsDesc.MinimumBufferSamplesCount;
      OVRHaptics.Config.OptimalBufferSamplesCount = controllerHapticsDesc.OptimalBufferSamplesCount;
      OVRHaptics.Config.MaximumBufferSamplesCount = controllerHapticsDesc.MaximumBufferSamplesCount;
    }
  }

  public class OVRHapticsChannel
  {
    private OVRHaptics.OVRHapticsOutput m_output;

    public OVRHapticsChannel(uint outputIndex) => this.m_output = OVRHaptics.m_outputs[(int) outputIndex];

    public void Preempt(OVRHapticsClip clip) => this.m_output.Preempt(clip);

    public void Queue(OVRHapticsClip clip) => this.m_output.Queue(clip);

    public void Mix(OVRHapticsClip clip) => this.m_output.Mix(clip);

    public void Clear() => this.m_output.Clear();
  }

  private class OVRHapticsOutput
  {
    private bool m_lowLatencyMode = true;
    private int m_prevSamplesQueued;
    private float m_prevSamplesQueuedTime;
    private int m_numPredictionHits;
    private int m_numPredictionMisses;
    private int m_numUnderruns;
    private List<OVRHaptics.OVRHapticsOutput.ClipPlaybackTracker> m_pendingClips = new List<OVRHaptics.OVRHapticsOutput.ClipPlaybackTracker>();
    private uint m_controller;
    private OVRNativeBuffer m_nativeBuffer = new OVRNativeBuffer(OVRHaptics.Config.MaximumBufferSamplesCount * OVRHaptics.Config.SampleSizeInBytes);
    private OVRHapticsClip m_paddingClip = new OVRHapticsClip();

    public OVRHapticsOutput(uint controller) => this.m_controller = controller;

    public void Process()
    {
      OVRPlugin.HapticsState controllerHapticsState = OVRPlugin.GetControllerHapticsState(this.m_controller);
      float num1 = Time.realtimeSinceStartup - this.m_prevSamplesQueuedTime;
      if (this.m_prevSamplesQueued > 0)
      {
        int num2 = this.m_prevSamplesQueued - (int) ((double) num1 * (double) OVRHaptics.Config.SampleRateHz + 0.5);
        if (num2 < 0)
          num2 = 0;
        if (controllerHapticsState.SamplesQueued - num2 == 0)
          ++this.m_numPredictionHits;
        else
          ++this.m_numPredictionMisses;
        if (num2 > 0 && controllerHapticsState.SamplesQueued == 0)
          ++this.m_numUnderruns;
        this.m_prevSamplesQueued = controllerHapticsState.SamplesQueued;
        this.m_prevSamplesQueuedTime = Time.realtimeSinceStartup;
      }
      int num3 = OVRHaptics.Config.OptimalBufferSamplesCount;
      if (this.m_lowLatencyMode)
      {
        float num4 = 1000f / (float) OVRHaptics.Config.SampleRateHz;
        int num5 = OVRHaptics.Config.MinimumSafeSamplesQueued + (int) Mathf.Ceil(num1 * 1000f / num4);
        if (num5 < num3)
          num3 = num5;
      }
      if (controllerHapticsState.SamplesQueued > num3)
        return;
      if (num3 > OVRHaptics.Config.MaximumBufferSamplesCount)
        num3 = OVRHaptics.Config.MaximumBufferSamplesCount;
      if (num3 > controllerHapticsState.SamplesAvailable)
        num3 = controllerHapticsState.SamplesAvailable;
      int num6 = 0;
      for (int index = 0; num6 < num3 && index < this.m_pendingClips.Count; ++index)
      {
        int num7 = num3 - num6;
        int num8 = this.m_pendingClips[index].Clip.Count - this.m_pendingClips[index].ReadCount;
        if (num7 > num8)
          num7 = num8;
        if (num7 > 0)
        {
          int length = num7 * OVRHaptics.Config.SampleSizeInBytes;
          int byteOffset = num6 * OVRHaptics.Config.SampleSizeInBytes;
          int startIndex = this.m_pendingClips[index].ReadCount * OVRHaptics.Config.SampleSizeInBytes;
          Marshal.Copy(this.m_pendingClips[index].Clip.Samples, startIndex, this.m_nativeBuffer.GetPointer(byteOffset), length);
          this.m_pendingClips[index].ReadCount += num7;
          num6 += num7;
        }
      }
      for (int index = this.m_pendingClips.Count - 1; index >= 0 && this.m_pendingClips.Count > 0; --index)
      {
        if (this.m_pendingClips[index].ReadCount >= this.m_pendingClips[index].Clip.Count)
          this.m_pendingClips.RemoveAt(index);
      }
      int num9 = num3 - (controllerHapticsState.SamplesQueued + num6);
      if (num9 < OVRHaptics.Config.MinimumBufferSamplesCount - num6)
        num9 = OVRHaptics.Config.MinimumBufferSamplesCount - num6;
      if (num9 > controllerHapticsState.SamplesAvailable)
        num9 = controllerHapticsState.SamplesAvailable;
      if (num9 > 0)
      {
        int length = num9 * OVRHaptics.Config.SampleSizeInBytes;
        Marshal.Copy(this.m_paddingClip.Samples, 0, this.m_nativeBuffer.GetPointer(num6 * OVRHaptics.Config.SampleSizeInBytes), length);
        num6 += num9;
      }
      if (num6 <= 0)
        return;
      OVRPlugin.HapticsBuffer hapticsBuffer;
      hapticsBuffer.Samples = this.m_nativeBuffer.GetPointer();
      hapticsBuffer.SamplesCount = num6;
      OVRPlugin.SetControllerHaptics(this.m_controller, hapticsBuffer);
      this.m_prevSamplesQueued = OVRPlugin.GetControllerHapticsState(this.m_controller).SamplesQueued;
      this.m_prevSamplesQueuedTime = Time.realtimeSinceStartup;
    }

    public void Preempt(OVRHapticsClip clip)
    {
      this.m_pendingClips.Clear();
      this.m_pendingClips.Add(new OVRHaptics.OVRHapticsOutput.ClipPlaybackTracker(clip));
    }

    public void Queue(OVRHapticsClip clip) => this.m_pendingClips.Add(new OVRHaptics.OVRHapticsOutput.ClipPlaybackTracker(clip));

    public void Mix(OVRHapticsClip clip)
    {
      int index1 = 0;
      int capacity = 0;
      int count;
      for (count = clip.Count; count > 0 && index1 < this.m_pendingClips.Count; ++index1)
      {
        int num = this.m_pendingClips[index1].Clip.Count - this.m_pendingClips[index1].ReadCount;
        count -= num;
        capacity += num;
      }
      if (count > 0)
        capacity += count;
      if (index1 > 0)
      {
        OVRHapticsClip clip1 = new OVRHapticsClip(capacity);
        OVRHapticsClip ovrHapticsClip = clip;
        int index2 = 0;
        for (int index3 = 0; index3 < index1; ++index3)
        {
          OVRHapticsClip clip2 = this.m_pendingClips[index3].Clip;
          for (int readCount = this.m_pendingClips[index3].ReadCount; readCount < clip2.Count; ++readCount)
          {
            if (OVRHaptics.Config.SampleSizeInBytes == 1)
            {
              byte sample = 0;
              if (index2 < ovrHapticsClip.Count && readCount < clip2.Count)
              {
                sample = (byte) Mathf.Clamp((int) ovrHapticsClip.Samples[index2] + (int) clip2.Samples[readCount], 0, (int) byte.MaxValue);
                ++index2;
              }
              else if (readCount < clip2.Count)
                sample = clip2.Samples[readCount];
              clip1.WriteSample(sample);
            }
          }
        }
        for (; index2 < ovrHapticsClip.Count; ++index2)
        {
          if (OVRHaptics.Config.SampleSizeInBytes == 1)
            clip1.WriteSample(ovrHapticsClip.Samples[index2]);
        }
        this.m_pendingClips[0] = new OVRHaptics.OVRHapticsOutput.ClipPlaybackTracker(clip1);
        for (int index4 = 1; index4 < index1; ++index4)
          this.m_pendingClips.RemoveAt(1);
      }
      else
        this.m_pendingClips.Add(new OVRHaptics.OVRHapticsOutput.ClipPlaybackTracker(clip));
    }

    public void Clear() => this.m_pendingClips.Clear();

    private class ClipPlaybackTracker
    {
      public int ReadCount { get; set; }

      public OVRHapticsClip Clip { get; set; }

      public ClipPlaybackTracker(OVRHapticsClip clip) => this.Clip = clip;
    }
  }
}
