using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : Singleton<CameraShake>
{
    private CinemachineImpulseSource _impulseSource;

    protected override void Awake()
    {
        base.Awake();

        if (!TryGetComponent(out _impulseSource))
        {
            _impulseSource = gameObject.AddComponent<CinemachineImpulseSource>();
        }

        _impulseSource.ImpulseDefinition.ImpulseType = CinemachineImpulseDefinition.ImpulseTypes.Uniform;
        _impulseSource.ImpulseDefinition.ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Explosion;
    }
}
