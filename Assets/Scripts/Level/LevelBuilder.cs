using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using LinkedLives.Game;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

namespace Level
{
    public class LevelBuilder : MonoBehaviour
    {
        public TileSetType TileSet;

        public TileSet[] AvailableTileSets;

        public bool DisableRenderers;

        public GameObject[] RendererParents;

        private void BuildLevel(bool preview)
        {
            var rendererList = new List<SpriteRenderer>();
            foreach (var rendererParent in RendererParents)
            {
                rendererList.AddRange(rendererParent.GetComponentsInChildren<SpriteRenderer>());
            }

            var renderers = rendererList.ToArray();

            Debug.Log("Found " + renderers.Length + " `Ground`-SpriteRenderers");


            if (DetectMisplacedRenderers(renderers))
            {
                return;
            }

            var levelSize = GetLevelSize(renderers);
            var blocks = CreateBlockArray(levelSize, renderers);

            //FillGround(blocks);
            UpdateTileTypes(blocks);

            if (preview)
            {
                OutputDebugFile(blocks);
            }
            else
            {
               // var layout = ReplaceSpriteRenderers(renderers, blocks);
                var layout = new GameObject("collider");
                layout.transform.position = new Vector3(-0.5f,-0.5f,0f);
                CreateHorizontalColliders(layout, blocks);
                CreateVerticalColliders(layout, blocks);
            }
        }

        private void CreateVerticalColliders(GameObject layout, TileType[,] blocks)
        {
            var maxX = blocks.GetLength(0);
            var maxY = blocks.GetLength(1);
            var edgePoints = new Vector2[2];

            for (int x = 0; x < maxX; x++)
            {
                for (int y = 0; y < maxY; y++)
                {
                    switch (blocks[x, y])
                    {
                        case TileType.BottomLeftCorner:
                            edgePoints[0] = new Vector2(x, y);
                            break;
                        case TileType.TopLeftInset:
                            edgePoints[0] = new Vector2(x, y + 1);
                            break;

                        case TileType.TopLeftCorner:
                            edgePoints[1] = new Vector2(x, y + 1);
                            CreateCollider(layout, edgePoints);
                            break;
                        case TileType.BottomLeftInset:
                            edgePoints[1] = new Vector2(x, y);
                            CreateCollider(layout, edgePoints);
                            break;
                        case TileType.BottomRightCorner:
                            edgePoints[1] = new Vector2(x + 1, y);
                            break;
                        case TileType.TopRightInset:
                            edgePoints[1] = new Vector2(x + 1, y + 1);
                            break;

                        case TileType.TopRightCorner:
                            edgePoints[0] = new Vector2(x + 1, y + 1);
                            CreateCollider(layout, edgePoints);
                            break;
                        case TileType.BottomRightInset:
                            edgePoints[0] = new Vector2(x + 1, y);
                            CreateCollider(layout, edgePoints);
                            break;
                    }
                }
            }
        }

        private static void CreateHorizontalColliders(GameObject layout, TileType[,] blocks)
        {
            var maxX = blocks.GetLength(0);
            var maxY = blocks.GetLength(1);
            var edgePoints = new Vector2[2];

            for (int y = 0; y < maxY; y++)
            {
                for (int x = 0; x < maxX; x++)
                {
                    switch (blocks[x, y])
                    {
                        case TileType.TopLeftCorner:
                            edgePoints[0] = new Vector2(x, y + 1);
                            break;
                        case TileType.TopRightInset:
                            edgePoints[0] = new Vector2(x + 1, y + 1);
                            break;

                        case TileType.TopRightCorner:
                            edgePoints[1] = new Vector2(x + 1, y + 1);
                            CreateCollider(layout, edgePoints);
                            break;
                        case TileType.TopLeftInset:
                            edgePoints[1] = new Vector2(x, y + 1);
                            CreateCollider(layout, edgePoints);
                            break;

                        case TileType.BottomLeftCorner:
                            edgePoints[1] = new Vector2(x, y);
                            break;
                        case TileType.BottomRightInset:
                            edgePoints[1] = new Vector2(x + 1, y);
                            break;

                        case TileType.BottomRightCorner:
                            edgePoints[0] = new Vector2(x + 1, y);
                            CreateCollider(layout, edgePoints);
                            break;
                        case TileType.BottomLeftInset:
                            edgePoints[0] = new Vector2(x, y);
                            CreateCollider(layout, edgePoints);
                            break;
                    }
                }
            }
        }

