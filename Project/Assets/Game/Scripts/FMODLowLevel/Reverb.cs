using UnityEngine;

/// <summary>
/// Hace que el sonido persista aunque se haya dejado de emitir el sonido
/// Son como ecos y dependen del recinto
/// </summary>
public class Reverb : MonoBehaviour
{
    private enum Presets { ALLEY, ARENA, AUDITORIUM, BATHROOM, CARPETTEDHALLWAY, CAVE, CITY, CONCERTHALL, FOREST, GENERIC, HALLWAY, HANGAR, LIVINGROOM, MOUNTAINS, OFF, PADDEDCELL, PARKINGLOT, PLAIN, QUARRY, ROOM, SEWERPIPE, STONECORRIDOR, STONEROOM, UNDERWATER, NONE };

    [SerializeField] private SphereCollider minSphere;
    [SerializeField] private SphereCollider maxSphere;

    /// <summary>
    /// Preset inicial
    /// </summary>
    [SerializeField] private Presets preset = Presets.NONE;

    private FMOD.Reverb3D reverb;
    private FMOD.REVERB_PROPERTIES reverbProperties;

    /// <summary>
    /// Crea la reverb y establece el preset inicial
    /// </summary>
    private void Start()
    {
        reverb = LowLevelSystem.Instance.CreateReverb();

        switch (preset)
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
            case Presets.CARPETTEDHALLWAY:
                reverbProperties = FMOD.PRESET.CARPETTEDHALLWAY();
                break;
            case Presets.CAVE:
                reverbProperties = FMOD.PRESET.CAVE();
                break;
            case Presets.CITY:
                reverbProperties = FMOD.PRESET.CITY();
                break;
            case Presets.CONCERTHALL:
                reverbProperties = FMOD.PRESET.CONCERTHALL();
                break;
            case Presets.FOREST:
                reverbProperties = FMOD.PRESET.FOREST();
                break;
            case Presets.GENERIC:
                reverbProperties = FMOD.PRESET.GENERIC();
                break;
            case Presets.HALLWAY:
                reverbProperties = FMOD.PRESET.HALLWAY();
                break;
            case Presets.HANGAR:
                reverbProperties = FMOD.PRESET.HANGAR();
                break;
            case Presets.LIVINGROOM:
                reverbProperties = FMOD.PRESET.LIVINGROOM();
                break;
            case Presets.MOUNTAINS:
                reverbProperties = FMOD.PRESET.MOUNTAINS();
                break;
            case Presets.OFF:
                reverbProperties = FMOD.PRESET.OFF();
                break;
            case Presets.PADDEDCELL:
                reverbProperties = FMOD.PRESET.PADDEDCELL();
                break;
            case Presets.PARKINGLOT:
                reverbProperties = FMOD.PRESET.PARKINGLOT();
                break;
            case Presets.PLAIN:
                reverbProperties = FMOD.PRESET.PLAIN();
                break;
            case Presets.QUARRY:
                reverbProperties = FMOD.PRESET.QUARRY();
                break;
            case Presets.ROOM:
                reverbProperties = FMOD.PRESET.ROOM();
                break;
            case Presets.SEWERPIPE:
                reverbProperties = FMOD.PRESET.SEWERPIPE();
                break;
            case Presets.STONECORRIDOR:
                reverbProperties = FMOD.PRESET.STONECORRIDOR();
                break;
            case Presets.STONEROOM:
                reverbProperties = FMOD.PRESET.STONEROOM();
                break;
            case Presets.UNDERWATER:
                reverbProperties = FMOD.PRESET.UNDERWATER();
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
