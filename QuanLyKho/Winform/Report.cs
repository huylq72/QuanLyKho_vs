using Microsoft.Reporting.WinForms;
using QuanLyKho.Model;
using Syncfusion.Windows.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyKho.Winform
{
    public partial class Report : Form
    {
        public Report()
        {
            InitializeComponent();
        



        }
        public Report(int id)
        {
            InitializeComponent();
            string sql = "SELECT MATHANG.Ten_MatHang, MATHANG.Ma_MatHang, DONVI.Loai_Dvi, CHITIETPHIEUNHAP.So_Luong, MATHANG.Gia_Nhap, (CHITIETPHIEUNHAP.So_Luong *MATHANG.Gia_Nhap ) thanhtien FROM DONVI INNER JOIN MATHANG ON DONVI.Ma_Dvi = MATHANG.Ma_Dvi INNER JOIN  CHITIETPHIEUNHAP ON MATHANG.Ma_MatHang = CHITIETPHIEUNHAP.Ma_MatHang INNER JOIN  PHIEUNHAP ON CHITIETPHIEUNHAP.Ma_PhieuNhap = PHIEUNHAP.Ma_PhieuNhap where PHIEUNHAP.Ma_PhieuNhap = " + id;
            DataTable data = DataProvider.Instance.ExecuteQuery(sql);
            Microsoft.Reporting.WinForms.ReportDataSource rds = new Microsoft.Reporting.WinForms.ReportDataSource();
            rds.Value = data;
            rds.Name = "PHIEUNHAP";
            this.reportViewer2.LocalReport.DataSources.Clear();
            this.reportViewer2.LocalReport.DataSources.Add(rds);
            this.reportViewer2.LocalReport.Refresh();
            this.reportViewer2.RefreshReport();

        }

        private void elementHost2_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }

        private void Report_Load(object sender, EventArgs e)
        {

            this.reportViewer2.RefreshReport();
        }
    }
    public class RP { 
        public RP(int id)
        {
            Report report = new Report(id);
            report.Show();
        }

    }
}
