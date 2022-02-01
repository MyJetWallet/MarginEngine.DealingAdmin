using System;
using System.Threading.Tasks;
using AntDesign;
using DealingAdmin.Abstractions;
using Serilog.Core;

namespace DealingAdmin.Shared.Services
{
    public class UserMessageService : IUserMessageService
    {
        private readonly MessageService _messageService;
        private readonly Logger _logger;

        public UserMessageService(
            MessageService messageService,
            Logger logger)
        {
            Console.WriteLine("MessageService init");

            messageService.Config(new MessageGlobalConfig
            {
                Top = 65,
                MaxCount = 3
            });

            _messageService = messageService;
            _logger = logger;
        }

        public async Task SuccessAsync(string text)
        {
            try
            {
                await _messageService.Success(text);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to show user message");
            }
        }

        public async Task WarningAsync(string text)
        {
            try
            {
                await _messageService.Warning(text);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to show user message");
            }
        }

        public async Task ErrorAsync(string text, Exception ex = null)
        {
            try
            {
                await _messageService.Error($"{text}  {ex?.Message}");
                
                if (ex == null)
                {
                    _logger.Warning($"UserMessage with error: {text}");
                }
                else
                {
                    _logger.Error($"UserMessage with error: {text} {ex.Message} {ex.StackTrace}");
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "Failed to show user message");
            }
        }
    }
}