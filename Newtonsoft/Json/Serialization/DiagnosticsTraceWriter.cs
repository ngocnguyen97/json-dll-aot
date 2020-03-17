// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.DiagnosticsTraceWriter
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using System;
using System.Diagnostics;

namespace Newtonsoft.Json.Serialization
{
  /// <summary>
  /// Represents a trace writer that writes to the application's <see cref="T:System.Diagnostics.TraceListener" /> instances.
  /// </summary>
  [Preserve]
  public class DiagnosticsTraceWriter : ITraceWriter
  {
    /// <summary>
    /// Gets the <see cref="T:System.Diagnostics.TraceLevel" /> that will be used to filter the trace messages passed to the writer.
    /// For example a filter level of <code>Info</code> will exclude <code>Verbose</code> messages and include <code>Info</code>,
    /// <code>Warning</code> and <code>Error</code> messages.
    /// </summary>
    /// <value>
    /// The <see cref="T:System.Diagnostics.TraceLevel" /> that will be used to filter the trace messages passed to the writer.
    /// </value>
    public TraceLevel LevelFilter { get; set; }

    private TraceEventType GetTraceEventType(TraceLevel level)
    {
      switch (level)
      {
        case TraceLevel.Error:
          return TraceEventType.Error;
        case TraceLevel.Warning:
          return TraceEventType.Warning;
        case TraceLevel.Info:
          return TraceEventType.Information;
        case TraceLevel.Verbose:
          return TraceEventType.Verbose;
        default:
          throw new ArgumentOutOfRangeException(nameof (level));
      }
    }

    /// <summary>
    /// Writes the specified trace level, message and optional exception.
    /// </summary>
    /// <param name="level">The <see cref="T:System.Diagnostics.TraceLevel" /> at which to write this trace.</param>
    /// <param name="message">The trace message.</param>
    /// <param name="ex">The trace exception. This parameter is optional.</param>
    public void Trace(TraceLevel level, string message, Exception ex)
    {
      if (level == TraceLevel.Off)
        return;
      TraceEventCache eventCache = new TraceEventCache();
      TraceEventType traceEventType = this.GetTraceEventType(level);
      foreach (TraceListener listener in Trace.Listeners)
      {
        if (!listener.IsThreadSafe)
        {
          lock (listener)
            listener.TraceEvent(eventCache, "Newtonsoft.Json", traceEventType, 0, message);
        }
        else
          listener.TraceEvent(eventCache, "Newtonsoft.Json", traceEventType, 0, message);
        if (Trace.AutoFlush)
          listener.Flush();
      }
    }
  }
}
