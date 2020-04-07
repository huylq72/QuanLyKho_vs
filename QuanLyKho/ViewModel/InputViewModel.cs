using Microsoft.Reporting.WinForms;
using QuanLyKho.Model;
using QuanLyKho.Winform;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace QuanLyKho.ViewModel
{
    public class InputViewModel : BaseViewModel
    {

        private ObservableCollection<MATHANG> _Object;
        public ObservableCollection<MATHANG> Object { get => _Object; set { _Object = value; OnPropertyChanged(); } }
        private ObservableCollection<ListPhieuNhap> _List;
        public ObservableCollection<ListPhieuNhap> List { get => _List; set { _List = value; OnPropertyChanged(); } }
        private ObservableCollection<NHACUNGCAP> _ListNCC;
        public ObservableCollection<NHACUNGCAP> ListNCC { get => _ListNCC; set { _ListNCC = value; OnPropertyChanged(); } }

        private int _Ma_MatHang;
        public int Ma_MatHang { get => _Ma_MatHang; set { _Ma_MatHang = value; OnPropertyChanged(); } }
        private string _Ten_MatHang;
        public string Ten_MatHang { get => _Ten_MatHang; set { _Ten_MatHang = value; OnPropertyChanged(); } }
        private int _So_Luong;
        public int So_Luong { get => _So_Luong; set { _So_Luong = value; OnPropertyChanged(); } }

        private MATHANG _SelectedObject;
        public MATHANG SelectedObject { get => _SelectedObject; set { _SelectedObject = value; OnPropertyChanged(); } }
        private NHACUNGCAP _SelectedNCC;
        public NHACUNGCAP SelectedNCC { get => _SelectedNCC; set { _SelectedNCC = value; OnPropertyChanged(); } }
        private ListPhieuNhap _SelectedItem;
        public ListPhieuNhap SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    MATHANG mh = new MATHANG(SelectedItem, getMadv(List_DV,SelectedItem.Ten_Don_vi));
                    SelectedObject = mh;
                    So_Luong = SelectedItem.So_Luong;
                }
            }
        }
        private DateTime _DateInput = DateTime.Now;
        public DateTime DateInput { get => _DateInput; set { _DateInput = value; OnPropertyChanged(); } }
        private ObservableCollection<DONVI> _List_DV = new ObservableCollection<DONVI>();
        public ObservableCollection<DONVI> List_DV { get => _List_DV; set { _List_DV = value; OnPropertyChanged(); } }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand AddInput { get; set; }
        public InputViewModel() {
            _Object = new ObservableCollection<MATHANG>();
            _List = new ObservableCollection<ListPhieuNhap>();
            _ListNCC = new ObservableCollection<NHACUNGCAP>();
            string query = "SELECT * from MATHANG";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                MATHANG mATHANG = new MATHANG(item);
                _Object.Add(mATHANG);
            }
            string query1 = "SELECT * from NHACUNGCAP";
            DataTable data1 = DataProvider.Instance.ExecuteQuery(query1);
            foreach (DataRow item in data1.Rows)
            {
                NHACUNGCAP mATHANG = new NHACUNGCAP(item);
                _ListNCC.Add(mATHANG);
            }
            string query2 = "SELECT * from DONVI";
            DataTable data2 = DataProvider.Instance.ExecuteQuery(query2);
            foreach (DataRow item in data2.Rows)
            {
                DONVI mATHANG = new DONVI(item);
                List_DV.Add(mATHANG);
            }



            AddCommand = new RelayCommand<object>((p) =>
            {
                if (So_Luong <= 0)
                    return false;
                return true;

            }, (p) =>
            {

                foreach(ListPhieuNhap list in _List)
                {
                    if(list.Ma_MatHang == SelectedObject.Ma_MatHang)
                    {
                        list.So_Luong = list.So_Luong + So_Luong;
                        return;
                    }
                }
                ListPhieuNhap listPhieuNhap = new ListPhieuNhap(SelectedObject.Ma_MatHang, So_Luong, SelectedObject.Ten_MatHang, getTendv(List_DV,SelectedObject.Ma_MatHang), SelectedObject.Gia_Nhap, SelectedObject.Gia_Ban);
                _List.Add(listPhieuNhap);

            });
            AddInput = new RelayCommand<object>((p) =>
            {
                if (_List.Count <= 0 || SelectedNCC == null)
                    return false;
                return true;

            }, (p) =>
            {
                //DialogResult result = System.Windows.MessageBox.Show("Bạn có muốn nhập hàng vào kho?", "Phần mềm quản lý kho", MessageBoxButton.YesNoCancel);
                if (System.Windows.Forms.MessageBox.Show("Bạn có muốn nhập hàng vào kho?", "Phần mềm quản lý kho", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string insert = "INSERT INTO Dbo.PHIEUNHAP (Ma_NCC,Ma_NV,Thoi_Gian) values";
                    string data123 = "(" + SelectedNCC.Ma_NCC + ",2,'" + DateInput.ToString() + "')";
                    DataProvider.Instance.ExecuteNonQuery(insert + data123);
                    DataTable topId = DataProvider.Instance.ExecuteQuery("select top 1 * from PHIEUNHAP ORDER BY Ma_PhieuNhap DESC");
                    string inCTPN = "INSERT INTO CHITIETPHIEUNHAP VALUES";
                    List<string> words = new List<string>();
                    foreach (ListPhieuNhap list in _List)
                    {
                        words.Add("("+ (int)topId.Rows[0]["Ma_PhieuNhap"] +","+list.Ma_MatHang+","+list.So_Luong+")");
 
                    }
                    DataProvider.Instance.ExecuteNonQuery(inCTPN + string.Join(",", words.ToArray()));
                    RP report = new RP((int)topId.Rows[0]["Ma_PhieuNhap"], SelectedNCC.Ten_NCC,DateInput);
                    

                }
                else
                {
                    return;
                }
                   
                
               



            });
            DeleteCommand = new RelayCommand<object>((p) =>
            {
                if (_List.Count <= 0 || SelectedItem == null)
                    return false;
                return true;

            }, (p) =>
            {
                RemoveItem(_List, SelectedItem);

                //ListPhieuNhap listPhieuNhap = new ListPhieuNhap(SelectedObject.Ma_MatHang, So_Luong, SelectedObject.Ten_MatHang);
                //_List.Add(listPhieuNhap);

            });



        }
        private string getTendv(ObservableCollection<DONVI> list ,int id)
        {
            foreach (DONVI temp in list)
            {
                if (temp.Ma_Dvi == id)
                {
                    
                    return temp.Loai_Dvi;
                }
            }
            return null;
        }
        private int getMadv(ObservableCollection<DONVI> list, string id)
        {
            foreach (DONVI temp in list)
            {
                if (temp.Loai_Dvi == id)
                {

                    return temp.Ma_Dvi;
                }
            }
            return 0;
        }
        public void RemoveItem(ObservableCollection<ListPhieuNhap> collection, ListPhieuNhap instance)
        {
            collection.Remove(collection.Where(i => i.Ma_MatHang == instance.Ma_MatHang).Single());
        }


    }
    
    
}
