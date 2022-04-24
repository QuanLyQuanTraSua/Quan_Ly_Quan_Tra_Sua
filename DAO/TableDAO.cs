using Phan_Mem_Quan_Ly_Quan_Tra_Sua.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Phan_Mem_Quan_Ly_Quan_Tra_Sua.DAO
{
    public class TableDAO
    {
        private static TableDAO instance;
        public static int TableWidth = 90;
        public static int TableHeight = 90;

        public static TableDAO Instance
        {
            get { if (instance == null) instance = new TableDAO(); return instance; }
            private set { instance = value; }
        }
        private TableDAO() { }

        public void SwitchTable(int id1, int id2)
        {
            DataProvider.Instance.ExecuteQuery("USP_SwitchTable @idTable1 , @idTable2", new object[] {id1, id2 });
        }

        public List<Table> LoadTableList()
        {
            List<Table> tablelist = new List<Table>();

            DataTable data = DataProvider.Instance.ExecuteQuery("USP_GetTableList");

            if (data == null)
            {
                MessageBox.Show($"Không thể kết nối dữ liệu.", "Lỗi");
            }

            foreach (DataRow items in data.Rows)
            {
                Table table = new Table(items);
                tablelist.Add(table);
            }

            return tablelist;
        }

        public List<Table> GetListTable()
        {
            List<Table> list = new List<Table>();

            string query = "select * from TableFood";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            if (data == null)
            {
                MessageBox.Show($"Không thể kết nối dữ liệu.", "Lỗi");
            }

            foreach (DataRow items in data.Rows)
            {
                Table table = new Table(items);
                list.Add(table);
            }

            return list;
        }


        public bool InsertTable(string name)
        {
            string query = string.Format("insert TableFood(name) values(N'{0}')", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool UpdateTable(int idTable, string name)
        {
            string query = string.Format("update TableFood set name = N'{0}' where id = {1}", name, idTable);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
    }
}
