using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.GoFish.Library;
using Game.GoFish.Models;
using LambdaSharp.Logger;
using Newtonsoft.Json;

namespace Game.GoFish.Intents {

    public class GiveUpCard {
        private readonly ILambdaLogLevelLogger _logger;
        private readonly IDependencyProvider _dependencyProvider;

        //--- Methods ---
        public GiveUpCard(IDependencyProvider dependencyProvider, ILambdaLogLevelLogger logger) {
            _dependencyProvider = dependencyProvider;
            _logger = logger;
        }

        public async Task<LexLambdaResponse> Run(GameSession gameSession, string requestedCard) {
            
            // set variables
            var gameId = gameSession.GameId;
            var gameDateTime = gameSession.GameStartDate;
            var totalPlayers = gameSession.TotalPlayers;
            var stubCards = gameSession.StubCards;
            var botPlayer = gameSession.CurrentPlayer();
            var userPlayer = gameSession.OpponentPlayer();
            var uriToS3Bucket = gameSession.UriToS3Bucket;
            var lexSessionAttributes = LexSessionAttributes.GoFishLexSession(gameId, gameDateTime.ToString("s"));
            var message = "";
            Card cardReceived;

            // ========================================================
            // GiveUpCard Intent is only run when it's the Bots Turn
            // ========================================================
            if (!botPlayer.IsABot) {

                // return message to user
                var myTurnResponse = Utilities.CustomResponseElicitIntent(Dialogue.NotBotTurn, lexSessionAttributes);
                _logger.LogInfo($"response {JsonConvert.SerializeObject(myTurnResponse)}");
                return myTurnResponse;
            }
            
            // ==============
            // Play the turn
            // ==============
            
            // name of the card the bot asked for
            if (!botPlayer.LastIntent.Split(" ").Last().TrimEnd('?').Equals(requestedCard)) {
                
                // return message to user
                var notCardAskedForResponse = Utilities.CustomResponseElicitIntent(Dialogue.NotCardAskedFor + " " + botPlayer.LastIntent, lexSessionAttributes);
                _logger.LogInfo($"response {JsonConvert.SerializeObject(notCardAskedForResponse)}");
                return notCardAskedForResponse;
            }
            
            // find card in other players hand
            var cardInOtherPlayerHandFound = userPlayer.Cards.FirstOrDefault(x => String.Equals(x.Name, requestedCard, StringComparison.CurrentCultureIgnoreCase));
            if (cardInOtherPlayerHandFound != null) {
                
                // remove from other players hand
                userPlayer.Cards.Remove(cardInOtherPlayerHandFound);
                cardReceived = cardInOtherPlayerHandFound;

                // give bot the card
                message = Dialogue.GiveBotCardResponse(requestedCard);
            } else {

                // remove from stub
                cardReceived = stubCards.FirstOrDefault();
                stubCards.Remove(cardReceived);

                // tell the user they don't have that card and it got one from the stub
                message = Dialogue.LiedAboutHavingTheCardResponse;

                if (stubCards.Count > 0) {
                    message += " " + Dialogue.PickedCardFromStub;
                }
                
            }
            
            // add card received to current players hand
            botPlayer.Cards.Add(cardReceived);
            
            // ===================================================
            // find if the current player has any matching cards
            // ===================================================

            // find any matching hands in current user hand and put into matched hands pile
            var matchingCards = Utilities.FindMatchingCards(botPlayer.Cards);
            botPlayer.Cards = botPlayer.Cards.Except(matchingCards).ToList();
            botPlayer.MatchedCards.AddRange(matchingCards);
            userPlayer.LastIntent = "";
            
            // =====================
            // check for game over
            // =====================
            var gameOverMessage = Utilities.IsGameOver(botPlayer, userPlayer);
            if (gameOverMessage) {
                message = Dialogue.GameIsOver(botPlayer.MatchedCards.Count, userPlayer.MatchedCards.Count);
                await Utilities.ItemPutDatabase(_dependencyProvider, gameId, gameDateTime.ToString("s"), null, message);
            } else {

                // ==================
                // make new session
                // ==================

                // new player list
                var players = new List<Player> {botPlayer, userPlayer};
                var newGameSession = Utilities.CreateGameSession(gameId, gameDateTime, userPlayer, players, stubCards, totalPlayers, uriToS3Bucket);

                // ==================
                // update database
                // ==================
                await Utilities.ItemPutDatabase(_dependencyProvider, gameId, gameDateTime.ToString("s"), newGameSession);
            }
            
            // ======================
            // send response to user
            // ======================
            var response = Utilities.CustomResponseElicitIntent(message, lexSessionAttributes);
            return response;
        }
    }
}
