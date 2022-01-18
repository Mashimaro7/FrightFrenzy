// Decompiled with JetBrains decompiler
// Type: loadText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class loadText : MonoBehaviour
{
  public string txtFile = "test";
  private string fileContents;
  private string[] fileLines;

  private void Start()
  {
  }

  private void Update()
  {
  }

  private void OnGUI()
  {
    this.fileContents = ((TextAsset) Resources.Load(this.txtFile)).text;
    this.fileLines = this.fileContents.Split(new string[1]
    {
      Environment.NewLine
    }, StringSplitOptions.None);
    GUILayout.Label(this.fileLines[0]);
  }
}
