using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using PrimeNumber.Models;

namespace PrimeNumber.Controllers;
public class CalcController : Controller
{
    private static string DotNumber(int dotNumber)
    {
        NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

        nfi.CurrencyGroupSeparator = ".";
        nfi.CurrencySymbol = "";
        var answer = Convert.ToDecimal(dotNumber).ToString("C0", nfi);

        return answer;
    }

    public IActionResult Index()
    {
        return View(new Calc());
    }

    [HttpPost]
    public IActionResult Index(Calc c, string calculate)
    {
        if (calculate == "sum")
        {
            bool isPrime = true;
            int i, j;

            int number = Convert.ToInt32(c.Number);

            int countIsPrime = 0;
            int countNotPrime = 0;

            var results = new List<int>();

            try
            {
                for (i = 2; i <= number; i++)
                {
                    for (j = 2; j <= number; j++)
                    {
                        if (i != j && i % j == 0)
                        {
                            countNotPrime++;
                            isPrime = false;
                            break;
                        }
                    }
                    if (isPrime)
                    {
                        countIsPrime++;
                        results.Add(i);
                        c.Result = results;
                        foreach (var item in results)
                        {
                            ViewBag.Result = string.Format("{0}", item);
                        }
                    }
                    isPrime = true;
                }
                ViewBag.MessageNumberToNumber = "Os números primos de 1 até " + DotNumber(number) + " são: ";
                ViewBag.CountIsPrime = "Quantidade de números primos: " + countIsPrime;
                ViewBag.CountNotPrime = "Quantidade de números que NÃO são primos: " + countNotPrime;
            }
            catch (Exception e)
            {
                ViewBag.FailedTryOne = "Parece que ocorreu um erro inesperado :(";
                Console.WriteLine(e);
            }
        }
        return View(c);
    }
}
