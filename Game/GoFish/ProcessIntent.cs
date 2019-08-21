using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.Core;
using Game.GoFish.Intents;
using Game.GoFish.Library;
using Game.GoFish.Models;
using LambdaSharp;
using LambdaSharp.Logger;
using Newtonsoft.Json;

namespace Game.GoFish {

    public class ProcessIntent {
        private readonly ILambdaLogLevelLogger _logger;
        private readonly IDependencyProvider _dependencyProvider;
        private string _uriToCardImage;

        //--- Methods ---
        public ProcessIntent(IDependencyProvider dependencyProvider, ILambdaLogLevelLogger logger, string uriToCardImage) {
            _dependencyProvider = dependencyProvider;
            _logger = logger;
            _uriToCardImage = uriToCardImage;
        }

        public async Task<LexLambdaResponse> Run(LexLambdaInputEvent lexInputEvent) {
            
            // lex session
            if (lexInputEvent.SessionAttributes == null || !lexInputEvent.SessionAttributes.TryGetValue("game_id", out var gameId)) {
                 gameId = Guid.NewGuid().ToString();
            }
            _logger.LogInfo($"gameId {gameId}");
            
            if (lexInputEvent.SessionAttributes == null || !lexInputEvent.SessionAttributes.TryGetValue("game_start_date", out var gameStartDate)) {
                gameStartDate = DateTime.Now.ToString("s");
            }
            
            // check if there's a game in session
            var lexGameSession = new Dictionary<string, AttributeValue> {
                ["game_id"] = new AttributeValue {
                    S = gameId
                }
            };
            var gameSessionResponse = await _dependencyProvider.DynamoDbGetItemAsync(lexGameSession);
            var gameInSession = gameSessionResponse.Item.Count > 0;
            
            // =====================
            // Check for game over
            // =====================
            if (gameInSession && gameSessionResponse.Item.TryGetValue("game_winner", out AttributeValue gameWinnerAttribute)) {

                // return message to user
                return Utilities.CustomResponseClose(gameWinnerAttribute.S);
            }
            GameSession gameSession = null;

            // set existing or set new game session
            if (gameInSession && gameSessionResponse.Item.TryGetValue("game_session", out var gameSessionString)) {
                gameSession = JsonConvert.DeserializeObject<GameSession>(gameSessionString.S);
                _logger.LogInfo($"gameSession {JsonConvert.SerializeObject(gameSession)}");
            }
            lexInputEvent.CurrentIntent.Slots.TryGetValue("GoFishCard", out var cardRequested);
            if (cardRequested != null) {
                _logger.LogInfo($"cardRequested {JsonConvert.SerializeObject(cardRequested)}");
            }

            // proxy request
            LexLambdaResponse response;
            switch (lexInputEvent.CurrentIntent.Name) {
                case "NewGame":
                    var newGame = new NewGame(_dependencyProvider, _logger, _uriToCardImage);
                    response = await newGame.Run(gameId, gameStartDate);
                    break;
                case "AskForCard":
                    if (gameInSession && gameSession != null) {
                        var askForCard = new AskForCard(_dependencyProvider, _logger);
                        response = await askForCard.Run(gameSession, cardRequested);
                    } else {
                        response = Utilities.CustomResponseClose(Dialogue.NoGameInSession);
                    }
                    break;
                case "GoFish":
                    if (gameInSession && gameSession != null) {
                        var goFish = new Intents.GoFish(_dependencyProvider, _logger);
                        response = await goFish.Run(gameSession);
                    } else {
                        response = Utilities.CustomResponseClose(Dialogue.NoGameInSession);
                    }
                    break;
                case "GiveUpCard":
                    if (gameInSession && gameSession != null) {
                        var giveUpCard = new GiveUpCard(_dependencyProvider, _logger);
                        response = await giveUpCard.Run(gameSession, cardRequested);
                    } else {
                        response = Utilities.CustomResponseClose(Dialogue.NoGameInSession);
                    }
                    break;
                case "Score":
                    if (gameInSession && gameSession != null) {
                        var score = new Score(_dependencyProvider, _logger);
                        response = await score.Run(gameSession);
                    } else {
                        response = Utilities.CustomResponseClose(Dialogue.NoGameInSession);
                    }
                    break;
                case "EndGame":
                    var endGame = new EndGame(_dependencyProvider, _logger);
                    response = await endGame.Run(gameSession);
                    break;
                default:
                    if (gameInSession) {
                        var lexSessionAttributes = LexSessionAttributes.GoFishLexSession(gameId, gameStartDate);
                        response = Utilities.CustomResponseElicitIntent(Dialogue.NoIntentFound, lexSessionAttributes);
                    } else {
                        response = Utilities.CustomResponseClose(Dialogue.NoGameInSession);
                    }
                    break;
            }
            _logger.LogInfo($"response {JsonConvert.SerializeObject(response)}");
            return response;
        }


    }
}
