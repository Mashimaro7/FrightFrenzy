// Decompiled with JetBrains decompiler
// Type: Assets.SimpleZip.Zip
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using Ionic.Zlib;
using System;
using System.Text;

namespace Assets.SimpleZip
{
  public static class Zip
  {
    public static byte[] Compress(string text) => ZlibStream.CompressString(text);

    public static string CompressToString(string text) => Convert.ToBase64String(Zip.Compress(text));

    public static string Decompress(byte[] bytes) => Encoding.UTF8.GetString(ZlibStream.UncompressBuffer(bytes));

    public static string Decompress(string data) => Zip.Decompress(Convert.FromBase64String(data));
  }
}
