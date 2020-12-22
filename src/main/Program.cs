using System;
using System.IO;
using CommandLine;

namespace TwoDimArrayPatchTool
{
  public static class Program
  {
    private static void Main(string[] args)
    {
      Parser.Default.ParseArguments<CLIOptions>(args)
        .WithParsed(Run);
    }

    private static void Run(CLIOptions options)
    {
      if (!ValidateOptions(options))
      {
        Environment.ExitCode = -1;
        return;
      }

      Patcher patcher = new Patcher(options);
      patcher.Run();
    }

    private static bool ValidateOptions(CLIOptions options)
    {
      if (!File.Exists(options.SourceFile))
      {
        Console.Error.WriteLine($"Source file not found: {options.SourceFile}");
        return false;
      }

      foreach (string patchPath in options.PatchFiles)
      {
        if (!File.Exists(patchPath))
        {
          Console.Error.WriteLine($"Patch file not found: {options.PatchFiles}");
          return false;
        }
      }

      return true;
    }
  }
}
