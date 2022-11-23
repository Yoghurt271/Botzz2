using HtmlAgilityPack;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace Botzz
{
    class Program
    {
        static void Main(string[] args)
        {
            Action();
        }

        static void Action() // Действия бота
        {
            string pageContent = LoadPage(url: @"https://www.gismeteo.ru/weather-vladimir-4350/now/"); // Загрузка страницы для погоды
            var document = new HtmlDocument();
            document.LoadHtml(pageContent);

            Console.WriteLine("Введите свое имя: ");
            string name = Console.ReadLine();

            Console.WriteLine("Здраствуйте " + name);

            Console.WriteLine("Что бы узнать что может бот напишите *помощь*");
            string textUser = "";
            while (textUser.ToLower() != "выход") // Заканчивает програму только после слова выход
            {
                textUser = Console.ReadLine();
                switch (textUser.ToLower())
                {
                  
                    case "погода":
                        HtmlNodeCollection links = document.DocumentNode.SelectNodes("/html/body/section[2]/div[1]/section[3]/div/div[3]/span[1]"); // Парсинг
                        HtmlNodeCollection descrip = document.DocumentNode.SelectNodes("/html/body/section[2]/div[1]/section[3]/div/div[5]");
                        if (links != null)
                        {
                            foreach (HtmlNode link in links)
                            {
                                string outputText = link.InnerText;
                                Console.WriteLine("Погода во владимире: " + outputText);
                            }
                        }

                        if (descrip != null)
                        {
                            foreach (HtmlNode desc in descrip)
                            {
                                string descText = desc.InnerText;
                                Console.WriteLine(descText);
                            }
                        }
                        break;

                    case "дата":
                        string date = DateTime.Now.ToString();
                        Console.WriteLine("Текущая дата: " + date);
                        break;

                    case "выход":
                        Console.WriteLine("До свидания " + name);
                        break;

                    case "помощь":
                        Console.WriteLine("Что бы узнать погоду напишите: *Погода*");
                        Console.WriteLine("Что бы узнать время и дату напишите: *Дата*");
                        Console.WriteLine("Что бы выйти напишите: *Выход*");
                        break;

                    default: 
                        Console.WriteLine("Что бы узнать команды бота напишите: *помощь*");
                        break;

                }
            }
            Console.ReadKey();

        }

        static string LoadPage(string url)// Метод чтение данных с сайта
        {
            var result = "";
            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var receiveStream = response.GetResponseStream();
                if (receiveStream != null)
                {
                    StreamReader readStream;
                    if (response.CharacterSet == null)
                        readStream = new StreamReader(receiveStream);
                    else
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    result = readStream.ReadToEnd();
                    readStream.Close();
                }
                response.Close();
            }
            return result;
        }
    }
}
