﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JContainer
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Threading;

namespace Newtonsoft.Json.Linq
{
  /// <summary>Represents a token that can contain other tokens.</summary>
  [Preserve]
  public abstract class JContainer : JToken, IList<JToken>, ICollection<JToken>, IEnumerable<JToken>, IEnumerable, ITypedList, IBindingList, IList, ICollection
  {
    internal ListChangedEventHandler _listChanged;
    internal AddingNewEventHandler _addingNew;
    private object _syncRoot;
    private bool _busy;

    /// <summary>
    /// Occurs when the list changes or an item in the list changes.
    /// </summary>
    public event ListChangedEventHandler ListChanged
    {
      add
      {
        this._listChanged += value;
      }
      remove
      {
        this._listChanged -= value;
      }
    }

    /// <summary>Occurs before an item is added to the collection.</summary>
    public event AddingNewEventHandler AddingNew
    {
      add
      {
        this._addingNew += value;
      }
      remove
      {
        this._addingNew -= value;
      }
    }

    /// <summary>Gets the container's children tokens.</summary>
    /// <value>The container's children tokens.</value>
    protected abstract IList<JToken> ChildrenTokens { get; }

    internal JContainer()
    {
    }

    internal JContainer(JContainer other)
      : this()
    {
      ValidationUtils.ArgumentNotNull((object) other, nameof (other));
      int index = 0;
      foreach (JToken jtoken in (IEnumerable<JToken>) other)
      {
        this.AddInternal(index, (object) jtoken, false);
        ++index;
      }
    }

