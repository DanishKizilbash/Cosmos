﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Cosmos {
    public static class GameManager {
        public enum GameState {
            MainMenu,
            GameRunning,
            GamePaused,
            GameLoading
        }
        public static GameState curGameState = GameState.GameRunning;
        public static Vector3 screenPos = new Vector3(0, 0, 0);
        public static Game currentGame;
        public static List<string> logs;
        public static void NewGame(float Seed, Vector3 MapSize) {
            Debug.Log("Loading New Game");
            curGameState = GameState.GameLoading;
            Loader.Init();
            InitManagers();            
            currentGame = new Game();
            currentGame.Start();
            curGameState = GameState.GameRunning;
        }
        private static void InitManagers() {
            Finder.Init();
            DrawManager.Init();
            //
            EntityManager.Init();
            TickManager.Init();            
        }
        public static void Update() {
            TickManager.Update();
            currentGame.Update();
            PhysicsManager.Update();
            DrawManager.Draw();


        }
        public static void LateUpdate() {
            currentGame.LateUpdate();
        }
    }

}