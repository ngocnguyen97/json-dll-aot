// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.MemoryTraceWriter
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Newtonsoft.Json.Serialization
{
  /// <summary>
  /// Represents a trace writer that writes to memory. When the trace message limit is
  /// reached then old trace messages will be removed as new messages are added.
  /// </summary>
  [Preserve]
  public class MemoryTraceWriter : ITraceWriter
  {
    private readonly Queue<string> _traceMessages;

    /// <summary>
    /// Gets the <see cref="T:System.Diagnostics.TraceLevel" /> that will be used to filter the trace messages passed to the writer.
    /// For example a filter level of <code>Info</code> will exclude <code>Verbose</code> messages and include <code>Info</code>,
    /// <code>Warning</code> and <code>Error</code> messages.
    /// </summary>
    /// <value>
    /// The <see cref="T:System.Diagnostics.TraceLevel" /> that will be used to filter the trace messages passed to the writer.
    /// </value>
    public TraceLevel LevelFilter { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Serialization.MemoryTraceWriter" /> class.
    /// </summary>
    public MemoryTraceWriter()
    {
      this.LevelFilter = TraceLevel.Verbose;
      this._traceMessages = new Queue<string>();
    }

    /// <summary>
    /// Writes the specified trace level, message and optional exception.
    /// </summary>
    /// <param name="level">The <see cref="T:System.Diagnostics.TraceLevel" /> at which to write this trace.</param>
    /// <param name="message">The trace message.</param>
    /// <param name="ex">The trace exception. This parameter is optional.</param>
    public void Trace(TraceLevel level, string message, Exception ex)
    {
      if (this._traceMessages.Count >= 1000)
        this._traceMessages.Dequeue();
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff", (IFormatProvider) CultureInfo.InvariantCulture));
      stringBuilder.Append(" ");
      stringBuilder.Append(level.ToString("g"));
      stringBuilder.Append(" ");
      stringBuilder.Append(message);
      this._traceMessages.Enqueue(stringBuilder.ToString());
    }

    /// <summary>
    /// Returns an enumeration of the most recent trace messages.
    /// </summary>
    /// <returns>An enumeration of the most recent trace messages.</returns>
    public IEnumerable<string> GetTraceMessages()
    {
      return (IEnumerable<string>) this._traceMessages;
    }

    /// <summary>
    /// Returns a <see cref="T:System.String" /> of the most recent trace messages.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.String" /> of the most recent trace messages.
    /// </returns>
    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string traceMessage in this._traceMessages)
      {
        if (stringBuilder.Length > 0)
          stringBuilder.AppendLine();
        stringBuilder.Append(traceMessage);
      }
      return stringBuilder.ToString();
    }
  }
}