    internal void CheckReentrancy()
    {
      if (this._busy)
        throw new InvalidOperationException("Cannot change {0} during a collection change event.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.GetType()));
    }

    internal virtual IList<JToken> CreateChildrenCollection()
    {
      return (IList<JToken>) new List<JToken>();
    }

    /// <summary>
    /// Raises the <see cref="E:Newtonsoft.Json.Linq.JContainer.AddingNew" /> event.
    /// </summary>
    /// <param name="e">The <see cref="T:System.ComponentModel.AddingNewEventArgs" /> instance containing the event data.</param>
    protected virtual void OnAddingNew(AddingNewEventArgs e)
    {
      AddingNewEventHandler addingNew = this._addingNew;
      if (addingNew == null)
        return;
      addingNew((object) this, e);
    }

    /// <summary>
    /// Raises the <see cref="E:Newtonsoft.Json.Linq.JContainer.ListChanged" /> event.
    /// </summary>
    /// <param name="e">The <see cref="T:System.ComponentModel.ListChangedEventArgs" /> instance containing the event data.</param>
    protected virtual void OnListChanged(ListChangedEventArgs e)
    {
      ListChangedEventHandler listChanged = this._listChanged;
      if (listChanged == null)
        return;
      this._busy = true;
      try
      {
        listChanged((object) this, e);
      }
      finally
      {
        this._busy = false;
      }
    }

    /// <summary>
    /// Gets a value indicating whether this token has child tokens.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this token has child values; otherwise, <c>false</c>.
    /// </value>
    public override bool HasValues
    {
      get
      {
        return this.ChildrenTokens.Count > 0;
      }
    }

    internal bool ContentsEqual(JContainer container)
    {
      if (container == this)
        return true;
      IList<JToken> childrenTokens1 = this.ChildrenTokens;
      IList<JToken> childrenTokens2 = container.ChildrenTokens;
      if (childrenTokens1.Count != childrenTokens2.Count)
        return false;
      for (int index = 0; index < childrenTokens1.Count; ++index)
      {
        if (!childrenTokens1[index].DeepEquals(childrenTokens2[index]))
          return false;
      }
      return true;
    }

    /// <summary>Get the first child token of this token.</summary>
    /// <value>
    /// A <see cref="T:Newtonsoft.Json.Linq.JToken" /> containing the first child token of the <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </value>
    public override JToken First
    {
      get
      {
        IList<JToken> childrenTokens = this.ChildrenTokens;
        return childrenTokens.Count <= 0 ? (JToken) null : childrenTokens[0];
      }
    }

    /// <summary>Get the last child token of this token.</summary>
    /// <value>
    /// A <see cref="T:Newtonsoft.Json.Linq.JToken" /> containing the last child token of the <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </value>
    public override JToken Last
    {
      get
      {
        IList<JToken> childrenTokens = this.ChildrenTokens;
        int count = childrenTokens.Count;
        return count <= 0 ? (JToken) null : childrenTokens[count - 1];
      }
    }

    /// <summary>
    /// Returns a collection of the child tokens of this token, in document order.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="T:Newtonsoft.Json.Linq.JToken" /> containing the child tokens of this <see cref="T:Newtonsoft.Json.Linq.JToken" />, in document order.
    /// </returns>
    public override JEnumerable<JToken> Children()
    {
      return new JEnumerable<JToken>((IEnumerable<JToken>) this.ChildrenTokens);
    }

    /// <summary>
    /// Returns a collection of the child values of this token, in document order.
    /// </summary>
    /// <typeparam name="T">The type to convert the values to.</typeparam>
    /// <returns>
    /// A <see cref="T:System.Collections.Generic.IEnumerable`1" /> containing the child values of this <see cref="T:Newtonsoft.Json.Linq.JToken" />, in document order.
    /// </returns>
    public override IEnumerable<T> Values<T>()
    {
      return this.ChildrenTokens.Convert<JToken, T>();
    }

    /// <summary>
    /// Returns a collection of the descendant tokens for this token in document order.
    /// </summary>
    /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> containing the descendant tokens of the <see cref="T:Newtonsoft.Json.Linq.JToken" />.</returns>
    public IEnumerable<JToken> Descendants()
    {
      return this.GetDescendants(false);
    }

    /// <summary>
    /// Returns a collection of the tokens that contain this token, and all descendant tokens of this token, in document order.
    /// </summary>
    /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> containing this token, and all the descendant tokens of the <see cref="T:Newtonsoft.Json.Linq.JToken" />.</returns>
    public IEnumerable<JToken> DescendantsAndSelf()
    {
      return this.GetDescendants(true);
    }

    internal IEnumerable<JToken> GetDescendants(bool self)
    {
      if (self)
        yield return (JToken) this;
      foreach (JToken childrenToken in (IEnumerable<JToken>) this.ChildrenTokens)
      {
        JToken o = childrenToken;
        yield return o;
        if (o is JContainer jcontainer)
        {
          foreach (JToken descendant in jcontainer.Descendants())
            yield return descendant;
        }
        o = (JToken) null;
      }
    }

    internal bool IsMultiContent(object content)
    {
      return content is IEnumerable && !(content is string) && !(content is JToken) && !(content is byte[]);
    }

    internal JToken EnsureParentToken(JToken item, bool skipParentCheck)
    {
      if (item == null)
        return (JToken) JValue.CreateNull();
      if (skipParentCheck || item.Parent == null && item != this && (!item.HasValues || this.Root != item))
        return item;
      item = item.CloneToken();
      return item;
    }

    internal abstract int IndexOfItem(JToken item);

    internal virtual void InsertItem(int index, JToken item, bool skipParentCheck)
    {
      IList<JToken> childrenTokens = this.ChildrenTokens;
      if (index > childrenTokens.Count)
        throw new ArgumentOutOfRangeException(nameof (index), "Index must be within the bounds of the List.");
      this.CheckReentrancy();
      item = this.EnsureParentToken(item, skipParentCheck);
      JToken jtoken1 = index == 0 ? (JToken) null : childrenTokens[index - 1];
      JToken jtoken2 = index == childrenTokens.Count ? (JToken) null : childrenTokens[index];
      this.ValidateToken(item, (JToken) null);
      item.Parent = this;
      item.Previous = jtoken1;
      if (jtoken1 != null)
        jtoken1.Next = item;
      item.Next = jtoken2;
      if (jtoken2 != null)
        jtoken2.Previous = item;
      childrenTokens.Insert(index, item);
      if (this._listChanged == null)
        return;
      this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
    }

    internal virtual void RemoveItemAt(int index)
    {
      IList<JToken> childrenTokens = this.ChildrenTokens;
      if (index < 0)
        throw new ArgumentOutOfRangeException(nameof (index), "Index is less than 0.");
      if (index >= childrenTokens.Count)
        throw new ArgumentOutOfRangeException(nameof (index), "Index is equal to or greater than Count.");
      this.CheckReentrancy();
      JToken jtoken1 = childrenTokens[index];
      JToken jtoken2 = index == 0 ? (JToken) null : childrenTokens[index - 1];
      JToken jtoken3 = index == childrenTokens.Count - 1 ? (JToken) null : childrenTokens[index + 1];
      if (jtoken2 != null)
        jtoken2.Next = jtoken3;
      if (jtoken3 != null)
        jtoken3.Previous = jtoken2;
      jtoken1.Parent = (JContainer) null;
      jtoken1.Previous = (JToken) null;
      jtoken1.Next = (JToken) null;
      childrenTokens.RemoveAt(index);
      if (this._listChanged == null)
        return;
      this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
    }

    internal virtual bool RemoveItem(JToken item)
    {
      int index = this.IndexOfItem(item);
      if (index < 0)
        return false;
      this.RemoveItemAt(index);
      return true;
    }

    internal virtual JToken GetItem(int index)
    {
      return this.ChildrenTokens[index];
    }

    internal virtual void SetItem(int index, JToken item)
    {
      IList<JToken> childrenTokens = this.ChildrenTokens;
      if (index < 0)
        throw new ArgumentOutOfRangeException(nameof (index), "Index is less than 0.");
      if (index >= childrenTokens.Count)
        throw new ArgumentOutOfRangeException(nameof (index), "Index is equal to or greater than Count.");
      JToken jtoken1 = childrenTokens[index];
      if (JContainer.IsTokenUnchanged(jtoken1, item))
        return;
      this.CheckReentrancy();
      item = this.EnsureParentToken(item, false);
      this.ValidateToken(item, jtoken1);
      JToken jtoken2 = index == 0 ? (JToken) null : childrenTokens[index - 1];
      JToken jtoken3 = index == childrenTokens.Count - 1 ? (JToken) null : childrenTokens[index + 1];
      item.Parent = this;
      item.Previous = jtoken2;
      if (jtoken2 != null)
        jtoken2.Next = item;
      item.Next = jtoken3;
      if (jtoken3 != null)
        jtoken3.Previous = item;
      childrenTokens[index] = item;
      jtoken1.Parent = (JContainer) null;
      jtoken1.Previous = (JToken) null;
      jtoken1.Next = (JToken) null;
      if (this._listChanged == null)
        return;
      this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
    }

    internal virtual void ClearItems()
    {
      this.CheckReentrancy();
      IList<JToken> childrenTokens = this.ChildrenTokens;
      foreach (JToken jtoken in (IEnumerable<JToken>) childrenTokens)
      {
        jtoken.Parent = (JContainer) null;
        jtoken.Previous = (JToken) null;
        jtoken.Next = (JToken) null;
      }
      childrenTokens.Clear();
      if (this._listChanged == null)
        return;
      this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
    }

    internal virtual void ReplaceItem(JToken existing, JToken replacement)
    {
      if (existing == null || existing.Parent != this)
        return;
      this.SetItem(this.IndexOfItem(existing), replacement);
    }

    internal virtual bool ContainsItem(JToken item)
    {
      return this.IndexOfItem(item) != -1;
    }

    internal virtual void CopyItemsTo(Array array, int arrayIndex)
    {
      if (array == null)
        throw new ArgumentNullException(nameof (array));
      if (arrayIndex < 0)
        throw new ArgumentOutOfRangeException(nameof (arrayIndex), "arrayIndex is less than 0.");
      if (arrayIndex >= array.Length && arrayIndex != 0)
        throw new ArgumentException("arrayIndex is equal to or greater than the length of array.");
      if (this.Count > array.Length - arrayIndex)
        throw new ArgumentException("The number of elements in the source JObject is greater than the available space from arrayIndex to the end of the destination array.");
      int num = 0;
      foreach (JToken childrenToken in (IEnumerable<JToken>) this.ChildrenTokens)
      {
        array.SetValue((object) childrenToken, arrayIndex + num);
        ++num;
      }
    }

    internal static bool IsTokenUnchanged(JToken currentValue, JToken newValue)
    {
      if (!(currentValue is JValue jvalue))
        return false;
      return jvalue.Type == JTokenType.Null && newValue == null || jvalue.Equals((object) newValue);
    }

    internal virtual void ValidateToken(JToken o, JToken existing)
    {
      ValidationUtils.ArgumentNotNull((object) o, nameof (o));
      if (o.Type == JTokenType.Property)
        throw new ArgumentException("Can not add {0} to {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) o.GetType(), (object) this.GetType()));
    }

    /// <summary>
    /// Adds the specified content as children of this <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="content">The content to be added.</param>
    public virtual void Add(object content)
    {
      this.AddInternal(this.ChildrenTokens.Count, content, false);
    }

    internal void AddAndSkipParentCheck(JToken token)
    {
      this.AddInternal(this.ChildrenTokens.Count, (object) token, true);
    }

    /// <summary>
    /// Adds the specified content as the first children of this <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="content">The content to be added.</param>
    public void AddFirst(object content)
    {
      this.AddInternal(0, content, false);
    }

    internal void AddInternal(int index, object content, bool skipParentCheck)
    {
      if (this.IsMultiContent(content))
      {
        IEnumerable enumerable = (IEnumerable) content;
        int index1 = index;
        foreach (object content1 in enumerable)
        {
          this.AddInternal(index1, content1, skipParentCheck);
          ++index1;
        }
      }
      else
      {
        JToken fromContent = JContainer.CreateFromContent(content);
        this.InsertItem(index, fromContent, skipParentCheck);
      }
    }

    internal static JToken CreateFromContent(object content)
    {
      return content is JToken jtoken ? jtoken : (JToken) new JValue(content);
    }

    /// <summary>
    /// Creates an <see cref="T:Newtonsoft.Json.JsonWriter" /> that can be used to add tokens to the <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <returns>An <see cref="T:Newtonsoft.Json.JsonWriter" /> that is ready to have content written to it.</returns>
    public JsonWriter CreateWriter()
    {
      return (JsonWriter) new JTokenWriter(this);
    }

    /// <summary>
    /// Replaces the children nodes of this token with the specified content.
    /// </summary>
    /// <param name="content">The content.</param>
    public void ReplaceAll(object content)
    {
      this.ClearItems();
      this.Add(content);
    }

    /// <summary>Removes the child nodes from this token.</summary>
    public void RemoveAll()
    {
      this.ClearItems();
    }

    internal abstract void MergeItem(object content, JsonMergeSettings settings);

    /// <summary>
    /// Merge the specified content into this <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <param name="content">The content to be merged.</param>
    public void Merge(object content)
    {
      this.MergeItem(content, new JsonMergeSettings());
    }

    /// <summary>
    /// Merge the specified content into this <see cref="T:Newtonsoft.Json.Linq.JToken" /> using <see cref="T:Newtonsoft.Json.Linq.JsonMergeSettings" />.
    /// </summary>
    /// <param name="content">The content to be merged.</param>
    /// <param name="settings">The <see cref="T:Newtonsoft.Json.Linq.JsonMergeSettings" /> used to merge the content.</param>
    public void Merge(object content, JsonMergeSettings settings)
    {
      this.MergeItem(content, settings);
    }

    internal void ReadTokenFrom(JsonReader reader, JsonLoadSettings options)
    {
      int depth = reader.Depth;
      if (!reader.Read())
        throw JsonReaderException.Create(reader, "Error reading {0} from JsonReader.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.GetType().Name));
      this.ReadContentFrom(reader, options);
      if (reader.Depth > depth)
        throw JsonReaderException.Create(reader, "Unexpected end of content while loading {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.GetType().Name));
    }

    internal void ReadContentFrom(JsonReader r, JsonLoadSettings settings)
    {
      ValidationUtils.ArgumentNotNull((object) r, nameof (r));
      IJsonLineInfo lineInfo = r as IJsonLineInfo;
      JContainer jcontainer = this;
      do
      {
        if ((jcontainer is JProperty jproperty ? jproperty.Value : (JToken) null) != null)
        {
          if (jcontainer == this)
            break;
          jcontainer = jcontainer.Parent;
        }
        switch (r.TokenType)
        {
          case JsonToken.None:
            continue;
          case JsonToken.StartObject:
            JObject jobject = new JObject();
            jobject.SetLineInfo(lineInfo, settings);
            jcontainer.Add((object) jobject);
            jcontainer = (JContainer) jobject;
            goto case JsonToken.None;
          case JsonToken.StartArray:
            JArray jarray = new JArray();
            jarray.SetLineInfo(lineInfo, settings);
            jcontainer.Add((object) jarray);
            jcontainer = (JContainer) jarray;
            goto case JsonToken.None;
          case JsonToken.StartConstructor:
            JConstructor jconstructor = new JConstructor(r.Value.ToString());
            jconstructor.SetLineInfo(lineInfo, settings);
            jcontainer.Add((object) jconstructor);
            jcontainer = (JContainer) jconstructor;
            goto case JsonToken.None;
          case JsonToken.PropertyName:
            string name = r.Value.ToString();
            JProperty jproperty1 = new JProperty(name);
            jproperty1.SetLineInfo(lineInfo, settings);
            JProperty jproperty2 = ((JObject) jcontainer).Property(name);
            if (jproperty2 == null)
              jcontainer.Add((object) jproperty1);
            else
              jproperty2.Replace((JToken) jproperty1);
            jcontainer = (JContainer) jproperty1;
            goto case JsonToken.None;
          case JsonToken.Comment:
            if (settings != null && settings.CommentHandling == CommentHandling.Load)
            {
              JValue comment = JValue.CreateComment(r.Value.ToString());
              comment.SetLineInfo(lineInfo, settings);
              jcontainer.Add((object) comment);
              goto case JsonToken.None;
            }
            else
              goto case JsonToken.None;
          case JsonToken.Integer:
          case JsonToken.Float:
          case JsonToken.String:
          case JsonToken.Boolean:
          case JsonToken.Date:
          case JsonToken.Bytes:
            JValue jvalue1 = new JValue(r.Value);
            jvalue1.SetLineInfo(lineInfo, settings);
            jcontainer.Add((object) jvalue1);
            goto case JsonToken.None;
          case JsonToken.Null:
            JValue jvalue2 = JValue.CreateNull();
            jvalue2.SetLineInfo(lineInfo, settings);
            jcontainer.Add((object) jvalue2);
            goto case JsonToken.None;
          case JsonToken.Undefined:
            JValue undefined = JValue.CreateUndefined();
            undefined.SetLineInfo(lineInfo, settings);
            jcontainer.Add((object) undefined);
            goto case JsonToken.None;
          case JsonToken.EndObject:
            if (jcontainer == this)
              return;
            jcontainer = jcontainer.Parent;
            goto case JsonToken.None;
          case JsonToken.EndArray:
            if (jcontainer == this)
              return;
            jcontainer = jcontainer.Parent;
            goto case JsonToken.None;
          case JsonToken.EndConstructor:
            if (jcontainer == this)
              return;
            jcontainer = jcontainer.Parent;
            goto case JsonToken.None;
          default:
            throw new InvalidOperationException("The JsonReader should not be on a token of type {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) r.TokenType));
        }
      }
      while (r.Read());
    }

    internal int ContentsHashCode()
    {
      int num = 0;
      foreach (JToken childrenToken in (IEnumerable<JToken>) this.ChildrenTokens)
        num ^= childrenToken.GetDeepHashCode();
      return num;
    }

    string ITypedList.GetListName(PropertyDescriptor[] listAccessors)
    {
      return string.Empty;
    }

    PropertyDescriptorCollection ITypedList.GetItemProperties(
      PropertyDescriptor[] listAccessors)
    {
      return !(this.First is ICustomTypeDescriptor first) ? (PropertyDescriptorCollection) null : first.GetProperties();
    }

    int IList<JToken>.IndexOf(JToken item)
    {
      return this.IndexOfItem(item);
    }

    void IList<JToken>.Insert(int index, JToken item)
    {
      this.InsertItem(index, item, false);
    }

    void IList<JToken>.RemoveAt(int index)
    {
      this.RemoveItemAt(index);
    }

    JToken IList<JToken>.this[int index]
    {
      get
      {
        return this.GetItem(index);
      }
      set
      {
        this.SetItem(index, value);
      }
    }

    void ICollection<JToken>.Add(JToken item)
    {
      this.Add((object) item);
    }

    void ICollection<JToken>.Clear()
    {
      this.ClearItems();
    }

    bool ICollection<JToken>.Contains(JToken item)
    {
      return this.ContainsItem(item);
    }

    void ICollection<JToken>.CopyTo(JToken[] array, int arrayIndex)
    {
      this.CopyItemsTo((Array) array, arrayIndex);
    }

    bool ICollection<JToken>.IsReadOnly
    {
      get
      {
        return false;
      }
    }

    bool ICollection<JToken>.Remove(JToken item)
    {
      return this.RemoveItem(item);
    }

    private JToken EnsureValue(object value)
    {
      if (value == null)
        return (JToken) null;
      if (value is JToken jtoken)
        return jtoken;
      throw new ArgumentException("Argument is not a JToken.");
    }

    int IList.Add(object value)
    {
      this.Add((object) this.EnsureValue(value));
      return this.Count - 1;
    }

    void IList.Clear()
    {
      this.ClearItems();
    }

    bool IList.Contains(object value)
    {
      return this.ContainsItem(this.EnsureValue(value));
    }

    int IList.IndexOf(object value)
    {
      return this.IndexOfItem(this.EnsureValue(value));
    }

    void IList.Insert(int index, object value)
    {
      this.InsertItem(index, this.EnsureValue(value), false);
    }

    bool IList.IsFixedSize
    {
      get
      {
        return false;
      }
    }

    bool IList.IsReadOnly
    {
      get
      {
        return false;
      }
    }

    void IList.Remove(object value)
    {
      this.RemoveItem(this.EnsureValue(value));
    }

    void IList.RemoveAt(int index)
    {
      this.RemoveItemAt(index);
    }

    object IList.this[int index]
    {
      get
      {
        return (object) this.GetItem(index);
      }
      set
      {
        this.SetItem(index, this.EnsureValue(value));
      }
    }

    void ICollection.CopyTo(Array array, int index)
    {
      this.CopyItemsTo(array, index);
    }

    /// <summary>Gets the count of child JSON tokens.</summary>
    /// <value>The count of child JSON tokens</value>
    public int Count
    {
      get
      {
        return this.ChildrenTokens.Count;
      }
    }

    bool ICollection.IsSynchronized
    {
      get
      {
        return false;
      }
    }

    object ICollection.SyncRoot
    {
      get
      {
        if (this._syncRoot == null)
          Interlocked.CompareExchange(ref this._syncRoot, new object(), (object) null);
        return this._syncRoot;
      }
    }

    void IBindingList.AddIndex(PropertyDescriptor property)
    {
    }

    object IBindingList.AddNew()
    {
      AddingNewEventArgs e = new AddingNewEventArgs();
      this.OnAddingNew(e);
      if (e.NewObject == null)
        throw new JsonException("Could not determine new value to add to '{0}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.GetType()));
      if (!(e.NewObject is JToken))
        throw new JsonException("New item to be added to collection must be compatible with {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) typeof (JToken)));
      JToken newObject = (JToken) e.NewObject;
      this.Add((object) newObject);
      return (object) newObject;
    }

    bool IBindingList.AllowEdit
    {
      get
      {
        return true;
      }
    }

    bool IBindingList.AllowNew
    {
      get
      {
        return true;
      }
    }

    bool IBindingList.AllowRemove
    {
      get
      {
        return true;
      }
    }

    void IBindingList.ApplySort(
      PropertyDescriptor property,
      ListSortDirection direction)
    {
      throw new NotSupportedException();
    }

    int IBindingList.Find(PropertyDescriptor property, object key)
    {
      throw new NotSupportedException();
    }

    bool IBindingList.IsSorted
    {
      get
      {
        return false;
      }
    }

    void IBindingList.RemoveIndex(PropertyDescriptor property)
    {
    }

    void IBindingList.RemoveSort()
    {
      throw new NotSupportedException();
    }

    ListSortDirection IBindingList.SortDirection
    {
      get
      {
        return ListSortDirection.Ascending;
      }
    }

    PropertyDescriptor IBindingList.SortProperty
    {
      get
      {
        return (PropertyDescriptor) null;
      }
    }

    bool IBindingList.SupportsChangeNotification
    {
      get
      {
        return true;
      }
    }

    bool IBindingList.SupportsSearching
    {
      get
      {
        return false;
      }
    }

    bool IBindingList.SupportsSorting
    {
      get
      {
        return false;
      }
    }

    internal static void MergeEnumerableContent(
      JContainer target,
      IEnumerable content,
      JsonMergeSettings settings)
    {
      switch (settings.MergeArrayHandling)
      {
        case MergeArrayHandling.Concat:
          IEnumerator enumerator1 = content.GetEnumerator();
          try
          {
            while (enumerator1.MoveNext())
            {
              JToken current = (JToken) enumerator1.Current;
              target.Add((object) current);
            }
            break;
          }
          finally
          {
            if (enumerator1 is IDisposable disposable)
              disposable.Dispose();
          }
        case MergeArrayHandling.Union:
          HashSet<JToken> jtokenSet = new HashSet<JToken>((IEnumerable<JToken>) target, (IEqualityComparer<JToken>) JToken.EqualityComparer);
          IEnumerator enumerator2 = content.GetEnumerator();
          try
          {
            while (enumerator2.MoveNext())
            {
              JToken current = (JToken) enumerator2.Current;
              if (jtokenSet.Add(current))
                target.Add((object) current);
            }
            break;
          }
          finally
          {
            if (enumerator2 is IDisposable disposable)
              disposable.Dispose();
          }
        case MergeArrayHandling.Replace:
          target.ClearItems();
          IEnumerator enumerator3 = content.GetEnumerator();
          try
          {
            while (enumerator3.MoveNext())
            {
              JToken current = (JToken) enumerator3.Current;
              target.Add((object) current);
            }
            break;
          }
          finally
          {
            if (enumerator3 is IDisposable disposable)
              disposable.Dispose();
          }
        case MergeArrayHandling.Merge:
          int num = 0;
          IEnumerator enumerator4 = content.GetEnumerator();
          try
          {
            while (enumerator4.MoveNext())
            {
              object current = enumerator4.Current;
              if (num < target.Count)
              {
                if (target[(object) num] is JContainer jcontainer)
                  jcontainer.Merge(current, settings);
                else if (current != null)
                {
                  JToken fromContent = JContainer.CreateFromContent(current);
                  if (fromContent.Type != JTokenType.Null)
                    target[(object) num] = fromContent;
                }
              }
              else
                target.Add(current);
              ++num;
            }
            break;
          }
          finally
          {
            if (enumerator4 is IDisposable disposable)
              disposable.Dispose();
          }
        default:
          throw new ArgumentOutOfRangeException(nameof (settings), "Unexpected merge array handling when merging JSON.");
      }
    }
  }
}