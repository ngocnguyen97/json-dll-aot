// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.ReflectionDelegateFactory
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Shims;
using System;
using System.Globalization;
using System.Reflection;

namespace Newtonsoft.Json.Utilities
{
  [Preserve]
  internal abstract class ReflectionDelegateFactory
  {
    public Func<T, object> CreateGet<T>(MemberInfo memberInfo)
    {
      switch (memberInfo)
      {
        case PropertyInfo propertyInfo:
          return this.CreateGet<T>(propertyInfo);
        case FieldInfo fieldInfo:
          return this.CreateGet<T>(fieldInfo);
        default:
          throw new Exception("Could not create getter for {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) memberInfo));
      }
    }

    public Action<T, object> CreateSet<T>(MemberInfo memberInfo)
    {
      switch (memberInfo)
      {
        case PropertyInfo propertyInfo:
          return this.CreateSet<T>(propertyInfo);
        case FieldInfo fieldInfo:
          return this.CreateSet<T>(fieldInfo);
        default:
          throw new Exception("Could not create setter for {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) memberInfo));
      }
    }

    public abstract MethodCall<T, object> CreateMethodCall<T>(MethodBase method);

    public abstract ObjectConstructor<object> CreateParameterizedConstructor(
      MethodBase method);

    public abstract Func<T> CreateDefaultConstructor<T>(Type type);

    public abstract Func<T, object> CreateGet<T>(PropertyInfo propertyInfo);

    public abstract Func<T, object> CreateGet<T>(FieldInfo fieldInfo);

    public abstract Action<T, object> CreateSet<T>(FieldInfo fieldInfo);

    public abstract Action<T, object> CreateSet<T>(PropertyInfo propertyInfo);
  }
}
