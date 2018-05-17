﻿using MoreMultiplayerInfo.Helpers;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MoreMultiplayerInfo.EventHandlers
{
    public class PlayerStateWatcher
    {
        public class PlayerLastActivity
        {
            private static Dictionary<string, string> ActivityDisplayNames => new Dictionary<string, string>
            {
                { "hoe", "Dug with a hoe" },
                { "pickaxe", "Swung a pickaxe" },
                { "axe", "Swung an axe" },
                { "watering can", "Sprinkled some water" },
                { "warped", "Switched areas" },
                { "scythe", "Slashed a scythe" },
                { "dirk", "Slashed a dagger" },
                { "sword", "Slashed a sword" },
                { "falchion", "Slashed a sword" },
                { "edge", "Slashed a sword" },
                { "blade", "Slashed a sword" },
                { "mallet", "Slammed a mallet" },
                { "swapped items", "Inventory management" },
                { "pole", "Went fishing" },
                { "rod", "Went fishing" },
                { "slingshot", "Fired a slingshot" },
                { "event", "Watching a cutscene" },
            };


            public string Activity { get; set; }

            public int When { get; set; }

            public Vector2 PositionBeforeEvent { get; set; }

            private int WhenInMinutes => GameTimeHelper.GameTimeToMinutes(When);            

            public string LocationName { get; set; }

            private int OneHourSpan => 60;

            private int HalfHour => OneHourSpan / 2;

            private int TwoHours => OneHourSpan * 2;

            private int MinutesSinceWhen => GameTimeHelper.GameTimeToMinutes(Game1.timeOfDay) - WhenInMinutes;

            public string GetDisplayText()
            {
                return $"Last Activity: {GetActivityDisplay()}";
            }


            private string GetActivityDisplay()
            {
                if (MinutesSinceWhen >= TwoHours)
                {
                    return "Nothing noteworthy";
                }

                if (ActivityDisplayNames.Keys.Any(k => Activity.Contains(k)))
                {
                    return ActivityDisplayNames.First(k => Activity.Contains(k.Key)).Value;
                }

                return Activity;
            }

            public string GetWhenDisplay()
            {
                if (MinutesSinceWhen <= HalfHour)
                {
                    return "just now";
                }

                if (MinutesSinceWhen < OneHourSpan)
                {
                    return $"{MinutesSinceWhen} minutes ago";
                }

                if (MinutesSinceWhen < TwoHours)
                {
                    return "one hour ago";
                }

                return "since " + Game1.getTimeOfDayString(When);
            }

        }

        public static Dictionary<long, PlayerLastActivity> LastActions { get; set; }

        public PlayerStateWatcher()
        {
            LastActions = new Dictionary<long, PlayerLastActivity>();
            GameEvents.EighthUpdateTick += WatchPlayerActions;
        }

        private void WatchPlayerActions(object sender, EventArgs e)
        {
            if (!Context.IsWorldReady) return;

            var players = PlayerHelpers.GetAllCreatedFarmers();

            foreach (var player in players)
            {
                var playerId = player.uniqueMultiplayerID;

                LastActions.GetOrCreateDefault(playerId);
                
                var currentLocation = player.currentLocation.name;

                if (currentLocation != LastActions[playerId].LocationName)
                {
                    LastActions[playerId] = new PlayerLastActivity
                    {
                        Activity = "warped",
                        LocationName = currentLocation,
                        When = Game1.timeOfDay,
                        PositionBeforeEvent = player.positionBeforeEvent
                    };
                    continue;
                }

                if (player.UsingTool)
                {
                    LastActions[playerId] = new PlayerLastActivity
                    {
                        Activity = player.CurrentTool?.Name.ToLower() ?? "N/A",
                        When = Game1.timeOfDay,
                        LocationName = currentLocation,
                        PositionBeforeEvent = player.positionBeforeEvent
                    };
                    continue;
                }

                if (player.positionBeforeEvent != new Vector2() && player.positionBeforeEvent != LastActions[playerId].PositionBeforeEvent)
                {
                    
                    LastActions[playerId] = new PlayerLastActivity
                    {
                        Activity = "event",
                        When = Game1.timeOfDay,
                        LocationName = currentLocation,
                        PositionBeforeEvent = player.positionBeforeEvent
                    };
                    continue;
                }
            }
        }

        public static PlayerLastActivity GetLastActionForPlayer(long playerId)
        {
            return LastActions.GetOrCreateDefault(playerId);
        }
    }
}
