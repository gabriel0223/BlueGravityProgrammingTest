using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    public void PlayFootstepSound()
    {
        AudioManager.instance.PlayRandomBetweenSounds(new []
        {
            Sounds.FootstepDirt01,
            Sounds.FootstepDirt02,
            Sounds.FootstepDirt03,
            Sounds.FootstepDirt04
        });
    }
}
