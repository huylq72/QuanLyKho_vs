//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuanLyKho.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class MATHANG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MATHANG()
        {
            this.CHITIETPHIEUNHAPs = new HashSet<CHITIETPHIEUNHAP>();
            this.CHITIETPHIEUXUATs = new HashSet<CHITIETPHIEUXUAT>();
        }
    
        public int Ma_MatHang { get; set; }
        public int Ma_Dvi { get; set; }
        public string Ten_MatHang { get; set; }
        public double Gia_Nhap { get; set; }
        public double Gia_Ban { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHITIETPHIEUNHAP> CHITIETPHIEUNHAPs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHITIETPHIEUXUAT> CHITIETPHIEUXUATs { get; set; }
        public virtual DONVI DONVI { get; set; }
    }
}
