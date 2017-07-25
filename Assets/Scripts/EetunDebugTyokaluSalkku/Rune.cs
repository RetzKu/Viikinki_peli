using UnityEngine;

public abstract class Rune : ScriptableObject
{
    [SerializeField]
    public Vec2[] Indices;

    public int Length
    {
        get { return Indices.Length; }
    }

    public AudioClip Sound;
    public Sprite sprite;
    public Sprite HudImage;
    public float Cd;

    public bool ValidateRune(Vec2[] runeIndices)
    {
        // Count tarkistetaan RuneHolderissa !

        for (int i = 0; i < Length; i++)
        {
            if (Indices[i] != runeIndices[i])
            {
                return false;
            }
        }
        return true;
    }

    public bool ValidateRune(bool[] runeIndices)
    {
        bool[] indiceBools = new bool[9];
        for (int i = 0; i < Indices.Length; i++)
        {
            indiceBools[TouchController.GetBoolIndex(Indices[i])] = true;
        }

        bool value = true;
        for (int i = 0; i < Length; i++)
        {
            if (indiceBools[i] != runeIndices[i])
            {
                value = false;
                break;
            }
        }
        // Debug.Log("Rune validated!");
        return value;
    }

    public abstract void init(GameObject owner);
    public abstract void Fire();

    public virtual void OnGui(RuneBarUiController ui, int i)
    {
        ui.OnCd(i, Cd);
    }

    public Sprite GetGuiImage()
    {
        return HudImage;
    }
}
