using Editty.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Editty.Models
{
    public class SearchManager : ISearchManager
    {
        private readonly RichTextBox _richTextBox;
        private List<TextRange> _foundRanges;
        private int _currentMatchIndex;

        public SearchManager(RichTextBox textBox)
        {
            _richTextBox = textBox;
            _foundRanges = new List<TextRange>();
            _currentMatchIndex = -1;
        }

        public void FindAll(string searchText)
        {
            ClearHighlights();
            _foundRanges.Clear();

            if (string.IsNullOrEmpty(searchText)) return;

            var startPointer = _richTextBox.Document.ContentStart;
            while (startPointer != null && startPointer.CompareTo(_richTextBox.Document.ContentEnd) < 0)
            {
                var endPointer = startPointer.GetPositionAtOffset(searchText.Length);
                if (endPointer == null) break;

                var textRange = new TextRange(startPointer, endPointer);
                if (textRange.Text.Equals(searchText, StringComparison.CurrentCultureIgnoreCase))
                {
                    _foundRanges.Add(textRange);
                    HighlightText(textRange);
                }
                startPointer = startPointer.GetPositionAtOffset(1, LogicalDirection.Forward);
            }

            if (_foundRanges.Count > 0)
            {
                _currentMatchIndex = 0;
                SelectCurrentMatch();
            }
        }

        public void SelectNextMatch()
        {
            if (_foundRanges.Count == 0) return;

            _currentMatchIndex = (_currentMatchIndex + 1) % _foundRanges.Count;
            SelectCurrentMatch();
        }

        public void SelectPreviousMatch()
        {
            if (_foundRanges.Count == 0) return;

            _currentMatchIndex = (_currentMatchIndex - 1 + _foundRanges.Count) % _foundRanges.Count;
            SelectCurrentMatch();
        }

        public void SelectCurrentMatch()
        {
            var currentRange = _foundRanges[_currentMatchIndex];
            _richTextBox.Selection.Select(currentRange.Start, currentRange.End);
            _richTextBox.Focus();
        }

        public void HighlightText(TextRange textRange)
        {
            textRange.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Yellow);
        }

        public void ClearHighlights()
        {
            var textRange = new TextRange(_richTextBox.Document.ContentStart, _richTextBox.Document.ContentEnd);
            textRange.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Transparent);
            _foundRanges.Clear();
            _currentMatchIndex = -1;
        }
    }
}
