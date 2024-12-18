﻿using Telegram.Bot;
using WTelegram;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TL;
using ReplyKeyboardMarkup = Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup;
using KeyboardButton = Telegram.Bot.Types.ReplyMarkups.KeyboardButton;
using Message = Telegram.Bot.Types.Message;
using Update = Telegram.Bot.Types.Update;
using JConsole;
using Spectre.Console;

namespace WagmiBot
{
    public class WagmiClientManager : ManagerBase<WagmiClientManager>
    {
        #region Properties

        private WagmiClient WagmiClient;
        private Task RunTask { get; set; }

        #endregion

        #region Life Cycle

        protected override bool Initialize()
        {
            string apiKeyString = XMLSettings.GetSetting(Settings.TelegramAPIKey);
            string apiHash = XMLSettings.GetSetting(Settings.TelegramAPIHash);
            string botToken = XMLSettings.GetSetting(Settings.WagmiBotToken);

            if (string.IsNullOrEmpty(apiKeyString))
                throw new ArgumentNullException(apiKeyString);
            if (string.IsNullOrEmpty(apiHash))
                throw new ArgumentNullException(apiHash);
            if (string.IsNullOrEmpty(botToken))
                throw new ArgumentNullException(botToken);

            int apiKey = int.Parse(apiKeyString);

            if (WagmiClient == null)
                WagmiClient = new WagmiClient(botToken, apiHash, apiKey);

            return true;
        }

        [Documentation(Documentation)]
        protected override List<MenuOption> GetMenuOptions()
        {
            List<MenuOption> menuOptions = new List<MenuOption>();

            if (RunTask == null)
                menuOptions.Add(new MenuOption(nameof(StartBot).SplitByCase(), StartBot));

            return menuOptions;
        }

        [Documentation(StartBotDocumentation)]
        private void StartBot()
        {
            if (RunTask == null)
                RunTask = Task.Run(() => WagmiClient.StartBotAsync());

            WriteHeaderToConsole();
        }

        protected override void WriteHeaderToConsole()
        {
            base.WriteHeaderToConsole();

            if (RunTask != null)
                AnsiConsole.MarkupLine("[green]{0} is running[/]\n", nameof(WagmiClient));
            else
                AnsiConsole.MarkupLine("[orange1]{0} is not running[/]\n", nameof(WagmiClient));
        }

        #endregion

        #region Documentation 

        private const string Documentation = "An interface for configuring and monitoring the WagmiBot Telegram bot";
        private const string StartBotDocumentation = "Starts the instance of the bot and starts responding to user input from Telegram";

        #endregion
    }
}
