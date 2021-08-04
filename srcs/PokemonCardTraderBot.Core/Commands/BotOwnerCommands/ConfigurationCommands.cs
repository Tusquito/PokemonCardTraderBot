using System.Threading.Tasks;
using Disqord.Bot;
using PokemonCardTraderBot.Common.Configurations;
using PokemonCardTraderBot.Core.Managers;
using PokemonTcgSdk.Models;
using Qmmands;

namespace PokemonCardTraderBot.Core.Commands.BotOwnerCommands
{
    [RequireBotOwner]
    public class ConfigurationCommands : DiscordGuildModuleBase
    {
        private readonly SetManager _setManager;
        private readonly CardSetsConfiguration _cardSetsConfiguration;
        
        public ConfigurationCommands(SetManager setManager, CardSetsConfiguration cardSetsConfiguration)
        {
            _setManager = setManager;
            _cardSetsConfiguration = cardSetsConfiguration;
        }
        [Command("bl-set")]
        public async Task<DiscordCommandResult> OnBlacklistSetCommand(string setCode)
        {
            SetData setData = await _setManager.GetBySetCodeAsync(setCode);
            
            if (setData == null)
            {
                return Reply("Invalid set code");
            }

            await _setManager.AddOrUpdateConfigurationEntry(new CardSetInfo
            {
                Code = setCode,
                IsBlacklisted = true
            });

            return Reply($"[{setCode}] has been blacklisted");
        }
    }
}