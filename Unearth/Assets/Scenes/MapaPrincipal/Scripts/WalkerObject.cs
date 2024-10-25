using UnityEngine;

public class WalkerObject
{

  

    public int LastBiome;
    public Vector2 Position;
    public Vector2 Direction;
    public float ChanceToChange;

    public WalkerObject(Vector2 pos, Vector2 dir, float chanceToChange, int lb){
        LastBiome = lb;
        Position = pos;
        Direction = dir;
        ChanceToChange = chanceToChange;
    }
}