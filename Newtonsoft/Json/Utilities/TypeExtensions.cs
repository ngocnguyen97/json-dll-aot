// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.TypeExtensions
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Newtonsoft.Json.Utilities
{
  [Preserve]
  internal static class TypeExtensions
  {
    public static MethodInfo Method(this Delegate d)
    {
      return d.Method;
    }

    public static MemberTypes MemberType(this MemberInfo memberInfo)
    {
      return memberInfo.MemberType;
    }

    public static bool ContainsGenericParameters(this Type type)
    {
      return type.ContainsGenericParameters;
    }

    public static bool IsInterface(this Type type)
    {
      return type.IsInterface;
    }

    public static bool IsGenericType(this Type type)
    {
      return type.IsGenericType;
    }

    public static bool IsGenericTypeDefinition(this Type type)
    {
      return type.IsGenericTypeDefinition;
    }

    public static Type BaseType(this Type type)
    {
      return type.BaseType;
    }

    public static Assembly Assembly(this Type type)
    {
      return type.Assembly;
    }

    public static bool IsEnum(this Type type)
    {
      return type.IsEnum;
    }

    public static bool IsClass(this Type type)
    {
      return type.IsClass;
    }

    public static bool IsSealed(this Type type)
    {
      return type.IsSealed;
    }

    public static bool IsAbstract(this Type type)
    {
      return type.IsAbstract;
    }

    public static bool IsVisible(this Type type)
    {
      return type.IsVisible;
    }

    public static bool IsValueType(this Type type)
    {
      return type.IsValueType;
    }

    public static bool AssignableToTypeName(this Type type, string fullTypeName, out Type match)
    {
      for (Type type1 = type; type1 != null; type1 = type1.BaseType())
      {
        if (string.Equals(type1.FullName, fullTypeName, StringComparison.Ordinal))
        {
          match = type1;
          return true;
        }
      }
      foreach (MemberInfo memberInfo in type.GetInterfaces())
      {
        if (string.Equals(memberInfo.Name, fullTypeName, StringComparison.Ordinal))
        {
          match = type;
          return true;
        }
      }
      match = (Type) null;
      return false;
    }

    public static bool AssignableToTypeName(this Type type, string fullTypeName)
    {
      return type.AssignableToTypeName(fullTypeName, out Type _);
    }

    public static bool ImplementInterface(this Type type, Type interfaceType)
    {
      for (Type type1 = type; type1 != null; type1 = type1.BaseType())
      {
        foreach (Type type2 in (IEnumerable<Type>) type1.GetInterfaces())
        {
          if (type2 == interfaceType || type2 != null && type2.ImplementInterface(interfaceType))
            return true;
        }
      }
      return false;
    }
  }
}
