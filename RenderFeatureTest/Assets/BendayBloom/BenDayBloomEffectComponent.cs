using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenu("Custom/Ben Day Bloom")]
public class BenDayBloomEffectComponent : VolumeComponent, IPostProcessComponent
{
    [Header("Bloom Settings")]
    public FloatParameter threshold = new FloatParameter(0.5f, true);
    public FloatParameter intensity = new FloatParameter(1.0f, true);
    public ClampedFloatParameter scatter = new ClampedFloatParameter(0.7f, 0, 1, true);
    public IntParameter clamp = new IntParameter(65472, true);
    public ClampedIntParameter maxIterations = new ClampedIntParameter(6, 0, 10);
    public NoInterpColorParameter tint = new NoInterpColorParameter(Color.white);

    [Header("Benday")]
    public IntParameter dotsDensity = new IntParameter(10, true);
    public ClampedFloatParameter dotsCutoff = new ClampedFloatParameter(0.5f, 0, 1, true);
    public Vector2Parameter scrollDirection = new Vector2Parameter(new Vector2(), true);


    public bool IsActive()
    {
        return true;
    }

    public bool IsTileCompatible()
    {
        return false;
    }
}
