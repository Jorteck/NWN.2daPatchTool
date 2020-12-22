using System.Collections.Generic;

namespace TwoDimArrayPatchTool
{
  public class TwoDimArray
  {
    public readonly List<TwoDimArrayEntry> Entries = new List<TwoDimArrayEntry>();

    public TwoDimArray(IEnumerable<dynamic> entries)
    {
      foreach (IDictionary<string,object> entry in entries)
      {
        Entries.Add(new TwoDimArrayEntry(entry));
      }
    }
  }
}
