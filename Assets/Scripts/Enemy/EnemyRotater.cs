using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enemyDir
{
    Right,
    Left,
    Down,
    Up,
    LU,
    LD,
    RU,
    RD,
    StillR,
    StillL,
    StillRD,
    StillRU,
    StillLD,
    StillLU
}

public class EnemyRotater {

    Vector2[] middles = new Vector2[10];

    Vector2 middleVec = new Vector2(0,0);
    
    public Vector2 playerPos { get; set; }

    EnemyType MyType;
    public enemyDir _myDir { get { return myDir; } }
    enemyDir myDir;
    public bool Lock = false;
    public bool rotToPl = false;
	// Use this for initialization
	public void init (EnemyType type) {
        MyType = type;
        myDir = enemyDir.StillR;
	}
	
	// Update is called once per frame
	public void UpdateRotation (Vector2 velocity, Vector2 ownPos) {
        if (!Lock)
        {
            if (!rotToPl)
            {
                if(MyType == EnemyType.Wolf)
                {
                    uptadeDir6(velocity);
                }
                else
                {
                    uptadeDir4(velocity);
                }
            }
            else
            {
                rotateToPlayer(ownPos,velocity);
            }
        }
	}
    void uptadeDir4(Vector2 velocity) // tämä voidaan tehdä myös keskiarvolla
    {
        float xx = Mathf.Abs(velocity.x);
        float yy = Mathf.Abs(velocity.y);

        if (xx >= yy)
        {
            if (velocity.x >= 0)
            {
                myDir = enemyDir.Right;
            }
            else
            {
                myDir = enemyDir.Left;
            }
        }
        else
        {
            if (velocity.y >= 0)
            {
                myDir = enemyDir.Up;
            }
            else
            {
                myDir = enemyDir.Down;
            }
        }
    }
    void uptadeDir6(Vector2 velocity) // tämä voidaan tehdä myös keskiarvolla
    {
        float xx = Mathf.Abs(velocity.x);
        float yy = Mathf.Abs(velocity.y);
        //if(velocity.magnitude == 0 && !rotToPl)
        //{
        //    myDir = enemyDir.Still;
        //    return;
        //}
        //if (velocity.magnitude == 0 && rotToPl)
        //{
        //    myDir = enemyDir.Still;
        //    return;
        //}
        if (xx >= yy)
        {
            if (velocity.x >= 0)
            {
                myDir = enemyDir.Right;
            }
            else
            {
                myDir = enemyDir.Left;
            }
        }
        else
        {
            if (velocity.y <= 0)
            {
                if (velocity.x >= 0)
                {
                    myDir = enemyDir.RD;
                }
                else
                {
                    myDir = enemyDir.LD;
                }
            }
            else
            {
                if (velocity.x >= 0)
                {
                    myDir = enemyDir.RU;
                }
                else
                {
                    myDir = enemyDir.LU;
                }
            }
        }
    }
    void rotateToPlayer(Vector2 ownPos,Vector2 velocity)
    {
        Vector2 dir = playerPos - ownPos;

        if(MyType == EnemyType.Wolf)
        {
            uptadeDir6(dir);
            if(velocity.magnitude == 0)
            {
                switch (myDir)
                {
                    case enemyDir.Right:
                        myDir = enemyDir.StillR;
                        break;
                    case enemyDir.Left:
                        myDir = enemyDir.StillL;
                        break;
                    case enemyDir.RD:
                        myDir = enemyDir.StillRD;
                        break;
                    case enemyDir.RU:
                        myDir = enemyDir.StillRU;
                        break;
                    case enemyDir.LD:
                        myDir = enemyDir.StillLD;
                        break;
                    case enemyDir.LU:
                        myDir = enemyDir.StillLU;
                        break;
                }
            }
        }
        else
        {
            uptadeDir4(dir);
        }
    }
    public void HardRotate(Vector2 ownPos,Vector2 velocity)
    {
        rotateToPlayer(ownPos,velocity);
        Lock = true;
    }

}
