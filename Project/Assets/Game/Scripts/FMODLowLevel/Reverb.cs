using UnityEngine;

/// <summary>
/// Hace que el sonido persista aunque se haya dejado de emitir el sonido
/// Son como ecos y dependen del recinto
/// </summary>
public class Reverb : MonoBehaviour {

    private enum Presets { ALLEY, ARENA, AUDITORIUM, BATHROOM ,NONE};

    [SerializeField] private SphereCollider minSphere;
    [SerializeField] private SphereCollider maxSphere;

    /// <summary>
    /// Preset inicial
    /// </summary>
    [SerializeField] private Presets preset;

    private FMOD.Reverb3D reverb;
    private FMOD.REVERB_PROPERTIES reverbProperties;

    /// <summary>
    /// Crea la reverb y establece el preset inicial
    /// </summary>
    private void Start()
    {
        reverb = LowLevelSystem.Instance.CreateReverb();
      
        switch(preset)
        {
            case Presets.ALLEY:
                reverbProperties = FMOD.PRESET.ALLEY();
                break;
            case Presets.ARENA:
                reverbProperties = FMOD.PRESET.ARENA();
                break;
            case Presets.AUDITORIUM:
                reverbProperties = FMOD.PRESET.AUDITORIUM();
                break;
            case Presets.BATHROOM:
                reverbProperties = FMOD.PRESET.BATHROOM();
                break;
            case Presets.NONE:
                reverbProperties = new FMOD.REVERB_PROPERTIES();
                break;
        }

        LowLevelSystem.ERRCHECK(reverb.setProperties(ref reverbProperties));

        FMOD.VECTOR pos = new FMOD.VECTOR();
        pos.x = transform.position.x;
        pos.y = transform.position.y;
        pos.z = transform.position.z;

        LowLevelSystem.ERRCHECK(reverb.set3DAttributes(ref pos, minSphere.radius, maxSphere.radius));
    }

    /// <summary>
    /// Activa/desactive la reverb
    /// </summary>
    /// <param name="active"></param>
    public void Active(bool active)
    {
        LowLevelSystem.ERRCHECK(reverb.setActive(active));
    }

}
