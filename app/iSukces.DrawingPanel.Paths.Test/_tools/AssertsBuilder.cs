﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace iSukces.DrawingPanel.Paths.Test
{
    internal class AssertsBuilder
    {
        protected void Add(double n, string name, int decimalPlaces = 6)
        {
            AssertEqual(
                n.ToString(CultureInfo.InvariantCulture),
                name,
                decimalPlaces.ToString(CultureInfo.InvariantCulture));
        }

        protected void Add(int n, string name) { AssertEqual(n.ToString(CultureInfo.InvariantCulture), name); }

        protected bool AssertCount<T>(IReadOnlyList<T> list, string name)
        {
            if (list is null)
            {
                AssertNull(name);
                return true;
            }

            switch (list.Count)
            {
                case 0: AssertEmpty(name);
                    break;
                case 1: AssertSingle(name);
                    break;
                default: Add(list.Count, name + "." + nameof(list.Count));
                    break;
            }

            return false;
        }
        
        
        
        protected void AssertList<T>(IReadOnlyList<T> a, string name, Action<T, string> itemAssert)
        {
            if (AssertCount(a, name))
                return;
            for (var index = 0; index < a.Count; index++)
            {
                var name2 = $"{name}[{index.ToCs()}]";
                itemAssert(a[index], name2);
            }
        }


        private void Assert(string method, params string[] x)
        {
            var a = string.Join(", ", x);
            WriteLine($"Assert.{method}({a});");
        }
        
        protected void AssertEqual(params string[] x) { Assert("Equal", x); }

        protected void AssertExEqual(params string[] a) { WriteLine("AssertEx.Equal(" + string.Join(", ", a) + ");"); }

        protected void AssertNull(string name) { Assert("Null", name); }

        protected void AssertSingle(string name) { Assert("Single", name); }
        protected void AssertEmpty(string name) { Assert("Empty", name); }

        protected string Create(Action action)
        {
            _sb = new StringBuilder();
            WriteLine("#region Asserts");
            action();
            WriteLine("#endregion");
            return _sb.ToString();
        }

        protected string Declare<T>(T ignore, string expression)
        {
            var n  = _variables.GetName(typeof(T), out var first);
            var ex = n + " = (" + typeof(T).Name + ")" + expression + ";";
            if (first)
                ex = "var " + ex;
            WriteLine(ex);
            return n;
        }

        protected void Release(string name) { _variables.Release(name); }

        protected void WriteLine(string x) { _sb.AppendLine(x); }

        protected string Code => _sb.ToString();

        private readonly VariablesDictionary _variables = new();

        private StringBuilder _sb;
    }
}
