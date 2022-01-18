// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Utility.DynamicShadowSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace UnityStandardAssets.Utility
{
  public class DynamicShadowSettings : MonoBehaviour
  {
    public Light sunLight;
    public float minHeight = 10f;
    public float minShadowDistance = 80f;
    public float minShadowBias = 1f;
    public float maxHeight = 1000f;
    public float maxShadowDistance = 10000f;
    public float maxShadowBias = 0.1f;
    public float adaptTime = 1f;
    private float m_SmoothHeight;
    private float m_ChangeSpeed;
    private float m_OriginalStrength = 1f;

    private void Start() => this.m_OriginalStrength = this.sunLight.shadowStrength;

    private void Update()
    {
      Ray ray = new Ray(Camera.main.transform.position, -Vector3.up);
      float target = this.transform.position.y;
      RaycastHit raycastHit;
      ref RaycastHit local = ref raycastHit;
      if (Physics.Raycast(ray, out local))
        target = raycastHit.distance;
      if ((double) Mathf.Abs(target - this.m_SmoothHeight) > 1.0)
        this.m_SmoothHeight = Mathf.SmoothDamp(this.m_SmoothHeight, target, ref this.m_ChangeSpeed, this.adaptTime);
      float t = Mathf.InverseLerp(this.minHeight, this.maxHeight, this.m_SmoothHeight);
      QualitySettings.shadowDistance = Mathf.Lerp(this.minShadowDistance, this.maxShadowDistance, t);
      this.sunLight.shadowBias = Mathf.Lerp(this.minShadowBias, this.maxShadowBias, (float) (1.0 - (1.0 - (double) t) * (1.0 - (double) t)));
      this.sunLight.shadowStrength = Mathf.Lerp(this.m_OriginalStrength, 0.0f, t);
    }
  }
}
