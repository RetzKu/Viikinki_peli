using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour {

    private HandRoot Hand;
        private List<Transform> Hands;

    public GameObject Weapon;
        private WeaponType Type;

    private List<GameObject> Torsos;
        private enemyDir Direction;

    private List<Sprite> Heads;
    private List<Sprite> Chests;

    private List<Animator> Animators;
    
    private enum DirectionState { Left,Down,Up,Right }

    private void Start()
    {
        /*Finding enemy hands*/
        Hands = new List<Transform>(3);
        Hands.Add(transform.FindChild("s_l_hand"));
        Hands.Add(transform.FindChild("d_r_hand"));
        Hands.Add(transform.FindChild("u_l_hand"));

        /*Finding enemy Torsos*/
        Torsos = new List<GameObject>(3);
        Torsos.Add(transform.Find("s_c_torso").gameObject);
        Torsos.Add(transform.Find("d_c_torso").gameObject);
        Torsos.Add(transform.Find("u_c_torso").gameObject);
    }

    public class SpriteChanger
    {

    }

    private class HandRoot
    {

    }
}
