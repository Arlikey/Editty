using Editty.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Editty.Models
{
    public class ListHandler : IListHandler
    {
        private RichTextBox _richTextBox;
        public ListHandler(RichTextBox richTextBox)
        {
            _richTextBox = richTextBox;
        }
        public void CreateOrderedList()
        {
            EditingCommands.ToggleNumbering.Execute(null, _richTextBox);
        }
        public void CreateUnorderedList()
        {
            EditingCommands.ToggleBullets.Execute(null, _richTextBox);
        }
    }
}
