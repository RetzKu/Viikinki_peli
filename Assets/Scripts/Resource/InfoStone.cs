using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoStone : Resource
{
    public Rune RuneToTeach;

    public bool PlayerInRange = false;

    public Color Default;


    public override void OnDead()
    {
        Destroy(gameObject);
    }

    public override void Init(bool destroyedVersion)
    {
    }

    private void Update()
    {

    }

    public void AlphaEffect()
    {
        if (PlayerInRange == false)
        {
            PlayerInRange = true;
            StartCoroutine(EffectBrightness(GameObject.Find("Player"))); 
        }

    }

    IEnumerator EffectBrightness(GameObject Player)
    {
        
        while (PlayerInRange == true)
        {
            
            float Distance = Vector2.Distance(Player.transform.position, transform.position);
            float t = EnemyMovement.map(Distance, 1.5f, 3f, 1,0);

            transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, t);

            if(Vector2.Distance(Player.transform.position,transform.position) > 4)
            {
                PlayerInRange = false;
                transform.GetChild(0).GetComponent<SpriteRenderer>().color = Default;

            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
