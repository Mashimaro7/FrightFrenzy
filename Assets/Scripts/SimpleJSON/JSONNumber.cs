// Decompiled with JetBrains decompiler
// Type: SimpleJSON.JSONNumber
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace SimpleJSON
{
  public class JSONNumber : JSONNode
  {
    private double m_Data;

    public override JSONNodeType Tag => JSONNodeType.Number;

    public override bool IsNumber => true;

    public override JSONNode.Enumerator GetEnumerator() => new JSONNode.Enumerator();

    public override string Value
    {
      get => this.m_Data.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      set
      {
        double result;
        if (!double.TryParse(value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result))
          return;
        this.m_Data = result;
      }
    }

    public override double AsDouble
    {
      get => this.m_Data;
      set => this.m_Data = value;
    }

    public override long AsLong
    {
      get => (long) this.m_Data;
      set => this.m_Data = (double) value;
    }

    public JSONNumber(double aData) => this.m_Data = aData;

    public JSONNumber(string aData) => this.Value = aData;

    public override JSONNode Clone() => (JSONNode) new JSONNumber(this.m_Data);

    internal override void WriteToStringBuilder(
      StringBuilder aSB,
      int aIndent,
      int aIndentInc,
      JSONTextMode aMode)
    {
      aSB.Append(this.Value);
    }

    private static bool IsNumeric(object value)
    {
      switch (value)
      {
        case int _:
        case uint _:
        case float _:
        case double _:
        case Decimal _:
        case long _:
        case ulong _:
        case short _:
        case ushort _:
        case sbyte _:
          return true;
        default:
          return value is byte;
      }
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if (base.Equals(obj))
        return true;
      JSONNumber jsonNumber = obj as JSONNumber;
      if ((JSONNode) jsonNumber != (object) null)
        return this.m_Data == jsonNumber.m_Data;
      return JSONNumber.IsNumeric(obj) && Convert.ToDouble(obj) == this.m_Data;
    }

    public override int GetHashCode() => this.m_Data.GetHashCode();

    public override void SerializeBinary(BinaryWriter aWriter)
    {
      aWriter.Write((byte) 4);
      aWriter.Write(this.m_Data);
    }
  }
}
