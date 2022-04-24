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
    public class FoodDAO
    {
        public int Food;
        private static FoodDAO instance;
        public static FoodDAO Instance
        {
            get { if (instance == null) instance = new FoodDAO(); return instance; }
            private set { instance = value; }
        }
        private FoodDAO() { }



        public List<Food> GetFoodByCategoryID(int id)
        {
            List<Food> list = new List<Food>();

            string query = "select * from Food where idCategory = " + id;

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            if (data == null)
            {
                MessageBox.Show($"Không thể kết nối dữ liệu.", "Lỗi");
            }

            foreach (DataRow items in data.Rows)
            {
                Food food = new Food(items);
                list.Add(food);
            }

            return list;
        }

        public List<Food> GetLisstFood()
        {
            List<Food> list = new List<Food>();

            string query = "select * from Food";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            if (data == null)
            {
                MessageBox.Show($"Không thể kết nối dữ liệu.", "Lỗi");
            }

            foreach (DataRow items in data.Rows)
            {
                Food food = new Food(items);
                list.Add(food);
            }

            return list;
        }

        public void DeleteFoodForByIDCategory(int id)
        {
            List<Food> listfood = GetFoodByCategoryID(id);
            foreach (Food items in listfood)
            {
                BillInfoDAO.Instance.DeleteBillInfoByFoodID(items.ID);
            }
            DeleteFoodByIDCategory(id);
        }

        public void DeleteFoodByIDCategory(int id)
        {
            DataProvider.Instance.ExecuteQuery("delete Food where idCategory = " + id);
        }

        public List<Food> SearchFoodByName(string name)
        {
            List<Food> list = new List<Food>();

            string query = string.Format("select * from Food where name like N'%{0}%'", name);

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow items in data.Rows)
            {
                Food food = new Food(items);
                list.Add(food);
            }

            return list;
        }

        public bool InsertFood(string name, int id, float price)
        {
            string query = string.Format("insert Food(name, idCategory, price) values(N'{0}', {1}, {2})", name, id, price);
            int result =  DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool UpdateFood(int idFood, string name, int id, float price)
        {
            string query = string.Format("update Food set name = N'{0}', idCategory = {1}, price = {2} where id = {3}", name, id, price, idFood);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool DeleteFood(int idFood)
        {
            BillInfoDAO.Instance.DeleteBillInfoByFoodID(idFood);
            string query = string.Format("delete Food where id = {0}", idFood);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
    }
}
