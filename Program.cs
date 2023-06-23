using System;
using System.IO;
using System.Net;
using System.Text.Json;
using clasesBTC;

class Program
{
    static void Main()
    {
        try
        {
            CryptoBTC pBTC = GetBTC();

            Console.WriteLine("----------- Precios Disponibles -----------");
            Console.WriteLine($"USD: ${pBTC.bpi.USD.rate_float:F2}");
            Console.WriteLine($"EUR: ${pBTC.bpi.EUR.rate_float:F2}");
            Console.WriteLine($"GBP: ${pBTC.bpi.GBP.rate_float:F2}");

            Console.WriteLine("\nIngrese el código de la moneda para mostrar sus características (USD, EUR, GBP):");
            string selectedCurrencyCode = Console.ReadLine()?.ToUpper();

            if (!string.IsNullOrEmpty(selectedCurrencyCode) && pBTC.bpi.GetType().GetProperty(selectedCurrencyCode) != null)
            {
                var selectedCurrency = pBTC.bpi.GetType().GetProperty(selectedCurrencyCode)?.GetValue(pBTC.bpi);
                Console.WriteLine("\n----------- Características de la Moneda -----------");
                Console.WriteLine($"Código: {selectedCurrency.GetType().GetProperty("code")?.GetValue(selectedCurrency)}");
                Console.WriteLine($"Descripción: {selectedCurrency.GetType().GetProperty("description")?.GetValue(selectedCurrency)}");
                Console.WriteLine($"Tasa de Cambio: ${selectedCurrency.GetType().GetProperty("rate_float")?.GetValue(selectedCurrency):F2}");
            }
            else
            {
                Console.WriteLine("\nMoneda inválida. No se encontraron características para la moneda seleccionada.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al consumir el servicio web: {ex.Message}");
        }
    }

    static CryptoBTC GetBTC()
    {
        var url = "https://api.coindesk.com/v1/bpi/currentprice.json";
        var request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        request.ContentType = "application/json";
        request.Accept = "application/json";

        using (WebResponse response = request.GetResponse())
        {
            using (Stream strReader = response.GetResponseStream())
            {
                using (StreamReader objReader = new StreamReader(strReader))
                {
                    string responseBody = objReader.ReadToEnd();
                    return JsonSerializer.Deserialize<CryptoBTC>(responseBody);
                }
            }
        }
    }
}
