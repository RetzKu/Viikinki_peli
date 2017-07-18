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

        /*Finding Animators*/
        Animators = new List<Animator>(3);
        foreach (GameObject t in Torsos) { Animators.Add(t.GetComponent<Animator>()); }
    }

    public class SpriteChanger
    {

    }

    private class HandRoot
    {
        private Transform _Hand;
        private GameObject _Weapon;

        public HandRoot(Transform Hand, GameObject Weapon) { _Hand = Hand; _Weapon = Weapon; }

        public void SwapHand(DirectionState Direction)
        {
            switch (Direction)
            {
                case DirectionState.Left: { break; }
                case DirectionState.Down: { break; }
                case DirectionState.Up: { break; }
                case DirectionState.Right: { break; }
            }
        }

        private void WeaponSettings(DirectionState Direction)
        {
            //if(_Weapon.GetComponent<Melee>() != null) { _Weapon.GetComponent<Melee>().Reposition(); }
        }

    }
}
