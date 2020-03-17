// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.ConvertUtils
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Shims;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Newtonsoft.Json.Utilities
{
  [Preserve]
  internal static class ConvertUtils
  {
    private static readonly Dictionary<Type, PrimitiveTypeCode> TypeCodeMap = new Dictionary<Type, PrimitiveTypeCode>()
    {
      {
        typeof (char),
        PrimitiveTypeCode.Char
      },
      {
        typeof (char?),
        PrimitiveTypeCode.CharNullable
      },
      {
        typeof (bool),
        PrimitiveTypeCode.Boolean
      },
      {
        typeof (bool?),
        PrimitiveTypeCode.BooleanNullable
      },
      {
        typeof (sbyte),
        PrimitiveTypeCode.SByte
      },
      {
        typeof (sbyte?),
        PrimitiveTypeCode.SByteNullable
      },
      {
        typeof (short),
        PrimitiveTypeCode.Int16
      },
      {
        typeof (short?),
        PrimitiveTypeCode.Int16Nullable
      },
      {
        typeof (ushort),
        PrimitiveTypeCode.UInt16
      },
      {
        typeof (ushort?),
        PrimitiveTypeCode.UInt16Nullable
      },
      {
        typeof (int),
        PrimitiveTypeCode.Int32
      },
      {
        typeof (int?),
        PrimitiveTypeCode.Int32Nullable
      },
      {
        typeof (byte),
        PrimitiveTypeCode.Byte
      },
      {
        typeof (byte?),
        PrimitiveTypeCode.ByteNullable
      },
      {
        typeof (uint),
        PrimitiveTypeCode.UInt32
      },
      {
        typeof (uint?),
        PrimitiveTypeCode.UInt32Nullable
      },
      {
        typeof (long),
        PrimitiveTypeCode.Int64
      },
      {
        typeof (long?),
        PrimitiveTypeCode.Int64Nullable
      },
      {
        typeof (ulong),
        PrimitiveTypeCode.UInt64
      },
      {
        typeof (ulong?),
        PrimitiveTypeCode.UInt64Nullable
      },
      {
        typeof (float),
        PrimitiveTypeCode.Single
      },
      {
        typeof (float?),
        PrimitiveTypeCode.SingleNullable
      },
      {
        typeof (double),
        PrimitiveTypeCode.Double
      },
      {
        typeof (double?),
        PrimitiveTypeCode.DoubleNullable
      },
      {
        typeof (DateTime),
        PrimitiveTypeCode.DateTime
      },
      {
        typeof (DateTime?),
        PrimitiveTypeCode.DateTimeNullable
      },
      {
        typeof (DateTimeOffset),
        PrimitiveTypeCode.DateTimeOffset
      },
      {
        typeof (DateTimeOffset?),
        PrimitiveTypeCode.DateTimeOffsetNullable
      },
      {
        typeof (Decimal),
        PrimitiveTypeCode.Decimal
      },
      {
        typeof (Decimal?),
        PrimitiveTypeCode.DecimalNullable
      },
      {
        typeof (Guid),
        PrimitiveTypeCode.Guid
      },
      {
        typeof (Guid?),
        PrimitiveTypeCode.GuidNullable
      },
      {
        typeof (TimeSpan),
        PrimitiveTypeCode.TimeSpan
      },
      {
        typeof (TimeSpan?),
        PrimitiveTypeCode.TimeSpanNullable
      },
      {
        typeof (Uri),
        PrimitiveTypeCode.Uri
      },
      {
        typeof (string),
        PrimitiveTypeCode.String
      },
      {
        typeof (byte[]),
        PrimitiveTypeCode.Bytes
      },
      {
        typeof (DBNull),
        PrimitiveTypeCode.DBNull
      }
    };
    private static readonly TypeInformation[] PrimitiveTypeCodes = new TypeInformation[19]
    {
      new TypeInformation()
      {
        Type = typeof (object),
        TypeCode = PrimitiveTypeCode.Empty
      },
      new TypeInformation()
      {
        Type = typeof (object),
        TypeCode = PrimitiveTypeCode.Object
      },
      new TypeInformation()
      {
        Type = typeof (object),
        TypeCode = PrimitiveTypeCode.DBNull
      },
      new TypeInformation()
      {
        Type = typeof (bool),
        TypeCode = PrimitiveTypeCode.Boolean
      },
      new TypeInformation()
      {
        Type = typeof (char),
        TypeCode = PrimitiveTypeCode.Char
      },
      new TypeInformation()
      {
        Type = typeof (sbyte),
        TypeCode = PrimitiveTypeCode.SByte
      },
      new TypeInformation()
      {
        Type = typeof (byte),
        TypeCode = PrimitiveTypeCode.Byte
      },
      new TypeInformation()
      {
        Type = typeof (short),
        TypeCode = PrimitiveTypeCode.Int16
      },
      new TypeInformation()
      {
        Type = typeof (ushort),
        TypeCode = PrimitiveTypeCode.UInt16
      },
      new TypeInformation()
      {
        Type = typeof (int),
        TypeCode = PrimitiveTypeCode.Int32
      },
      new TypeInformation()
      {
        Type = typeof (uint),
        TypeCode = PrimitiveTypeCode.UInt32
      },
      new TypeInformation()
      {
        Type = typeof (long),
        TypeCode = PrimitiveTypeCode.Int64
      },
      new TypeInformation()
      {
        Type = typeof (ulong),
        TypeCode = PrimitiveTypeCode.UInt64
      },
      new TypeInformation()
      {
        Type = typeof (float),
        TypeCode = PrimitiveTypeCode.Single
      },
      new TypeInformation()
      {
        Type = typeof (double),
        TypeCode = PrimitiveTypeCode.Double
      },
      new TypeInformation()
      {
        Type = typeof (Decimal),
        TypeCode = PrimitiveTypeCode.Decimal
      },
      new TypeInformation()
      {
        Type = typeof (DateTime),
        TypeCode = PrimitiveTypeCode.DateTime
      },
      new TypeInformation()
      {
        Type = typeof (object),
        TypeCode = PrimitiveTypeCode.Empty
      },
      new TypeInformation()
      {
        Type = typeof (string),
        TypeCode = PrimitiveTypeCode.String
      }
    };
    private static readonly ThreadSafeStore<ConvertUtils.TypeConvertKey, Func<object, object>> CastConverters = new ThreadSafeStore<ConvertUtils.TypeConvertKey, Func<object, object>>(new Func<ConvertUtils.TypeConvertKey, Func<object, object>>(ConvertUtils.CreateCastConverter));

    public static PrimitiveTypeCode GetTypeCode(Type t)
    {
      return ConvertUtils.GetTypeCode(t, out bool _);
    }

    public static PrimitiveTypeCode GetTypeCode(Type t, out bool isEnum)
    {
      PrimitiveTypeCode primitiveTypeCode;
      if (ConvertUtils.TypeCodeMap.TryGetValue(t, out primitiveTypeCode))
      {
        isEnum = false;
        return primitiveTypeCode;
      }
      if (t.IsEnum())
      {
        isEnum = true;
        return ConvertUtils.GetTypeCode(Enum.GetUnderlyingType(t));
      }
      if (ReflectionUtils.IsNullableType(t))
      {
        Type underlyingType = Nullable.GetUnderlyingType(t);
        if (underlyingType.IsEnum())
        {
          Type t1 = typeof (Nullable<>).MakeGenericType(Enum.GetUnderlyingType(underlyingType));
          isEnum = true;
          return ConvertUtils.GetTypeCode(t1);
        }
      }
      isEnum = false;
      return PrimitiveTypeCode.Object;
    }

    public static TypeInformation GetTypeInformation(IConvertible convertable)
    {
      return ConvertUtils.PrimitiveTypeCodes[(int) convertable.GetTypeCode()];
    }

    public static bool IsConvertible(Type t)
    {
      return typeof (IConvertible).IsAssignableFrom(t);
    }

    public static TimeSpan ParseTimeSpan(string input)
    {
      return TimeSpan.Parse(input);
    }

    private static Func<object, object> CreateCastConverter(ConvertUtils.TypeConvertKey t)
    {
      MethodInfo method = t.TargetType.GetMethod("op_Implicit", new Type[1]
      {
        t.InitialType
      });
      if (method == null)
        method = t.TargetType.GetMethod("op_Explicit", new Type[1]
        {
          t.InitialType
        });
      if (method == null)
        return (Func<object, object>) null;
      MethodCall<object, object> call = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>((MethodBase) method);
      return (Func<object, object>) (o => call((object) null, new object[1]
      {
        o
      }));
    }

    public static object Convert(object initialValue, CultureInfo culture, Type targetType)
    {
      object obj;
      switch (ConvertUtils.TryConvertInternal(initialValue, culture, targetType, out obj))
      {
        case ConvertUtils.ConvertResult.Success:
          return obj;
        case ConvertUtils.ConvertResult.CannotConvertNull:
          throw new Exception("Can not convert null {0} into non-nullable {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) initialValue.GetType(), (object) targetType));
        case ConvertUtils.ConvertResult.NotInstantiableType:
          throw new ArgumentException("Target type {0} is not a value type or a non-abstract class.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) targetType), nameof (targetType));
        case ConvertUtils.ConvertResult.NoValidConversion:
          throw new InvalidOperationException("Can not convert from {0} to {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) initialValue.GetType(), (object) targetType));
        default:
          throw new InvalidOperationException("Unexpected conversion result.");
      }
    }

    private static bool TryConvert(
      object initialValue,
      CultureInfo culture,
      Type targetType,
      out object value)
    {
      try
      {
        if (ConvertUtils.TryConvertInternal(initialValue, culture, targetType, out value) == ConvertUtils.ConvertResult.Success)
          return true;
        value = (object) null;
        return false;
      }
      catch
      {
        value = (object) null;
        return false;
      }
    }

    private static ConvertUtils.ConvertResult TryConvertInternal(
      object initialValue,
      CultureInfo culture,
      Type targetType,
      out object value)
    {
      if (initialValue == null)
        throw new ArgumentNullException(nameof (initialValue));
      if (ReflectionUtils.IsNullableType(targetType))
        targetType = Nullable.GetUnderlyingType(targetType);
      Type type = initialValue.GetType();
      if (targetType == type)
      {
        value = initialValue;
        return ConvertUtils.ConvertResult.Success;
      }
      if (ConvertUtils.IsConvertible(initialValue.GetType()) && ConvertUtils.IsConvertible(targetType))
      {
        if (targetType.IsEnum())
        {
          if (initialValue is string)
          {
            value = Enum.Parse(targetType, initialValue.ToString(), true);
            return ConvertUtils.ConvertResult.Success;
          }
          if (ConvertUtils.IsInteger(initialValue))
          {
            value = Enum.ToObject(targetType, initialValue);
            return ConvertUtils.ConvertResult.Success;
          }
        }
        value = Convert.ChangeType(initialValue, targetType, (IFormatProvider) culture);
        return ConvertUtils.ConvertResult.Success;
      }
      switch (initialValue)
      {
        case DateTime _ when targetType == typeof (DateTimeOffset):
          value = (object) new DateTimeOffset((DateTime) initialValue);
          return ConvertUtils.ConvertResult.Success;
        case byte[] _ when targetType == typeof (Guid):
          value = (object) new Guid((byte[]) initialValue);
          return ConvertUtils.ConvertResult.Success;
        case Guid _ when targetType == typeof (byte[]):
          value = (object) ((Guid) initialValue).ToByteArray();
          return ConvertUtils.ConvertResult.Success;
        case string str:
          if (targetType == typeof (Guid))
          {
            value = (object) new Guid(str);
            return ConvertUtils.ConvertResult.Success;
          }
          if (targetType == typeof (Uri))
          {
            value = (object) new Uri(str, UriKind.RelativeOrAbsolute);
            return ConvertUtils.ConvertResult.Success;
          }
          if (targetType == typeof (TimeSpan))
          {
            value = (object) ConvertUtils.ParseTimeSpan(str);
            return ConvertUtils.ConvertResult.Success;
          }
          if (targetType == typeof (byte[]))
          {
            value = (object) Convert.FromBase64String(str);
            return ConvertUtils.ConvertResult.Success;
          }
          if (targetType == typeof (Version))
          {
            Version result;
            if (ConvertUtils.VersionTryParse(str, out result))
            {
              value = (object) result;
              return ConvertUtils.ConvertResult.Success;
            }
            value = (object) null;
            return ConvertUtils.ConvertResult.NoValidConversion;
          }
          if (typeof (Type).IsAssignableFrom(targetType))
          {
            value = (object) Type.GetType(str, true);
            return ConvertUtils.ConvertResult.Success;
          }
          break;
      }
      TypeConverter converter1 = ConvertUtils.GetConverter(type);
      if (converter1 != null && converter1.CanConvertTo(targetType))
      {
        value = converter1.ConvertTo((ITypeDescriptorContext) null, culture, initialValue, targetType);
        return ConvertUtils.ConvertResult.Success;
      }
      TypeConverter converter2 = ConvertUtils.GetConverter(targetType);
      if (converter2 != null && converter2.CanConvertFrom(type))
      {
        value = converter2.ConvertFrom((ITypeDescriptorContext) null, culture, initialValue);
        return ConvertUtils.ConvertResult.Success;
      }
      if (initialValue == DBNull.Value)
      {
        if (ReflectionUtils.IsNullable(targetType))
        {
          value = ConvertUtils.EnsureTypeAssignable((object) null, type, targetType);
          return ConvertUtils.ConvertResult.Success;
        }
        value = (object) null;
        return ConvertUtils.ConvertResult.CannotConvertNull;
      }
      if (targetType.IsInterface() || targetType.IsGenericTypeDefinition() || targetType.IsAbstract())
      {
        value = (object) null;
        return ConvertUtils.ConvertResult.NotInstantiableType;
      }
      value = (object) null;
      return ConvertUtils.ConvertResult.NoValidConversion;
    }

    /// <summary>
    /// Converts the value to the specified type. If the value is unable to be converted, the
    /// value is checked whether it assignable to the specified type.
    /// </summary>
    /// <param name="initialValue">The value to convert.</param>
    /// <param name="culture">The culture to use when converting.</param>
    /// <param name="targetType">The type to convert or cast the value to.</param>
    /// <returns>
    /// The converted type. If conversion was unsuccessful, the initial value
    /// is returned if assignable to the target type.
    /// </returns>
    public static object ConvertOrCast(object initialValue, CultureInfo culture, Type targetType)
    {
      if (targetType == typeof (object))
        return initialValue;
      if (initialValue == null && ReflectionUtils.IsNullable(targetType))
        return (object) null;
      object obj;
      return ConvertUtils.TryConvert(initialValue, culture, targetType, out obj) ? obj : ConvertUtils.EnsureTypeAssignable(initialValue, ReflectionUtils.GetObjectType(initialValue), targetType);
    }

    private static object EnsureTypeAssignable(object value, Type initialType, Type targetType)
    {
      Type type = value?.GetType();
      if (value != null)
      {
        if (targetType.IsAssignableFrom(type))
          return value;
        Func<object, object> func = ConvertUtils.CastConverters.Get(new ConvertUtils.TypeConvertKey(type, targetType));
        if (func != null)
          return func(value);
      }
      else if (ReflectionUtils.IsNullable(targetType))
        return (object) null;
      throw new ArgumentException("Could not cast or convert from {0} to {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, initialType != null ? (object) initialType.ToString() : (object) "{null}", (object) targetType));
    }

    internal static TypeConverter GetConverter(Type t)
    {
      return JsonTypeReflector.GetTypeConverter(t);
    }

    public static bool VersionTryParse(string input, out Version result)
    {
      try
      {
        result = new Version(input);
        return true;
      }
      catch
      {
        result = (Version) null;
        return false;
      }
    }

    public static bool IsInteger(object value)
    {
      switch (ConvertUtils.GetTypeCode(value.GetType()))
      {
        case PrimitiveTypeCode.SByte:
        case PrimitiveTypeCode.Int16:
        case PrimitiveTypeCode.UInt16:
        case PrimitiveTypeCode.Int32:
        case PrimitiveTypeCode.Byte:
        case PrimitiveTypeCode.UInt32:
        case PrimitiveTypeCode.Int64:
        case PrimitiveTypeCode.UInt64:
          return true;
        default:
          return false;
      }
    }

    public static ParseResult Int32TryParse(
      char[] chars,
      int start,
      int length,
      out int value)
    {
      value = 0;
      if (length == 0)
        return ParseResult.Invalid;
      bool flag = chars[start] == '-';
      if (flag)
      {
        if (length == 1)
          return ParseResult.Invalid;
        ++start;
        --length;
      }
      int num1 = start + length;
      if (length > 10 || length == 10 && (int) chars[start] - 48 > 2)
      {
        for (int index = start; index < num1; ++index)
        {
          int num2 = (int) chars[index] - 48;
          if (num2 < 0 || num2 > 9)
            return ParseResult.Invalid;
        }
        return ParseResult.Overflow;
      }
      for (int index1 = start; index1 < num1; ++index1)
      {
        int num2 = (int) chars[index1] - 48;
        if (num2 < 0 || num2 > 9)
          return ParseResult.Invalid;
        int num3 = 10 * value - num2;
        if (num3 > value)
        {
          for (int index2 = index1 + 1; index2 < num1; ++index2)
          {
            int num4 = (int) chars[index2] - 48;
            if (num4 < 0 || num4 > 9)
              return ParseResult.Invalid;
          }
          return ParseResult.Overflow;
        }
        value = num3;
      }
      if (!flag)
      {
        if (value == int.MinValue)
          return ParseResult.Overflow;
        value = -value;
      }
      return ParseResult.Success;
    }

    public static ParseResult Int64TryParse(
      char[] chars,
      int start,
      int length,
      out long value)
    {
      value = 0L;
      if (length == 0)
        return ParseResult.Invalid;
      bool flag = chars[start] == '-';
      if (flag)
      {
        if (length == 1)
          return ParseResult.Invalid;
        ++start;
        --length;
      }
      int num1 = start + length;
      if (length > 19)
      {
        for (int index = start; index < num1; ++index)
        {
          int num2 = (int) chars[index] - 48;
          if (num2 < 0 || num2 > 9)
            return ParseResult.Invalid;
        }
        return ParseResult.Overflow;
      }
      for (int index1 = start; index1 < num1; ++index1)
      {
        int num2 = (int) chars[index1] - 48;
        if (num2 < 0 || num2 > 9)
          return ParseResult.Invalid;
        long num3 = 10L * value - (long) num2;
        if (num3 > value)
        {
          for (int index2 = index1 + 1; index2 < num1; ++index2)
          {
            int num4 = (int) chars[index2] - 48;
            if (num4 < 0 || num4 > 9)
              return ParseResult.Invalid;
          }
          return ParseResult.Overflow;
        }
        value = num3;
      }
      if (!flag)
      {
        if (value == long.MinValue)
          return ParseResult.Overflow;
        value = -value;
      }
      return ParseResult.Success;
    }

    public static bool TryConvertGuid(string s, out Guid g)
    {
      if (s == null)
        throw new ArgumentNullException(nameof (s));
      if (new Regex("^[A-Fa-f0-9]{8}-([A-Fa-f0-9]{4}-){3}[A-Fa-f0-9]{12}$").Match(s).Success)
      {
        g = new Guid(s);
        return true;
      }
      g = Guid.Empty;
      return false;
    }

    public static int HexTextToInt(char[] text, int start, int end)
    {
      int num = 0;
      for (int index = start; index < end; ++index)
        num += ConvertUtils.HexCharToInt(text[index]) << (end - 1 - index) * 4;
      return num;
    }

    private static int HexCharToInt(char ch)
    {
      if (ch <= '9' && ch >= '0')
        return (int) ch - 48;
      if (ch <= 'F' && ch >= 'A')
        return (int) ch - 55;
      if (ch <= 'f' && ch >= 'a')
        return (int) ch - 87;
      throw new FormatException("Invalid hex character: " + ch.ToString());
    }

    internal struct TypeConvertKey
    {
      private readonly Type _initialType;
      private readonly Type _targetType;

      public Type InitialType
      {
        get
        {
          return this._initialType;
        }
      }

      public Type TargetType
      {
        get
        {
          return this._targetType;
        }
      }

      public TypeConvertKey(Type initialType, Type targetType)
      {
        this._initialType = initialType;
        this._targetType = targetType;
      }

      public override int GetHashCode()
      {
        return this._initialType.GetHashCode() ^ this._targetType.GetHashCode();
      }

      public override bool Equals(object obj)
      {
        return obj is ConvertUtils.TypeConvertKey other && this.Equals(other);
      }

      public bool Equals(ConvertUtils.TypeConvertKey other)
      {
        return this._initialType == other._initialType && this._targetType == other._targetType;
      }
    }

    internal enum ConvertResult
    {
      Success,
      CannotConvertNull,
      NotInstantiableType,
      NoValidConversion,
    }
  }
}
