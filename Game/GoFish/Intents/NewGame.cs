using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.GoFish.Library;
using Game.GoFish.Models;
using LambdaSharp.Logger;
using Newtonsoft.Json;

namespace Game.GoFish.Intents {

    public class NewGame {
        private readonly IDependencyProvider _dependencyProvider;
        private readonly ILambdaLogLevelLogger _logger;
        private readonly string _uriToS3Bucket;

        //--- Methods ---
        public NewGame(IDependencyProvider dependencyProvider, ILambdaLogLevelLogger logger, string uriToS3Bucket) {
            _dependencyProvider = dependencyProvider;
            _logger = logger;
            _uriToS3Bucket = uriToS3Bucket;
        }

        public async Task<LexLambdaResponse> Run(string gameId, string gameStartDate) {

            var lexSessionAttributes = LexSessionAttributes.GoFishLexSession(gameId, gameStartDate);
            _logger.LogInfo($"lexSessionAttributes {JsonConvert.SerializeObject(lexSessionAttributes)}");

            // "mix" the cards
            var shuffledDeckOfCards = GoFishCards.Init(_uriToS3Bucket).ShuffleDeck().ToList();
            _logger.LogInfo($"shuffledDeckOfCards {JsonConvert.SerializeObject(shuffledDeckOfCards)}");

            // get the total number of players * number of init cards / remove from list
            var totalPlayers = 2;
            var initCardPerPlayer = 7;
            var numberOfCardsToDistribute = totalPlayers * initCardPerPlayer;
            _logger.LogInfo($"numberOfCardsToDistribute {numberOfCardsToDistribute}");

            // get the stub cards to put on the field
            var stubCards = shuffledDeckOfCards.Skip(numberOfCardsToDistribute).Take(shuffledDeckOfCards.Count).ToList();
            _logger.LogInfo($"stubCards {JsonConvert.SerializeObject(stubCards)}");

            // get the cards to distribute to the players
            var cardsToDistribute = shuffledDeckOfCards.Take(numberOfCardsToDistribute).ToList();
            _logger.LogInfo($"cardsToDistribute {JsonConvert.SerializeObject(cardsToDistribute)}");

            // create the players
            var players = new List<Player>();
            for (var playerNumber = 1; playerNumber <= totalPlayers; playerNumber++) {
                List<Card> filteredCardsToDistribute;
                bool isABot;
                if (playerNumber == 1) {
                    filteredCardsToDistribute = cardsToDistribute.Where((x, i) => i % 2 != 0).ToList();
                    isABot = true;
                } else {
                    filteredCardsToDistribute = cardsToDistribute.Where((x, i) => i % playerNumber == 0).ToList();
                    isABot = false;
                }
                var player = new Player {
                    Cards = filteredCardsToDistribute, 
                    Id = playerNumber.ToString(), 
                    MatchedCards = new List<Card>(),
                    IsABot = isABot
                };
                players.Add(player);
            }

            // who's turn is next -- make it the users turn
            var playerNextTurn = players[1];
            
            // create a new game session
            var gameSession = Utilities.CreateGameSession(lexSessionAttributes.GameId, Convert.ToDateTime(lexSessionAttributes.GameStartDate), playerNextTurn, players, stubCards, totalPlayers, _uriToS3Bucket);

            // add game session to the database
            var databaseItem = new DatabaseItem {
                GameId = gameSession.GameId, 
                GameStartDate = gameSession.GameStartDate.ToString("s"), 
                GameSession = JsonConvert.SerializeObject(gameSession)
            };
            _logger.LogInfo($"databaseItem {JsonConvert.SerializeObject(databaseItem)}");
            _logger.LogInfo($"databaseItemd {JsonConvert.SerializeObject(databaseItem.ToDictionary())}");
            await _dependencyProvider.DynamoDbPutItemAsync(databaseItem.ToDictionary());

            // return message to user
            var response = Utilities.CustomResponseElicitIntent(Dialogue.ReadyToPlay, lexSessionAttributes);
            return response;
        }
    }
}
