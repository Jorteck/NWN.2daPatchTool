using System.Collections.Generic;
using CommandLine;

namespace TwoDimArrayPatcher
{
  public class CLIOptions
  {
    [Option('i', "source", Required = true, HelpText = "2da file to be patched.")]
    public string SourceFile { get; set; }

    [Option('p', "patch", Required = true, HelpText = "Patch csv/2da to apply.")]
    public IEnumerable<string> PatchFiles { get; set; }

    [Option('o', "output", Required = true, HelpText = "Merged 2da output path.")]
    public string DestFile { get; set; }
  }
}
