using Microsoft.Win32;
using OfficeOpenXml;
using QuanLyKho.Model;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace QuanLyKho.ViewModel
{
    public class ReportModel
    {
        public int Id { get; set; }
        public string TenMatHang { get; set; }
        public string DonVi { get; set; }
        public double GiaNhap { get; set; }
        public double GiaBan { get; set; }
        public int SoLuongNhap { get; set; }
        public int SoLuongBan { get; set; }
        public int SoLuongTonHienTai { get; set; }
    }

    public class ReportViewModel : BaseViewModel
    {
        private ObservableCollection<ReportModel> _listData = new ObservableCollection<ReportModel>();

        public ObservableCollection<ReportModel> ListData
        {
            get => _listData;
            set
            {
                _listData = value;
                OnPropertyChanged();
            }
        }

        private DateTime _statDate;

        public DateTime StartDate
        {
            get => _statDate;
            set
            {
                _statDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime _endDate;

        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged();
            }
        }

        public ICommand FilterCommand { get; set; }
        public ICommand ExportFileCommand { get; set; }

        public ReportViewModel()
        {
            var today = DateTime.Now;
            StartDate = new DateTime(today.Year, today.Month, 1);
            EndDate = new DateTime(today.Year, today.Month, 25);
            MappingData(GetFilterData(StartDate, EndDate));
            
            FilterCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                MappingData(GetFilterData(StartDate, EndDate));
            });

            ExportFileCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                GenerateExcelFile();
            });


        }

        private DataTable GetFilterData(DateTime startDate, DateTime endDate)
        {
            if (StartDate > DateTime.Now && StartDate >= EndDate)
            {
                MessageBox.Show("Input không hợp lệ");
                return null;
            }

            string query = $"EXECUTE dbo.GetFilterData @StartDate = '{startDate.ToString("yyyy-MM-dd")}', @EndDate = '{endDate.ToString("yyyy-MM-dd")}'";
            return DataProvider.Instance.ExecuteQuery(query);
        }

        private void MappingData(DataTable data)
        {
            if (data == null)
            {
                return;
            }

            ListData.Clear();
            foreach (DataRow row in data.Rows)
            {
                var reportObject = new ReportModel
                {
                    Id = (int)row.ItemArray[0],
                    TenMatHang = row.ItemArray[1].ToString(),
                    DonVi = row.ItemArray[2].ToString(),
                    GiaNhap = (double)row.ItemArray[3],
                    GiaBan = (double)row.ItemArray[4],
                    SoLuongNhap = (int)row.ItemArray[5],
                    SoLuongBan = (int)row.ItemArray[6],
                    SoLuongTonHienTai = (int)row.ItemArray[7],
                };
                ListData.Add(reportObject);
            }
        }



        private void GenerateExcelFile()
        {
            string filePath = "";
            // tạo SaveFileDialog để lưu file excel
            SaveFileDialog dialog = new SaveFileDialog();

            // chỉ lọc ra các file có định dạng Excel
            dialog.Filter = "Excel | *.xlsx | Excel 2013 | *.xls";

            // Nếu mở file và chọn nơi lưu file thành công sẽ lưu đường dẫn lại dùng
            if (dialog.ShowDialog() == true)
            {
                filePath = dialog.FileName;
            }
            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("Đường dẫn báo cáo không hợp lệ");
                return;
            }
            try
            {

                CreateExcelFile(filePath);
                MessageBox.Show("Xuất excel thành công!");
            }
            catch (Exception EE)
            {
                MessageBox.Show("Có lỗi khi lưu file!");
            }
        }


        private void CreateExcelFile(string filePath)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var p = new ExcelPackage())
                {
                    p.Workbook.Worksheets.Add("QuanLyKhoSheet");
                    var workSheet = p.Workbook.Worksheets[0];
                    workSheet.Name = "HangTonReport";

                    // Header
                    workSheet.Cells[1, 1].Value = "Thống kê thông tin tồn hàng từ ngày "+StartDate.ToString() + " đến ngày "+EndDate.ToString();

                    string[] arrColumnHeader = { "Mã mặt hàng", "Tên mặt hàng", "Đơn vị","Giá nhập", "Giá xuất", "SL nhập", "SL xuất", "SL tồn hiện tại" };
                    for (int i = 1; i <= arrColumnHeader.Length; i++)
                    {
                        workSheet.Cells[2, i].Value = arrColumnHeader[i - 1];
                    }


                    int rowIndex = 3;
                    foreach (var item in _listData)
                    {
                        var colIndex = 1;
                        // Excel bắt đầu từ 1 không phải từ 0
                        workSheet.Cells[rowIndex, colIndex++].Value = item.Id;
                        workSheet.Cells[rowIndex, colIndex++].Value = item.TenMatHang;
                        workSheet.Cells[rowIndex, colIndex++].Value = item.DonVi;
                        workSheet.Cells[rowIndex, colIndex++].Value = item.GiaNhap;
                        workSheet.Cells[rowIndex, colIndex++].Value = item.GiaBan;
                        workSheet.Cells[rowIndex, colIndex++].Value = item.SoLuongNhap;
                        workSheet.Cells[rowIndex, colIndex++].Value = item.SoLuongBan;
                        workSheet.Cells[rowIndex, colIndex++].Value = item.SoLuongTonHienTai;
                        rowIndex++;
                    }

                    byte[] bin = p.GetAsByteArray();
                    File.WriteAllBytes(filePath, bin);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
