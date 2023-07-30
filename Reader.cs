public class Reader
{
    private readonly TextReader reader;

    public Reader(TextReader reader)
    {
        this.reader = reader;
    }

    public Reader(string text): this(new StringReader(text)) {}

    public int Row { get; private set; } = 1;

    public int Col { get; private set; } = 1;

    public int Peek() => reader.Peek();

    public int Read()
    {
        var c = reader.Read();
        if (c == '\n')
        {
            Row++;
            Col = 1;
        }
        else
        {
            Col++;
        }

        return c;
    }
}