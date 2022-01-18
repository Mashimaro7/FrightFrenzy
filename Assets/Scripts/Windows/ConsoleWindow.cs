// Decompiled with JetBrains decompiler
// Type: Windows.ConsoleWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CAF5CC1D-7384-442D-BD63-2B15CA6E7486
// Assembly location: C:\Program Files (x86)\xRC Simulator\xRC Simulator_Data\Managed\Assembly-CSharp.dll

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

namespace Windows
{
  [SuppressUnmanagedCodeSecurity]
  public class ConsoleWindow
  {
    private TextWriter oldOutput;
    private StreamWriter standardOutput;
    private const int STD_INPUT_HANDLE = -10;
    private const int STD_OUTPUT_HANDLE = -11;

    public void Initialize()
    {
      if (!ConsoleWindow.AttachConsole(uint.MaxValue))
        ConsoleWindow.AllocConsole();
      this.oldOutput = Console.Out;
    }

    public void MyWrite(string msg) => Console.WriteLine(msg);

    public void Shutdown() => ConsoleWindow.FreeConsole();

    public void SetTitle(string strName) => ConsoleWindow.SetConsoleTitleA(strName);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool AttachConsole(uint dwProcessId);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool AllocConsole();

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool FreeConsole();

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    private static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("kernel32.dll")]
    private static extern bool SetConsoleTitleA(string lpConsoleTitle);
  }
}
