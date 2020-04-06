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
    public class UnitViewModel : BaseViewModel
    {
        private ObservableCollection<DONVI> _List = new ObservableCollection<DONVI>();
        public ObservableCollection<DONVI> List { get=> _List; set { _List = value;OnPropertyChanged();}}
        private string _Loai_Dvi;
        public string Loai_Dvi { get => _Loai_Dvi; set { _Loai_Dvi = value; OnPropertyChanged(); } }
        private DONVI _SelectedItem;
        public DONVI SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    Loai_Dvi = SelectedItem.Loai_Dvi;
                }
            }
        }
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }

        public UnitViewModel()
        {
            string query = "SELECT * from DONVI";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                DONVI DONVI =  new DONVI(item);
                _List.Add(DONVI);
            }
            AddCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(Loai_Dvi))
                    return false;
                string querySearch = "select * from DONVI where Loai_Dvi = N'" + Loai_Dvi.ToString() + "'" ;
                DataTable dataSearch = DataProvider.Instance.ExecuteQuery(querySearch);
                if (dataSearch.Rows.Count > 0) return false;
                    return true;

            }, (p) =>
            {
                string queryIs = "insert into donvi(Loai_Dvi) values(N'"+Loai_Dvi.ToString()+"')";
                DataProvider.Instance.ExecuteNonQuery(queryIs);
                string lastid = "select top 1 * from DONVI ORDER BY Ma_Dvi DESC";
                DataTable dataLastId = DataProvider.Instance.ExecuteQuery(lastid);
                var dv = new DONVI((int)dataLastId.Rows[0]["Ma_Dvi"], Loai_Dvi);
                List.Add(dv);

            });
            EditCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(Loai_Dvi) || SelectedItem == null)
                    return false;
                string querySearch = "select * from DONVI where Loai_Dvi = N'" + Loai_Dvi.ToString() + "'";
                DataTable dataSearch = DataProvider.Instance.ExecuteQuery(querySearch);
                if (dataSearch.Rows.Count > 0) return false;
                return true;

            }, (p) =>
            {
                string queryUd = "Update DONVI set Loai_Dvi = N'" + Loai_Dvi.ToString() + "' where Ma_Dvi = " + SelectedItem.Ma_Dvi;
                DataProvider.Instance.ExecuteNonQuery(queryUd);
                SelectedItem.Loai_Dvi = Loai_Dvi;

            });

        }
    }
}
