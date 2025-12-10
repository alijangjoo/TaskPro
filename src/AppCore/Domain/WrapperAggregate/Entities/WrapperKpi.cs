namespace Suspect.TaskPro.AppCore.Domain.WrapperAggregate.Entities;

using OysterFx.AppCore.Domain.Aggregates;
using OysterFx.AppCore.Domain.Exceptions;

public class WrapperKpi : Aggregate
{
    public string? Name { get; private set; }
    public string? Expression { get; private set; }
    public int TargetValue { get; private set; }
    public string? Unit { get; private set; }
    public string? Description { get; private set; }
    public double? CurrentValue { get; private set; }

    private WrapperKpi() { }

    private WrapperKpi(
        string? name,
        string? expression,
        int targetValue,
        string? unit,
        string? description,
        double? currentValue)
    {
        Name = name;
        Expression = expression;
        TargetValue = targetValue;
        Unit = unit;
        Description = description;
        CurrentValue = currentValue;
    }

    public static WrapperKpi CreateInstance(
        string? name,
        string? expression,
        int targetValue,
        string? unit,
        string? description,
        double? currentValue)
        => new(name, expression, targetValue, unit, description, currentValue);

    public void UpdateExpression(string expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
            throw new AggregateStateExceptions("Expression cannot be empty", "");

        Expression = expression;
    }

    public void UpdateTarget(int value)
    {
        if (value < 0)
            throw new AggregateStateExceptions("Target must be positive", "");

        TargetValue = value;
    }

    //public void Evaluate(IDictionary<string, double> variables, IExpressionEngine engine)
    //{
    //    if (Expression == null)
    //        throw new AggregateStateExceptions("Expression is empty", "");

    //    CurrentValue = engine.Evaluate(Expression, variables);
    //}

    public bool IsTargetAchieved()
        => CurrentValue.HasValue && CurrentValue.Value >= TargetValue;
}