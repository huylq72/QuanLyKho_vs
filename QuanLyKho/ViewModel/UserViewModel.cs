using QuanLyKho.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLyKho.ViewModel
{
    public class UserViewModel : BaseViewModel
    {
   
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ChangePasswordCommand { get; set; }
        public ICommand BaoCaoCommand { get; set; }

        //tab Them
        public ICommand ThemCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }    
        public ICommand HuyCommand { get; set; }

        //tab change
        public ICommand ThayDoiCommand { get; set; }
        public ICommand ThoatCommand { get; set; }
        public ICommand PassWChangedCommand { get; set; }
        public ICommand NewPasswordChangedCommand { get; set; }
        public ICommand RepeatNewPasswordChangedCommand { get; set; }



        private CHUCVU chuc;

        private string _Role;
        public string Role { get => _Role; set { _Role = value; OnPropertyChanged(); } }

        private List<NHANVIEN> _List = new List<NHANVIEN>();
        public List<NHANVIEN> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private List<CHUCVU> _ListCV = new List<CHUCVU>();
        public List<CHUCVU> ListCV { get => _ListCV; set { _ListCV = value; OnPropertyChanged(); } }

        private NHANVIEN _SelectedItem;
        public NHANVIEN SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    Ten_DN = SelectedItem.Ten_DN;
                    Ten_NV = SelectedItem.Ten_NV;
                    Ngay_Sinh = SelectedItem.Ngay_Sinh;
                    Dia_Chi = SelectedItem.Dia_Chi;
                    Role = quyen(SelectedItem.CHUCVU.Ten_Chuc_Vu);
                    SelectedCV = SelectedItem.CHUCVU;

                }
            }
        }

        private CHUCVU _SelectedCV;
        public CHUCVU SelectedCV { get => _SelectedCV; set{_SelectedCV = value; OnPropertyChanged();}}

        
        public UserViewModel()
        {
          
            string query1 = "SELECT * from CHUCVU";
            DataTable data1 = DataProvider.Instance.ExecuteQuery(query1);
            foreach (DataRow item in data1.Rows)
            {
                CHUCVU cv = new CHUCVU(item);
                _ListCV.Add(cv);
            }

            setList();

            chuc = Chuc(LoginViewModel.User);

            // tab thêm nhân viên
            AddCommand = new RelayCommand<object>((p) => {
                if (!(chuc.Ten_Chuc_Vu.Equals("Quản Lý Kho") || chuc.Ten_Chuc_Vu.Equals("Giám Sát Kho")))
                    return false;
                return true; }, 
                (p) => { AddUserWindow au = new AddUserWindow(); au.ShowDialog(); });

            ThemCommand = new RelayCommand<object>((p) => { return true;},
                (p) => {

                    string str = "EXEC THEMNV @MaCV , @TenNV , @NgaySinh , @DiaChi , @TenDN , @Mk ";
                    int c = DataProvider.Instance.ExecuteNonQuery(str, new object[] { SelectedCV.Ma_Chuc_Vu, Ten_NV, Ngay_Sinh, Dia_Chi , Ten_DN , Password });
                    if (c != 0)
                        MessageBox.Show("Đã thêm thành công !");
                    else
                        MessageBox.Show("Thêm thất bại !");

                });
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });
            HuyCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { p.Close(); });

            // tab thay đổi mật khẩu

            ChangePasswordCommand = new RelayCommand<object>((p) => { return true; }, (p) => { ChangePasswordWindow cpw = new ChangePasswordWindow(); cpw.ShowDialog(); });

            ThayDoiCommand = new RelayCommand<object>((p) => {
                if (String.IsNullOrEmpty(PassW) || String.IsNullOrEmpty(NewPassword) || String.IsNullOrEmpty(RepeatNewPassword))
                    return false;
                return true; },
                (p) => {

                    if (!PassW.Equals(LoginViewModel.Pass))
                        MessageBox.Show("Mật khẩu cũ không đúng !");
                    else if (!NewPassword.Equals(RepeatNewPassword))
                        MessageBox.Show("Mật khẩu xác nhận không chính xác !");
                    else
                    {
                        string str = "EXEC CHANGEPASS @TENDN , @MK ";
                        int c = DataProvider.Instance.ExecuteNonQuery(str, new object[] {LoginViewModel.User , NewPassword });
                        if (c != 0)
                            MessageBox.Show("Thay đổi thành công!");
                        else
                            MessageBox.Show("Thay đổi thất bại !");
                    }                   
                });

            PassWChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { PassW = p.Password; });
            NewPasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { NewPassword = p.Password; });
            RepeatNewPasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { RepeatNewPassword = p.Password; });

            ThoatCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { p.Close(); });


            //tab xóa nhân viên

            DeleteCommand = new RelayCommand<object>((p) => {

                if (!(chuc.Ten_Chuc_Vu.Equals("Quản Lý Kho") || chuc.Ten_Chuc_Vu.Equals("Giám Sát Kho")))
                    return false;
                if ( String.IsNullOrEmpty(Ten_DN))
                    return false;
                return true;
            }, (p) =>
            {
                
                string str = "EXEC XOANV @Ten_DN ";
                int c = DataProvider.Instance.ExecuteNonQuery(str, new object[] {SelectedItem.Ten_DN });
               
                if (c != 0)
                {
                    foreach (NHANVIEN nv in _List)
                        if (SelectedItem.Ten_DN.Equals(nv.Ten_DN))
                        {
                            _List.Remove(nv);
                            break;
                        }
                    MessageBox.Show("Xóa thành công !");
                }                 
                else
                    MessageBox.Show("Xóa thất bại");

            });


            // sửa nhân viên
            EditCommand = new RelayCommand<object>((p) => {
                if (!(chuc.Ten_Chuc_Vu.Equals("Quản Lý Kho") || chuc.Ten_Chuc_Vu.Equals("Giám Sát Kho")))
                    return false;
                if (SelectedItem == null)
                    return false;
              
                return true;
            }, (p) =>
            {
                int c = 0;
                foreach (NHANVIEN nv in List)
                {
                    if (SelectedItem.Ma_NV == nv.Ma_NV)
                        continue;
                    if (Ten_DN.Equals(nv.Ten_DN))
                        c++;
                }

                if (c == 0)
                {
                    string str = "EXEC UPDATE_NV @Ma_NV , @Ma_CV , @Ten_NV , @Ngay_Sinh , @Dia_Chi , @Ten_DN , @Mk ";
                    int d = DataProvider.Instance.ExecuteNonQuery(str, new object[] { SelectedItem.Ma_NV, SelectedCV.Ma_Chuc_Vu, Ten_NV, Ngay_Sinh, Dia_Chi, Ten_DN, SelectedItem.MK });

                    if (d != 0)
                    {
                        MessageBox.Show("Sửa thành công");
                        List.Clear();
                        setList();
                    }
                    else
                        MessageBox.Show("Sửa thất bại");
                }
                else
                {
                    MessageBox.Show("Đã tồn tại tên đăng nhập");
                }


            });

            // Báo cáo
            BaoCaoCommand = new RelayCommand<object>((p) => {
                if (!(chuc.Ten_Chuc_Vu.Equals("Quản Lý Kho") || chuc.Ten_Chuc_Vu.Equals("Giám Sát Kho")))
                    return false;
                return true;
            }, (p) => {
                FormReport fr = new FormReport();
                string str = "select Ma_NV , Ten_DN , MK, Ten_Chuc_Vu , Ten_NV , Ngay_Sinh , Dia_Chi from NHANVIEN, CHUCVU where NHANVIEN.Ma_Chuc_Vu = CHUCVU.Ma_Chuc_Vu";
                DataTable dt = DataProvider.Instance.ExecuteQuery(str);
                fr.dataGridView.DataSource = dt;

                fr.ShowDialog();
            }
            );
        }

        private string quyen(string tencv)
        {
            string quyen = "";

            if (tencv.Equals("Quản Lý Kho") || tencv.Equals("Giám Sát Kho"))
                quyen = "Admin";
            else
                quyen = "Member";
            return quyen;
        }

        private CHUCVU Chuc(string tenUser)
        {
            CHUCVU cv = new CHUCVU();
            foreach (NHANVIEN nv in List)
                if(tenUser.Equals(nv.Ten_DN))
                {
                    cv = nv.CHUCVU;
                    break;
                }                 
            return cv;
        }

        void setList()
        {
            string query = "SELECT * from NHANVIEN";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                NHANVIEN nv = new NHANVIEN(item);
                CHUCVU cv = new CHUCVU();
                foreach (CHUCVU chucvu in _ListCV)
                {
                    if (nv.Ma_CV == chucvu.Ma_Chuc_Vu)
                    {
                        cv = chucvu;
                        break;
                    }
                }
                NHANVIEN nv1 = new NHANVIEN(nv.Ma_NV, cv, nv.Ten_NV, nv.Ngay_Sinh, nv.Dia_Chi, nv.Ten_DN, nv.MK);
                _List.Add(nv1);
            }

        }

        private string _Ten_DN;
        public string Ten_DN { get => _Ten_DN; set { _Ten_DN = value; OnPropertyChanged(); } }

        private string _Password;
        public string Password { get => _Password; set { _Password = value; OnPropertyChanged(); } }

        private string _PassW;
        public string PassW { get => _PassW; set { _PassW = value; OnPropertyChanged(); } }

        private string _NewPassword;
        public string NewPassword { get => _NewPassword; set { _NewPassword = value; OnPropertyChanged(); } }

        private string _RepeatNewPassword;
        public string RepeatNewPassword { get => _RepeatNewPassword; set { _RepeatNewPassword = value; OnPropertyChanged(); } }

        private string _Ten_NV;
        public string Ten_NV { get => _Ten_NV; set { _Ten_NV = value; OnPropertyChanged(); } }

        private DateTime _Ngay_Sinh;
        public DateTime Ngay_Sinh { get => _Ngay_Sinh; set { _Ngay_Sinh = value; OnPropertyChanged(); } }

        private string _Dia_Chi;
        public string Dia_Chi { get => _Dia_Chi; set { _Dia_Chi = value; OnPropertyChanged(); } }
        private CHUCVU _CHUCVU;
        public virtual CHUCVU CHUCVU { get => _CHUCVU; set { _CHUCVU = value; OnPropertyChanged(); } }
    }
}
