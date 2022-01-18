// Decompiled with JetBrains decompiler
// Type: UnityStandardAssets.Utility.SimpleActivatorMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace UnityStandardAssets.Utility
{
  public class SimpleActivatorMenu : MonoBehaviour
  {
    public GUIText camSwitchButton;
    public GameObject[] objects;
    private int m_CurrentActiveObject;

    private void OnEnable()
    {
      this.m_CurrentActiveObject = 0;
      this.camSwitchButton.text = this.objects[this.m_CurrentActiveObject].name;
    }

    public void NextCamera()
    {
      int num = this.m_CurrentActiveObject + 1 >= this.objects.Length ? 0 : this.m_CurrentActiveObject + 1;
      for (int index = 0; index < this.objects.Length; ++index)
        this.objects[index].SetActive(index == num);
      this.m_CurrentActiveObject = num;
      this.camSwitchButton.text = this.objects[this.m_CurrentActiveObject].name;
    }
  }
}
