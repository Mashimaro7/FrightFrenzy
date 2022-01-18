// Decompiled with JetBrains decompiler
// Type: DuckStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class DuckStates : MonoBehaviour
{
  public bool red;
  public bool blue;
  public DuckStates.DuckPositions mystate = DuckStates.DuckPositions.NonCarousel;

  public enum DuckPositions
  {
    OffField,
    Placed,
    Touched,
    Scored,
    NonCarousel,
  }
}
