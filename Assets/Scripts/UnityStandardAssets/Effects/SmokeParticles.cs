// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Effects.SmokeParticles
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace UnityStandardAssets.Effects
{
  public class SmokeParticles : MonoBehaviour
  {
    public AudioClip[] extinguishSounds;

    private void Start()
    {
      this.GetComponent<AudioSource>().clip = this.extinguishSounds[Random.Range(0, this.extinguishSounds.Length)];
      this.GetComponent<AudioSource>().Play();
    }
  }
}
