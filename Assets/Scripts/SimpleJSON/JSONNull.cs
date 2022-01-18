// Decompiled with JetBrains decompiler
// Type: SimpleJSON.JSONNull
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.IO;
using System.Text;

namespace SimpleJSON
{
  public class JSONNull : JSONNode
  {
    private static JSONNull m_StaticInstance = new JSONNull();
    public static bool reuseSameInstance = true;

    public static JSONNull CreateOrGet() => JSONNull.reuseSameInstance ? JSONNull.m_StaticInstance : new JSONNull();

    private JSONNull()
    {
    }

    public override JSONNodeType Tag => JSONNodeType.NullValue;

    public override bool IsNull => true;

    public override JSONNode.Enumerator GetEnumerator() => new JSONNode.Enumerator();

    public override string Value
    {
      get => "null";
      set
      {
      }
    }

    public override bool AsBool
    {
      get => false;
      set
      {
      }
    }

    public override JSONNode Clone() => (JSONNode) JSONNull.CreateOrGet();

    public override bool Equals(object obj) => this == obj || obj is JSONNull;

    public override int GetHashCode() => 0;

    internal override void WriteToStringBuilder(
      StringBuilder aSB,
      int aIndent,
      int aIndentInc,
      JSONTextMode aMode)
    {
      aSB.Append("null");
    }

    public override void SerializeBinary(BinaryWriter aWriter) => aWriter.Write((byte) 5);
  }
}
