//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sipro_2017
{
    using System;
    using System.Collections.Generic;
    
    public partial class naoConforExpedicao
    {
        public int id { get; set; }
        public string guiaExpedicao { get; set; }
        public Nullable<int> tiponaoconformidade_id { get; set; }
        public string obs { get; set; }
        public System.DateTime dataInicio { get; set; }
        public Nullable<bool> expedido { get; set; }
    
        public virtual camposControlo camposControlo { get; set; }
    }
}