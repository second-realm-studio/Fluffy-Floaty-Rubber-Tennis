using Balls;
using Constants;
using PlayerEntities;
using UnityEngine;
using XiheFramework.Runtime;

namespace Test {
    public class SpawnPlayer : MonoBehaviour {
        private void Start() {
            var left = Game.Entity.InstantiateEntity<TennisPlayerEntity>("TennisPlayerEntity_Goat");
            Game.Blackboard.SetData(BlackboardDataNames.EntityIdP1, left.EntityId);
            left.transform.position = new Vector3(-50f, 0, 0);
            left.isRightSide = false;
            left.inputId = 0;
            left.racketRenderer.material.color = new Color(0.9686275f, 0.6196079f, 0.6392157f);
            left.gameObject.layer = LayerMask.NameToLayer("Player1");
            foreach (Transform child in left.transform) {
                child.gameObject.layer = LayerMask.NameToLayer("Player1");
            }

            var right = Game.Entity.InstantiateEntity<TennisPlayerEntity>("TennisPlayerEntity_Rabbit");
            Game.Blackboard.SetData(BlackboardDataNames.EntityIdP2, right.EntityId);
            right.transform.position = new Vector3(50f, 0, 0);
            right.inputId = 1;
            right.isRightSide = true;
            right.racketRenderer.material.color = new Color(1f, 0.854902f, 0.5764706f);
            right.gameObject.layer = LayerMask.NameToLayer("Player2");
            foreach (Transform child in right.transform) {
                child.gameObject.layer = LayerMask.NameToLayer("Player2");
            }

            //spawn ball
            var ball = Game.Entity.InstantiateEntity<GeneralBallEntity>("BallEntity_GeneralBall");
            Game.Blackboard.SetData(BlackboardDataNames.EntityIdBall, ball.EntityId);
            ball.transform.position = new Vector3(0, 0, 0);
            ball.rigidBody.velocity = Vector3.zero;

            //enable input catergory
            Game.Input(0).controllers.maps.SetMapsEnabled(true, InputNames.CategoryGame);
            Game.Input(1).controllers.maps.SetMapsEnabled(true, InputNames.CategoryGame);

            Game.UI.ActivateUI(UINames.GameHud);
        }
    }
}