using System;

namespace iSukces.DrawingPanel.Paths.Test;

public struct TypedExpression
{
    public TypedExpression(string code, Type type)
    {
        Code = code;
        Type = type;
    }

    public string Code { get; }
    public Type   Type { get; }

    public bool SameType(Type requestedType)
    {
        return requestedType == Type;
            
    }
    public static TypedExpression Make<T>(string code)
    {
        return new TypedExpression(code, typeof(T));
    }
}
