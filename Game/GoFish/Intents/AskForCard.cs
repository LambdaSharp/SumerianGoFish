using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.GoFish.Library;
using Game.GoFish.Models;
using LambdaSharp.Logger;
using Newtonsoft.Json;

namespace Game.GoFish.Intents {
    
    public class AskForCard {
        private readonly IDependencyProvider _dependencyProvider;
        private readonly ILambdaLogLevelLogger _logger;

        //--- Methods ---
        public AskForCard(IDependencyProvider dependencyProvider, ILambdaLogLevelLogger logger) {
            _dependencyProvider = dependencyProvider;
            _logger = logger;
        }

        public async Task<LexLambdaResponse> Run(GameSession gameSession, string requestedCard) {

            // set variables
            var gameId = gameSession.GameId;
            var gameDateTime = gameSession.GameStartDate;
            var totalPlayers = gameSession.TotalPlayers;
            var stubCards = gameSession.StubCards;
            var userPlayer = gameSession.CurrentPlayer();
            var botPlayer = gameSession.OpponentPlayer();
            var uriToS3Bucket = gameSession.UriToS3Bucket;
            var lexSessionAttributes = LexSessionAttributes.GoFishLexSession(gameId, gameDateTime.ToString("s"));
            string message;
            var botCardRequest = "";
            Card cardReceived = null;

            // ========================================================
            // AskForCard Intent is only run when it's the User's Turn
            // ========================================================
            if (userPlayer.IsABot) {

                // return message to user
                var myTurnResponse = Utilities.CustomResponseElicitIntent(Dialogue.StillBotTurn(userPlayer.LastIntent), lexSessionAttributes);
                return myTurnResponse;
            }
            
            // ==============
            // Play the turn
            // ==============
            
            // find card in other players hand
            var cardInOtherPlayerHandFound = botPlayer.Cards.FirstOrDefault(x => String.Equals(x.Name, requestedCard, StringComparison.CurrentCultureIgnoreCase));
            _logger.LogInfo($"requestedCard {requestedCard}");
            _logger.LogInfo($"cardInOtherPlayerHandFound {JsonConvert.SerializeObject(cardInOtherPlayerHandFound)}");
            
            if (cardInOtherPlayerHandFound != null) {

                // remove from other players hand
                botPlayer.Cards.Remove(cardInOtherPlayerHandFound);
                cardReceived = cardInOtherPlayerHandFound;
                
                // opponent has the card
                message = Dialogue.BotHasCard(requestedCard);
            } else {
                if (stubCards.Count > 0) {
                    
                    // remove top card from stub
                    _logger.LogInfo($"stubCards {JsonConvert.SerializeObject(stubCards)}");
                    cardReceived = stubCards.FirstOrDefault();
                    _logger.LogInfo($"cardReceived {JsonConvert.SerializeObject(cardReceived)}");
                    stubCards.Remove(cardReceived);
                }
                
                // opponent doesn't have the card
                message = Dialogue.BotDoesNotHaveCard;
                _logger.LogInfo($"message {message}");
            }
            
            if (cardReceived != null) {
                
                // add card to current players hand
                userPlayer.Cards.Add(cardReceived);
            }
            
            // ===================================================
            // find if the current player has any matching cards
            // ===================================================
            
            // find any matching hands in current user hand and put into matched hands pile
            var matchingCards = Utilities.FindMatchingCards(userPlayer.Cards);
            _logger.LogInfo($"matchingCards {JsonConvert.SerializeObject(matchingCards)}");
            userPlayer.Cards = userPlayer.Cards.Except(matchingCards).ToList();
            userPlayer.MatchedCards.AddRange(matchingCards);

            // =====================
            // check for game over
            // =====================
            var gameOverMessage = Utilities.IsGameOver(botPlayer, userPlayer);
            if (gameOverMessage) {
                message = Dialogue.GameIsOver(botPlayer.MatchedCards.Count, userPlayer.MatchedCards.Count);
                await Utilities.ItemPutDatabase(_dependencyProvider, gameId, gameDateTime.ToString("s"), null, message);
            } else {
                
                // =============================================
                // determine which card the bot should ask for
                // =============================================
                var botCardToAsk = WhichCardToAskFor();
                _logger.LogInfo($"botCardToAsk {botCardToAsk}");
                botCardRequest = Dialogue.DoYouHaveACard(botCardToAsk);
                botPlayer.LastIntent = botCardRequest;
                
                // ==================
                // make new session
                // ==================
            
                // new player list
                var players = new List<Player> {
                    userPlayer,
                    botPlayer
                };
                var newGameSession = Utilities.CreateGameSession(gameId, gameDateTime, botPlayer, players, stubCards, totalPlayers, uriToS3Bucket);
                _logger.LogInfo($"newGameSession {JsonConvert.SerializeObject(newGameSession)}");

                // ==================
                // update database
                // ==================
                await Utilities.ItemPutDatabase(_dependencyProvider, gameId, gameDateTime.ToString("s"), newGameSession);
            }

            // ======================
            // send response to user
            // ======================
            var response = Utilities.CustomResponseElicitIntent(message + botCardRequest, lexSessionAttributes);
            return response;
        }

        private static string WhichCardToAskFor() {
            return "martini";
        }
    }
}
