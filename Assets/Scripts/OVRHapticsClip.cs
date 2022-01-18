// Decompiled with JetBrains decompiler
// Type: OVRHapticsClip
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class OVRHapticsClip
{
  public int Count { get; private set; }

  public int Capacity { get; private set; }

  public byte[] Samples { get; private set; }

  public OVRHapticsClip()
  {
    this.Capacity = OVRHaptics.Config.MaximumBufferSamplesCount;
    this.Samples = new byte[this.Capacity * OVRHaptics.Config.SampleSizeInBytes];
  }

  public OVRHapticsClip(int capacity)
  {
    this.Capacity = capacity >= 0 ? capacity : 0;
    this.Samples = new byte[this.Capacity * OVRHaptics.Config.SampleSizeInBytes];
  }

  public OVRHapticsClip(byte[] samples, int samplesCount)
  {
    this.Samples = samples;
    this.Capacity = this.Samples.Length / OVRHaptics.Config.SampleSizeInBytes;
    this.Count = samplesCount >= 0 ? samplesCount : 0;
  }

  public OVRHapticsClip(OVRHapticsClip a, OVRHapticsClip b)
  {
    int count = a.Count;
    if (b.Count > count)
      count = b.Count;
    this.Capacity = count;
    this.Samples = new byte[this.Capacity * OVRHaptics.Config.SampleSizeInBytes];
    for (int index = 0; index < a.Count || index < b.Count; ++index)
    {
      if (OVRHaptics.Config.SampleSizeInBytes == 1)
      {
        byte sample = 0;
        if (index < a.Count && index < b.Count)
          sample = (byte) Mathf.Clamp((int) a.Samples[index] + (int) b.Samples[index], 0, (int) byte.MaxValue);
        else if (index < a.Count)
          sample = a.Samples[index];
        else if (index < b.Count)
          sample = b.Samples[index];
        this.WriteSample(sample);
      }
    }
  }

  public OVRHapticsClip(AudioClip audioClip, int channel = 0)
  {
    float[] numArray = new float[audioClip.samples * audioClip.channels];
    audioClip.GetData(numArray, 0);
    this.InitializeFromAudioFloatTrack(numArray, (double) audioClip.frequency, audioClip.channels, channel);
  }

  public void WriteSample(byte sample)
  {
    if (this.Count >= this.Capacity)
      return;
    if (OVRHaptics.Config.SampleSizeInBytes == 1)
      this.Samples[this.Count * OVRHaptics.Config.SampleSizeInBytes] = sample;
    ++this.Count;
  }

  public void Reset() => this.Count = 0;

  private void InitializeFromAudioFloatTrack(
    float[] sourceData,
    double sourceFrequency,
    int sourceChannelCount,
    int sourceChannel)
  {
    double num1 = (sourceFrequency + 1E-06) / (double) OVRHaptics.Config.SampleRateHz;
    if (num1 < 1.0)
      return;
    int num2 = (int) num1;
    double num3 = num1 - (double) num2;
    double num4 = 0.0;
    int length = sourceData.Length;
    this.Count = 0;
    this.Capacity = length / sourceChannelCount / num2 + 1;
    this.Samples = new byte[this.Capacity * OVRHaptics.Config.SampleSizeInBytes];
    int index = sourceChannel % sourceChannelCount;
    while (index < length)
    {
      if (OVRHaptics.Config.SampleSizeInBytes == 1)
        this.WriteSample((byte) ((double) Mathf.Clamp01(Mathf.Abs(sourceData[index])) * (double) byte.MaxValue));
      index += num2 * sourceChannelCount;
      num4 += num3;
      if ((int) num4 > 0)
      {
        index += (int) num4 * sourceChannelCount;
        num4 -= (double) (int) num4;
      }
    }
  }
}
