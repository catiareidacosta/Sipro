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
    
    public partial class OrdemFabrico
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrdemFabrico()
        {
            this.naoConformidadesOFs = new HashSet<naoConformidadesOF>();
            this.naoConformidadesOFs1 = new HashSet<naoConformidadesOF>();
            this.ParagemProducaos = new HashSet<ParagemProducao>();
            this.ParagemProducaos1 = new HashSet<ParagemProducao>();
        }
    
        public int id { get; set; }
        public Nullable<int> produto_id { get; set; }
        public Nullable<int> numPessoas { get; set; }
        public System.DateTime dataInicio { get; set; }
        public Nullable<System.DateTime> dataFim { get; set; }
        public Nullable<decimal> racioPessoa { get; set; }
        public Nullable<int> encomenda_id { get; set; }
        public Nullable<int> fases_id { get; set; }
        public Nullable<int> numQuebras { get; set; }
        public Nullable<int> numFaltas { get; set; }
        public Nullable<int> quebras_id { get; set; }
        public Nullable<int> qtdPalteteProduto { get; set; }
        public Nullable<bool> estadoEmbalagemOk { get; set; }
        public Nullable<int> faltas_id { get; set; }
        public Nullable<decimal> qtdFabricado { get; set; }
        public Nullable<System.DateTime> dataInicioPausa { get; set; }
        public Nullable<System.DateTime> dataFimPausa { get; set; }
        public string LOTE { get; set; }
        public Nullable<decimal> tempoPausaMinutos { get; set; }
        public string estadoOF { get; set; }
        public Nullable<System.DateTime> validade { get; set; }
    
        public virtual Encomenda Encomenda { get; set; }
        public virtual Encomenda Encomenda1 { get; set; }
        public virtual FasesOrdemFabrico FasesOrdemFabrico { get; set; }
        public virtual FasesOrdemFabrico FasesOrdemFabrico1 { get; set; }
        public virtual MotivoFalta MotivoFalta { get; set; }
        public virtual MotivoFalta MotivoFalta1 { get; set; }
        public virtual MotivoQuebra MotivoQuebra { get; set; }
        public virtual MotivoQuebra MotivoQuebra1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<naoConformidadesOF> naoConformidadesOFs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<naoConformidadesOF> naoConformidadesOFs1 { get; set; }
        public virtual produto produto { get; set; }
        public virtual produto produto1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParagemProducao> ParagemProducaos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParagemProducao> ParagemProducaos1 { get; set; }
    }
}
