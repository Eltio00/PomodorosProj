using UnityEngine;

public class MachineHouseHandler : MonoBehaviour
{
    [SerializeField] AudioSource gearSource;
    [SerializeField] AudioSource tonfSource1;
    [SerializeField] AudioSource tonfSource2;

    [SerializeField] private AudioClip tonfClip;
    [SerializeField] private AudioClip gearClip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PlayGearSound()
    {
        if (!gearSource.enabled)
            gearSource.enabled = true;
        gearSource.PlayOneShot(gearClip);   
    }

    public void PlayTonf1Sound()
    {
        if (!tonfSource1.enabled)
            tonfSource1.enabled = true;
        tonfSource1.PlayOneShot(tonfClip);
    }
    public void PlayTonf2Sound()
    {
        if (!tonfSource2.enabled)
            tonfSource2.enabled = true;
        tonfSource2.PlayOneShot(tonfClip);
    }
}

