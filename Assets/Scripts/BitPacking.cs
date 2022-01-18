// Decompiled with JetBrains decompiler
// Type: BitPacking
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using System.Text;
using UnityEngine;

internal static class BitPacking
{
  private static Dictionary<char, char> hash_enc = new Dictionary<char, char>()
  {
    {
      '0',
      char.MinValue
    },
    {
      '1',
      '\u0001'
    },
    {
      '2',
      '\u0002'
    },
    {
      '3',
      '\u0003'
    },
    {
      '4',
      '\u0004'
    },
    {
      '5',
      '\u0005'
    },
    {
      '6',
      '\u0006'
    },
    {
      '7',
      '\a'
    },
    {
      '8',
      '\b'
    },
    {
      '9',
      '\t'
    },
    {
      '.',
      '\n'
    },
    {
      '-',
      '\v'
    },
    {
      '+',
      '\f'
    },
    {
      'e',
      '\r'
    },
    {
      'E',
      '\r'
    },
    {
      '\u0011',
      '\u000E'
    },
    {
      '\u0012',
      '\u000F'
    }
  };
  private static char[] buffer = new char[20000];

  public static byte[] Compress(string inputraw)
  {
    switch (inputraw)
    {
      case "":
      case null:
        return new byte[0];
      default:
        if (inputraw.Length > 19999)
          return Encoding.UTF8.GetBytes(inputraw.ToCharArray());
        char[] charArray = inputraw.ToCharArray();
        int length1 = charArray.Length;
        int num = 0;
        char minValue = char.MinValue;
        int length2 = 0;
        for (int index = 0; index < length1; ++index)
        {
          char ch = charArray[index];
          if (ch >= '0' && ch <= '9')
          {
            minValue += (char) ((int) ch - 48 << num++ * 4);
          }
          else
          {
            switch (ch)
            {
              case '\u0011':
                minValue += (char) (14 << num++ * 4);
                break;
              case '\u0012':
                minValue += (char) (15 << num++ * 4);
                break;
              case '+':
                minValue += (char) (12 << num++ * 4);
                break;
              case '-':
                minValue += (char) (11 << num++ * 4);
                break;
              case '.':
                minValue += (char) (10 << num++ * 4);
                break;
              case 'E':
                minValue += (char) (13 << num++ * 4);
                break;
              case 'e':
                minValue += (char) (13 << num++ * 4);
                break;
              default:
                Debug.LogError((object) ("Bit packing failed on " + inputraw));
                return Encoding.UTF8.GetBytes(inputraw.ToCharArray());
            }
          }
          if (num == 2)
          {
            BitPacking.buffer[length2++] = minValue;
            minValue = char.MinValue;
            num = 0;
          }
        }
        if (num > 0)
        {
          BitPacking.buffer[length2] = minValue;
          BitPacking.buffer[length2++] += 'à';
        }
        return MyUtils.CharToByteArray(BitPacking.buffer, length2);
    }
  }

  public static byte[] Decompress(byte[] input)
  {
    if (input == null || input.Length == 0)
      return new byte[0];
    if (input.Length > 9999)
      return input;
    int length1 = input.Length;
    int length2 = 0;
    for (int index1 = 0; index1 < length1; ++index1)
    {
      byte num1 = input[index1];
      for (int index2 = 0; index2 < 2; ++index2)
      {
        byte num2 = index2 == 0 ? (byte) ((uint) num1 & 15U) : (byte) ((int) num1 >> 4 & 15);
        if (num2 >= (byte) 0 && num2 <= (byte) 9)
        {
          BitPacking.buffer[length2++] = (char) ((uint) num2 + 48U);
        }
        else
        {
          switch (num2)
          {
            case 10:
              BitPacking.buffer[length2++] = '.';
              continue;
            case 11:
              BitPacking.buffer[length2++] = '-';
              continue;
            case 12:
              BitPacking.buffer[length2++] = '+';
              continue;
            case 13:
              BitPacking.buffer[length2++] = 'e';
              continue;
            case 14:
              BitPacking.buffer[length2++] = '\u0011';
              continue;
            case 15:
              BitPacking.buffer[length2++] = '\u0012';
              continue;
            default:
              continue;
          }
        }
      }
    }
    return MyUtils.CharToByteArray(BitPacking.buffer, length2);
  }
}
