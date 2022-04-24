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
    public class BillDAO
    {
        private static BillDAO instance;
        public static BillDAO Instance
        {
            get { if (instance == null) instance = new BillDAO(); return instance; }
            private set { instance = value; }
        }
        private BillDAO() { }

        public int GetUnCheckBillIDByTableID(int id)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("select * from Bill where idTable = " + id + " and status = 0");

            if (data == null)
            {
                MessageBox.Show($"Không thể kết nối dữ liệu.", "Lỗi");
            }

            if (data.Rows.Count > 0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.ID;
            }

            return -1;
        }

        public bool GetCheckCountBillByFoodID(int id)
        {
            string query = string.Format("select * from Food as f, BillInfo as bi, Bill as b where {0} = bi.idFood and  bi.idBill = b.id and b.status = 0", id);
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            if (data == null)
            {
                MessageBox.Show($"Không thể kết nối dữ liệu.", "Lỗi");
            }

            if (data.Rows.Count > 0)
            {
                return true;
            }

            return false;
        }

        public bool GetCheckCountBillByCategoryID(int id)
        {
            string query = string.Format("select * from FoodCategory as fc, Bill as b, BillInfo as bi, Food as f where {0} = f.idCategory and f.id = bi.idFood and bi.idBill = b.id and b.status = 0", id);
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            if (data == null)
            {
                MessageBox.Show($"Không thể kết nối dữ liệu.", "Lỗi");
            }

            if (data.Rows.Count > 0)
            {
                return true;
            }

            return false;
        }

        public bool GetCheckCountBillByTableID(int id)
        {
            string query = string.Format("select * from TableFood as t, Bill as b, BillInfo as bi where {0} = b.idTable and b.id = bi.idBill and b.status = 0", id);
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            if (data == null)
            {
                MessageBox.Show($"Không thể kết nối dữ liệu.", "Lỗi");
            }

            if (data.Rows.Count > 0)
            {
                return true;
            }

            return false;
        }
        public void CheckOut(int id, int discount, float totalPrice)
        {
            string query = "update Bill set DateCheckOut = GETDATE(), status = 1, discount = " + discount + ", totalPrice =  " + totalPrice + "  where id = " + id;

            DataProvider.Instance.ExecuteNonQuery(query);
        }

        public List<Bill> GetBillByIDTable(int id)
        {
            List<Bill> list = new List<Bill>();

            string query = "select * from Bill where status = 0 and idTable = " + id;

            DataTable data = DataProvider.Instance.ExecuteQuery(query);


            if (data == null)
            {
                MessageBox.Show($"Không thể kết nối dữ liệu.", "Lỗi");
            }

            foreach (DataRow items in data.Rows)
            {
                Bill food = new Bill(items);
                list.Add(food);
            }

            return list;
        }

        public DataTable GetBillListByDate(DateTime checkIn, DateTime checkOut)
        {
            return DataProvider.Instance.ExecuteQuery("exec USP_getListBillByDate @checkIn , @checkOut", new object[] { checkIn, checkOut});
        }

        public void InsertBill(int id)
        {
            DataProvider.Instance.ExecuteNonQuery("exec USP_InsertBill @idTable ", new object[] { id });
        }

        public int GetMaxIDBill()
        {
            try
            {
                return (int)DataProvider.Instance.ExecuteScalar("select MAX(id) from Bill");
            }
            catch
            {
                return 1;
            } 
        }

    }
}
