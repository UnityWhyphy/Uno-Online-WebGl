using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance;

    [SerializeField] Transform cardObject, startPos, DestinationPos;

}