        private static void CreateCollider(GameObject layout, Vector2[] edgePoints)
        {
            var edgeCollider = layout.AddComponent<EdgeCollider2D>();
            edgeCollider.points = edgePoints;
        }

        private void UpdateTileTypes(TileType[,] blocks)
        {
            var maxX = blocks.GetLength(0);
            var maxY = blocks.GetLength(1);
            for (int x = 0; x < maxX; x++)
            {
                for (int y = 0; y < maxY; y++)
                {
                    if (blocks[x, y] != TileType.Center) continue;

                    blocks[x, y] = GetTileType(blocks, x, y, maxX, maxY);
                }
            }
        }

        private static TileType GetTileType(TileType[,] blocks, int x, int y, int maxX, int maxY)
        {
            var left = x != 0 && blocks[x - 1, y] != TileType.None;
            var bottom = y != 0 && blocks[x, y - 1] != TileType.None;
            var right = x != maxX - 1 && blocks[x + 1, y] != TileType.None;
            var top = y != maxY - 1 && blocks[x, y + 1] != TileType.None;

            var bottomLeft = x != 0 && y != 0 && blocks[x - 1, y - 1] != TileType.None;
            var bottomRight = x != maxX - 1 && y != 0 && blocks[x + 1, y - 1] != TileType.None;
            var topRight = x != maxX - 1 && y != maxY - 1 && blocks[x + 1, y + 1] != TileType.None;
            var topLeft = x != 0 && y != maxY - 1 && blocks[x - 1, y + 1] != TileType.None;

            var allNeighbors = left && bottom && right && top;

            if (allNeighbors && bottomLeft && bottomRight && topRight && topLeft) return TileType.Center;

            if (allNeighbors && bottomRight && topRight && topLeft) return TileType.BottomLeftInset;
            if (allNeighbors && bottomLeft && topRight && topLeft) return TileType.BottomRightInset;
            if (allNeighbors && bottomLeft && bottomRight && topLeft) return TileType.TopRightInset;
            if (allNeighbors && bottomLeft && bottomRight && topRight) return TileType.TopLeftInset;

            if (top && bottom && right && bottomRight && topRight) return TileType.LeftEdge;
            if (right && left && top && topLeft && topRight) return TileType.BottomEdge;
            if (top && bottom && left && bottomLeft && topLeft) return TileType.RightEdge;
            if (right && left && bottom && bottomLeft && bottomRight) return TileType.TopEdge;

            if (right && top && topRight) return TileType.BottomLeftCorner;
            if (left && top && topLeft) return TileType.BottomRightCorner;
            if (left && bottom && bottomLeft) return TileType.TopRightCorner;
            if (right && bottom && bottomRight) return TileType.TopLeftCorner;

            return TileType.None;
        }

