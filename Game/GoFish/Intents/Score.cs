using System.Linq;
using System.Threading.Tasks;
using Game.GoFish.Library;
using Game.GoFish.Models;
using LambdaSharp.Logger;

namespace Game.GoFish.Intents {

    public class Score {
        private readonly ILambdaLogLevelLogger _logger;
        private IDependencyProvider _dependencyProvider;

        //--- Methods ---
        public Score(IDependencyProvider dependencyProvider, ILambdaLogLevelLogger logger) {
            _dependencyProvider = dependencyProvider;
            _logger = logger;
        }

        public async Task<LexLambdaResponse> Run(GameSession gameSession) {
            
            var lexSessionAttributes = LexSessionAttributes.GoFishLexSession(gameSession.GameId, gameSession.GameStartDate.ToString("s"));
            var botScore = gameSession.Players.FirstOrDefault(x => x.IsABot).MatchedCards.Select(x => x.Name).Distinct().ToList().Count;
            var opponentScore = gameSession.Players.FirstOrDefault(x => !x.IsABot).MatchedCards.Select(x => x.Name).Distinct().ToList().Count;
            var message = Dialogue.CurrentScore(botScore, opponentScore);

            // return message to user
            var response = Utilities.CustomResponseElicitIntent(message, lexSessionAttributes);
            return response;
        }
    }
}
