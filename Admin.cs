using Phan_Mem_Quan_Ly_Quan_Tra_Sua.DAO;
using Phan_Mem_Quan_Ly_Quan_Tra_Sua.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Phan_Mem_Quan_Ly_Quan_Tra_Sua
{
    public partial class Admin : Form
    {
        BindingSource foodList = new BindingSource();
        BindingSource accountlist = new BindingSource();
        BindingSource categorylist = new BindingSource();
        BindingSource tablelist = new BindingSource();


        public Account loginAccount;
        public Admin()
        {
            InitializeComponent();
            load();
        }

        #region Methods

        List<Food> SearchFoodByName(string name)
        {
            List<Food> listFood = FoodDAO.Instance.SearchFoodByName(name);


            return listFood;
        }
        void load()
        {
            dtgvFood.DataSource = foodList;
            dtgvAccount.DataSource = accountlist;
            dtgvCategory.DataSource = categorylist;
            dtgvTable.DataSource = tablelist;


            LoadDateTimePikerBill();
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);


            LoadListFood();
            LoadListCategory();
            LoadListAccount();
            LoadListTable();

            LoadCategoryIntoCombobox(cbFoodCategory);

            AddFoodBiding();
            AddCategoryBiding();
            AddTableBiding();
            AddAccountBiding();
        }

        // Event Account


        void AddFoodBiding()
        {
            txbFoodName.DataBindings.Add(new Binding("Text" , dtgvFood.DataSource,  "name", true, DataSourceUpdateMode.Never));
            txbFoodID.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "id", true, DataSourceUpdateMode.Never));
            nmFoodPrice.DataBindings.Add(new Binding("Value", dtgvFood.DataSource, "price", true, DataSourceUpdateMode.Never));
        }

        void AddAccountBiding()
        {
            txbUserName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            txbDisplayName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            nmTypeAccount.DataBindings.Add(new Binding("Value", dtgvAccount.DataSource, "TYPE", true, DataSourceUpdateMode.Never));
        }

        void AddCategoryBiding()
        {
            txbCategoryID.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "id", true, DataSourceUpdateMode.Never));
            txbNameCategory.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "name", true, DataSourceUpdateMode.Never));
        }

        void AddTableBiding()
        {
            txbTableID.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "id", true, DataSourceUpdateMode.Never));
            txbTableName.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "name", true, DataSourceUpdateMode.Never));
        }
        



        void LoadCategoryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";

        }


        

        void LoadDateTimePikerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);
        }

        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDate(checkIn, checkOut);

        }


        void LoadListFood()
        {
            cbFoodCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            foodList.DataSource = FoodDAO.Instance.GetLisstFood();
        }


        void LoadListAccount()
        {
            accountlist.DataSource = AccountDAO.Instance.GetListAccount();
        }

        void LoadListCategory()
        {
            categorylist.DataSource = CategoryDAO.Instance.GetListCategory();
        }

        void LoadListTable()
        {
            tablelist.DataSource = TableDAO.Instance.LoadTableList();
        }

        void AddAccount(string userName, string DislayName, int type)
        {
            if(AccountDAO.Instance.GetAccountByUserName(userName) != null)
            {
                MessageBox.Show("Tài khoản đã bị trùng vui lòng nhập lại.", "Lỗi");
            }

            if(AccountDAO.Instance.InsertAccount(userName, DislayName, type))
            {
                MessageBox.Show("Thêm tài khoản thành công.", "Thông báo");
                LoadListAccount();
            } else
            {
                MessageBox.Show("Thêm tài khoản không thành công.", "Thông báo");

            }
        }

        void EditAccount(string userName, string displayName, int type)
        {
            if (loginAccount.UserName.Equals(userName))
            {
                MessageBox.Show("Không thể sửa tài khoản của chính mình tại mục này.", "Lỗi");
                return;
            }
            if (AccountDAO.Instance.UpdateAccount(userName, displayName, type))
            {
                MessageBox.Show("Sửa tài khoản thành công.", "Thông báo");
                LoadListAccount();
            }
            else
            {
                MessageBox.Show("Sửa tài khoản không thành công.", "Thông báo");

            }
        }

        void DeleteAccount(string userName)
        {
            if (loginAccount.UserName.Equals(userName))
            {
                MessageBox.Show("Không thể xóa tài khoản vì bạn đang đăng nhập.", "Lỗi");
                return;
            }
            if (AccountDAO.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Xóa tài khoản thành công.", "Thông báo");
                LoadListAccount();
            }
            else
            {
                MessageBox.Show("Xóa tài khoản không thành công.", "Thông báo");

            }
        }

        void ResetPassWord(string userName)
        {
            if (AccountDAO.Instance.ResetPassWord(userName))
            {
                MessageBox.Show("Reset tài khoản thành công.", "Thông báo");
                LoadListAccount();
            }
            else
            {
                MessageBox.Show("Reset tài khoản không thành công.", "Thông báo");

            }
        }

        #endregion

        #region Events
        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }

        // Event Food
        private void btnShowFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }
        private void txbFoodID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtgvFood.SelectedCells.Count > 0)
                {
                    var idfood = dtgvFood.SelectedCells[0].OwningRow.Cells["CategoryID"].Value;
                    if (idfood == null)
                    {
                        return;
                    }
                    int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["CategoryID"].Value;

                    Category category = CategoryDAO.Instance.GetCategoryByID(id);

                    cbFoodCategory.SelectedItem = category;

                    int index = -1;
                    int i = 0;

                    foreach (Category items in cbFoodCategory.Items)
                    {
                        if (items.ID == category.ID)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }

                    cbFoodCategory.SelectedIndex = index;
                }
            }
            catch
            {
                MessageBox.Show("Đã hết món ăn");
                return;
            }
            
        }
        private void btnAddFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            if(cbFoodCategory.Text == null || cbFoodCategory.Text == "")
            {
                MessageBox.Show("Bạn phải thêm danh mục trước khi thêm món.", "Thông báo");
                return;
            } 
            int categoryID = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;

            if (FoodDAO.Instance.InsertFood(name, categoryID, price))
            {
                MessageBox.Show("Thêm món thành công", "Thông báo");
                LoadListFood();
                if (insertFood != null)
                {
                    insertFood(this, new EventArgs());
                }
            } else
            {
                MessageBox.Show("Thêm món không thành công! Vui lòng thử lại", "Thông báo");
            }
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int categoryID = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;
            int id = Convert.ToInt32(txbFoodID.Text);

            if (FoodDAO.Instance.UpdateFood(id, name, categoryID, price))
            {
                MessageBox.Show("Sửa món thành công", "Thông báo");
                LoadListFood();
                if (updateFood != null)
                {
                    updateFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Sửa món không thành công! Vui lòng thử lại", "Thông báo");
            }
        }
        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            foodList.DataSource =  SearchFoodByName(txbSearchNameFood.Text);
        }
        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbFoodID.Text);

            if (FoodDAO.Instance.DeleteFood(id))
            {
                MessageBox.Show("Xóa món thành công", "Thông báo");
                LoadListFood();
                if(deleteFood != null)
                {
                    deleteFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Xóa món không thành công! Vui lòng thử lại", "Thông báo");
            }
        }

        // Event Category
        private void btnShowCategory_Click(object sender, EventArgs e)
        {
            LoadListCategory();
        }
        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            string name = txbNameCategory.Text;


            if (CategoryDAO.Instance.InsertCategory(name))
            {
                MessageBox.Show("Thêm danh mục thành công", "Thông báo");
                LoadListCategory();
                LoadListFood();
                LoadCategoryIntoCombobox(cbFoodCategory);
                if (insertCategory != null)
                {
                    insertCategory(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Thêm danh mục không thành công! Vui lòng thử lại", "Thông báo");
            }
        }
        private void btnEditCategory_Click(object sender, EventArgs e)
        {
            string name = txbNameCategory.Text;

            int id = Convert.ToInt32(txbCategoryID.Text);

            if (CategoryDAO.Instance.UpdateCategory(id, name))
            {
                MessageBox.Show("Sửa danh mục thành công", "Thông báo");
                LoadListCategory();
                LoadListFood();
                LoadCategoryIntoCombobox(cbFoodCategory);
                if (updateCategory != null)
                {
                    updateCategory(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Sửa danh mục không thành công! Vui lòng thử lại", "Thông báo");
            }
        }

        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbCategoryID.Text);

            if (CategoryDAO.Instance.DeleteCategory(id))
            {
                MessageBox.Show("Xóa danh mục thành công", "Thông báo");
                LoadListCategory();
                LoadListFood();
                LoadCategoryIntoCombobox(cbFoodCategory);
                if (deleteCategory != null)
                {
                    deleteCategory(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Xóa danh mục không thành công! Vui lòng thử lại", "Thông báo");
            }
        }

        //Event Table

        private void btnShowTable_Click(object sender, EventArgs e)
        {
            LoadListTable();
        }

        private void btnEditTable_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbTableID.Text);
            string name = txbTableName.Text;


            if (TableDAO.Instance.UpdateTable(id, name))
            {
                MessageBox.Show("Sửa bàn thành công", "Thông báo");
                LoadListFood();
                LoadListTable();
                if (updateTable != null)
                {
                    updateTable(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Sửa bàn không thành công! Vui lòng thử lại", "Thông báo");
            }
        }

        private void btnDeleteTable_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbTableID.Text);
            if (TableDAO.Instance.DeleteTable(id))
            {
                MessageBox.Show("Xóa bàn thành công", "Thông báo");
                LoadListFood();
                LoadListTable();
                if (deleteTable != null)
                {
                    deleteTable(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Xóa bàn không thành công! Vui lòng thử lại", "Thông báo");
            }
        }

        private void btnAddTable_Click(object sender, EventArgs e)
        {
            string name = txbTableName.Text;


            if (TableDAO.Instance.InsertTable(name))
            {
                MessageBox.Show("Thêm bàn thành công", "Thông báo");
                LoadListFood();
                LoadListTable();
                if (insertTable != null)
                {
                    insertTable(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Thêm bàn không thành công! Vui lòng thử lại", "Thông báo");
            }
        }

        //EventHander Table

        private event EventHandler insertTable;
        public event EventHandler InsertTable
        {
            add { insertTable += value; }
            remove { insertTable -= value; }
        }

        private event EventHandler deleteTable;
        public event EventHandler DeleteTable
        {
            add { deleteTable += value; }
            remove { deleteTable -= value; }
        }

        private event EventHandler updateTable;
        public event EventHandler UpdateTable
        {
            add { updateTable += value; }
            remove { updateTable -= value; }
        }

        // EventHandler Category
        private event EventHandler insertCategory;
        public event EventHandler InsertCategory
        {
            add { insertCategory += value; }
            remove { insertCategory -= value; }
        }

        private event EventHandler deleteCategory;
        public event EventHandler DeleteCategory
        {
            add { deleteCategory += value; }
            remove { deleteCategory -= value; }
        }

        private event EventHandler updateCategory;
        public event EventHandler UpdateCategory
        {
            add { updateCategory += value; }
            remove { updateCategory -= value; }
        }


        // EventHandler Food

        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }

        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }

        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }
        
        private void btnShowAccount_Click(object sender, EventArgs e)
        {
            LoadListAccount();
        }

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            string displayName = txbDisplayName.Text;
            int type = (int)nmTypeAccount.Value;

            AddAccount(userName, displayName, type);
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            DeleteAccount(userName);
        }

        private void btnEditAccount_Click(object sender, EventArgs e)
        {

            string userName = txbUserName.Text;
            string displayName = txbDisplayName.Text;
            int type = (int)nmTypeAccount.Value;

            EditAccount(userName, displayName, type);

            
        }

        private void btnResetPassWord_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            ResetPassWord(userName);
        }


        #endregion


    }
}
