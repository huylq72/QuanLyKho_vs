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
    public class CustomerViewModel : BaseViewModel
    {
        private ObservableCollection<KHACHHANG> _List = new ObservableCollection<KHACHHANG>();
        public ObservableCollection<KHACHHANG> List { get => _List; set { _List = value; OnPropertyChanged(); } }
        private string _Ten_KH;
        public string Ten_KH { get => _Ten_KH; set { _Ten_KH = value; OnPropertyChanged(); } }

        private string _Dia_Chi;
        public string Dia_Chi { get => _Dia_Chi; set { _Dia_Chi = value; OnPropertyChanged(); } }

        private string _Dienthoai;
        public string Dienthoai { get => _Dienthoai; set { _Dienthoai = value; OnPropertyChanged(); } }

        private string _Email;
        public string Email { get => _Email; set { _Email = value; OnPropertyChanged(); } }

        private KHACHHANG _SelectedItem;
        public KHACHHANG SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    Ten_KH = SelectedItem.Ten_KH;
                    Dia_Chi = SelectedItem.Dia_Chi;
                    Dienthoai = SelectedItem.Dienthoai;
                    Email = SelectedItem.Email;
                }
            }
        }
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }

        public CustomerViewModel()
        {
            string query = "SELECT * from KHACHHANG";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                KHACHHANG KHACHHANG = new KHACHHANG(item);
                _List.Add(KHACHHANG);
            }
            AddCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(Ten_KH)|| string.IsNullOrEmpty(Dia_Chi)|| string.IsNullOrEmpty(Dienthoai)|| string.IsNullOrEmpty(Email))
                    return false;
                string querySearch = "select * from KHACHHANG where Ten_KH = N'" + Ten_KH.ToString() + "' and Dia_Chi= N'" + Dia_Chi.ToString() + "' and Dienthoai= " + Dienthoai.ToString() + " and Email= N'" + Email.ToString() + "'";
                return true;

            }, (p) =>
            {
                string queryIs = "insert into KHACHHANG(Ten_KH, Dia_Chi, Dienthoai,Email) values(N'" + Ten_KH.ToString() + "', N'"+Dia_Chi.ToString()+"', "+Dienthoai.ToString()+", N'"+Email.ToString()+"')";
                DataProvider.Instance.ExecuteNonQuery(queryIs);
                string lastid = "select top 1 * from KHACHHANG ORDER BY Ma_KH DESC";
                DataTable dataLastId = DataProvider.Instance.ExecuteQuery(lastid);
                var kh = new KHACHHANG((int)dataLastId.Rows[0]["Ma_KH"], Ten_KH, Dia_Chi,Dienthoai,Email);
                List.Add(kh);

            });
            EditCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(Ten_KH)|| string.IsNullOrEmpty(Dia_Chi)|| string.IsNullOrEmpty(Dienthoai)|| string.IsNullOrEmpty(Email) || SelectedItem == null)
                    return false;
                string querySearch = "select * from KHACHHANG where Ten_KH = N'" + Ten_KH.ToString() + "' and Dia_Chi= N'" + Dia_Chi.ToString() + "' and Dienthoai= " + Dienthoai.ToString() + " and Email= N'" + Email.ToString() + "'";
                DataTable dataSearch = DataProvider.Instance.ExecuteQuery(querySearch);
                if (dataSearch.Rows.Count > 0) return false;
                return true;

            }, (p) =>
            {
                string queryUd = "Update KHACHHANG set Ten_KH = N'" + Ten_KH.ToString() + "', Dia_Chi = N'" + Dia_Chi.ToString() + "', DienThoai = " + Dienthoai.ToString() + ", Email = N'" + Email.ToString() + "' where Ma_KH = " + SelectedItem.Ma_KH;
                DataProvider.Instance.ExecuteNonQuery(queryUd);
                SelectedItem.Ten_KH = Ten_KH;
                SelectedItem.Dia_Chi = Dia_Chi;
                SelectedItem.Dienthoai = Dienthoai;
                SelectedItem.Email = Email;

            });

        }
    }
}
