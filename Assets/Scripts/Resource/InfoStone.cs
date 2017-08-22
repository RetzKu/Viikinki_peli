using System.Collections;
using UnityEngine;

public class InfoStone : Resource
{
    public Rune RuneToTeach;
    public bool PlayerInRange = false;

    public Color Default;
    private float Distance;

    public bool RecipeLearned = false;
    private bool FadeDone = false;

    private float StartTime;
    public float Duration;

    public override void OnDead()
    {
        Destroy(gameObject);
    }

    public override void Init(bool destroyedVersion)
    {
    }

    void Start()
    {
        if (RuneToTeach != null)
        {
            RuneToTeach.init(this.gameObject);
            RuneToTeach.InitInternalIndices();
        }
    }

    //  private void Update()
    //  {
    //      if (Input.GetKeyDown(KeyCode.E))
    //      {
    //      }
    //  }

     //              ------------Rune rune----------------

    public void TryToTeachRune(Vec2[] positions, int realSize)
    {
        var craftingrecipeHolder = CraftingManager.Instance.GetComponent<RuneHolder>();
        if (RuneToTeach != null && !RecipeLearned)
        {
            if (RuneToTeach.Length == realSize && RuneToTeach.ValidateRune(positions))
            {
                LearnStone();
                craftingrecipeHolder.AddRune(RuneToTeach);
                // print("teached " + RuneToTeach.name);
                // RecipeLearned = true;
            }
        }
        else
        {
            Debug.LogWarning(gameObject.name + " does not contain rune or already learned");
        }
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

            Distance = Vector2.Distance(Player.transform.position, transform.position);
            float t = EnemyMovement.map(Distance, 3, 4, 0.7f, 0);

            if (FadeDone == true)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, t);
                transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, t);
            }
            else
            {
                transform.GetChild(2).GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, t);
            }

            if (Vector2.Distance(Player.transform.position, transform.position) > 5)
            {
                PlayerInRange = false;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void LearnStone()
    {
        float tmp = Vector3.Distance(GameObject.Find("Player").transform.position, transform.position);
        //rune holderiin uusi rune
        if (Vector3.Distance(GameObject.Find("Player").transform.position, transform.position) < 3)
        {
            if (RecipeLearned == false)
            {
                StartTime = Time.time;
                StartCoroutine(RecipeLearnedFade(StartTime));
                RecipeLearned = true;
                // GameObject.Find("Player").GetComponent<RuneHolder>().AddRune(RuneToTeach);
            }

        }
    }

    IEnumerator RecipeLearnedFade(float StartTime)
    {
        while (FadeDone == false)
        {
            float t = (Time.time - StartTime) / Duration;
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, Mathf.SmoothStep(0.1f, 1, t));
            transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, Mathf.SmoothStep(0.1f, 1, t));

            if (t >= 1)
            {
                transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = false;
                FadeDone = true;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}

