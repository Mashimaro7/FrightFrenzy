// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Utility.FPSCounter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

namespace UnityStandardAssets.Utility
{
  [RequireComponent(typeof (Text))]
  public class FPSCounter : MonoBehaviour
  {
    private const float fpsMeasurePeriod = 0.5f;
    private int m_FpsAccumulator;
    private float m_FpsNextPeriod;
    private int m_CurrentFps;
    private const string display = "{0} FPS";
    private Text m_Text;

    private void Start()
    {
      this.m_FpsNextPeriod = Time.realtimeSinceStartup + 0.5f;
      this.m_Text = this.GetComponent<Text>();
    }

    private void Update()
    {
      ++this.m_FpsAccumulator;
      if ((double) Time.realtimeSinceStartup <= (double) this.m_FpsNextPeriod)
        return;
      this.m_CurrentFps = (int) ((double) this.m_FpsAccumulator / 0.5);
      this.m_FpsAccumulator = 0;
      this.m_FpsNextPeriod += 0.5f;
      this.m_Text.text = string.Format("{0} FPS", (object) this.m_CurrentFps);
    }
  }
}
