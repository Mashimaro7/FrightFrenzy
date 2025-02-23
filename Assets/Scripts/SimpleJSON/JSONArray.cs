﻿// Decompiled with JetBrains decompiler
// Type: SimpleJSON.JSONArray
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SimpleJSON
{
  public class JSONArray : JSONNode
  {
    private List<JSONNode> m_List = new List<JSONNode>();
    private bool inline;

    public override bool Inline
    {
      get => this.inline;
      set => this.inline = value;
    }

    public override JSONNodeType Tag => JSONNodeType.Array;

    public override bool IsArray => true;

    public override JSONNode.Enumerator GetEnumerator() => new JSONNode.Enumerator(this.m_List.GetEnumerator());

    public override JSONNode this[int aIndex]
    {
      get => aIndex < 0 || aIndex >= this.m_List.Count ? (JSONNode) new JSONLazyCreator((JSONNode) this) : this.m_List[aIndex];
      set
      {
        if (value == (object) null)
          value = (JSONNode) JSONNull.CreateOrGet();
        if (aIndex < 0 || aIndex >= this.m_List.Count)
          this.m_List.Add(value);
        else
          this.m_List[aIndex] = value;
      }
    }

    public override JSONNode this[string aKey]
    {
      get => (JSONNode) new JSONLazyCreator((JSONNode) this);
      set
      {
        if (value == (object) null)
          value = (JSONNode) JSONNull.CreateOrGet();
        this.m_List.Add(value);
      }
    }

    public override int Count => this.m_List.Count;

    public override void Add(string aKey, JSONNode aItem)
    {
      if (aItem == (object) null)
        aItem = (JSONNode) JSONNull.CreateOrGet();
      this.m_List.Add(aItem);
    }

    public override JSONNode Remove(int aIndex)
    {
      if (aIndex < 0 || aIndex >= this.m_List.Count)
        return (JSONNode) null;
      JSONNode jsonNode = this.m_List[aIndex];
      this.m_List.RemoveAt(aIndex);
      return jsonNode;
    }

    public override JSONNode Remove(JSONNode aNode)
    {
      this.m_List.Remove(aNode);
      return aNode;
    }

    public override JSONNode Clone()
    {
      JSONArray jsonArray = new JSONArray();
      jsonArray.m_List.Capacity = this.m_List.Capacity;
      foreach (JSONNode jsonNode in this.m_List)
      {
        if (jsonNode != (object) null)
          jsonArray.Add(jsonNode.Clone());
        else
          jsonArray.Add((JSONNode) null);
      }
      return (JSONNode) jsonArray;
    }

    public override IEnumerable<JSONNode> Children
    {
      get
      {
        foreach (JSONNode jsonNode in this.m_List)
          yield return jsonNode;
      }
    }

    internal override void WriteToStringBuilder(
      StringBuilder aSB,
      int aIndent,
      int aIndentInc,
      JSONTextMode aMode)
    {
      aSB.Append('[');
      int count = this.m_List.Count;
      if (this.inline)
        aMode = JSONTextMode.Compact;
      for (int index = 0; index < count; ++index)
      {
        if (index > 0)
          aSB.Append(',');
        if (aMode == JSONTextMode.Indent)
          aSB.AppendLine();
        if (aMode == JSONTextMode.Indent)
          aSB.Append(' ', aIndent + aIndentInc);
        this.m_List[index].WriteToStringBuilder(aSB, aIndent + aIndentInc, aIndentInc, aMode);
      }
      if (aMode == JSONTextMode.Indent)
        aSB.AppendLine().Append(' ', aIndent);
      aSB.Append(']');
    }

    public override void SerializeBinary(BinaryWriter aWriter)
    {
      aWriter.Write((byte) 1);
      aWriter.Write(this.m_List.Count);
      for (int index = 0; index < this.m_List.Count; ++index)
        this.m_List[index].SerializeBinary(aWriter);
    }
  }
}
