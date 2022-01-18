// Decompiled with JetBrains decompiler
// Type: CodeMonkey.Assets
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace CodeMonkey
{
  public class Assets : MonoBehaviour
  {
    private static Assets _i;
    public Sprite s_White;
    public Material m_White;

    public static Assets i
    {
      get
      {
        if ((Object) Assets._i == (Object) null)
          Assets._i = Object.Instantiate<Assets>(Resources.Load<Assets>("CodeMonkeyAssets"));
        return Assets._i;
      }
    }
  }
}
