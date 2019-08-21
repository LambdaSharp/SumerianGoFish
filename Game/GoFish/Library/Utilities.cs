using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Game.GoFish.Models;
using Newtonsoft.Json;

namespace Game.GoFish.Library {
    public static class Utilities {

        public static LexLambdaResponseTypeClose CustomResponseClose(string message, LexSessionAttributes sessionAttributes = null, string fulfillment = "Fulfilled") {
            return new LexLambdaResponseTypeClose {
                DialogAction = new LexLambdaResponseDialogActionTypeClose {
                    Message = new LexLambdaResponseMessage {
                        Content = message,
                        ContentType = "PlainText"
                    },
                    Type = "Close",
                    FulfillmentState = fulfillment
                },
                SessionAttributes = sessionAttributes == null ? new Dictionary<string, string>() : sessionAttributes.ToDictionary()
            };
        }
        
        public static LexLambdaResponseTypeElicitIntent CustomResponseElicitIntent(string message, LexSessionAttributes sessionAttributes = null) {
            return new LexLambdaResponseTypeElicitIntent {
                DialogAction = new LexLambdaResponseDialogActionTypeElicitIntent {
                    Message = new LexLambdaResponseMessage {
                        Content = message,
                        ContentType = "PlainText"
                    },
                    Type = "ElicitIntent"
                },
                SessionAttributes = sessionAttributes == null ? new Dictionary<string, string>() : sessionAttributes.ToDictionary()
            };
        }

        public static List<Card> FindMatchingCards(List<Card> cards) {
            var matchingCards = new List<Card>();
            foreach (var card in cards) {
                if (cards.Any(x => x.Name == card.Name && x.Id != card.Id)) {
                    matchingCards.Add(card);
                }
            }
            return matchingCards;
        }

        public static GameSession CreateGameSession(string gameId, DateTime gameDateTime, Player nextTurn, List<Player> players, List<Card> stubCards, int totalPlayers, string uriToS3Bucket) {

            // updated game session
            return new GameSession {
                GameId = gameId,
                GameStartDate = gameDateTime,
                NextTurn = nextTurn,
                Players = players,
                StubCards = stubCards,
                TotalPlayers = totalPlayers,
                UriToS3Bucket = uriToS3Bucket
            };

        }

        public static async Task ItemPutDatabase(IDependencyProvider dependencyProvider, string gameId, string gameDateTime, GameSession gameSession, string gameWinner = null) {
            
            // update database
            var databaseItem = new DatabaseItem {
                GameId = gameId,
                GameStartDate = gameDateTime,
                GameSession = JsonConvert.SerializeObject(gameSession)
            };
            if (gameWinner != null) {
                databaseItem.GameWinner = gameWinner;
            }
            await dependencyProvider.DynamoDbPutItemAsync(databaseItem.ToDictionary());
        }

        public static async Task ItemDeleteDatabase(IDependencyProvider dependencyProvider, string gameId) {

            // updated game session
            var deleteItemRequest = new DatabaseItem {
                GameId = gameId
            };
            await dependencyProvider.DynamoDbDeleteItemAsync(deleteItemRequest.ToDictionary());
        }

        public static bool IsGameOver(Player botPlayer, Player opponentPlayer) {
            return botPlayer.Cards.Count == 0 && opponentPlayer.Cards.Count == 0;
        }
    }
}