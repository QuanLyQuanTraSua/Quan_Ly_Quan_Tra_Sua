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
    public class CategoryDAO
    {
        private static CategoryDAO instance;
        public static CategoryDAO Instance
        {
            get { if (instance == null) instance = new CategoryDAO(); return instance; }
            private set { instance = value; }
        }
        private CategoryDAO() { }
        public List<Category> GetListCategory()
        {
            List<Category> list = new List<Category>();

            string query = "select * from FoodCategory";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            if(data == null)
            {
                MessageBox.Show($"Không thể kết nối dữ liệu.", "Lỗi");
            }

            foreach(DataRow items in data.Rows)
            {
                Category category = new Category(items);
                list.Add(category);
            }

            return list;
        }

        public Category GetCategoryByID(int id)
        {
            Category category = null;

            string query = "select * from FoodCategory where id = " + id;

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            if (data == null)
            {
                MessageBox.Show($"Không thể kết nối dữ liệu.", "Lỗi");
            }

            foreach (DataRow items in data.Rows)
            {
                category = new Category(items);
                return category;
            }

            return category;
        }

        public bool InsertCategory(string name)
        {
            string query = string.Format("insert FoodCategory(name) values(N'{0}')", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool UpdateCategory(int idCategory, string name)
        {
            string query = string.Format("update FoodCategory set name = N'{0}' where id = {1}", name, idCategory);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool DeleteCategory(int idCategory)
        {
            FoodDAO.Instance.DeleteFoodForByIDCategory(idCategory);
            string query = string.Format("delete FoodCategory where id = {0}", idCategory);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

    }
}
