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
    
    public partial class Apuramento
    {
        public int id { get; set; }
        public Nullable<int> tempo_producaoo { get; set; }
        public System.DateTime data_encomenda { get; set; }
        public int producoes_id { get; set; }
    
        public virtual produto produto { get; set; }
    }
}