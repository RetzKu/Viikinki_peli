using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

[CreateAssetMenu(menuName = "Runes/FreezeRune")]
public class FreezeRune : Rune
{
    public float Range = 10f;
    private GameObject owner;
    private RuneEffectLauncher launcher;

    public override void init(GameObject owner)
    {
        this.owner = owner;
        this.launcher = owner.GetComponent<RuneEffectLauncher>();
    }

    // TODO: varmaan object pool olisi hyvä idis
    public override void Fire()
    {
        launcher.Fire(sprite);

        Vector2 pos = owner.transform.position;


        LayerMask mask = LayerMask.GetMask("Enemy");
        var colliders = Physics2D.CircleCastAll(pos, Range, new Vector2(0, 0), 0, mask);
        foreach (var collider in colliders)
        {
            Destroy(collider.transform.gameObject);
        }
        Debug.Log("FreezeRune lähettää terveisensä");

    }
    // Rune effect funktio, jota kutsuttaisiin Launcherista takaisin
    // OnStart Ja OnRuneEnd, joihin laitettaisiin esim buffin statsit ja rune effectiä voisi myös kutsua coroutinellä takaisin
}

[CreateAssetMenu(menuName = "Runes/BuffRune NOT IMPLEMENTED!")]
public class WeaponBuffRune : Rune
{
    // effect?
    //private GameObject owner;
    public float Range = 10f;

    public float duration;

    // statsit:
    // HP
    // Attack
    // Movement Speed 
    // joku cool buff: Immortal jne...
    // Debuffs:
    // CC
    // Statsit

    public override void init(GameObject owner)
    {
        //this.owner = owner;     // Launcheriin visuaalinene efectio jos on sellainen 
    }

    public override void Fire()
    {
        // owner.applyBuff();
        // laita timer joka lopettaa buffing keston
    }
}
