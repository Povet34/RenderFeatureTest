using System;
using UnityEngine.Rendering;

[Serializable]
[VolumeComponentMenu("Custom/DitheringVolumeComponent")]
public class DitheringVolumeComponent : VolumeComponent
{
    public ClampedFloatParameter intensity = new ClampedFloatParameter(value: 0.0f, min: 0f, max: 1f, overrideState: true);

    public bool IsActive() => intensity.value > 0f;
}
