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
    
    public partial class naoConformidadesOF
    {
        public int id { get; set; }
        public Nullable<int> ordemFabrico_id { get; set; }
        public Nullable<int> ref_id { get; set; }
        public Nullable<int> gtin_id { get; set; }
        public string Motivo { get; set; }
        public Nullable<int> tipo_id { get; set; }
        public string resolucao { get; set; }
    
        public virtual TipoNaoConformidade TipoNaoConformidade { get; set; }
        public virtual produto produto { get; set; }
        public virtual gtinRecebido gtinRecebido { get; set; }
        public virtual OrdemFabrico OrdemFabrico { get; set; }
        public virtual OrdemFabrico OrdemFabrico1 { get; set; }
    }
}
