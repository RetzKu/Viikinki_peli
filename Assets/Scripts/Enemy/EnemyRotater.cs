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
    Still
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
        myDir = enemyDir.Still;
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
                rotateToPlayer(ownPos);
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
        if(velocity.magnitude == 0 && !rotToPl)
        {
            myDir = enemyDir.Still;
            return;
        }
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
    void rotateToPlayer(Vector2 ownPos)
    {
        Vector2 dir = playerPos - ownPos;

        if(MyType == EnemyType.Wolf)
        {
            uptadeDir6(dir);
        }
        else
        {
            uptadeDir4(dir);
        }
    }
}
