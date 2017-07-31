using UnityEngine;

[CreateAssetMenu(menuName = "Runes/Aoe rune")]
public class FreezeRune : Rune
{
    // public AoeEffectData2 test;

    private GameObject _owner;
    private RuneEffectLauncher _launcher;
    private LayerMask _collisionMask;

    // public Tyyppi tyyppi;
    public AoeEffectData EffectData;
    [Header("TODO: frameille parempi korvaus (aika)")]

    [Header("if size == 0; mask = Enemy")]
    public string[] CollisionMaskValues;

    public override void init(GameObject owner)
    {
        this._owner = owner;
        this._launcher = owner.GetComponent<RuneEffectLauncher>();

        if (_launcher == null)
        {
            Debug.LogError("Laita RuneEffectLaucher.cs omistajalle (pelaaja?)");
        }

        if (CollisionMaskValues != null)
        {
            _collisionMask = LayerMask.GetMask(CollisionMaskValues);
        }
        else
        {
            _collisionMask = LayerMask.GetMask("Enemy");
        }
    }

    public override void Fire()
    {
        // launcheri tekee likaisen työn maailman kanssa visuaalinen collision

        // AiDebuff aiBuff = CreateInstance<AiDebuff>(); // debug
        // aiBuff.Duration = 5f;
        // aiBuff.SlowPercent = 0.5f;
        // aiBuff.Time = 5f;

        // EffectData.BuffToApply = aiBuff;
        // test.BuffToApply = aiBuff;

        _launcher.FireAoeEffect(sprite, EffectData, _collisionMask);

        Vector2 pos = _owner.transform.position;
        Debug.Log("FreezeRune lähettää terveisensä!");

        // Laukaise cast efecti
        // tyyppi.cast(_owner);
    }

    //public override void OnGui(RuneBarUiController ui)
    //{
    //}
}


