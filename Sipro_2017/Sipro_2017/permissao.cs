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
    
    public partial class permissao
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public permissao()
        {
            this.listagemAcessoes = new HashSet<listagemAcesso>();
        }
    
        public int id { get; set; }
        public bool escrita { get; set; }
        public Nullable<int> listagem_acessos { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<listagemAcesso> listagemAcessoes { get; set; }
    }
}
