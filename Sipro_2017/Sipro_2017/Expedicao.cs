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
    
    public partial class Expedicao
    {
        public System.Guid id { get; set; }
        public string numeroGuia { get; set; }
        public Nullable<int> id_camposControl { get; set; }
        public Nullable<int> camposControl_id { get; set; }
        public string matricula { get; set; }
        public string cmr { get; set; }
        public int morada { get; set; }
        public Nullable<System.DateTime> dataExpedicao { get; set; }
    
        public virtual camposControlo camposControlo { get; set; }
        public virtual camposControlo camposControlo1 { get; set; }
        public virtual camposControlo camposControlo2 { get; set; }
        public virtual camposControlo camposControlo3 { get; set; }
        public virtual morada morada1 { get; set; }
        public virtual morada morada2 { get; set; }
    }
}
