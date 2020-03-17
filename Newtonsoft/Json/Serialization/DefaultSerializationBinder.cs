// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.DefaultSerializationBinder
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;

namespace Newtonsoft.Json.Serialization
{
  /// <summary>
  /// The default serialization binder used when resolving and loading classes from type names.
  /// </summary>
  [Preserve]
  public class DefaultSerializationBinder : SerializationBinder
  {
    internal static readonly DefaultSerializationBinder Instance = new DefaultSerializationBinder();
    private readonly ThreadSafeStore<DefaultSerializationBinder.TypeNameKey, Type> _typeCache = new ThreadSafeStore<DefaultSerializationBinder.TypeNameKey, Type>(new Func<DefaultSerializationBinder.TypeNameKey, Type>(DefaultSerializationBinder.GetTypeFromTypeNameKey));

    private static Type GetTypeFromTypeNameKey(
      DefaultSerializationBinder.TypeNameKey typeNameKey)
    {
      string assemblyName = typeNameKey.AssemblyName;
      string typeName = typeNameKey.TypeName;
      if (assemblyName == null)
        return Type.GetType(typeName);
      Assembly assembly1 = Assembly.Load(assemblyName);
      if (assembly1 == null)
      {
        foreach (Assembly assembly2 in AppDomain.CurrentDomain.GetAssemblies())
        {
          if (assembly2.FullName == assemblyName)
          {
            assembly1 = assembly2;
            break;
          }
        }
      }
      if (assembly1 == null)
        throw new JsonSerializationException("Could not load assembly '{0}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) assemblyName));
      Type type = assembly1.GetType(typeName);
      if (type != null)
        return type;
      throw new JsonSerializationException("Could not find type '{0}' in assembly '{1}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) typeName, (object) assembly1.FullName));
    }

    /// <summary>
    /// When overridden in a derived class, controls the binding of a serialized object to a type.
    /// </summary>
    /// <param name="assemblyName">Specifies the <see cref="T:System.Reflection.Assembly" /> name of the serialized object.</param>
    /// <param name="typeName">Specifies the <see cref="T:System.Type" /> name of the serialized object.</param>
    /// <returns>
    /// The type of the object the formatter creates a new instance of.
    /// </returns>
    public override Type BindToType(string assemblyName, string typeName)
    {
      return this._typeCache.Get(new DefaultSerializationBinder.TypeNameKey(assemblyName, typeName));
    }

    internal struct TypeNameKey
    {
      internal readonly string AssemblyName;
      internal readonly string TypeName;

      public TypeNameKey(string assemblyName, string typeName)
      {
        this.AssemblyName = assemblyName;
        this.TypeName = typeName;
      }

      public override int GetHashCode()
      {
        return (this.AssemblyName != null ? this.AssemblyName.GetHashCode() : 0) ^ (this.TypeName != null ? this.TypeName.GetHashCode() : 0);
      }

      public override bool Equals(object obj)
      {
        return obj is DefaultSerializationBinder.TypeNameKey other && this.Equals(other);
      }

      public bool Equals(DefaultSerializationBinder.TypeNameKey other)
      {
        return this.AssemblyName == other.AssemblyName && this.TypeName == other.TypeName;
      }
    }
  }
}
