using System;

namespace iSukces.DrawingPanel.Paths.Test
{
    public struct TypedExpression
    {
        public TypedExpression(string expression, Type type)
        {
            Expression = expression;
            Type       = type;
        }

        public string Expression { get; }
        public Type   Type       { get; }
    }
}