        private static void OutputDebugFile(TileType[,] blocks)
        {
            var sb = new StringBuilder();
            for (int y = blocks.GetLength(1) - 1; y >= 0; y--)
            {
                for (int x = 0; x < blocks.GetLength(0); x++)
                {
                    switch (blocks[x, y])
                    {
                        case TileType.Center:
                        case TileType.None:
                            sb.Append(" ");
                            break;
                        case TileType.LeftEdge:
                        case TileType.RightEdge:
                            sb.Append("┃");
                            break;
                        case TileType.TopEdge:
                        case TileType.BottomEdge:
                            sb.Append("━");
                            break;
                        case TileType.BottomRightInset:
                        case TileType.TopLeftCorner:
                            sb.Append("┏");
                            break;
                        case TileType.BottomLeftInset:
                        case TileType.TopRightCorner:
                            sb.Append("┓");
                            break;
                        case TileType.TopRightInset:
                        case TileType.BottomLeftCorner:
                            sb.Append("┗");
                            break;
                        case TileType.TopLeftInset:
                        case TileType.BottomRightCorner:
                            sb.Append("┛");
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                sb.AppendLine();
            }

            try
            {
                var fileName = Path.Combine(Path.GetTempPath(), "level_preview.lvl");
                Debug.Log("Writing to file " + fileName);
                File.WriteAllText(fileName, sb.ToString());
                Process.Start(fileName);
            }
                // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception)
            {
            }
        }

        private static TileType[,] CreateBlockArray(Vector2 levelSize, SpriteRenderer[] renderers)
        {
            var levelX = Mathf.RoundToInt(levelSize.x);
            var levelY = Mathf.RoundToInt(levelSize.y);

            var blocks = new TileType[levelX + 1, levelY + 1];

            foreach (var spriteRenderer in renderers)
            {
                var position = spriteRenderer.transform.position;
                var x = Mathf.RoundToInt(position.x);
                var y = Mathf.RoundToInt(position.y);
                if (spriteRenderer.transform.rotation != Quaternion.identity)
                {
                    blocks[x - 1, y] = TileType.Center;
                }
                else
                {
                    blocks[x, y] = TileType.Center;
                }
            }

            return blocks;
        }

        private static Vector2 GetLevelSize(SpriteRenderer[] renderers)
        {
            var minPos = new Vector2(float.MaxValue, float.MaxValue);
            var maxPos = new Vector2(float.MinValue, float.MinValue);

            foreach (var spriteRenderer in renderers)
            {
                var position = spriteRenderer.transform.position;
                minPos = Vector2.Min(minPos, position);
                maxPos = Vector2.Max(maxPos, position);
            }

            Debug.Log("MinPos:" + minPos);
            Debug.Log("MaxPos:" + maxPos);

            if (minPos.magnitude > 0)
            {
                FixOrigin(minPos);
            }

            return maxPos - minPos;
        }

        private static void FixOrigin(Vector2 minPos)
        {
            Debug.Log("Moving objects in scenes to fit origin.");

            var rootGameObjects = GetOpenLevelScenes().SelectMany(scene => scene.GetRootGameObjects());

            foreach (var rootGameObject in rootGameObjects)
            {
                var position = rootGameObject.transform.position;
                var newPosition = new Vector3(position.x - minPos.x, position.y - minPos.y, position.z);
                rootGameObject.transform.position = newPosition;
            }
        }

        private static List<Scene> GetOpenLevelScenes()
        {
            var scenes = new List<Scene>();
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.name == "Game") continue;
                scenes.Add(scene);
            }

            return scenes;
        }

        private static bool DetectMisplacedRenderers(SpriteRenderer[] renderers)
        {
            var brokenRenderers = 0;
            foreach (var spriteRenderer in renderers)
            {
                spriteRenderer.color = Color.white;

                var position = spriteRenderer.transform.localPosition;
                var x = Mathf.RoundToInt(position.x);
                var y = Mathf.RoundToInt(position.y);

                if (Mathf.Abs(x - position.x) > 0.001f)
                {
                    spriteRenderer.color = Color.red;
                    brokenRenderers++;
                }
                else if (Mathf.Abs(y - position.y) > 0.001f)
                {
                    spriteRenderer.color = Color.red;
                    brokenRenderers++;
                }
            }

            // ReSharper disable once InvertIf
            if (brokenRenderers > 0)
            {
                Debug.Log("Found " + brokenRenderers + "  renderers that were not placed on grid!");

                return true;
            }

            return false;
        }

        public void PreviewLevel()
        {
            BuildLevel(true);
        }

        public void BuildLevel()
        {
            BuildLevel(false);
        }
    }

    public enum TileSetType
    {
        Default,
        Cave,
    }
}