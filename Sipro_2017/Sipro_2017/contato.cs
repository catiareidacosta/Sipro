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
    
    public partial class contato
    {
        public int id { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string telf { get; set; }
        public Nullable<int> telm { get; set; }
        public string cargo { get; set; }
        public Nullable<int> contato_morada { get; set; }
    
        public virtual morada morada { get; set; }
    }
}