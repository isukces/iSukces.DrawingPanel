using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace iSukces.DrawingPanel.Paths.Test
{
    internal class AssertsBuilder
    {
        protected static TypedExpression CastIfNecessary<TRequested>(TypedExpression expression)
        {
            return CastIfNecessary(expression, typeof(TRequested));
        }

        protected static TypedExpression CastIfNecessary(TypedExpression expression, Type requestedType)
        {
            if (expression.SameType(requestedType))
                return expression;
            var code = $"({requestedType.Name}){expression}";
            return new TypedExpression(code, requestedType);
        }

        protected static TypedExpression CastIfNecessary<TKnown, TRequested>(string expression)
        {
            var typedExpression = new TypedExpression(expression, typeof(TKnown));
            return CastIfNecessary<TRequested>(typedExpression);
        }

        protected void Add(double n, string name, int decimalPlaces = 6)
        {
            AssertEqual(
                n.ToString(CultureInfo.InvariantCulture),
                name,
                decimalPlaces.ToString(CultureInfo.InvariantCulture));
        }

        protected void Add(int n, string name) { AssertEqual(n.ToString(CultureInfo.InvariantCulture), name); }


        private void Assert(string method, params string[] x)
        {
            var a = string.Join(", ", x);
            WriteLine($"Assert.{method}({a});");
        }

        protected bool AssertCount<T>(IReadOnlyList<T> list, string name)
        {
            if (list is null)
            {
                AssertNull(name);
                return true;
            }

            switch (list.Count)
            {
                case 0:
                    AssertEmpty(name);
                    break;
                case 1:
                    AssertSingle(name);
                    break;
                default:
                    Add(list.Count, name + "." + nameof(list.Count));
                    break;
            }

            return false;
        }

        protected void AssertEmpty(string name) { Assert("Empty", name); }


        protected void AssertEqualBool(bool expected, string expression)
        {
            if (expected)
                AssertTrue(expression);
            else
                AssertFalse(expression);
        }
        protected void AssertEqual(params string[] x) { Assert("Equal", x); }

        protected void AssertExEqual(params string[] a) { WriteLine("AssertEx.Equal(" + string.Join(", ", a) + ");"); }
        protected void AssertTrue(string a) { WriteLine("Assert.True(" + a + ");"); }
        protected void AssertFalse(string a) { WriteLine("Assert.False(" + a + ");"); }
        

        protected void AssertList<T>(IReadOnlyList<T> a, string name, Action<T, string, Type> itemAssert,
            bool resolveRealType)
        {
            if (AssertCount(a, name))
                return;
            for (var index = 0; index < a.Count; index++)
            {
                var expression = $"{name}[{index.ToCs()}]";

                var value = a[index];
                if (value is null)
                {
                    AssertNull(expression);
                    continue;
                }

                var type = resolveRealType ? value.GetType() : typeof(T);

                var code    = TypedExpression.Make<T>(expression);
                var tmpName = Declare(type, code);
                itemAssert(value, tmpName, type);
                Release(tmpName);
            }
        }

        protected void AssertNull(string name) { Assert("Null", name); }

        protected void AssertSingle(string name) { Assert("Single", name); }

        protected void CastAndDoAction<T>(T expressionCode, Type expressionType, string name,
            Action<TypedExpression> action)
        {
            if (expressionType == typeof(T))
            {
                action(new TypedExpression(name, expressionType));
                return;
            }

            var casted = CastIfNecessary<T>(new TypedExpression(name, expressionType));
            name = Declare(expressionCode, casted);
            action(new TypedExpression(name, typeof(T)));
            Release(name);
        }

        public string Create(Action action)
        {
            _sb = new StringBuilder();
            WriteLine("#region Asserts");
            action();
            WriteLine("#endregion");
            return _sb.ToString();
        }


        protected string Declare<TVariable>(TVariable ignore, TypedExpression expression)
        {
            return Declare<TVariable>(expression);
        }

        protected string Declare<TVariable>(TypedExpression expression)
        {
            return Declare(typeof(TVariable), expression);
        }

        protected string Declare(Type variableType, TypedExpression expression)
        {
            var n    = _variables.GetName(variableType, out var first);
            var code = expression.Code;
            if (!expression.SameType(variableType))
                code = $"({variableType.Name}){code}";
            var ex = $"{n} = {code};";
            if (first)
                ex = "var " + ex;
            WriteLine(ex);
            return n;
        }

        protected string Declare<T>(T ignore, string expression, bool cast = true)
        {
            return Declare<T>(expression, cast);
        }

        protected string Declare<T>(string expression, bool cast = true)
        {
            return Declare(typeof(T), expression, cast);
        }

        protected string Declare(Type t, string expression, bool cast = true)
        {
            var n       = _variables.GetName(t, out var first);
            var castStr = cast ? $"({t.Name})" : "";
            var ex      = $"{n} = {castStr}{expression};";
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
