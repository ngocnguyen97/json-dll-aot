// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.ReflectionObject
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Shims;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Newtonsoft.Json.Utilities
{
  [Preserve]
  internal class ReflectionObject
  {
    public ObjectConstructor<object> Creator { get; private set; }

    public IDictionary<string, ReflectionMember> Members { get; private set; }

    public ReflectionObject()
    {
      this.Members = (IDictionary<string, ReflectionMember>) new Dictionary<string, ReflectionMember>();
    }

    public object GetValue(object target, string member)
    {
      return this.Members[member].Getter(target);
    }

    public void SetValue(object target, string member, object value)
    {
      this.Members[member].Setter(target, value);
    }

    public Type GetType(string member)
    {
      return this.Members[member].MemberType;
    }

    public static ReflectionObject Create(Type t, params string[] memberNames)
    {
      return ReflectionObject.Create(t, (MethodBase) null, memberNames);
    }

    public static ReflectionObject Create(
      Type t,
      MethodBase creator,
      params string[] memberNames)
    {
      ReflectionObject reflectionObject = new ReflectionObject();
      ReflectionDelegateFactory reflectionDelegateFactory = JsonTypeReflector.ReflectionDelegateFactory;
      if (creator != null)
        reflectionObject.Creator = reflectionDelegateFactory.CreateParameterizedConstructor(creator);
      else if (ReflectionUtils.HasDefaultConstructor(t, false))
      {
        Func<object> ctor = reflectionDelegateFactory.CreateDefaultConstructor<object>(t);
        reflectionObject.Creator = (ObjectConstructor<object>) (args => ctor());
      }
      foreach (string memberName in memberNames)
      {
        MemberInfo[] member = t.GetMember(memberName, BindingFlags.Instance | BindingFlags.Public);
        if (member.Length != 1)
          throw new ArgumentException("Expected a single member with the name '{0}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) memberName));
        MemberInfo memberInfo = ((IEnumerable<MemberInfo>) member).Single<MemberInfo>();
        ReflectionMember reflectionMember = new ReflectionMember();
        switch (memberInfo.MemberType())
        {
          case MemberTypes.Field:
          case MemberTypes.Property:
            if (ReflectionUtils.CanReadMemberValue(memberInfo, false))
              reflectionMember.Getter = reflectionDelegateFactory.CreateGet<object>(memberInfo);
            if (ReflectionUtils.CanSetMemberValue(memberInfo, false, false))
            {
              reflectionMember.Setter = reflectionDelegateFactory.CreateSet<object>(memberInfo);
              break;
            }
            break;
          case MemberTypes.Method:
            MethodInfo methodInfo = (MethodInfo) memberInfo;
            if (methodInfo.IsPublic)
            {
              ParameterInfo[] parameters = methodInfo.GetParameters();
              if (parameters.Length == 0 && methodInfo.ReturnType != typeof (void))
              {
                MethodCall<object, object> call = reflectionDelegateFactory.CreateMethodCall<object>((MethodBase) methodInfo);
                reflectionMember.Getter = (Func<object, object>) (target => call(target, new object[0]));
                break;
              }
              if (parameters.Length == 1 && methodInfo.ReturnType == typeof (void))
              {
                MethodCall<object, object> call = reflectionDelegateFactory.CreateMethodCall<object>((MethodBase) methodInfo);
                object obj;
                reflectionMember.Setter = (Action<object, object>) ((target, arg) => obj = call(target, new object[1]
                {
                  arg
                }));
                break;
              }
              break;
            }
            break;
          default:
            throw new ArgumentException("Unexpected member type '{0}' for member '{1}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) memberInfo.MemberType(), (object) memberInfo.Name));
        }
        if (ReflectionUtils.CanReadMemberValue(memberInfo, false))
          reflectionMember.Getter = reflectionDelegateFactory.CreateGet<object>(memberInfo);
        if (ReflectionUtils.CanSetMemberValue(memberInfo, false, false))
          reflectionMember.Setter = reflectionDelegateFactory.CreateSet<object>(memberInfo);
        reflectionMember.MemberType = ReflectionUtils.GetMemberUnderlyingType(memberInfo);
        reflectionObject.Members[memberName] = reflectionMember;
      }
      return reflectionObject;
    }
  }
}
