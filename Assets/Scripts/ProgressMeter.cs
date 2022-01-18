// Decompiled with JetBrains decompiler
// Type: ProgressMeter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class ProgressMeter : MonoBehaviour
{
  private Image progressbar;
  private Canvas mycanvas;

  private void Start()
  {
    this.progressbar = this.gameObject.GetComponent<Image>();
    this.mycanvas = this.gameObject.GetComponent<Canvas>();
    this.SetProgress(0.0f);
  }

  public void SetProgress(float value)
  {
    if ((Object) this.mycanvas == (Object) null)
      return;
    if ((double) value <= 0.0)
    {
      this.progressbar.fillAmount = 0.0f;
      this.mycanvas.enabled = false;
    }
    else
    {
      this.mycanvas.enabled = true;
      this.progressbar.fillAmount = (double) value > 1.0 ? 1f : value;
    }
  }

  public void SetColor(Color newcolor)
  {
    if ((Object) this.progressbar == (Object) null)
      this.progressbar = this.gameObject.GetComponent<Image>();
    if ((Object) this.progressbar == (Object) null)
      return;
    this.progressbar.color = newcolor;
  }
}
