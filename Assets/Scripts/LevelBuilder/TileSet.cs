using System;
using LevelBuilder;
using UnityEngine;

namespace LinkedLives.Game
{
    [Serializable]
    public struct TileSet
    {
        public TileSetType Type;
        public Tile[] Tiles;
    }

    [Serializable]
    public struct Tile
    {
        public TileType Type;
        public Sprite Sprite;
    }
}