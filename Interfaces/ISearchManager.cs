using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Editty.Interfaces
{
    public interface ISearchManager
    {
        public void FindAll(string searchText);
        public void SelectNextMatch();
        public void SelectPreviousMatch();
        public void SelectCurrentMatch();
        public void HighlightText(TextRange textRange);
        public void ClearHighlights();

    }
}
