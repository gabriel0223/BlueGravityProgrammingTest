using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    public enum Surfaces
    {
        Dirt, Grass
    }

    [SerializeField] private string[] grassFootstepsAudios;
    [SerializeField] private string[] dirtFootstepsAudios;
    [HideInInspector] public Surfaces currentSurface = Surfaces.Dirt;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayFootstepSound()
    {
        switch (currentSurface)
        {
            case Surfaces.Grass:
                AudioManager.instance.PlayRandomBetweenSounds(grassFootstepsAudios);
                break;
            default:
                AudioManager.instance.PlayRandomBetweenSounds(dirtFootstepsAudios);
                break;
        }
    }
}
