using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CsvHelper;

namespace TwoDimArrayPatcher
{
  public class Patcher
  {
    private readonly CLIOptions options;

    public Patcher(CLIOptions options)
    {
      this.options = options;
    }

    public void Run()
    {
      TwoDimArray source = TwoDimArrayFactory.Load2da(options.SourceFile);
      foreach (string patchPath in options.PatchFiles)
      {
        Console.WriteLine($"Processing patch: {patchPath}");
        TwoDimArray patch = TwoDimArrayFactory.Load2da(patchPath);
        ProcessPatch(source, patch);
        Console.WriteLine($"Patch processed: {patchPath}");
      }

      ExportResult(source);
    }

    private void ProcessPatch(TwoDimArray source, TwoDimArray patch)
    {
      foreach (TwoDimArrayEntry entry in patch.Entries)
      {
        string command = (string)entry.Values["PATCH"];
        int index = Convert.ToInt32(entry.Values["ID"]);

        switch (command.ToUpperInvariant())
        {
          case "ADD":
            DoAdd(source, index, entry);
            break;
          case "REPLACE":
            DoReplace(source.Entries[index], entry);
            break;
          case "REMOVE":
            DoRemove(source, index);
            break;
        }
      }
    }

    private void ExportResult(TwoDimArray result)
    {
      Console.WriteLine($"Exporting patched 2da to \"{options.DestFile}\"");
      TwoDimArrayTool tool = TwoDimArrayTool.Start($"-l csv -o \"{options.DestFile}\"");

      using (CsvWriter csvWriter = new CsvWriter(tool.StandardInput, CultureInfo.InvariantCulture))
      {
        csvWriter.WriteRecords(result.Entries.Select(entry => entry.Values).Cast<object>());
      }

      tool.WaitAndCheckExitCode();
    }

    private void DoAdd(TwoDimArray source, int index, TwoDimArrayEntry entryToAdd)
    {
      if (index < source.Entries.Count)
      {
        Console.WriteLine($"ADD patch entry at index {index} overwrites an existing value. Use REPLACE if replacement is intentional. Skipping...");
        return;
      }

      if (index > source.Entries.Count)
      {
        TwoDimArrayEntry padding = TwoDimArrayFactory.CreateEmptyEntry(source.Entries[0].Values.Keys);
        for (int i = source.Entries.Count - 1; i < index; i++)
        {
          source.Entries.Add(padding);
        }
      }

      entryToAdd.Values.Remove("ID");
      entryToAdd.Values.Remove("PATCH");
      source.Entries.Add(entryToAdd);
    }

    private void DoReplace(TwoDimArrayEntry source, TwoDimArrayEntry patch)
    {
      foreach (KeyValuePair<string,object> entry in patch.Values)
      {
        if (entry.Key == "PATCH" || entry.Key == "ID")
        {
          continue;
        }

        if (!source.Values.ContainsKey(entry.Key))
        {
          Console.WriteLine($"Source 2da does not contain column {entry.Key}, and will be skipped.");
          continue;
        }

        source.Values[entry.Key] = entry.Value;
      }
    }

    private void DoRemove(TwoDimArray source, int index)
    {
      source.Entries.RemoveAt(index);
    }
  }
}
