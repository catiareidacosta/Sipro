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
    
    public partial class ParagemProducao
    {
        public int id { get; set; }
        public Nullable<int> OF_id { get; set; }
        public Nullable<long> qtdProduzida { get; set; }
        public Nullable<int> motivoParagem_id { get; set; }
    
        public virtual FimOperaco FimOperaco { get; set; }
        public virtual OrdemFabrico OrdemFabrico { get; set; }
        public virtual OrdemFabrico OrdemFabrico1 { get; set; }
    }
}