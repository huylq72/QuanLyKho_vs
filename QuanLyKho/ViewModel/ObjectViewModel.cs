using QuanLyKho.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuanLyKho.ViewModel
{
    public class ObjectViewModel : BaseViewModel
    {
        private ObservableCollection<MATHANG> _List = new ObservableCollection<MATHANG>();
        public ObservableCollection<MATHANG> List { get => _List; set { _List = value; OnPropertyChanged(); } }
        public int Ma_MatHang { get; set; }


       
        private int _Ma_Dvi { get; set; }
        public int Ma_Dvi { get => _Ma_Dvi; set { _Ma_Dvi = value; OnPropertyChanged(); } }
        private string _Ten_MatHang { get; set; }
        public string Ten_MatHang { get => _Ten_MatHang; set { _Ten_MatHang = value; OnPropertyChanged(); } }
        private double _Gia_Nhap { get; set; }
        public double Gia_Nhap { get => _Gia_Nhap; set { _Gia_Nhap = value; OnPropertyChanged(); } }

        private double _Gia_Ban { get; set; }
        public double Gia_Ban { get => _Gia_Ban; set { _Gia_Ban = value; OnPropertyChanged(); } }

        private MATHANG _SelectedItem;
        public MATHANG SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    Ten_MatHang = SelectedItem.Ten_MatHang;
                    Ma_Dvi = SelectedItem.Ma_Dvi;
                    Gia_Nhap = SelectedItem.Gia_Nhap;
                    Gia_Ban = SelectedItem.Gia_Ban;
                }
            }
        }
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        
        public ObjectViewModel()
        {
            
            string query = "SELECT * from MATHANG";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                MATHANG MATHANG = new MATHANG(item);
                _List.Add(MATHANG);
            }
           
            AddCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(Ten_MatHang) || Ma_Dvi<1 || Gia_Nhap<1 || Gia_Ban<1)
                    return false;
                string querySearch = "select * from MATHANG where Ten_MatHang = N'" + Ten_MatHang.ToString() + "' and Ma_Dvi= " + Ma_Dvi + " and Gia_Nhap= " +Gia_Nhap + " and Gia_Ban= " + Gia_Ban + "";
                return true;

            }, (p) =>
            {
                string queryIs = "insert into MATHANG(Ten_MatHang, Ma_Dvi, Gia_Nhap,Gia_Ban) values(N'" + Ten_MatHang.ToString() + "', " + Ma_Dvi + ", " + Gia_Nhap + ", " +Gia_Ban + ")";
                DataProvider.Instance.ExecuteNonQuery(queryIs);
                string lastid = "select top 1 * from MATHANG ORDER BY Ma_MatHang DESC";
                DataTable dataLastId = DataProvider.Instance.ExecuteQuery(lastid);
                var mh = new MATHANG((int)dataLastId.Rows[0]["Ma_MatHang"], Ten_MatHang, (int)Ma_Dvi, (double)Gia_Nhap, (double)Gia_Ban);
                List.Add(mh);

            });
            EditCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(Ten_MatHang) || Ma_Dvi < 1 || Gia_Nhap < 1 || Gia_Ban < 1 || SelectedItem == null)
                    return false;
                string querySearch = "select * from MATHANG where Ten_MatHang = N'" + Ten_MatHang.ToString() + "' and Ma_Dvi= " + Ma_Dvi + " and Gia_Nhap= " + Gia_Nhap + " and Gia_Ban= " + Gia_Ban + "";
                DataTable dataSearch = DataProvider.Instance.ExecuteQuery(querySearch);
                if (dataSearch.Rows.Count > 0) return false;
                return true;

            }, (p) =>
            {
                string queryUd = "Update MAT_HANG set Ten_MatHang = N'" + Ten_MatHang.ToString() + "', Ma_Dvi = " +Ma_Dvi + ", Gia_Nhap = " + Gia_Nhap + ", Gia_Ban = " + Gia_Ban + " where Ma_MatHang = " + SelectedItem.Ma_MatHang;
                DataProvider.Instance.ExecuteNonQuery(queryUd);
                SelectedItem.Ten_MatHang = Ten_MatHang;
                SelectedItem.Ma_Dvi = Ma_Dvi;
                SelectedItem.Gia_Nhap = Gia_Nhap;
                SelectedItem.Gia_Ban = Gia_Ban;

            });

        }
    }
}
