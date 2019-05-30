using System;

public class NumberEnding
{
    public string Calculate(int number)
    {
        string suff = string.Empty;

        var ones = number % 10;
        var tens = Math.Floor(number / 10m) % 10;
        if (tens == 1)
        {
            suff = "th";
        }
        else
        {
            switch (ones)
            {
                case 1: suff = "st"; break;
                case 2: suff = "nd"; break;
                case 3: suff = "rd"; break;
                default: suff = "th"; break;
            }
        }
        return suff;
    }
}
