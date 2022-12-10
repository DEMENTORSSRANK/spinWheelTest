using System;
using UnityEngine;

namespace ViewModel
{
    [Serializable]
    public struct FruitItem
    {
        [SerializeField] private Sprite _avatar;

        [Range(1, 3)] [SerializeField] private float _coefficient;

        [Range(1, 100)] [SerializeField] private int _percentsShow;

        public Sprite Avatar => _avatar;

        public float Coefficient => _coefficient;

        public int PercentsShow => _percentsShow;
    }
}