// Decompiled with JetBrains decompiler
// Type: TMPro_LicenseValidator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "TMPro_LicenseValidator", menuName = "TMPro License Entry Validator")]
public class TMPro_LicenseValidator : TMP_InputValidator
{
  public override char Validate(ref string text, ref int pos, char ch)
  {
    if (!char.IsDigit(ch) && !char.IsLetter(ch) || pos >= 19)
      return char.MinValue;
    if (char.IsLetter(ch))
      ch = char.ToUpper(ch);
    if (text.Length == 4 || text.Length == 9 || text.Length == 14)
      text += "-";
    if (pos == 4 || pos == 9 || pos == 14)
      ++pos;
    if (pos < text.Length)
    {
      text = text.Insert(pos++, ch.ToString());
    }
    else
    {
      text += ch.ToString();
      ++pos;
    }
    if (pos <= text.Length - 1)
    {
      text = text.Remove(pos, 1);
      return ch;
    }
    if (text.Length == 4 || text.Length == 9 || text.Length == 14)
      text += "-";
    return ch;
  }
}
