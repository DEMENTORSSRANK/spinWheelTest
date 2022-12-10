using System;
using TMPro;
using UnityEngine;

namespace View
{
    [Serializable]
    public struct TmpTextValueView
    {
        [SerializeField] private TMP_Text _text;

        [TextArea] [SerializeField] private string _format;

        public void UpdateView(int value) =>
            _text.text = string.IsNullOrEmpty(_format) ? value.ToString() : string.Format(_format, value);
    }
}