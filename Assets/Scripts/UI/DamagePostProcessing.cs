using UnityEngine;
using System.Collections;

public class DamagePostProcessing : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] float intesity = 0;

    UnityEngine.Rendering.VolumeProfile volumeProfile;
    UnityEngine.Rendering.Universal.Vignette vignette;

    bool isPlaying = false;

    void Start()
    {
        volumeProfile = GetComponent<UnityEngine.Rendering.Volume>()?.profile;
        if (!volumeProfile) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));

        if (!volumeProfile.TryGet(out vignette)) throw new System.NullReferenceException(nameof(vignette));

        if (vignette)
		{
            vignette.SetAllOverridesTo(false);
        }
    }

    public void PlayDamageEffect()
	{
        StartCoroutine(TakeDamageEffect());
    }

    IEnumerator TakeDamageEffect()
	{
		if (!isPlaying)
		{
            isPlaying = true;
            intesity = 1f;
            vignette.intensity.Override(0.75f);
            vignette.SetAllOverridesTo(true);

            yield return new WaitForSeconds(0.02f);

            while (intesity > 0)
            {
                intesity -= 0.01f;

                if (intesity < 0) intesity = 0;
                vignette.intensity.Override(intesity);

                yield return new WaitForSeconds(0.01f);
            }

            vignette.SetAllOverridesTo(false);
            isPlaying = false;
            yield break;
        }
	}
}
