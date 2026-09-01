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
    
    // 쉐이크를 생성하는 메서드
    public void Shake(float force)
    {
        /* 난수 발생
         * Random.Range(0, 10) => 0, 1, 2, ..., 9 (정수)
         * Random.Range(0.0f, 10.0f) => 0.0f, ... , 10.0f (실수)
         */
        var velocity = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0f);
        _impulseSource.GenerateImpulse(velocity * force);
    }
}
