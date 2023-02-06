using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using HtmlAgilityPack;

namespace TelegramBot
{

    class Program
    {
        static ITelegramBotClient bot = new TelegramBotClient("5731996499:AAHyGkStw57O3HxCqMG9kxlZtMXP5ihnX7c");
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
                if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
                {
                    var message = update.Message;
                    if (message.Text.ToLower() == "/start")
                    {
                        await botClient.SendTextMessageAsync(message.Chat, "Здравствуйте, укажите ваш знак зодиака, введите соответствующую цифру:\n 0 - Овен\n 1 - Телец\n 2 - Близнецы\n 3 - Рак\n 4 - Лев\n 5 - Дева\n 6 - Весы\n 7 - Скорпион\n 8 - Стрелец\n 9 - Козерог\n 10 - Водолей\n 11 - Рыбы");
                        await botClient.SendTextMessageAsync(message.Chat, "Гороскоп обновляется каждый день, приходи еще и завтра, для этого можешь написать 'Привет' =)");

                        return;
                    }

                    else if (message.Text.ToLower() == "привет")
                    {
                        await botClient.SendTextMessageAsync(message.Chat, "Приветик, укажи знак зодиака:\n 0 - Овен\n 1 - Телец\n 2 - Близнецы\n 3 - Рак\n 4 - Лев\n 5 - Дева\n 6 - Весы\n 7 - Скорпион\n 8 - Стрелец\n 9 - Козерог\n 10 - Водолей\n 11 - Рыбы");

                        return;

                    }

                    else if (0 <= Convert.ToInt32(message.Text) || Convert.ToInt32(message.Text) <= 11)
                    {
                        int i = Convert.ToInt32(message.Text);
                        HtmlWeb web = new HtmlWeb();
                        HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                        doc = web.Load("https://74.ru/horoscope/daily/");



                        string? znak = Convert.ToInt32(message.Text) switch
                        {
                            0 => "Гороскоп для Овна:",
                            1 => "Гороскоп для Тельца:",
                            2 => "Гороскоп для Близнецов:",
                            3 => "Гороскоп для Рака:",
                            4 => "Гороскоп для Льва:",
                            5 => "Гороскоп для Девы:",
                            6 => "Гороскоп для Весов:",
                            7 => "Гороскоп для Скорпиона:",
                            8 => "Гороскоп для Стрельца:",
                            9 => "Гороскоп для Козерога:",
                            10 => "Гороскоп для Водолея:",
                            11 => "Гороскоп для Рыб:",
                        };

                        await botClient.SendTextMessageAsync(message.Chat, znak);
                        string? text = doc.DocumentNode.SelectNodes("//div[@class='BDPZt KUbeq']")[i].InnerText;
                        await botClient.SendTextMessageAsync(message.Chat, text);
                        return;
                    }


                    else
                    {
                        await botClient.SendTextMessageAsync(message.Chat, "Ничего не понятно");
                        return;
                    }
                }
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex);              
            }
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, 
            };
            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
            Console.ReadLine();
        }
    }
}


