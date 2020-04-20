using QuanLyKho.Model;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Input;
using System.Linq;

namespace QuanLyKho.ViewModel
{
    public class ObjectViewModel : BaseViewModel
    {
        #region Properties
        private ObservableCollection<MATHANG> _listMatHang = new ObservableCollection<MATHANG>();
        public ObservableCollection<MATHANG> ListMatHang
        {
            get => _listMatHang;
            set
            {
                _listMatHang = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<DONVI> _listDonVi = new ObservableCollection<DONVI>();
        public ObservableCollection<DONVI> ListDonVi
        {
            get => _listDonVi;
            set
            {
                _listDonVi = value;
                OnPropertyChanged();
            }
        }

        public int Ma_MatHang { get; set; }

        private int _maDonVi;
        public int MaDonVi
        {
            get => _maDonVi;
            set
            {
                _maDonVi = value;
                OnPropertyChanged();
            }
        }

        private string _loaiDonVi;
        public string LoaiDonVi
        {
            get => _loaiDonVi;
            set
            {
                _loaiDonVi = value;
                OnPropertyChanged();
            }
        }

        private string _Ten_MatHang;
        public string Ten_MatHang
        {
            get => _Ten_MatHang; set
            {
                _Ten_MatHang = value;
                OnPropertyChanged();
            }
        }
        private double _Gia_Nhap;
        public double Gia_Nhap
        {
            get => _Gia_Nhap; set
            {
                _Gia_Nhap = value;
                OnPropertyChanged();
            }
        }

        private double _Gia_Ban;
        public double Gia_Ban
        {
            get => _Gia_Ban; set
            {
                _Gia_Ban = value;
                OnPropertyChanged();
            }
        }


        private int _donViIndex;

        public int DonViIndex
        {
            get { return _donViIndex; }
            set
            {
                _donViIndex = value;
                OnPropertyChanged();
            }
        }

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
                    //Ma_Dvi = SelectedItem.DONVI.Ma_Dvi;
                    DonViIndex = SelectedItem.DONVI.Ma_Dvi - 1;
                    Gia_Nhap = SelectedItem.Gia_Nhap;
                    Gia_Ban = SelectedItem.Gia_Ban;
                }
            }
        }

        #endregion

        #region Commands
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }

        #endregion



        public ObjectViewModel()
        {
            string query1 = "SELECT * from DONVI";
            DataTable data1 = DataProvider.Instance.ExecuteQuery(query1);
            foreach (DataRow item in data1.Rows)
            {
                DONVI dv = new DONVI(item);
                _listDonVi.Add(dv);
            }

            string query = "SELECT * from MATHANG";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                MATHANG MATHANG = new MATHANG(item);
                string tenDV = "";
                DONVI donVi = new DONVI();
                foreach (DONVI dv in _listDonVi)
                {
                    if (MATHANG.Ma_Dvi == dv.Ma_Dvi)
                    {
                        tenDV = dv.Loai_Dvi;
                        donVi = dv;
                        break;
                    }
                }

                MATHANG mh = new MATHANG(MATHANG.Ma_MatHang, MATHANG.Ten_MatHang, tenDV, donVi, MATHANG.Gia_Nhap, MATHANG.Gia_Ban);
                _listMatHang.Add(mh);
            }

            AddCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(Ten_MatHang) || DonViIndex < 0 || Gia_Nhap < 1 || Gia_Ban < 1)
                {
                    return false;
                }

                string querySearch = $"select * from MATHANG where Ten_MatHang = N'{Ten_MatHang}' and Ma_Dvi= {DonViIndex + 1} and Gia_Nhap= {Gia_Nhap} and Gia_Ban= {Gia_Ban}";
                return true;

            }, (p) =>
            {
                AddData();
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(Ten_MatHang) || DonViIndex < 0 || Gia_Nhap < 1 || Gia_Ban < 1 || SelectedItem == null)
                {
                    return false;
                }

                string querySearch = $"select * from MATHANG where Ten_MatHang = N'{Ten_MatHang}' and Ma_Dvi= {DonViIndex + 1} and Gia_Nhap= {Gia_Nhap} and Gia_Ban= {Gia_Ban}";
                DataTable dataSearch = DataProvider.Instance.ExecuteQuery(querySearch);
                if (dataSearch.Rows.Count > 0) return false;
                return true;

            }, (p) =>
            {
                UpdateData();
            });

            
        }

        private void AddData()
        {
            string queryIs = $"insert into MATHANG(Ten_MatHang, Ma_Dvi, Gia_Nhap,Gia_Ban) values(N'{Ten_MatHang}', {DonViIndex + 1}, {Gia_Nhap}, {Gia_Ban})";
            DataProvider.Instance.ExecuteNonQuery(queryIs);
            
            string lastId = "select top 1 * from MATHANG ORDER BY Ma_MatHang DESC";
            DataTable dataLastId = DataProvider.Instance.ExecuteQuery(lastId);

            int maDonVi = DonViIndex + 1;
            string loaiDonVi = ListDonVi.First(x => x.Ma_Dvi == (DonViIndex + 1)).Loai_Dvi;
            DONVI donVi = new DONVI { Ma_Dvi = maDonVi, Loai_Dvi = loaiDonVi };
            
            var mh = new MATHANG((int)dataLastId.Rows[0]["Ma_MatHang"], Ten_MatHang, loaiDonVi, donVi, Gia_Nhap, Gia_Ban);
            ListMatHang.Add(mh);
        }

        private void UpdateData()
        {
            string queryUd = $"Update MATHANG set Ten_MatHang = N'{Ten_MatHang}', Ma_Dvi = {DonViIndex + 1}, Gia_Nhap = {Gia_Nhap}, Gia_Ban = {Gia_Ban} where Ma_MatHang = {SelectedItem.Ma_MatHang}";
            DataProvider.Instance.ExecuteNonQuery(queryUd);


            var index = ListMatHang.ToList().FindIndex(x => x.Ma_MatHang == SelectedItem.Ma_MatHang);
            if (index >= 0)
            {
                ListMatHang[index].Ten_MatHang = Ten_MatHang;
                ListMatHang[index].Ma_Dvi = DonViIndex + 1;
                ListMatHang[index].Loai_Dvi = ListDonVi.First(x => x.Ma_Dvi == (DonViIndex + 1)).Loai_Dvi;
                ListMatHang[index].Gia_Nhap = Gia_Nhap;
                ListMatHang[index].Gia_Ban = Gia_Ban;
            }
        }
    }
}
