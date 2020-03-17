// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonPath.JPath
// Assembly: Newtonsoft.Json, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C34B75D-E3FC-4B43-BABB-D260B14FFEEB
// Assembly location: D:\Git\intellik-2019\Assets\ThirdParty\JsonDotNet\Assemblies\AOT\Newtonsoft.Json.dll

using Newtonsoft.Json.Shims;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Newtonsoft.Json.Linq.JsonPath
{
  [Preserve]
  internal class JPath
  {
    private readonly string _expression;
    private int _currentIndex;

    public List<PathFilter> Filters { get; private set; }

    public JPath(string expression)
    {
      ValidationUtils.ArgumentNotNull((object) expression, nameof (expression));
      this._expression = expression;
      this.Filters = new List<PathFilter>();
      this.ParseMain();
    }

    private void ParseMain()
    {
      int currentIndex1 = this._currentIndex;
      this.EatWhitespace();
      if (this._expression.Length == this._currentIndex)
        return;
      if (this._expression[this._currentIndex] == '$')
      {
        if (this._expression.Length == 1)
          return;
        switch (this._expression[this._currentIndex + 1])
        {
          case '.':
          case '[':
            ++this._currentIndex;
            currentIndex1 = this._currentIndex;
            break;
        }
      }
      if (this.ParsePath(this.Filters, currentIndex1, false))
        return;
      int currentIndex2 = this._currentIndex;
      this.EatWhitespace();
      if (this._currentIndex < this._expression.Length)
        throw new JsonException("Unexpected character while parsing path: " + this._expression[currentIndex2].ToString());
    }

    private bool ParsePath(List<PathFilter> filters, int currentPartStartIndex, bool query)
    {
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      bool flag4 = false;
      while (this._currentIndex < this._expression.Length && !flag4)
      {
        char indexerOpenChar = this._expression[this._currentIndex];
        switch (indexerOpenChar)
        {
          case ' ':
            if (this._currentIndex < this._expression.Length)
            {
              flag4 = true;
              continue;
            }
            continue;
          case '(':
          case '[':
            if (this._currentIndex > currentPartStartIndex)
            {
              string str = this._expression.Substring(currentPartStartIndex, this._currentIndex - currentPartStartIndex);
              if (str == "*")
                str = (string) null;
              PathFilter pathFilter1;
              if (!flag1)
              {
                pathFilter1 = (PathFilter) new FieldFilter()
                {
                  Name = str
                };
              }
              else
              {
                pathFilter1 = (PathFilter) new ScanFilter();
                ((ScanFilter) pathFilter1).Name = str;
              }
              PathFilter pathFilter2 = pathFilter1;
              filters.Add(pathFilter2);
              flag1 = false;
            }
            filters.Add(this.ParseIndexer(indexerOpenChar));
            ++this._currentIndex;
            currentPartStartIndex = this._currentIndex;
            flag2 = true;
            flag3 = false;
            continue;
          case ')':
          case ']':
            flag4 = true;
            continue;
          case '.':
            if (this._currentIndex > currentPartStartIndex)
            {
              string str = this._expression.Substring(currentPartStartIndex, this._currentIndex - currentPartStartIndex);
              if (str == "*")
                str = (string) null;
              PathFilter pathFilter1;
              if (!flag1)
              {
                pathFilter1 = (PathFilter) new FieldFilter()
                {
                  Name = str
                };
              }
              else
              {
                pathFilter1 = (PathFilter) new ScanFilter();
                ((ScanFilter) pathFilter1).Name = str;
              }
              PathFilter pathFilter2 = pathFilter1;
              filters.Add(pathFilter2);
              flag1 = false;
            }
            if (this._currentIndex + 1 < this._expression.Length && this._expression[this._currentIndex + 1] == '.')
            {
              flag1 = true;
              ++this._currentIndex;
            }
            ++this._currentIndex;
            currentPartStartIndex = this._currentIndex;
            flag2 = false;
            flag3 = true;
            continue;
          default:
            if (query && (indexerOpenChar == '=' || indexerOpenChar == '<' || (indexerOpenChar == '!' || indexerOpenChar == '>') || (indexerOpenChar == '|' || indexerOpenChar == '&')))
            {
              flag4 = true;
              continue;
            }
            if (flag2)
              throw new JsonException("Unexpected character following indexer: " + indexerOpenChar.ToString());
            ++this._currentIndex;
            continue;
        }
      }
      bool flag5 = this._currentIndex == this._expression.Length;
      if (this._currentIndex > currentPartStartIndex)
      {
        string str = this._expression.Substring(currentPartStartIndex, this._currentIndex - currentPartStartIndex).TrimEnd();
        if (str == "*")
          str = (string) null;
        PathFilter pathFilter1;
        if (!flag1)
        {
          pathFilter1 = (PathFilter) new FieldFilter()
          {
            Name = str
          };
        }
        else
        {
          pathFilter1 = (PathFilter) new ScanFilter();
          ((ScanFilter) pathFilter1).Name = str;
        }
        PathFilter pathFilter2 = pathFilter1;
        filters.Add(pathFilter2);
      }
      else if (flag3 && flag5 | query)
        throw new JsonException("Unexpected end while parsing path.");
      return flag5;
    }

    private PathFilter ParseIndexer(char indexerOpenChar)
    {
      ++this._currentIndex;
      char indexerCloseChar = indexerOpenChar == '[' ? ']' : ')';
      this.EnsureLength("Path ended with open indexer.");
      this.EatWhitespace();
      if (this._expression[this._currentIndex] == '\'')
        return this.ParseQuotedField(indexerCloseChar);
      return this._expression[this._currentIndex] == '?' ? this.ParseQuery(indexerCloseChar) : this.ParseArrayIndexer(indexerCloseChar);
    }

    private PathFilter ParseArrayIndexer(char indexerCloseChar)
    {
      int currentIndex = this._currentIndex;
      int? nullable1 = new int?();
      List<int> intList = (List<int>) null;
      int num = 0;
      int? nullable2 = new int?();
      int? nullable3 = new int?();
      int? nullable4 = new int?();
      while (this._currentIndex < this._expression.Length)
      {
        char c = this._expression[this._currentIndex];
        if (c == ' ')
        {
          nullable1 = new int?(this._currentIndex);
          this.EatWhitespace();
        }
        else
        {
          int? nullable5;
          if ((int) c == (int) indexerCloseChar)
          {
            nullable5 = nullable1;
            int length = (nullable5 ?? this._currentIndex) - currentIndex;
            if (intList != null)
            {
              if (length == 0)
                throw new JsonException("Array index expected.");
              int int32 = Convert.ToInt32(this._expression.Substring(currentIndex, length), (IFormatProvider) CultureInfo.InvariantCulture);
              intList.Add(int32);
              return (PathFilter) new ArrayMultipleIndexFilter()
              {
                Indexes = intList
              };
            }
            if (num > 0)
            {
              if (length > 0)
              {
                int int32 = Convert.ToInt32(this._expression.Substring(currentIndex, length), (IFormatProvider) CultureInfo.InvariantCulture);
                if (num == 1)
                  nullable3 = new int?(int32);
                else
                  nullable4 = new int?(int32);
              }
              return (PathFilter) new ArraySliceFilter()
              {
                Start = nullable2,
                End = nullable3,
                Step = nullable4
              };
            }
            if (length == 0)
              throw new JsonException("Array index expected.");
            int int32_1 = Convert.ToInt32(this._expression.Substring(currentIndex, length), (IFormatProvider) CultureInfo.InvariantCulture);
            return (PathFilter) new ArrayIndexFilter()
            {
              Index = new int?(int32_1)
            };
          }
          switch (c)
          {
            case '*':
              ++this._currentIndex;
              this.EnsureLength("Path ended with open indexer.");
              this.EatWhitespace();
              if ((int) this._expression[this._currentIndex] != (int) indexerCloseChar)
                throw new JsonException("Unexpected character while parsing path indexer: " + c.ToString());
              return (PathFilter) new ArrayIndexFilter();
            case ',':
              nullable5 = nullable1;
              int length1 = (nullable5 ?? this._currentIndex) - currentIndex;
              if (length1 == 0)
                throw new JsonException("Array index expected.");
              if (intList == null)
                intList = new List<int>();
              string str = this._expression.Substring(currentIndex, length1);
              intList.Add(Convert.ToInt32(str, (IFormatProvider) CultureInfo.InvariantCulture));
              ++this._currentIndex;
              this.EatWhitespace();
              currentIndex = this._currentIndex;
              nullable1 = new int?();
              continue;
            case ':':
              nullable5 = nullable1;
              int length2 = (nullable5 ?? this._currentIndex) - currentIndex;
              if (length2 > 0)
              {
                int int32 = Convert.ToInt32(this._expression.Substring(currentIndex, length2), (IFormatProvider) CultureInfo.InvariantCulture);
                switch (num)
                {
                  case 0:
                    nullable2 = new int?(int32);
                    break;
                  case 1:
                    nullable3 = new int?(int32);
                    break;
                  default:
                    nullable4 = new int?(int32);
                    break;
                }
              }
              ++num;
              ++this._currentIndex;
              this.EatWhitespace();
              currentIndex = this._currentIndex;
              nullable1 = new int?();
              continue;
            default:
              if (!char.IsDigit(c) && c != '-')
                throw new JsonException("Unexpected character while parsing path indexer: " + c.ToString());
              if (nullable1.HasValue)
                throw new JsonException("Unexpected character while parsing path indexer: " + c.ToString());
              ++this._currentIndex;
              continue;
          }
        }
      }
      throw new JsonException("Path ended with open indexer.");
    }

    private void EatWhitespace()
    {
      while (this._currentIndex < this._expression.Length && this._expression[this._currentIndex] == ' ')
        ++this._currentIndex;
    }

    private PathFilter ParseQuery(char indexerCloseChar)
    {
      ++this._currentIndex;
      this.EnsureLength("Path ended with open indexer.");
      if (this._expression[this._currentIndex] != '(')
        throw new JsonException("Unexpected character while parsing path indexer: " + this._expression[this._currentIndex].ToString());
      ++this._currentIndex;
      QueryExpression expression = this.ParseExpression();
      ++this._currentIndex;
      this.EnsureLength("Path ended with open indexer.");
      this.EatWhitespace();
      if ((int) this._expression[this._currentIndex] != (int) indexerCloseChar)
        throw new JsonException("Unexpected character while parsing path indexer: " + this._expression[this._currentIndex].ToString());
      return (PathFilter) new QueryFilter()
      {
        Expression = expression
      };
    }

    private QueryExpression ParseExpression()
    {
      QueryExpression queryExpression = (QueryExpression) null;
      CompositeExpression compositeExpression1 = (CompositeExpression) null;
      while (this._currentIndex < this._expression.Length)
      {
        this.EatWhitespace();
        if (this._expression[this._currentIndex] != '@')
          throw new JsonException("Unexpected character while parsing path query: " + this._expression[this._currentIndex].ToString());
        ++this._currentIndex;
        List<PathFilter> filters = new List<PathFilter>();
        if (this.ParsePath(filters, this._currentIndex, true))
          throw new JsonException("Path ended with open query.");
        this.EatWhitespace();
        this.EnsureLength("Path ended with open query.");
        object obj = (object) null;
        QueryOperator queryOperator;
        if (this._expression[this._currentIndex] == ')' || this._expression[this._currentIndex] == '|' || this._expression[this._currentIndex] == '&')
        {
          queryOperator = QueryOperator.Exists;
        }
        else
        {
          queryOperator = this.ParseOperator();
          this.EatWhitespace();
          this.EnsureLength("Path ended with open query.");
          obj = this.ParseValue();
          this.EatWhitespace();
          this.EnsureLength("Path ended with open query.");
        }
        BooleanQueryExpression booleanQueryExpression1 = new BooleanQueryExpression();
        booleanQueryExpression1.Path = filters;
        booleanQueryExpression1.Operator = queryOperator;
        booleanQueryExpression1.Value = queryOperator != QueryOperator.Exists ? new JValue(obj) : (JValue) null;
        BooleanQueryExpression booleanQueryExpression2 = booleanQueryExpression1;
        if (this._expression[this._currentIndex] == ')')
        {
          if (compositeExpression1 == null)
            return (QueryExpression) booleanQueryExpression2;
          compositeExpression1.Expressions.Add((QueryExpression) booleanQueryExpression2);
          return queryExpression;
        }
        if (this._expression[this._currentIndex] == '&' && this.Match("&&"))
        {
          if (compositeExpression1 == null || compositeExpression1.Operator != QueryOperator.And)
          {
            CompositeExpression compositeExpression2 = new CompositeExpression();
            compositeExpression2.Operator = QueryOperator.And;
            CompositeExpression compositeExpression3 = compositeExpression2;
            compositeExpression1?.Expressions.Add((QueryExpression) compositeExpression3);
            compositeExpression1 = compositeExpression3;
            if (queryExpression == null)
              queryExpression = (QueryExpression) compositeExpression1;
          }
          compositeExpression1.Expressions.Add((QueryExpression) booleanQueryExpression2);
        }
        if (this._expression[this._currentIndex] == '|' && this.Match("||"))
        {
          if (compositeExpression1 == null || compositeExpression1.Operator != QueryOperator.Or)
          {
            CompositeExpression compositeExpression2 = new CompositeExpression();
            compositeExpression2.Operator = QueryOperator.Or;
            CompositeExpression compositeExpression3 = compositeExpression2;
            compositeExpression1?.Expressions.Add((QueryExpression) compositeExpression3);
            compositeExpression1 = compositeExpression3;
            if (queryExpression == null)
              queryExpression = (QueryExpression) compositeExpression1;
          }
          compositeExpression1.Expressions.Add((QueryExpression) booleanQueryExpression2);
        }
      }
      throw new JsonException("Path ended with open query.");
    }

    private object ParseValue()
    {
      char c = this._expression[this._currentIndex];
      if (c == '\'')
        return (object) this.ReadQuotedString();
      if (!char.IsDigit(c))
      {
        switch (c)
        {
          case '-':
            break;
          case 'f':
            if (this.Match("false"))
              return (object) false;
            goto label_21;
          case 'n':
            if (this.Match("null"))
              return (object) null;
            goto label_21;
          case 't':
            if (this.Match("true"))
              return (object) true;
            goto label_21;
          default:
            goto label_21;
        }
      }
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(c);
      for (++this._currentIndex; this._currentIndex < this._expression.Length; ++this._currentIndex)
      {
        char ch = this._expression[this._currentIndex];
        switch (ch)
        {
          case ' ':
          case ')':
            string s = stringBuilder.ToString();
            if (s.IndexOfAny(new char[3]{ '.', 'E', 'e' }) != -1)
            {
              double result;
              if (double.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, (IFormatProvider) CultureInfo.InvariantCulture, out result))
                return (object) result;
              throw new JsonException("Could not read query value.");
            }
            long result1;
            if (long.TryParse(s, NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture, out result1))
              return (object) result1;
            throw new JsonException("Could not read query value.");
          default:
            stringBuilder.Append(ch);
            continue;
        }
      }
label_21:
      throw new JsonException("Could not read query value.");
    }

    private string ReadQuotedString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      ++this._currentIndex;
      while (this._currentIndex < this._expression.Length)
      {
        char ch = this._expression[this._currentIndex];
        if (ch == '\\' && this._currentIndex + 1 < this._expression.Length)
        {
          ++this._currentIndex;
          if (this._expression[this._currentIndex] == '\'')
          {
            stringBuilder.Append('\'');
          }
          else
          {
            if (this._expression[this._currentIndex] != '\\')
              throw new JsonException("Unknown escape chracter: \\" + this._expression[this._currentIndex].ToString());
            stringBuilder.Append('\\');
          }
          ++this._currentIndex;
        }
        else
        {
          if (ch == '\'')
          {
            ++this._currentIndex;
            return stringBuilder.ToString();
          }
          ++this._currentIndex;
          stringBuilder.Append(ch);
        }
      }
      throw new JsonException("Path ended with an open string.");
    }

    private bool Match(string s)
    {
      int currentIndex = this._currentIndex;
      foreach (char ch in s)
      {
        if (currentIndex >= this._expression.Length || (int) this._expression[currentIndex] != (int) ch)
          return false;
        ++currentIndex;
      }
      this._currentIndex = currentIndex;
      return true;
    }

    private QueryOperator ParseOperator()
    {
      if (this._currentIndex + 1 >= this._expression.Length)
        throw new JsonException("Path ended with open query.");
      if (this.Match("=="))
        return QueryOperator.Equals;
      if (this.Match("!=") || this.Match("<>"))
        return QueryOperator.NotEquals;
      if (this.Match("<="))
        return QueryOperator.LessThanOrEquals;
      if (this.Match("<"))
        return QueryOperator.LessThan;
      if (this.Match(">="))
        return QueryOperator.GreaterThanOrEquals;
      if (this.Match(">"))
        return QueryOperator.GreaterThan;
      throw new JsonException("Could not read query operator.");
    }

    private PathFilter ParseQuotedField(char indexerCloseChar)
    {
      List<string> stringList = (List<string>) null;
      while (this._currentIndex < this._expression.Length)
      {
        string str = this.ReadQuotedString();
        this.EatWhitespace();
        this.EnsureLength("Path ended with open indexer.");
        if ((int) this._expression[this._currentIndex] == (int) indexerCloseChar)
        {
          if (stringList != null)
          {
            stringList.Add(str);
            return (PathFilter) new FieldMultipleFilter()
            {
              Names = stringList
            };
          }
          return (PathFilter) new FieldFilter()
          {
            Name = str
          };
        }
        if (this._expression[this._currentIndex] != ',')
          throw new JsonException("Unexpected character while parsing path indexer: " + this._expression[this._currentIndex].ToString());
        ++this._currentIndex;
        this.EatWhitespace();
        if (stringList == null)
          stringList = new List<string>();
        stringList.Add(str);
      }
      throw new JsonException("Path ended with open indexer.");
    }

    private void EnsureLength(string message)
    {
      if (this._currentIndex >= this._expression.Length)
        throw new JsonException(message);
    }

    internal IEnumerable<JToken> Evaluate(JToken t, bool errorWhenNoMatch)
    {
      return JPath.Evaluate(this.Filters, t, errorWhenNoMatch);
    }

    internal static IEnumerable<JToken> Evaluate(
      List<PathFilter> filters,
      JToken t,
      bool errorWhenNoMatch)
    {
      IEnumerable<JToken> current = (IEnumerable<JToken>) new JToken[1]
      {
        t
      };
      foreach (PathFilter filter in filters)
        current = filter.ExecuteFilter(current, errorWhenNoMatch);
      return current;
    }
  }
}
