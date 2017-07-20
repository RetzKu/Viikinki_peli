using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ResourceManager.cs inisoi ruumiit 
public class Corpse : Resource
{
    public static Sprite[] CorpseSprites;
    private bool canDrop = true;

    public override void OnDead()
    {
        if (canDrop)
        {
            // TODO: tehokkaampi drop
            for(int i = 0; i < 10; i++)
                DropScript.Drop(ResourceManager.Instance.GetCorpseDrops(), transform);
        }
    }

    public override void Init(bool destroyedVersion)
    {
        transform.localScale = new Vector3(3f, 3f, 3f);
        int index = Random.Range(0, CorpseSprites.Length);
        Sprite sprite = CorpseSprites[index];

        var sr = GetComponent<SpriteRenderer>();
        sr.sprite = sprite;

        dead = destroyedVersion;

        if (destroyedVersion)
            canDrop = false;
    }
}
