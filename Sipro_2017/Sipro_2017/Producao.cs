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
    
    public partial class Producao
    {
        public int id { get; set; }
        public int qtd_produzir { get; set; }
        public int qtd_produzida { get; set; }
        public Nullable<int> tempo_total_producao { get; set; }
        public string observacoes { get; set; }
        public bool concluido { get; set; }
        public Nullable<int> encomenda_id { get; set; }
    
        public virtual Encomenda Encomenda { get; set; }
        public virtual Encomenda Encomenda1 { get; set; }
    }
}
