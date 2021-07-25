using System;
using System.Collections.Generic;
using System.Linq;
using Disqord;
using Disqord.Extensions.Interactivity.Menus.Paged;
using PokemonTcgSdk.Models;

namespace PokemonCardTraderBot.Common.Extensions
{
    public static class PaginationExtensions
    {
        public static IEnumerable<Page> ToPages(this Dictionary<string, List<SetData>> sets)
        {
            List<Page> pages = new();

            foreach (var (seriesName, setsData) in sets)
            {
                pages.Add(new Page()
                    .WithEmbeds(new LocalEmbed().WithColor(Color.Aquamarine)
                        .WithTitle($"{seriesName} series sets")
                        .WithDescription(setsData.Select(x => $"**[{x.Code}]** {x.Name}").Aggregate((x, y) => $"{x}\n{y}"))
                        .WithTimestamp(DateTimeOffset.UtcNow)
                    )
                );
            }

            return pages;
        }

        public static IEnumerable<Page> ToPages(this List<PokemonCard> cards, SetData set)
        {
            List<Page> pages = new();
            if (cards == null || !cards.Any())
            {
                return Array.Empty<Page>();
            }
            pages.Add(new Page()
                .WithEmbeds(new LocalEmbed()
                    .WithTitle($"{set.Name}'s booster")
                    .WithThumbnailUrl(set.LogoUrl)
                    .WithTimestamp(DateTimeOffset.UtcNow)
                    .WithColor(Color.Aquamarine)
                    .WithDescription("**Content:** 5 Commons, 3 Uncommons, 1 Rare +, 1 Reverse-Holo")
                    .WithFooter(set.Name, set.SymbolUrl)
                )
            );
            pages.AddRange(cards.Select(card => new Page()
                .WithEmbeds(new LocalEmbed()
                    .WithColor(Color.Aquamarine)
                    .WithTitle($"[{card.Rarity}] {card.Name}")
                    .WithImageUrl(card.ImageUrlHiRes)
                    .WithTimestamp(DateTimeOffset.UtcNow)
                    .WithFooter(set.Name, set.SymbolUrl)
                )
            ));

            pages.Add(new Page()
                .WithEmbeds(new LocalEmbed()
                    .WithTitle($"[RECAP] {set.Name}'s booster")
                    .WithThumbnailUrl(set.LogoUrl)
                    .WithTimestamp(DateTimeOffset.UtcNow)
                    .WithColor(Color.Aquamarine)
                    .WithDescription(cards.Select(x => $"**[{x.Rarity}]** {x.Name}").Aggregate((x,y) => $"{x}\n{y}"))
                    .WithFooter(set.Name, set.SymbolUrl)
                )
            );

            return pages;
        }
    }
}