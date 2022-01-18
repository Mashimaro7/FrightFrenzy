// Decompiled with JetBrains decompiler
// Type: Assets.SimpleZip.Example
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.SimpleZip
{
  public class Example : MonoBehaviour
  {
    public Text Text;

    public void Start()
    {
      try
      {
        string str1 = "El perro de San Roque no tiene rabo porque Ramón Rodríguez se lo ha robado.";
        string text = str1 + str1 + str1 + str1 + str1;
        string data = Zip.CompressToString(text);
        string str2 = Zip.Decompress(data);
        this.Text.text = string.Format("Plain text: {0}\n\nCompressed: {1}\n\nDecompressed: {2}", (object) text, (object) data, (object) str2);
      }
      catch (Exception ex)
      {
        this.Text.text = ex.ToString();
      }
    }
  }
}
