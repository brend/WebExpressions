using NUnit.Framework;

namespace Tests;

[TestFixture]
public class YourExpressionEvaluatorTests
{
    [Test]
    public void TestExpression1()
    {
        var evaluator = new Evaluator("3*x + 7");
        double result = evaluator.Evaluate("x", 5);

        Assert.AreEqual(22, result);
    }

    [Test]
    public void TestExpression2()
    {
        var evaluator = new Evaluator("x * pi / 180");
        double result = evaluator.Evaluate(Valuation.Math().Extend("x", 90));

        Assert.AreEqual(Math.PI / 2, result);
    }

    [Test]
    public void TestExpression3()
    {
        var evaluator = new Evaluator("width * height");
        double result = evaluator.Evaluate("width", 12, "height", 10);

        Assert.AreEqual(120, result);
    }

    [Test]
    public void TestExpression4()
    {
        var evaluator = new Evaluator("x * y + z");
        double result = evaluator.Evaluate("x", 2, "y", 3, "z", 4);

        Assert.AreEqual(10, result);
    }

    [Test]
    public void TestExpression5()
    {
        Assert.Throws<Parse.ParseException>(() => new Evaluator("x * y + "));
    }

    [Test]
    public void TestExpression6()
    {
        Assert.Throws<Parse.ParseException>(() => new Evaluator("x * y + 1 2"));
    }
}
