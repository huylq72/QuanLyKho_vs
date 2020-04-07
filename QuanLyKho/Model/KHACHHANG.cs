
namespace QuanLyKho.Model
{
    using QuanLyKho.ViewModel;
    using System;
    using System.Collections.Generic;
    using System.Data;

    public partial class KHACHHANG : BaseViewModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KHACHHANG(int Ma_KH, string Ten_KH, string Dia_Chi, string Dienthoai, string Email)
        {
            this.Ma_KH = Ma_KH;
            this.Ten_KH = Ten_KH;
            this.Dia_Chi = Dia_Chi;
            this.Dienthoai = Dienthoai;
            this.Email = Email;
        }
        public KHACHHANG(DataRow row)
        {
            this.Ma_KH = (int)row["Ma_KH"];
            this.Ten_KH = row["Ten_KH"].ToString();
            this.Dia_Chi = row["Dia_Chi"].ToString();
            this.Dienthoai = row["Dienthoai"].ToString();
            this.Email = row["Email"].ToString();

        }
        public int Ma_KH { get; set; }
        private string _Ten_KH { get; set; }
        public string Ten_KH { get => _Ten_KH; set { _Ten_KH = value; OnPropertyChanged(); } }

        private string _Dia_Chi { get; set; }
        public string Dia_Chi { get => _Dia_Chi; set { _Dia_Chi = value; OnPropertyChanged(); } }

        private string _Dienthoai { get; set; }
        public string Dienthoai { get => _Dienthoai; set { _Dienthoai = value; OnPropertyChanged(); } }

        private string _Email { get; set; }
        public string Email { get => _Email; set { _Email = value; OnPropertyChanged(); } }
    }
}
