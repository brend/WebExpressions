namespace Parse;

public class ParseException : Exception
{
    public int Row { get; }
    
    public int Col { get; }

    public ParseException(int row, int col, string message) : base(message)
    {
        Row = row;
        Col = col;
    }

    public override string ToString() => $"Error at {Row}:{Col}: {Message}";
}

public class Parser
{
    private readonly string expression;
    private readonly Lex.Lexer lexer;   

    public Parser(string expression)
    {
        this.expression = expression;
        this.lexer = new Lex.Lexer(expression);
    }

    public Exp.Expression Parse()
    {
        var expression = ParseExpression();

        if (lexer.Peek() is Lex.Token token)
        {
            throw new ParseException(token.Row, token.Col, $"Unexpected token: {token}");
        }

        return expression;
    }

    private Exp.Expression ParseExpression()
    {
        var left = ParseTerm();

        while (true)
        {
            var token = lexer.Peek();

            if (token is Lex.Plus || token is Lex.Minus)
            {
                lexer.Read();

                var right = ParseTerm();
                var operationType = token is Lex.Plus ? Exp.BinaryOperationType.Add : Exp.BinaryOperationType.Subtract;

                left = new Exp.BinaryOperation(left, right, operationType);
            }
            else
            {
                return left;
            }
        }
    }

    private Exp.Expression ParseTerm()
    {
        var left = ParseFactor();

        while (true)
        {
            var token = lexer.Peek();
            if (token is Lex.Multiply || token is Lex.Divide)
            {
                lexer.Read();
                var right = ParseFactor();
                left = new Exp.BinaryOperation(left, right, token is Lex.Multiply ? Exp.BinaryOperationType.Multiply : Exp.BinaryOperationType.Divide);
            }
            else
            {
                return left;
            }
        }
    }

    private Exp.Expression ParseFactor()
    {
        var token = lexer.Peek();
        
        if (token is Lex.Constant constant)
        {
            lexer.Read();
            return new Exp.Constant(constant.Value);
        }

        else if (token is Lex.Variable variable)
        {
            lexer.Read();
            return new Exp.Variable(variable.Name);
        }
        else if (token is Lex.Minus)
        {
            lexer.Read();
            var expression = ParseFactor();
            return new Exp.UnaryOperation(expression, Exp.UnaryOperationType.Negate);
        }
        else if (token is Lex.LParen)
        {
            lexer.Read();

            var expression = ParseExpression();
            var nextToken = lexer.Peek();

            if (nextToken is not Lex.RParen)
            {
                if (nextToken is null)
                {
                    throw new ParseException(0, 0, $"Expected ')' but got end of input");
                }
                else
                {
                    throw new ParseException(token.Row, token.Col, $"Expected ')' but got {nextToken.GetType().Name} {nextToken.Text}");
                }
            }

            lexer.Read();

            return expression;
        }
        else
        {
            if (token is null)
            {
                throw new ParseException(0, 0, $"Unexpected end of input");
            }
            else
            {
                throw new ParseException(token.Row, token.Col, $"Unexpected token {token.GetType().Name} {token.Text}");
            }
        }
    }
}