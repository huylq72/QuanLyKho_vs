using QuanLyKho.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModel
{
    public class UnitViewModel : BaseViewModel
    {
        private List<DONVI> _List = new List<DONVI>();
        public List<DONVI> List { get=> _List; set { _List = value;OnPropertyChanged();}}
        public UnitViewModel()
        {
            string query = "SELECT * from DONVI";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                DONVI unit =  new DONVI(item);
                _List.Add(unit);
            }

       
        }
    }
}
