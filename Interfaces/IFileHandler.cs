﻿using Editty.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editty.Interfaces
{
    interface IFileHandler
    {
        public Task OpenFileAsync(object parameter, TextDocument document);
    }
}
