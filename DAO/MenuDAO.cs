using Phan_Mem_Quan_Ly_Quan_Tra_Sua.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Phan_Mem_Quan_Ly_Quan_Tra_Sua.DAO
{
    public class MenuDAO
    {
        private static MenuDAO instance;
        public static MenuDAO Instance
        {
            get { if (instance == null) instance = new MenuDAO(); return instance; }
            private set { instance = value; }
        }
        private MenuDAO() { }

        public List<Menu> GetListMenuByTable(int id)
        {
            List<Menu> listMenu = new List<Menu>();

            string query = "select f.name, bi.count, f.price, f.price * bi.count as N'totalPrice' from BillInfo as bi, Bill as b, Food as f where bi.idBill = b.id and bi.idFood = f.id and b.status = 0 and b.idTable = " + id;
            DataTable data = DataProvider.Instance.ExecuteQuery(query);


            foreach(DataRow items in data.Rows)
            {
                Menu menu = new Menu(items);
                listMenu.Add(menu);
            }

            return listMenu;
        }
    }
}
