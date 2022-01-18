// Decompiled with JetBrains decompiler
// Type: GUI_IncField
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class GUI_IncField : MonoBehaviour
{
  private void Start()
  {
  }

  private void Update()
  {
  }

  public void IncrementField()
  {
    InputField component = this.GetComponent<InputField>();
    if (!(bool) (Object) component)
      return;
    component.text = (int.Parse(component.text) + 1).ToString();
  }

  public void DecrementField()
  {
    InputField component = this.GetComponent<InputField>();
    if (!(bool) (Object) component)
      return;
    component.text = (int.Parse(component.text) - 1).ToString();
  }
}
