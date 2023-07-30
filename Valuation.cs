public class Valuation: Dictionary<string, double>
{
    public static Valuation Create(params object[] args)
    {
        var valuation = new Valuation();

        for (int i = 0; i + 1 < args.Length; i += 2)
        {
            valuation[Convert.ToString(args[i]) ?? throw new ArgumentException("String expected")] = 
                Convert.ToDouble(args[i + 1]);
        }

        return valuation;
    }

    public static Valuation Math()
    {
        return Create(
            "pi", System.Math.PI,
            "e", System.Math.E,
            "phi", (1 + System.Math.Sqrt(5)) / 2,
            "tau", 2 * System.Math.PI,
            "sqrt2", System.Math.Sqrt(2),
            "sqrt3", System.Math.Sqrt(3)
        );
    }

    public Valuation Extend(params object[] args)
    {
        var valuation = new Valuation();

        foreach (var pair in this)
        {
            valuation[pair.Key] = pair.Value;
        }

        var extension = Create(args);

        foreach (var pair in extension)
        {
            valuation[pair.Key] = pair.Value;
        }

        return valuation;
    }
}