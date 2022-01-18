// Decompiled with JetBrains decompiler
// Type: ChangeColor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeColor : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
  private void OnEnable()
  {
  }

  public void SetRed(float value) => this.OnValueChanged(value, 0);

  public void SetGreen(float value) => this.OnValueChanged(value, 1);

  public void SetBlue(float value) => this.OnValueChanged(value, 2);

  public void OnValueChanged(float value, int channel)
  {
    Color color = Color.white;
    if ((Object) this.GetComponent<Renderer>() != (Object) null)
      color = this.GetComponent<Renderer>().material.color;
    else if ((Object) this.GetComponent<Light>() != (Object) null)
      color = this.GetComponent<Light>().color;
    color[channel] = value;
    if ((Object) this.GetComponent<Renderer>() != (Object) null)
    {
      this.GetComponent<Renderer>().material.color = color;
    }
    else
    {
      if (!((Object) this.GetComponent<Light>() != (Object) null))
        return;
      this.GetComponent<Light>().color = color;
    }
  }

  public void OnPointerClick(PointerEventData data)
  {
    if ((Object) this.GetComponent<Renderer>() != (Object) null)
    {
      this.GetComponent<Renderer>().material.color = new Color(Random.value, Random.value, Random.value, 1f);
    }
    else
    {
      if (!((Object) this.GetComponent<Light>() != (Object) null))
        return;
      this.GetComponent<Light>().color = new Color(Random.value, Random.value, Random.value, 1f);
    }
  }
}
