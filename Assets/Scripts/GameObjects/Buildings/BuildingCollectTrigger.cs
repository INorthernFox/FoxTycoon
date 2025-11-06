using GameObjects.Players;
using UnityEngine;

namespace GameObjects.Buildings
{
    public class BuildingCollectTrigger : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        private Building _building;

        public Transform Target => _target;
        
        public void SetModel(Building building)
        {
            _building =  building;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if(!ValidateInput(other, out PlayerObject player))
                return;
            
            _building.OnPlayerStartCollectResources(player.Model);
        }
        
        private void OnTriggerExit(Collider other)
        {
           if(!ValidateInput(other, out PlayerObject player))
               return;
           
           _building.OnPlayerStopCollectResources(player.Model);
        }
        
        private bool ValidateInput(Collider other, out PlayerObject player)
        {
            player = null;
            return _building != null && other.transform.TryGetComponent(out player);
        }
    }
}