namespace PlasterSkull.Framework.Core;

public static class MathHelper
{
    private const int s_orderDeviationMultiplierRange = 1000;
    private const double s_orderDeviationStepBits = 10e+7;

    public static double CalculateOrder(double? previousOrder, double? nextOrder, bool descending = false)
    {
        if (descending)
            (nextOrder, previousOrder) = (previousOrder, nextOrder);

        double order = true switch
        {
            _ when previousOrder != null && nextOrder != null => (previousOrder.Value + nextOrder.Value) / 2,
            _ when previousOrder != null => previousOrder.Value + 1,
            _ when nextOrder != null => nextOrder.Value - 1,
            _ => 0
        };

        double deviationStep = (Math.BitIncrement(order) - order) * s_orderDeviationStepBits;

        if (previousOrder != null && nextOrder != null)
        {
            double maxDeviationStep = (nextOrder.Value - previousOrder.Value) / s_orderDeviationMultiplierRange;
            if (deviationStep > maxDeviationStep)
                deviationStep = maxDeviationStep;
        }

        int deviationMultiplier = Random.Shared.Next(0, s_orderDeviationMultiplierRange) - (s_orderDeviationMultiplierRange / 2);
        double deviation = deviationStep * deviationMultiplier;
        order += deviation;

        return order;
    }
}
