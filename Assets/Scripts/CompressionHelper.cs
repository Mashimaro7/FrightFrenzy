// Decompiled with JetBrains decompiler
// Type: CompressionHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public static class CompressionHelper
{
  private static readonly uint HLOG = 14;
  private static readonly uint HSIZE = 16384;
  private static readonly uint MAX_LIT = 32;
  private static readonly uint MAX_OFF = 8192;
  private static readonly uint MAX_REF = 264;
  private static readonly long[] HashTable = new long[(int) CompressionHelper.HSIZE];
  private static byte[] tempBuffer = new byte[60000];

  public static byte[] Compress(string inputBytes)
  {
    switch (inputBytes)
    {
      case "":
      case null:
        return new byte[0];
      default:
        return CompressionHelper.Compress(MyUtils.CharToByteArray(inputBytes.ToCharArray(), inputBytes.Length));
    }
  }

  public static byte[] Compress(byte[] inputBytes)
  {
    if (inputBytes == null || inputBytes.Length == 0)
      return new byte[0];
    int count = CompressionHelper.lzf_compress(inputBytes, ref CompressionHelper.tempBuffer);
    if (count == 0)
    {
      Debug.LogError((object) ("lzf_compress failed. Input length = " + (object) inputBytes.Length));
      return new byte[0];
    }
    byte[] dst = new byte[count];
    Buffer.BlockCopy((Array) CompressionHelper.tempBuffer, 0, (Array) dst, 0, count);
    return dst;
  }

  public static byte[] Decompress(byte[] inputBytes)
  {
    if (inputBytes == null || inputBytes.Length == 0)
      return new byte[0];
    int count = CompressionHelper.lzf_decompress(inputBytes, ref CompressionHelper.tempBuffer);
    if (count == 0)
    {
      Debug.LogError((object) ("lzf_decompress failed. Input Length = " + (object) inputBytes.Length));
      return new byte[0];
    }
    byte[] dst = new byte[count];
    Buffer.BlockCopy((Array) CompressionHelper.tempBuffer, 0, (Array) dst, 0, count);
    return dst;
  }

  public static int lzf_compress(byte[] input, ref byte[] output)
  {
    int length1 = input.Length;
    int length2 = output.Length;
    if (length1 == 0 || length2 == 0)
      return 0;
    if (length1 == 1)
    {
      output[0] = input[0];
      return 1;
    }
    Array.Clear((Array) CompressionHelper.HashTable, 0, (int) CompressionHelper.HSIZE);
    uint index1 = 0;
    uint num1 = 0;
    uint num2 = (uint) input[(int) index1] << 8 | (uint) input[(int) index1 + 1];
    int num3 = 0;
label_5:
    do
    {
      if ((long) index1 < (long) (length1 - 2))
      {
        num2 = num2 << 8 | (uint) input[(int) index1 + 2];
        long index2 = (long) ((num2 ^ num2 << 5) >> 24 - (int) CompressionHelper.HLOG - (int) num2 * 5 & CompressionHelper.HSIZE - 1U);
        long index3 = CompressionHelper.HashTable[index2];
        CompressionHelper.HashTable[index2] = (long) index1;
        long num4;
        if ((num4 = (long) index1 - index3 - 1L) < (long) CompressionHelper.MAX_OFF && (long) (index1 + 4U) < (long) length1 && index3 > 0L && (int) input[index3] == (int) input[(int) index1] && (int) input[index3 + 1L] == (int) input[(int) index1 + 1] && (int) input[index3 + 2L] == (int) input[(int) index1 + 2])
        {
          uint num5 = 2;
          uint num6 = (uint) length1 - index1 - num5;
          uint num7 = num6 > CompressionHelper.MAX_REF ? CompressionHelper.MAX_REF : num6;
          if ((long) num1 + (long) num3 + 1L + 3L >= (long) length2)
            return 0;
          do
          {
            ++num5;
          }
          while (num5 < num7 && (int) input[index3 + (long) num5] == (int) input[(int) index1 + (int) num5]);
          if (num3 != 0)
          {
            output[(int) num1++] = (byte) (num3 - 1);
            num3 = -num3;
            do
            {
              output[(int) num1++] = input[(long) index1 + (long) num3];
            }
            while (++num3 != 0);
          }
          uint num8 = num5 - 2U;
          uint num9 = index1 + 1U;
          uint num10;
          if (num8 < 7U)
          {
            byte[] numArray = output;
            int index4 = (int) num1;
            num10 = (uint) (index4 + 1);
            int num11 = (int) (byte) ((ulong) (num4 >> 8) + (ulong) (num8 << 5));
            numArray[index4] = (byte) num11;
          }
          else
          {
            byte[] numArray1 = output;
            int index5 = (int) num1;
            uint num12 = (uint) (index5 + 1);
            int num13 = (int) (byte) ((ulong) (num4 >> 8) + 224UL);
            numArray1[index5] = (byte) num13;
            byte[] numArray2 = output;
            int index6 = (int) num12;
            num10 = (uint) (index6 + 1);
            int num14 = (int) (byte) (num8 - 7U);
            numArray2[index6] = (byte) num14;
          }
          byte[] numArray3 = output;
          int index7 = (int) num10;
          num1 = (uint) (index7 + 1);
          int num15 = (int) (byte) num4;
          numArray3[index7] = (byte) num15;
          uint index8 = num9 + (num8 - 1U);
          uint num16 = ((uint) input[(int) index8] << 8 | (uint) input[(int) index8 + 1]) << 8 | (uint) input[(int) index8 + 2];
          CompressionHelper.HashTable[(int) ((num16 ^ num16 << 5) >> 24 - (int) CompressionHelper.HLOG - (int) num16 * 5) & (int) CompressionHelper.HSIZE - 1] = (long) index8;
          uint num17 = index8 + 1U;
          num2 = num16 << 8 | (uint) input[(int) num17 + 2];
          CompressionHelper.HashTable[(int) ((num2 ^ num2 << 5) >> 24 - (int) CompressionHelper.HLOG - (int) num2 * 5) & (int) CompressionHelper.HSIZE - 1] = (long) num17;
          index1 = num17 + 1U;
          continue;
        }
      }
      else if ((long) index1 == (long) length1)
        goto label_23;
      ++num3;
      ++index1;
    }
    while ((long) num3 != (long) CompressionHelper.MAX_LIT);
    if ((long) (num1 + 1U + CompressionHelper.MAX_LIT) >= (long) length2)
      return 0;
    output[(int) num1++] = (byte) (CompressionHelper.MAX_LIT - 1U);
    num3 = -num3;
    do
    {
      output[(int) num1++] = input[(long) index1 + (long) num3];
    }
    while (++num3 != 0);
    goto label_5;
label_23:
    if (num3 != 0)
    {
      if ((long) num1 + (long) num3 + 1L >= (long) length2)
        return 0;
      output[(int) num1++] = (byte) (num3 - 1);
      int num18 = -num3;
      do
      {
        output[(int) num1++] = input[(long) index1 + (long) num18];
      }
      while (++num18 != 0);
    }
    return (int) num1;
  }

  public static int lzf_decompress(byte[] input, ref byte[] output)
  {
    int length1 = input.Length;
    int length2 = output.Length;
    if (length1 == 0 || length2 == 0)
      return 0;
    if (length1 == 1)
    {
      output[0] = input[0];
      return 1;
    }
    uint num1 = 0;
    uint num2 = 0;
    do
    {
      uint num3 = (uint) input[(int) num1++];
      if (num3 < 32U)
      {
        uint num4 = num3 + 1U;
        if ((long) (num2 + num4) > (long) length2)
          return 0;
        do
        {
          output[(int) num2++] = input[(int) num1++];
        }
        while (--num4 != 0U);
      }
      else
      {
        uint num5 = num3 >> 5;
        int num6 = (int) num2 - (((int) num3 & 31) << 8) - 1;
        if (num5 == 7U)
          num5 += (uint) input[(int) num1++];
        int num7 = num6 - (int) input[(int) num1++];
        if ((long) (uint) ((int) num2 + (int) num5 + 2) > (long) length2 || num7 < 0)
          return 0;
        byte[] numArray1 = output;
        int index1 = (int) num2;
        uint num8 = (uint) (index1 + 1);
        byte[] numArray2 = output;
        int index2 = num7;
        int num9 = index2 + 1;
        int num10 = (int) numArray2[index2];
        numArray1[index1] = (byte) num10;
        byte[] numArray3 = output;
        int index3 = (int) num8;
        num2 = (uint) (index3 + 1);
        byte[] numArray4 = output;
        int index4 = num9;
        int num11 = index4 + 1;
        int num12 = (int) numArray4[index4];
        numArray3[index3] = (byte) num12;
        do
        {
          output[(int) num2++] = output[num11++];
        }
        while (--num5 != 0U);
      }
    }
    while ((long) num1 < (long) length1);
    return (int) num2;
  }
}
