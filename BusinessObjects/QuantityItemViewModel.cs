﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItalianPizza.BusinessObjects
{
    public class QuantityItemViewModel
    {
        public int IdItem { get; set; }

        public string Cantidad { get; set; }

        public string Nombre { get; set; }

        public string Precio { get; set; }

        public int IdOrder { get; set; }
    }
}
