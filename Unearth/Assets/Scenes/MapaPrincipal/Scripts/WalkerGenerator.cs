using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TerrainTools;
using UnityEngine.Tilemaps;

public class WalkerGenerator : MonoBehaviour
{
    public enum Grid
    {

        CENTER = 0,
        VULCAN = 1,
        MOUNTAIN = 2,
        FOREST = 3,
        JUNGLE = 4,
        TUNDRA = 5,
        DESERT = 6,
        PLAINS = 7,
        OCEAN = 8,
        EMPTY = 9
    }



    //Variables

    public Grid[,] gridHandler;
    public List<WalkerObject> Walkers;
    public Tilemap mainTileMap;
    public Tilemap backTileMap;
    
    public string mapSize;

    public int MaximumWalkers = 10;
    public int TileCount = default;
    public float FillPercentage = 0.4f;
    public float WaitTime = 0.05f;


    private Tile[] tiles;
    private int MapWidth;
    private int MapHeight;

    private int MapMiddle;

    void Start()
    {
        mapSize = PlayerPrefs.GetString("tamMapa");
        tiles = Resources.LoadAll<Tile>("");
        InitializeGrid();
    }

    void InitializeGrid()
    {

        

        switch(mapSize){
            case "big": MapHeight = 31; MapWidth = 31; break;
            case "medium": MapHeight = 15; MapWidth = 15; break;
            case "small": MapHeight = 9; MapWidth = 9; break;
        }


        gridHandler = new Grid[MapWidth, MapHeight];

        for (int x = 0; x < gridHandler.GetLength(0); x++)
        {
            for (int y = 0; y < gridHandler.GetLength(1); y++)
            {
                gridHandler[x, y] = Grid.EMPTY;
            }
        }

        Walkers = new List<WalkerObject>();

        MapMiddle = (int) MapWidth/2;
        Vector3Int TileCenter = new Vector3Int(MapMiddle,MapMiddle, MapMiddle);



        WalkerObject curWalker = new WalkerObject(new Vector2(TileCenter.x, TileCenter.y), GetDirection(), 0.5f, (int) Grid.CENTER);
        gridHandler[TileCenter.x, TileCenter.y] = Grid.CENTER;
        Walkers.Add(curWalker);

        TileCount++;

        

        CreateFloors();
    }

    Vector2 GetDirection()
    {
        int choice = Mathf.FloorToInt(UnityEngine.Random.value * 3.99f);

        switch (choice)
        {
            case 0:
                return Vector2.down;
            case 1:
                return Vector2.left;
            case 2:
                return Vector2.up;
            case 3:
                return Vector2.right;
            default:
                return Vector2.zero;
        }
    }


    Grid BiomaComum(Grid LastBiome){
        float prob = UnityEngine.Random.value;
        if(LastBiome == Grid.PLAINS){
            if(prob <= 0.65)return Grid.FOREST;
            return Grid.DESERT;
        }

        if(prob <= 0.5)return Grid.PLAINS;
        if(prob <= 0.75)return Grid.TUNDRA;
        return Grid.JUNGLE;
    }

    Grid BiomaIncomum(Grid LastBiome){
        float prob = UnityEngine.Random.value;
        if(LastBiome == Grid.DESERT){
            return Grid.PLAINS;
        }
        if(LastBiome == Grid.JUNGLE){
            return Grid.FOREST;
        }
        if(prob <= 0.65)return Grid.FOREST;
        return Grid.MOUNTAIN;
    }

    Grid BiomaRaro(Grid LastBiome){
        float prob = UnityEngine.Random.value;
        if(LastBiome == Grid.MOUNTAIN){
            if(prob <= 0.65)return Grid.TUNDRA;
            return Grid.VULCAN;
        }

        return Grid.MOUNTAIN;
    }

    Grid changeBiome(Grid LastBiome){
        if(LastBiome == Grid.PLAINS || LastBiome == Grid.FOREST)return BiomaComum(LastBiome);
        if(LastBiome == Grid.DESERT || LastBiome == Grid.TUNDRA || LastBiome == Grid.JUNGLE)return BiomaIncomum(LastBiome);
        return BiomaRaro(LastBiome);
    }

