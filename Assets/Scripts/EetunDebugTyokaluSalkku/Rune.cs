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
    public Sprite OnCdHudImage;
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

    public bool ValidateRune(bool[] runeIndices, int[] touches)
    {
        int[] indiceCounts = new int[9];
        for (int i = 0; i < Indices.Length; i++)
        {
            indiceCounts[TouchController.GetBoolIndex(Indices[i])]++;
        }

        bool value = true;
        for (int i = 0; i < touches.Length; i++)
        {
            if (indiceCounts[i] != touches[i])
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
        ui.OnCd(i, Cd, OnCdHudImage);
    }

    public Sprite GetGuiImage()
    {
        return HudImage;
    }
}
