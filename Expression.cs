namespace Exp;

public interface Expression
{

}

public struct Constant: Expression
{
    public readonly double Value;

    public Constant(double value)
    {
        Value = value;
    }

    public override string ToString() => Value.ToString();
}

public struct Variable: Expression
{
    public readonly string Name;

    public Variable(string name)
    {
        Name = name;
    }

    public override string ToString() => Name;
}

public enum BinaryOperationType
{
    Add,
    Subtract,
    Multiply,
    Divide
}

public enum UnaryOperationType
{
    Negate
}

public struct BinaryOperation: Expression
{
    public readonly Expression Left;
    public readonly Expression Right;
    public readonly BinaryOperationType Type;

    public BinaryOperation(Expression left, Expression right, BinaryOperationType type)
    {
        Left = left;
        Right = right;
        Type = type;
    }

    public override string ToString() => $"({Left} {TypeToString()} {Right})";

    private string TypeToString()
    {
        switch (Type)
        {
            case BinaryOperationType.Add:
                return "+";
            case BinaryOperationType.Subtract:
                return "-";
            case BinaryOperationType.Multiply:
                return "*";
            case BinaryOperationType.Divide:
                return "/";
            default:
                throw new Exception($"Unknown binary operation type: {Type}");
        }
    }
}

public struct UnaryOperation: Expression
{
    public readonly Expression Expression;
    public readonly UnaryOperationType Type;

    public UnaryOperation(Expression expression, UnaryOperationType type)
    {
        Expression = expression;
        Type = type;
    }

    public override string ToString() => $"({TypeToString()}{Expression})";

    private string TypeToString()
    {
        switch (Type)
        {
            case UnaryOperationType.Negate:
                return "-";
            default:
                throw new Exception($"Unknown unary operation type: {Type}");
        }
    }
}