    Grid getBiome(Grid LastBiome){
        if(LastBiome == Grid.CENTER)return Grid.PLAINS;
        float prob = UnityEngine.Random.value;
        if (prob>0.70)return changeBiome(LastBiome);

        
        return LastBiome;
    }

    void CreateFloors()
    {
        while ((float)TileCount / (float)gridHandler.Length < FillPercentage)
        {
            foreach (WalkerObject curWalker in Walkers)
            {
                Vector3Int curPos = new Vector3Int((int)curWalker.Position.x, (int)curWalker.Position.y, (int) curWalker.Position.y );

                if (gridHandler[curPos.x, curPos.y] > Grid.PLAINS)
                {
                    int r = (int) (UnityEngine.Random.value*10);
     
                    TileCount++;
                    gridHandler[curPos.x, curPos.y] = getBiome((Grid) curWalker.LastBiome);
                }

                curWalker.LastBiome = (int) gridHandler[curPos.x, curPos.y];
            }


            //Walker Methods
            ChanceToRemove();
            ChanceToRedirect();
            ChanceToCreate();
            UpdatePosition();

    
        }

        PaintMap();
    }

    void ChanceToRemove()
    {
        int updatedCount = Walkers.Count;
        for (int i = 0; i < updatedCount; i++)
        {
            if (UnityEngine.Random.value < Walkers[i].ChanceToChange && Walkers.Count > 1)
            {
                Walkers.RemoveAt(i);
                break;
            }
        }
    }

    void ChanceToRedirect()
    {
        for (int i = 0; i < Walkers.Count; i++)
        {
            if (UnityEngine.Random.value < Walkers[i].ChanceToChange)
            {
                WalkerObject curWalker = Walkers[i];
                curWalker.Direction = GetDirection();
                Walkers[i] = curWalker;
            }
        }
    }

    void ChanceToCreate()
    {
        int updatedCount = Walkers.Count;
        for (int i = 0; i < updatedCount; i++)
        {
            if (UnityEngine.Random.value < Walkers[i].ChanceToChange && Walkers.Count < MaximumWalkers)
            {
                Vector2 newDirection = GetDirection();
                Vector2 newPosition = Walkers[i].Position;

                WalkerObject newWalker = new WalkerObject(newPosition, newDirection, 0.5f, Walkers[i].LastBiome);
                Walkers.Add(newWalker);
            }
        }
    }

    void UpdatePosition()
    {
        for (int i = 0; i < Walkers.Count; i++)
        {
            WalkerObject FoundWalker = Walkers[i];
            FoundWalker.Position += FoundWalker.Direction;
            FoundWalker.Position.x = Mathf.Clamp(FoundWalker.Position.x, 1, gridHandler.GetLength(0) - 2);
            FoundWalker.Position.y = Mathf.Clamp(FoundWalker.Position.y, 1, gridHandler.GetLength(1) - 2);
            Walkers[i] = FoundWalker;
        }
    }

    void PaintMap()
    {
        mainTileMap.SetTile(new Vector3Int(0,0,0), tiles[8]);
        for (int x = 0; x < gridHandler.GetLength(0) - 1; x++)
        {
            for (int y = 0; y < gridHandler.GetLength(1) - 1; y++)
            {
                int selected = 0;
                Vector3Int currentPos = new Vector3Int(x-MapMiddle,y-MapMiddle,(y-MapMiddle)*-1);
                switch(gridHandler[x, y]){
                    case Grid.CENTER: selected = 8;break;
                    case Grid.EMPTY: gridHandler[x , y] = Grid.OCEAN; selected = 6;  break;
                    case Grid.VULCAN: selected = 11;break;
                    case Grid.TUNDRA: selected = 3;break;
                    case Grid.JUNGLE: selected = 4; break;
                    case Grid.FOREST: selected = 2; break;
                    case Grid.DESERT: selected = 1; break;
                    case Grid.MOUNTAIN: selected = 5; break;
                    case Grid.PLAINS: selected = 7; break;

                }
                mainTileMap.SetTile(currentPos, tiles[selected]);
            }
        }
        
        for(int x = -100; x <= 100; x++){
                
            for(int y = -100; y <= 100; y++){

                backTileMap.SetTile(new Vector3Int(x,y,y), tiles[6]);

            }
        }
    }


}