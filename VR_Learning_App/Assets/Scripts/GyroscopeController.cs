using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroscopeController : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO _toggleSprings;

    [SerializeField] private HingeJoint m_InnerRing;
    [SerializeField] private HingeJoint m_MiddleRing;
    [SerializeField] private HingeJoint m_OuterRing;

    private bool areSpringsOn = false;

    private void OnEnable()
    {
        _toggleSprings.OnEventRaised += ToggleSprings;
        areSpringsOn = m_InnerRing.useSpring;
    }

    private void OnDisable()
    {
        _toggleSprings.OnEventRaised -= ToggleSprings;
    }

    private void ToggleSprings()
    {

        m_InnerRing.useSpring = !areSpringsOn;
        m_MiddleRing.useSpring = !areSpringsOn;
        m_OuterRing.useSpring = !areSpringsOn;
        areSpringsOn = !areSpringsOn;


    }
}
