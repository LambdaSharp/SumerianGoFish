using System.Threading.Tasks;
using Game.GoFish.Library;
using Game.GoFish.Models;
using LambdaSharp.Logger;

namespace Game.GoFish.Intents {

    public class EndGame {
        private readonly ILambdaLogLevelLogger _logger;
        private readonly IDependencyProvider _dependencyProvider;

        //--- Methods ---
        public EndGame(IDependencyProvider dependencyProvider, ILambdaLogLevelLogger logger) {
            _dependencyProvider = dependencyProvider;
            _logger = logger;
        }

        public async Task<LexLambdaResponse> Run(GameSession gameSession) {

            // ======================
            // delete from database
            // ======================
            await Utilities.ItemDeleteDatabase(_dependencyProvider, gameSession.GameId);
            
            // ======================
            // send response to user
            // ======================
            var response = Utilities.CustomResponseClose(Dialogue.ThankYouEndGame, new LexSessionAttributes());
            return response;
        }
    }
}
