using Game.GoFish.Models;

namespace Game.GoFish.Library {
    public static class Dialogue {
        public static string NoGameInSession = "No game in session. To start a new game say, new game.";
        public static string NoIntentFound = "I'm sorry I do not understand.";
        public static string ReadyToPlay = "Let's do this! I'm ready to play!";
        public static string ThankYouEndGame = "Ok, ending game. Thanks for playing Go Fish with me.";
        public static string BotDoesNotHaveCard = "Go fish. ";
        public static string LiedAboutHavingTheCardResponse = "I've been told you do not have that card in your hand.";
        public static string PickedCardFromStub = "I'm taking a new card.";
        public static string NotBotTurn = "It's your turn. Ask me for a card.";
        public static string NoMoreStubCards = "Ok. Your turn.";
        public static string NotCardAskedFor = "That's not the card I asked for.";

        public static string StillBotTurn(string lastIntent) {
            return $"It's still my turn. {lastIntent}";
        }

        public static string DoYouHaveACard(string cardRequest) {
            return $"Do you have a {cardRequest}?";
        }

        public static string BotHasCard(string requestedCard) {
            return $"I have a {requestedCard}. ";
        }

        public static string GiveBotCardResponse(string requestedCard) {
            return $"Thank you for the {requestedCard}. ";
        }

        public static string CurrentScore(int botScore, int opponentScore) {
            return $"Current Score. I have {botScore} points, you have {opponentScore} points.";
        }

        public static string GameIsOver(int botScore, int opponentScore) {
            var message = $"You win {opponentScore} to {botScore}.";
            if (botScore > opponentScore) {
                message = $"I win {botScore} to {opponentScore}.";
            } else if (botScore == opponentScore) {
                message = $"Tie game {botScore} to {opponentScore}.";
            }
            return $"Game over. {message}";
        }
    }
}