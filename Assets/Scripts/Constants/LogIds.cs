namespace Constants
{
    public static class LogIds
    {
        public static class GameSceneManager
        {
            public const string LoadStart = "GameSceneManager.Load.Start";
            public const string LoadAlreadyLoaded = "GameSceneManager.Load.AlreadyLoaded";
            public const string LoadOpNull = "GameSceneManager.Load.OpNull";
            public const string LoadNotLoaded = "GameSceneManager.Load.NotLoaded";
            public const string LoadComplete = "GameSceneManager.Load.Complete";

            public const string UnloadStart = "GameSceneManager.Unload.Start";
            public const string UnloadNoop = "GameSceneManager.Unload.Noop";
            public const string UnloadOpNull = "GameSceneManager.Unload.OpNull";
            public const string UnloadComplete = "GameSceneManager.Unload.Complete";
        }

        public static class StateMachines
        {
            public const string InvalidStateTransition = "BaseStateMachine.ChangeState.InvalidTransition";
            public const string TransitionSameState = "BaseStateMachine.GetState.SameState";
            public const string NotRegisteredState = "BaseStateMachine.GetState.NotRegistered";
        }

        public static class LevelsFactory
        {
            public const string CreateBuildings = "LevelsFactory.Create.CreateBuildings";
        }

        public static class Player
        {
            public const string CollectResource = "Player.CollectResource";
        }

        public static class Save
        {
            public const string AutoSaveSafeSave = "AutoSave.SafeSave";
        }

        public static class SaveManager
        {
            public const string LoadStart = "SaveManager.Load.Start";
            public const string LoadRead = "SaveManager.Load.Read";
            public const string LoadEmpty = "SaveManager.Load.EmptySave";
            public const string LoadDeserialized = "SaveManager.Load.Deserialized";
            public const string LoadComplete = "SaveManager.Load.Complete";

            public const string SaveStart = "SaveManager.Save.Start";
            public const string SerializeStart = "SaveManager.Save.Serialize";
            public const string WriteStart = "SaveManager.Save.Write";
            public const string SaveComplete = "SaveManager.Save.Saved";
        }
    }
}