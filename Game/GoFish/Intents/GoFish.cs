using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.GoFish.Library;
using Game.GoFish.Models;
using LambdaSharp.Logger;
using Newtonsoft.Json;

namespace Game.GoFish.Intents {

    public class GoFish {
        private readonly ILambdaLogLevelLogger _logger;
        private IDependencyProvider _dependencyProvider;

        //--- Methods ---
        public GoFish(IDependencyProvider dependencyProvider, ILambdaLogLevelLogger logger) {
            _dependencyProvider = dependencyProvider;
            _logger = logger;
        }

        public async Task<LexLambdaResponse> Run(GameSession gameSession) {

            // set variables
            var gameId = gameSession.GameId;
            var gameDateTime = gameSession.GameStartDate;
            var totalPlayers = gameSession.TotalPlayers;
            var stubCards = gameSession.StubCards;
            var botPlayer = gameSession.CurrentPlayer();
            var userPlayer = gameSession.OpponentPlayer();
            var uriToS3Bucket = gameSession.UriToS3Bucket;
            var message = Dialogue.NoMoreStubCards;
            var lexSessionAttributes = LexSessionAttributes.GoFishLexSession(gameId, gameDateTime.ToString("s"));
            
            // ========================================================
            // GoFish Intent is only run when it's the Bots Turn
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

            if (stubCards.Count > 0) {
                
                // remove top card from stub
                var cardReceived = stubCards.FirstOrDefault();
                stubCards.Remove(cardReceived);

                // add card to current players hand
                botPlayer.Cards.Add(cardReceived);
                
                message = Dialogue.PickedCardFromStub;
            }

            // ===================================================
            // find if the current player has any matching cards
            // ===================================================
            
            // find any matching hands in current user hand and put into matched hands pile
            var matchingCards = Utilities.FindMatchingCards(botPlayer.Cards);
            botPlayer.Cards = botPlayer.Cards.Except(matchingCards).ToList();
            botPlayer.MatchedCards.AddRange(matchingCards);
            
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
                var players = new List<Player> {
                    botPlayer,
                    userPlayer
                };
                var newGameSession = Utilities.CreateGameSession(gameId, gameDateTime, userPlayer, players, stubCards, totalPlayers, uriToS3Bucket);
                _logger.LogInfo($"newGameSession {JsonConvert.SerializeObject(newGameSession)}");

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
