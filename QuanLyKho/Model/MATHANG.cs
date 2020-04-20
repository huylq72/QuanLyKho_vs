﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuanLyKho.Model
{
    using QuanLyKho.ViewModel;
    using System;
    using System.Collections.Generic;
    using System.Data;

    public partial class MATHANG : BaseViewModel
    {
        private ListPhieuXuat selectedItem;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MATHANG(int Mamh, string Tenmh, int Madv, double Gia_Nhap, double Gia_Ban)
        {
            this.Ma_MatHang = Mamh;
            this.Ten_MatHang = Tenmh;
            this.Ma_Dvi = Madv;
            this.Gia_Nhap = Gia_Nhap;
            this.Gia_Ban = Gia_Ban;
        }

        public MATHANG(int Mamh, string Tenmh, string loaidv, DONVI donVi, double Gia_Nhap, double Gia_Ban)
        {
            this.Ma_MatHang = Mamh;
            this.Ten_MatHang = Tenmh;
            this.Loai_Dvi = loaidv;
            DONVI = donVi;
            this.Gia_Nhap = Gia_Nhap;
            this.Gia_Ban = Gia_Ban;
        }


        public MATHANG(int Mamh, string Tenmh, string loaidv, double Gia_Nhap, double Gia_Ban)
        {
            this.Ma_MatHang = Mamh;
            this.Ten_MatHang = Tenmh;
            this.Loai_Dvi = loaidv;
            this.Gia_Nhap = Gia_Nhap;
            this.Gia_Ban = Gia_Ban;
        }
        public MATHANG(ListPhieuNhap ls)
        {
            this.Ten_MatHang = ls.Ten_MatHang;
            this.Ma_MatHang = ls.Ma_MatHang;
            this.Gia_Nhap = ls.Gia_Nhap;
            this.Gia_Ban = ls.Gia_Ban;

        }
        public MATHANG(ListPhieuNhap ls, int donvi)
        {
            this.Ten_MatHang = ls.Ten_MatHang;
            this.Ma_MatHang = ls.Ma_MatHang;
            this.Gia_Nhap = ls.Gia_Nhap;
            this.Gia_Ban = ls.Gia_Ban;
            this.Ma_Dvi = donvi;

        }

        public MATHANG(DataRow row)
        {
            this.Ma_MatHang = (int)row["Ma_MatHang"];
            this.Ten_MatHang = row["Ten_MatHang"].ToString();
            this.Ma_Dvi = (int)row["Ma_Dvi"];
            this.Gia_Nhap = (double)row["Gia_Nhap"];
            this.Gia_Ban = (double)row["Gia_Ban"];
        }

        public MATHANG(ListPhieuXuat selectedItem)
        {
            this.selectedItem = selectedItem;
        }

        public int Ma_MatHang { get; set; }

        private int _Ma_Dvi { get; set; }
        public int Ma_Dvi { get => _Ma_Dvi; set { _Ma_Dvi = value; OnPropertyChanged(); } }
        private string _Ten_MatHang { get; set; }
        public string Ten_MatHang { get => _Ten_MatHang; set { _Ten_MatHang = value; OnPropertyChanged(); } }

        private string _Loai_Dvi { get; set; }
        public string Loai_Dvi { get => _Loai_Dvi; set { _Loai_Dvi = value; OnPropertyChanged(); } }
        private double _Gia_Nhap { get; set; }
        public double Gia_Nhap { get => _Gia_Nhap; set { _Gia_Nhap = value; OnPropertyChanged(); } }

        private double _Gia_Ban { get; set; }
        public double Gia_Ban { get => _Gia_Ban; set { _Gia_Ban = value; OnPropertyChanged(); } }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHITIETPHIEUNHAP> CHITIETPHIEUNHAPs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHITIETPHIEUXUAT> CHITIETPHIEUXUATs { get; set; }
        public virtual DONVI DONVI { get; set; }
    }
}
