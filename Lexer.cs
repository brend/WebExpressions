using System.Text;

namespace Lex;

public class Lexer
{
    private readonly string expression;
    private readonly Reader reader;
    private readonly Queue<Token> lookahead = new Queue<Token>();

    public Lexer(string expression)
    {
        this.expression = expression;
        this.reader = new Reader(expression);
    }

    public Token? Peek()
    {
        if (lookahead.Count == 0)
        {
            if (ReadToken() is Token token)
            {
                lookahead.Enqueue(token);
            }
            else 
            {
                return null;
            }
        }

        return lookahead.Peek();
    }

    public Token? Read()
    {
        if (lookahead.Count > 0)
        {
            return lookahead.Dequeue();
        }
        return ReadToken();
    }

    private Token? ReadToken()
    {
        ConsumeWhitespace();

        var d = reader.Peek();

        if (d == -1)
        {
            return null;
        }

        char c = (char)d;
        
        if (char.IsDigit(c))
        {
            return ReadNumber();
        }
        
        if (char.IsLetter(c))
        {
            return ReadVariable();
        }
        
        return ReadOperator();
   }

    private void ConsumeWhitespace()
    {
         while (true)
         {
              var c = reader.Peek();
              if (c == -1)
              {
                break;
              }
              else if (char.IsWhiteSpace((char)c))
              {
                reader.Read();
              }
              else
              {
                break;
              }
         }
    }

    private Token ReadNumber()
    {
        var builder = new StringBuilder();

        while (true)
        {
            var c = reader.Peek();
            if (c == -1)
            {
                break;
            }
            else if (char.IsDigit((char)c) || (char)c == '.')
            {
                builder.Append((char)reader.Read());
            }
            else
            {
                break;
            }
        }

        var text = builder.ToString();

        return new Constant(text, reader.Row, reader.Col, double.Parse(text));
    }

    private Token ReadVariable()
    {
        var builder = new StringBuilder();

        while (true)
        {
            var c = reader.Peek();
            if (c == -1)
            {
                break;
            }
            else if (char.IsLetter((char)c) || char.IsDigit((char)c))
            {
                builder.Append((char)reader.Read());
            }
            else
            {
                break;
            }
        }

        var text = builder.ToString();

        return new Variable(text, reader.Row, reader.Col, text);
    }

    private Token ReadOperator()
    {
        var c = (char)reader.Read();
        switch (c)
        {
            case '+':
                return new Plus(c.ToString(), reader.Row, reader.Col);
            case '-':
                return new Minus(c.ToString(), reader.Row, reader.Col);
            case '*':
                return new Multiply(c.ToString(), reader.Row, reader.Col);
            case '/':
                return new Divide(c.ToString(), reader.Row, reader.Col);
            case '(':
                return new LParen(c.ToString(), reader.Row, reader.Col);
            case ')':
                return new RParen(c.ToString(), reader.Row, reader.Col);
            default:
                throw new Exception($"Unexpected character in {reader.Row}:{reader.Col}: {c}");
        }
    }
}

public abstract class Token
{
    public string Text { get; }
    public int Row { get; }
    public int Col { get; }

    protected Token(string text, int row, int col)
    {
        Text = text;
        Row = row;
        Col = col;
    }
}

public class Constant: Token
{
    public readonly double Value;

    public Constant(string text, int row, int col, double value)
    : base(text, row, col)
    {
        Value = value;
    }
}

public class Variable: Token
{
    public readonly string Name;

    public Variable(string text, int row, int col, string name)
    : base(text, row, col)
    {
        Name = name;
    }
}

public class Plus: Token 
{
    public Plus(string text, int row, int col): base(text, row, col)
    {
    }
}

public class Minus: Token
{
    public Minus(string text, int row, int col): base(text, row, col)
    {
    }
}

public class Multiply: Token
{
    public Multiply(string text, int row, int col): base(text, row, col)
    {
    }
}

public class Divide: Token 
{
    public Divide(string text, int row, int col): base(text, row, col)
    {
    }
}

public class LParen: Token 
{
    public LParen(string text, int row, int col): base(text, row, col)
    {
    }
}

public class RParen: Token 
{
    public RParen(string text, int row, int col): base(text, row, col)
    {
    }
}