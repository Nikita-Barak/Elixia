using UnityEngine;

public class PoisonPotionEffect : BasePotionEffect
{
    [SerializeField] GameObject poisonCloud;
    [SerializeField] private float totalDuration; // The total duration of the poison
    [SerializeField] private float poisonEmissionDuration; // duration for visual poison particle emission.
    [SerializeField] private float startColliderScale; // starting scale of the poison collider.
    [SerializeField] private float endColliderScale; // ending scale of the poison collider.
    [SerializeField] private float scalingDuration; // duration of collider expansion.

    private GameObject currentPoisonCloud;

    public override void ActivatePotionEffect(GameObject potion)
    {
        CreatePoisonCloud(potion);
    }

    public void CreatePoisonCloud(GameObject obj)
    {
        currentPoisonCloud = Instantiate(poisonCloud, obj.transform.position, Quaternion.identity);

        PoisonEffectController currentPoisonCloudScript = currentPoisonCloud.GetComponent<PoisonEffectController>();
        currentPoisonCloudScript.SetupPoisonCloud
        (
            totalDuration, poisonEmissionDuration, startColliderScale, endColliderScale, scalingDuration
        );
    }
}
