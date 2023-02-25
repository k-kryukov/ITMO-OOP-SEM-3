namespace Banks.Models;

public class DepositPercentsCalculator
{
    private SortedDictionary<decimal, decimal> _percentsMapping;
    private decimal _rateForHugeSum;

    public DepositPercentsCalculator(SortedDictionary<decimal, decimal> percentsMapping, decimal rateForHugeSum)
    {
        _percentsMapping = percentsMapping;
        _rateForHugeSum = rateForHugeSum;
    }

    public decimal CalculatePercents(decimal sum)
    {
        decimal res = 0;
        foreach (KeyValuePair<decimal, decimal> kvp in _percentsMapping)
        {
            if (sum >= kvp.Key)
                res = kvp.Value;
            else
                break;
        }

        return res;
    }
}