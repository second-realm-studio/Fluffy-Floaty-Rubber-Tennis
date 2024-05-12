using System;
using Constants;
using PlayerEntities;
using UnityEngine;
using XiheFramework.Core.Base;
using XiheFramework.Runtime;

namespace Procedures.MainFSM {
    public class StartGameLoop : MonoBehaviour {
        private void Start() {
            Game.Event.Subscribe(GameManager.onXiheFrameworkInitialized, OnXiheFrameworkInitialized);
        }

        private void OnXiheFrameworkInitialized(object sender, object e) {
            var fsm = Game.Fsm.CreateStateMachine("MainGameLoop");
            fsm.AddState(GameLoopStatesNames.Entry, new EntryState(fsm, null));
            fsm.AddState(GameLoopStatesNames.Menu, new MenuState(fsm, null));
            fsm.AddState(GameLoopStatesNames.Selection, new SelectionState(fsm, null));
            fsm.AddState(GameLoopStatesNames.Game, new TennisGameState(fsm, null));
            fsm.AddState(GameLoopStatesNames.GameOver, new GameOverState(fsm, null));
            fsm.SetDefaultState(GameLoopStatesNames.Entry);
            fsm.Start();
        }
    }
}