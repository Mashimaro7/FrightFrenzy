// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Utility.ParticleSystemDestroyer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
  public class ParticleSystemDestroyer : MonoBehaviour
  {
    public float minDuration = 8f;
    public float maxDuration = 10f;
    private float m_MaxLifetime;
    private bool m_EarlyStop;

    private IEnumerator Start()
    {
      ParticleSystemDestroyer particleSystemDestroyer = this;
      ParticleSystem[] systems = particleSystemDestroyer.GetComponentsInChildren<ParticleSystem>();
      foreach (ParticleSystem particleSystem in systems)
        particleSystemDestroyer.m_MaxLifetime = Mathf.Max(particleSystem.main.startLifetime.constant, particleSystemDestroyer.m_MaxLifetime);
      float stopTime = Time.time + Random.Range(particleSystemDestroyer.minDuration, particleSystemDestroyer.maxDuration);
      while ((double) Time.time < (double) stopTime && !particleSystemDestroyer.m_EarlyStop)
        yield return (object) null;
      Debug.Log((object) ("stopping " + particleSystemDestroyer.name));
      foreach (ParticleSystem particleSystem in systems)
        particleSystem.emission.enabled = false;
      particleSystemDestroyer.BroadcastMessage("Extinguish", SendMessageOptions.DontRequireReceiver);
      yield return (object) new WaitForSeconds(particleSystemDestroyer.m_MaxLifetime);
      Object.Destroy((Object) particleSystemDestroyer.gameObject);
    }

    public void Stop() => this.m_EarlyStop = true;
  }
}
