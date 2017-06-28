using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneEffectLauncher : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Fire(Sprite sprite)
    {
        StartCoroutine(LaunchEffect(sprite));
    }

    IEnumerator LaunchEffect(Sprite sprite)
    {
        GameObject go = new GameObject("Jää runiiddi :D");
        var renderer = go.AddComponent<SpriteRenderer>();
        renderer.sortingLayerName = "RuneEffects";
        renderer.sprite = sprite;
        go.transform.position = this.transform.position;
        // rotato 

        for (int i = 0; i < 60; i++)
        {
            go.transform.localScale = new Vector3(go.transform.localScale.x + 0.10f, go.transform.localScale.y + 0.10f);
            go.transform.Rotate(new Vector3(0, 0, 360 / 60));
            yield return null;
        }
        
        yield return new WaitForSeconds(2f);
       
        Destroy(go);
    }
}
