using System;
using GameObjects.Players.StateMachines;
using GameObjects.Players.StateMachines.States;
using GameObjects.ResourcesSpace;
using GameObjects.ResourcesSpace.Views;
using Infrastructure.Loggers;
using Infrastructure.SaveServices.Interfaces;
using UniRx;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace GameObjects.Players
{
    public class PlayerFactory : IFactory<Transform, int, Vector3, ResourcesDatabase, PlayerObject>
    {
        private readonly PlayerObject _prefab;
        private readonly ISaveManager _saveManager;
        private readonly IGameLogger _logger;
        private readonly PlayerResourcesView _playerResourcesView;

        public PlayerFactory(
            PlayerObject prefab,
            ISaveManager saveManager,
            IGameLogger logger, PlayerResourcesView playerResourcesView)
        {
            _prefab = prefab;
            _saveManager = saveManager;
            _logger = logger;
            _playerResourcesView = playerResourcesView;
        }

        public PlayerObject Create(
            Transform root,
            int levelId,
            Vector3 position,
            ResourcesDatabase resourcesDatabase)
        {
            PlayerObject playerObject = Object.Instantiate(_prefab, root);
            playerObject.transform.position = position;
            PlayerData data = new(levelId);
            _saveManager.RegisterSaveable(data);

            InitResourcesTransferView(
                playerObject.ResourcesTransferView,
                resourcesDatabase, data);

            Player player = CreatePlayerModel(data, playerObject);

            CreateResourcesView(root, resourcesDatabase, data);
            
            return CreateStateMachine(playerObject, player);
        }
        private Player CreatePlayerModel(PlayerData data, PlayerObject playerObject)
        {

            Player player = new(data, _logger);

            playerObject.SetModel(player);
            return player;
        }

        private PlayerObject CreateStateMachine(PlayerObject playerObject, Player player)
        {
            IPlayerState[] states = {
                new CollectState(playerObject.Animator),
                new IdleState(playerObject.Animator),
                new MoveState(playerObject.Animator),
            };
            
            PlayerStateMachine playerStateMachine = new(_logger, states);
            PlayerStateSwitcher playerStateSwitcher = new(playerStateMachine, playerObject.Mover, player);
            playerStateSwitcher.Initialization();
            playerObject.AddToObjectStream(playerStateSwitcher);
            playerObject.AddToObjectStream(playerStateMachine);
            return playerObject;
        }

        private void CreateResourcesView(Transform root, ResourcesDatabase resourcesDatabase, PlayerData data)
        {
            PlayerResourcesView view = Object.Instantiate(_playerResourcesView, root);
            view.SetResourcesDatabase(resourcesDatabase);

            foreach(IResourcesData resourcesData in data.Resources)
                view.Add(resourcesData);

            IDisposable stream = data.NewResourceAdded.Subscribe(view.Add);
            view.AddStream(stream);
        }

        private void InitResourcesTransferView(ResourcesTransferView view,
            ResourcesDatabase resourcesDatabase,
            PlayerData data)
        {
            view.SetResourcesDatabase(resourcesDatabase);

            IDisposable resourcesTransferStream = data.ResourceAdded.Subscribe(view.Add);
            view.AddStream(resourcesTransferStream);
        }
    }
}