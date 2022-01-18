// Decompiled with JetBrains decompiler
// Type: SimpleJSON.JSONString
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.IO;
using System.Text;

namespace SimpleJSON
{
  public class JSONString : JSONNode
  {
    private string m_Data;

    public override JSONNodeType Tag => JSONNodeType.String;

    public override bool IsString => true;

    public override JSONNode.Enumerator GetEnumerator() => new JSONNode.Enumerator();

    public override string Value
    {
      get => this.m_Data;
      set => this.m_Data = value;
    }

    public JSONString(string aData) => this.m_Data = aData;

    public override JSONNode Clone() => (JSONNode) new JSONString(this.m_Data);

    internal override void WriteToStringBuilder(
      StringBuilder aSB,
      int aIndent,
      int aIndentInc,
      JSONTextMode aMode)
    {
      aSB.Append('"').Append(JSONNode.Escape(this.m_Data)).Append('"');
    }

    public override bool Equals(object obj)
    {
      if (base.Equals(obj))
        return true;
      if (obj is string str)
        return this.m_Data == str;
      JSONString jsonString = obj as JSONString;
      return (JSONNode) jsonString != (object) null && this.m_Data == jsonString.m_Data;
    }

    public override int GetHashCode() => this.m_Data.GetHashCode();

    public override void SerializeBinary(BinaryWriter aWriter)
    {
      aWriter.Write((byte) 3);
      aWriter.Write(this.m_Data);
    }
  }
}
