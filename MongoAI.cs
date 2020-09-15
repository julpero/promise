using System;
using MongoDB.Bson;

namespace promise
{
    public class MongoAI
    {
        public ObjectId Id {get; set;}
        public string AiName {get; set;}
        public PlayerAI PlayerAI {get; set;}
        public int Points {get; set;}
        public int PromisesKept {get; set;}
        public int Evolution {get; set;}
        public DateTime Created {get; set;}
        public string GameGuid {get; set;}

        public void SetGameGuid(string gameGuid)
        {
            GameGuid = gameGuid;
        }

        public MongoAI(string guid, PlayerAI playerAI, int points, int promisesKept, int evolution = 0)
        {
            AiName = guid;
            PlayerAI = playerAI;
            Points = points;
            PromisesKept = promisesKept;
            Created = DateTime.Now;
            Evolution = evolution;
        }

        public MongoAI(string guid, PlayerAI playerAI, int evolution = 0)
        {
            AiName = guid;
            PlayerAI = playerAI;
            Points = 0;
            PromisesKept = 0;
            Created = DateTime.Now;
            Evolution = evolution + 1;
        }

        public MongoAI(string guid, PlayerAI playerAI, PlayerAI playerAI2, bool average, int evolution = 0)
        {
            AiName = guid;
            PlayerAI = new PlayerAI(guid, playerAI, playerAI2, average);
            Points = 0;
            PromisesKept = 0;
            Created = DateTime.Now;
            Evolution = evolution + 1;
        }

        public MongoAI(int playerAIid)
        {
            PlayerAI = new PlayerAI(playerAIid);
            AiName = PlayerAI.AiName;
            Points = 0;
            PromisesKept = 0;
            Created = DateTime.Now;
            Evolution = 99999;
        }

        public MongoAI(string guid, int? evolution = null)
        {
            PlayerAI = new PlayerAI(guid);
            AiName = PlayerAI.AiName;
            Points = 0;
            PromisesKept = 0;
            Created = DateTime.Now;
            Evolution = evolution ?? 9999;
        }
    }
}
