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
    
    public partial class docQualidade
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public docQualidade()
        {
            this.funcionarios = new HashSet<funcionario>();
        }
    
        public int id { get; set; }
        public long n_modelo { get; set; }
        public string nome { get; set; }
        public Nullable<System.DateTime> data_upload { get; set; }
        public string inserir_ficheiro { get; set; }
        public System.DateTime data_inicio { get; set; }
        public Nullable<System.DateTime> data_fim { get; set; }
        public int tipo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<funcionario> funcionarios { get; set; }
        public virtual tipoDocumento tipoDocumento { get; set; }
    }
}