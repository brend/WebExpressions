using Exp;

public class Evaluator
{
    public Expression Expression { get; }

    public Evaluator(Expression expression)
    {
        Expression = expression;
    }

    public Evaluator(string expression)
    {
        Expression = new Parse.Parser(expression).Parse();
    }

    public double Evaluate(Valuation valuation) 
        => EvaluateExpression(valuation, Expression);

    public double Evaluate(params object[] args)
        => EvaluateExpression(Valuation.Create(args), Expression);

    public double Evaluate(double x) 
        => Evaluate(new Valuation {["x"] = x});

    private double EvaluateExpression(Valuation valuation, Expression expression)
    {
        switch (expression)
        {
            case Constant constant:
                return constant.Value;
            case Variable variable:
                return valuation[variable.Name];
            case BinaryOperation binaryOperation:
                return EvaluateBinaryOperation(valuation, binaryOperation);
            case UnaryOperation unaryOperation:
                return EvaluateUnaryOperation(valuation, unaryOperation);
            default:
                throw new Exception($"Unknown expression type: {expression.GetType()}");
        }
    }

    private double EvaluateBinaryOperation(Valuation valuation, BinaryOperation binaryOperation)
    {
        var left = EvaluateExpression(valuation, binaryOperation.Left);
        var right = EvaluateExpression(valuation, binaryOperation.Right);

        switch (binaryOperation.Type)
        {
            case BinaryOperationType.Add:
                return left + right;
            case BinaryOperationType.Subtract:
                return left - right;
            case BinaryOperationType.Multiply:
                return left * right;
            case BinaryOperationType.Divide:
                return left / right;
            default:
                throw new Exception($"Unknown binary operation type: {binaryOperation.Type}");
        }
    }

    private double EvaluateUnaryOperation(Valuation valuation, UnaryOperation unaryOperation)
    {
        var value = EvaluateExpression(valuation, unaryOperation.Expression);

        switch (unaryOperation.Type)
        {
            case UnaryOperationType.Negate:
                return -value;
            default:
                throw new Exception($"Unknown unary operation type: {unaryOperation.Type}");
        }
    }
}