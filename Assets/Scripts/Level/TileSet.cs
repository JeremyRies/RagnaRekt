using System;
using LinkedLives.Game;
using UnityEngine;

namespace Level
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