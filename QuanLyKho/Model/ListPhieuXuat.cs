using QuanLyKho.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace QuanLyKho.Model
{
    public class ListPhieuXuat : BaseViewModel
    {

        public int Ma_MatHang { get; set; }
        private string _Ten_MatHang { get; set; }
        public string Ten_MatHang { get => _Ten_MatHang; set { _Ten_MatHang = value; OnPropertyChanged(); } }
        private string _Ten_Don_vi { get; set; }
        public string Ten_Don_vi { get => _Ten_Don_vi; set { _Ten_Don_vi = value; OnPropertyChanged(); } }
        private int _So_Luong { get; set; }
        public int So_Luong { get => _So_Luong; set { _So_Luong = value; OnPropertyChanged(); } }
        private double _Gia_Ban { get; set; }
        public double Gia_Ban { get => _Gia_Ban; set { _Gia_Ban = value; OnPropertyChanged(); } }
     

        public ListPhieuXuat(int mmh, int sl, string tmh, string tdv,  double gb)
        {
            this.Ten_MatHang = tmh;
            this.Ma_MatHang = mmh;
            this.So_Luong = sl;
            this.Ten_Don_vi = tdv;
            this.Gia_Ban = gb;

        }
        public ListPhieuXuat(DataRow row)
        {
            this.Ten_MatHang = row["Ma_PhieuXuat"].ToString();
            this.Ma_MatHang = (int)row["Ma_MatHang"];
            this.So_Luong = (int)row["So_Luong"];
            this.Ten_Don_vi = row["Ten_Don_vi"].ToString();
            this.Gia_Ban = (double)row["Gia_Ban"];

        }

    }
}


