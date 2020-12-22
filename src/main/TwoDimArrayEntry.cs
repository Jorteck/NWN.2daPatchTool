using System.Collections.Generic;

namespace TwoDimArrayPatchTool
{
  public class TwoDimArrayEntry
  {
    public readonly IDictionary<string, object> Values;

    public TwoDimArrayEntry(IDictionary<string, object> values)
    {
      Values = values;
    }
  }
}
