using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public class Roulette
    {
        private readonly List<WinLine> _cachedWins = new List<WinLine>(6);

        private readonly Vector2Int[] _cachedWinElementPositions = new Vector2Int[3];

        public IEnumerable<WinLine> CheckWins(SpriteRenderer[,] items)
        {
            _cachedWins.Clear();

            Sprite checkElement;

            bool success;

            for (int y = 0; y < items.GetLength(0); y++)
            {
                checkElement = items[y, 0].sprite;

                success = true;

                for (int x = 1; x < items.GetLength(1); x++)
                {
                    if (items[y, x].sprite == checkElement)
                        continue;

                    success = false;

                    break;
                }

                if (success)
                {
                    for (int x = 0; x < items.GetLength(1); x++) 
                        _cachedWinElementPositions[x] = new Vector2Int(x, y);

                    _cachedWins.Add(new WinLine(_cachedWinElementPositions));
                }
            }

            for (int x = 0; x < items.GetLength(1); x++)
            {
                checkElement = items[0, x].sprite;
            
                success = true;
                
                _cachedWinElementPositions[0] = new Vector2Int(x, 0);  
            
                for (int y = 0; y < items.GetLength(0); y++)
                {
                    if (items[y, x].sprite == checkElement)
                    {
                        _cachedWinElementPositions[y] = new Vector2Int(x, y);
                        
                        continue;
                    }
            
                    success = false;
                    
                    break;
                }
                
                if (success)
                    _cachedWins.Add(new WinLine(_cachedWinElementPositions));
            }

            return _cachedWins;
        }
    }
}