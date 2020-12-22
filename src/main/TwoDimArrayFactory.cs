using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using CsvHelper;

namespace TwoDimArrayPatchTool
{
  public static class TwoDimArrayFactory
  {
    private const string EmptyValue = "****";

    public static TwoDimArray Load2da(string path)
    {
      TextReader textReader;
      TwoDimArrayTool tool = null;

      if (path.EndsWith(".2da"))
      {
        tool = TwoDimArrayTool.Start($"-k csv -i \"{path}\"");
        textReader = tool.StandardOutput;
      }
      else
      {
        textReader = File.OpenText(path);
      }

      using CsvReader csvReader = new CsvReader(textReader, CultureInfo.InvariantCulture);
      TwoDimArray twoDimArray = new TwoDimArray(csvReader.GetRecords<dynamic>());

      tool?.WaitAndCheckExitCode();
      return twoDimArray;
    }

    public static TwoDimArrayEntry CreateEmptyEntry(ICollection<string> columns)
    {
      IDictionary<string, object> entry = new ExpandoObject();
      foreach (string key in columns)
      {
        entry.Add(key, EmptyValue);
      }

      return new TwoDimArrayEntry(entry);
    }
  }
}
