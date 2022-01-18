// Decompiled with JetBrains decompiler
// Type: ShowSliderValue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof (Text))]
public class ShowSliderValue : MonoBehaviour
{
  public void UpdateLabel(float value)
  {
    Text component = this.GetComponent<Text>();
    if (!((Object) component != (Object) null))
      return;
    component.text = Mathf.RoundToInt(value * 100f).ToString() + "%";
  }
}
