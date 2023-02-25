namespace Banks.Models;
using System.Text;

public class PassportData
{
    public string _series;
    public string _number;

    public PassportData(string series, string number)
    {
        _series = series;
        _number = number;
    }
}