// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.SceneUtils.SlowMoButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

namespace UnityStandardAssets.SceneUtils
{
  public class SlowMoButton : MonoBehaviour
  {
    public Sprite FullSpeedTex;
    public Sprite SlowSpeedTex;
    public float fullSpeed = 1f;
    public float slowSpeed = 0.3f;
    public Button button;
    private bool m_SlowMo;

    private void Start() => this.m_SlowMo = false;

    private void OnDestroy() => Time.timeScale = 1f;

    public void ChangeSpeed()
    {
      this.m_SlowMo = !this.m_SlowMo;
      Image targetGraphic = this.button.targetGraphic as Image;
      if ((Object) targetGraphic != (Object) null)
        targetGraphic.sprite = this.m_SlowMo ? this.SlowSpeedTex : this.FullSpeedTex;
      this.button.targetGraphic = (Graphic) targetGraphic;
      Time.timeScale = this.m_SlowMo ? this.slowSpeed : this.fullSpeed;
    }
  }
}
