using UnityEngine;

namespace GameObjects.ResourcesSpace
{
    [CreateAssetMenu(menuName = "Game/Resources/Settings", fileName = "ResourcesSettings", order = 0)]
    public class ResourcesSettings : ScriptableObject
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _name;
        [SerializeField] private float _productionPerSecond;
        [SerializeField] private AnimationCurve _transferSpeed;
        [SerializeField] private GameResourcesType _type;

        public Sprite Icon => _icon;
        public string Name => _name;
        public float ProductionPerSecond => _productionPerSecond;
        public GameResourcesType Type => _type;
        public AnimationCurve TransferSpeed => _transferSpeed;
    }

}