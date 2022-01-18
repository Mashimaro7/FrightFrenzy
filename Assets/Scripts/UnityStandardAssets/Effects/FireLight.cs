// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Effects.FireLight
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace UnityStandardAssets.Effects
{
  public class FireLight : MonoBehaviour
  {
    private float m_Rnd;
    private bool m_Burning = true;
    private Light m_Light;

    private void Start()
    {
      this.m_Rnd = Random.value * 100f;
      this.m_Light = this.GetComponent<Light>();
    }

    private void Update()
    {
      if (!this.m_Burning)
        return;
      this.m_Light.intensity = 2f * Mathf.PerlinNoise(this.m_Rnd + Time.time, (float) ((double) this.m_Rnd + 1.0 + (double) Time.time * 1.0));
      this.transform.localPosition = Vector3.up + new Vector3(Mathf.PerlinNoise((float) ((double) this.m_Rnd + 0.0 + (double) Time.time * 2.0), (float) ((double) this.m_Rnd + 1.0 + (double) Time.time * 2.0)) - 0.5f, Mathf.PerlinNoise((float) ((double) this.m_Rnd + 2.0 + (double) Time.time * 2.0), (float) ((double) this.m_Rnd + 3.0 + (double) Time.time * 2.0)) - 0.5f, Mathf.PerlinNoise((float) ((double) this.m_Rnd + 4.0 + (double) Time.time * 2.0), (float) ((double) this.m_Rnd + 5.0 + (double) Time.time * 2.0)) - 0.5f) * 1f;
    }

    public void Extinguish()
    {
      this.m_Burning = false;
      this.m_Light.enabled = false;
    }
  }
}
