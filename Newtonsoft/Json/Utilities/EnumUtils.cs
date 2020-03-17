// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.EnumUtils
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Newtonsoft.Json.Utilities
{
  [Preserve]
  internal static class EnumUtils
  {
    private static readonly ThreadSafeStore<Type, BidirectionalDictionary<string, string>> EnumMemberNamesPerType = new ThreadSafeStore<Type, BidirectionalDictionary<string, string>>(new Func<Type, BidirectionalDictionary<string, string>>(EnumUtils.InitializeEnumType));

    private static BidirectionalDictionary<string, string> InitializeEnumType(
      Type type)
    {
      BidirectionalDictionary<string, string> bidirectionalDictionary = new BidirectionalDictionary<string, string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      foreach (FieldInfo field in type.GetFields())
      {
        string name = field.Name;
        string second = field.GetCustomAttributes(typeof (EnumMemberAttribute), true).Cast<EnumMemberAttribute>().Select<EnumMemberAttribute, string>((Func<EnumMemberAttribute, string>) (a => a.Value)).SingleOrDefault<string>() ?? field.Name;
        if (bidirectionalDictionary.TryGetBySecond(second, out string _))
          throw new InvalidOperationException("Enum name '{0}' already exists on enum '{1}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) second, (object) type.Name));
        bidirectionalDictionary.Set(name, second);
      }
      return bidirectionalDictionary;
    }

    public static IList<T> GetFlagsValues<T>(T value) where T : struct
    {
      Type type = typeof (T);
      if (!type.IsDefined(typeof (FlagsAttribute), false))
        throw new ArgumentException("Enum type {0} is not a set of flags.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) type));
      Type underlyingType = Enum.GetUnderlyingType(value.GetType());
      ulong uint64 = Convert.ToUInt64((object) value, (IFormatProvider) CultureInfo.InvariantCulture);
      IList<EnumValue<ulong>> namesAndValues = EnumUtils.GetNamesAndValues<T>();
      IList<T> objList = (IList<T>) new List<T>();
      foreach (EnumValue<ulong> enumValue in (IEnumerable<EnumValue<ulong>>) namesAndValues)
      {
        if (((long) uint64 & (long) enumValue.Value) == (long) enumValue.Value && enumValue.Value != 0UL)
          objList.Add((T) Convert.ChangeType((object) enumValue.Value, underlyingType, (IFormatProvider) CultureInfo.CurrentCulture));
      }
      if (objList.Count == 0 && namesAndValues.SingleOrDefault<EnumValue<ulong>>((Func<EnumValue<ulong>, bool>) (v => v.Value == 0UL)) != null)
        objList.Add(default (T));
      return objList;
    }

    /// <summary>
    /// Gets a dictionary of the names and values of an Enum type.
    /// </summary>
    /// <returns></returns>
    public static IList<EnumValue<ulong>> GetNamesAndValues<T>() where T : struct
    {
      return EnumUtils.GetNamesAndValues<ulong>(typeof (T));
    }

    /// <summary>
    /// Gets a dictionary of the names and values of an Enum type.
    /// </summary>
    /// <param name="enumType">The enum type to get names and values for.</param>
    /// <returns></returns>
    public static IList<EnumValue<TUnderlyingType>> GetNamesAndValues<TUnderlyingType>(
      Type enumType)
      where TUnderlyingType : struct
    {
      if (enumType == null)
        throw new ArgumentNullException(nameof (enumType));
      if (!enumType.IsEnum())
        throw new ArgumentException("Type {0} is not an Enum.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) enumType), nameof (enumType));
      IList<object> values = EnumUtils.GetValues(enumType);
      IList<string> names = EnumUtils.GetNames(enumType);
      IList<EnumValue<TUnderlyingType>> enumValueList = (IList<EnumValue<TUnderlyingType>>) new List<EnumValue<TUnderlyingType>>();
      for (int index = 0; index < values.Count; ++index)
      {
        try
        {
          enumValueList.Add(new EnumValue<TUnderlyingType>(names[index], (TUnderlyingType) Convert.ChangeType(values[index], typeof (TUnderlyingType), (IFormatProvider) CultureInfo.CurrentCulture)));
        }
        catch (OverflowException ex)
        {
          throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Value from enum with the underlying type of {0} cannot be added to dictionary with a value type of {1}. Value was too large: {2}", (object) Enum.GetUnderlyingType(enumType), (object) typeof (TUnderlyingType), (object) Convert.ToUInt64(values[index], (IFormatProvider) CultureInfo.InvariantCulture)), (Exception) ex);
        }
      }
      return enumValueList;
    }

    public static IList<object> GetValues(Type enumType)
    {
      if (!enumType.IsEnum())
        throw new ArgumentException("Type '" + enumType.Name + "' is not an enum.");
      List<object> objectList = new List<object>();
      foreach (FieldInfo fieldInfo in ((IEnumerable<FieldInfo>) enumType.GetFields()).Where<FieldInfo>((Func<FieldInfo, bool>) (f => f.IsLiteral)))
      {
        object obj = fieldInfo.GetValue((object) enumType);
        objectList.Add(obj);
      }
      return (IList<object>) objectList;
    }

    public static IList<string> GetNames(Type enumType)
    {
      if (!enumType.IsEnum())
        throw new ArgumentException("Type '" + enumType.Name + "' is not an enum.");
      List<string> stringList = new List<string>();
      foreach (FieldInfo fieldInfo in ((IEnumerable<FieldInfo>) enumType.GetFields()).Where<FieldInfo>((Func<FieldInfo, bool>) (f => f.IsLiteral)))
        stringList.Add(fieldInfo.Name);
      return (IList<string>) stringList;
    }

    public static object ParseEnumName(string enumText, bool isNullable, Type t)
    {
      if (enumText == string.Empty & isNullable)
        return (object) null;
      BidirectionalDictionary<string, string> map = EnumUtils.EnumMemberNamesPerType.Get(t);
      string str;
      if (enumText.IndexOf(',') != -1)
      {
        string[] strArray = enumText.Split(',');
        for (int index = 0; index < strArray.Length; ++index)
        {
          string enumText1 = strArray[index].Trim();
          strArray[index] = EnumUtils.ResolvedEnumName(map, enumText1);
        }
        str = string.Join(", ", strArray);
      }
      else
        str = EnumUtils.ResolvedEnumName(map, enumText);
      return Enum.Parse(t, str, true);
    }

    public static string ToEnumName(Type enumType, string enumText, bool camelCaseText)
    {
      BidirectionalDictionary<string, string> bidirectionalDictionary = EnumUtils.EnumMemberNamesPerType.Get(enumType);
      string[] strArray = enumText.Split(',');
      for (int index = 0; index < strArray.Length; ++index)
      {
        string first = strArray[index].Trim();
        string second;
        bidirectionalDictionary.TryGetByFirst(first, out second);
        second = second ?? first;
        if (camelCaseText)
          second = StringUtils.ToCamelCase(second);
        strArray[index] = second;
      }
      return string.Join(", ", strArray);
    }

    private static string ResolvedEnumName(
      BidirectionalDictionary<string, string> map,
      string enumText)
    {
      string first;
      map.TryGetBySecond(enumText, out first);
      return first ?? enumText;
    }
  }
}
