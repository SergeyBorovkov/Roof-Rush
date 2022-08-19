using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class FinishCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _finishCamera;
    
    private CinemachineOrbitalTransposer _orbitalTransposer;
    private float _targetBias = -136.5f;
    private int _priority = 4;
    private float _rotateDuration = 1.5f;

    private void Start()
    {
        _orbitalTransposer = _finishCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();    
    }

    public void Play()
    {
        _finishCamera.Priority = _priority;        

        DOTween.To(x => _orbitalTransposer.m_Heading.m_Bias = x, 0, _targetBias, _rotateDuration);     
    }
}