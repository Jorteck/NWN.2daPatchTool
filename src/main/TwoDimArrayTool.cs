using System;
using System.Diagnostics;
using System.IO;

namespace TwoDimArrayPatcher
{
  public class TwoDimArrayTool
  {
    private const string ToolCommand = "nwn_twoda";

    private Process process;

    public StreamWriter StandardInput => process.StandardInput;
    public StreamReader StandardOutput => process.StandardOutput;

    public static TwoDimArrayTool Start(string arguments)
    {
      Process process = new Process();
      process.StartInfo.UseShellExecute = false;
      process.StartInfo.CreateNoWindow = false;
      process.StartInfo.RedirectStandardInput = true;
      process.StartInfo.RedirectStandardOutput = true;
      process.StartInfo.RedirectStandardError = true;
      process.StartInfo.FileName = ToolCommand;
      process.StartInfo.Arguments = arguments;

      TwoDimArrayTool twoDimArrayTool = new TwoDimArrayTool
      {
        process = process
      };

      process.Start();
      return twoDimArrayTool;
    }

    public void WaitAndCheckExitCode()
    {
      process.StandardInput.Close();
      process.WaitForExit();

      if (process.ExitCode != 0)
      {
        throw new Exception($"Failed to run: {process.StartInfo.FileName} {process.StartInfo.Arguments}\n" +
          $"Exit Code {process.ExitCode}: {process.StandardError.ReadToEnd()}");
      }
    }
  }
}
