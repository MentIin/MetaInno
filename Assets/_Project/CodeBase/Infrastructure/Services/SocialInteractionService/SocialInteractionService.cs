using System;
using System.Collections.Generic;

namespace CodeBase.Infrastructure.Services.SocialInteractionService
{
    /// <summary>
    /// Handles player social interactions, friendship levels, and content unlocking
    /// </summary>
    public class SocialInteractionService
    {
        private Dictionary<string, int> _friendshipLevels; // PlayerID -> friendship level (1-10)
        private int _dailyGiftsGiven;
        private readonly int _playerCharisma; // Player's social stat
    
        public SocialInteractionService(int initialCharisma)
        {
            _friendshipLevels = new Dictionary<string, int>();
            _dailyGiftsGiven = 0;
            _playerCharisma = initialCharisma;
        }

        /// <summary>
        /// Processes interaction between players and updates friendship levels
        /// </summary>
        /// <returns>Result containing new friendship level and unlocked content</returns>
        public InteractionResult Interact(string playerId, InteractionType type)
        {
            // Initialize new friendship if first interaction
            if (!_friendshipLevels.ContainsKey(playerId))
            {
                _friendshipLevels[playerId] = 1;
            }

            int pointsGained = CalculateFriendshipPoints(type);
            _friendshipLevels[playerId] = Math.Clamp(_friendshipLevels[playerId] + pointsGained, 1, 10);

            return new InteractionResult
            {
                NewFriendshipLevel = _friendshipLevels[playerId],
                UnlockedMiniGame = CheckForUnlock(playerId)
            };
        }

        /// <summary>
        /// Calculates friendship points based on interaction type and player stats
        /// </summary>
        private int CalculateFriendshipPoints(InteractionType type)
        {
            return type switch
            {
                InteractionType.Chat => 1 + (_playerCharisma / 20), // Charisma boosts chat
                InteractionType.Gift => CanGiveGift() ? 3 : 0,        // Limited daily gifts
                InteractionType.JointActivity => 2,                   // Fixed value for activities
                _ => 0
            };
        }

        /// <summary>
        /// Checks if player can send gift (daily limit enforcement)
        /// </summary>
        private bool CanGiveGift()
        {
            if (_dailyGiftsGiven >= 3) return false;
            _dailyGiftsGiven++;
            return true;
        }

        /// <summary>
        /// Checks if friendship level unlocks special content
        /// </summary>
        private string CheckForUnlock(string playerId)
        {
            return _friendshipLevels[playerId] switch
            {
                3 => "CafeQuiz",          // Level 3 unlock
                5 => "ParkDance",          // Level 5 unlock
                8 => "RoofTopConcert",     // Level 8 unlock
                _ => null                  // No unlock
            };
        }

        /// <summary>
        /// Resets daily limitations (call at server daily reset)
        /// </summary>
        public void ResetDailyLimits()
        {
            _dailyGiftsGiven = 0;
        }
    }

    public enum InteractionType { Chat, Gift, JointActivity }

    public struct InteractionResult
    {
        public int NewFriendshipLevel;
        public string UnlockedMiniGame; // Null if nothing unlocked
    }
}