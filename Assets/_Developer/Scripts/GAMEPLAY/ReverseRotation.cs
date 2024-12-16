using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseRotation : MonoBehaviour
{
    public GameObject reverseScriptObj;
    public TrailRenderer[] trail;
    public ParticleSystem[] trailParticle;

    [SerializeField] float speed;

    void Update()
    {
        transform.Rotate(Vector3.forward * speed * Time.deltaTime);
    }

    public void ColorChangeOnReverse(Color color, float speed)
    {
        this.speed = speed < 0 ? -800 : 800;

        Gradient gradient = new Gradient();

        gradient.SetKeys(
        new GradientColorKey[] { new GradientColorKey(Color.white, 0), new GradientColorKey(color, 1) },
        new GradientAlphaKey[] { new GradientAlphaKey(0.7f, 0), new GradientAlphaKey(0, 1) });

        trail[0].colorGradient = gradient;
        trail[1].colorGradient = gradient;

        var main = trailParticle[0].main;
        main.startColor = color;
        var main2 = trailParticle[1].main;
        main2.startColor = color;

        Invoke(nameof(ResetThisScript), 2.5f);
    }

    public void ResetThisScript()
    {
        trail[0].colorGradient = new Gradient();
        trail[1].colorGradient = new Gradient();

        var main = trailParticle[0].main;
        main.startColor = new ParticleSystem.MinMaxGradient();
        var main2 = trailParticle[1].main;
        main2.startColor = new ParticleSystem.MinMaxGradient();

        this.gameObject.SetActive(false);
    }
}
