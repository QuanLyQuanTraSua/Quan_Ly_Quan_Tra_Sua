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
        public Admin()
        {
            InitializeComponent();
            load();
        }

        #region Mothods

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

            dropDownList();
            LoadDateTimePikerBill();
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);

            LoadListFood();
            LoadAcount();
            LoadCategory();
            LoadTable();

            LoadCategoryIntoCombobox(cbFoodCategory);

            AddFoodBiding();
            AddAccountBiding();
            AddCategoryBiding();
            AddTableBiding();
        }

        void dropDownList()
        {
            cbAccountType.DropDownStyle = ComboBoxStyle.DropDownList;         
        }

        void AddFoodBiding()
        {
            txbFoodName.DataBindings.Add(new Binding("Text" , dtgvFood.DataSource,  "Name", true, DataSourceUpdateMode.Never));
            txbFoodID.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "ID", true, DataSourceUpdateMode.Never));
            nmFoodPrice.DataBindings.Add(new Binding("Value", dtgvFood.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }

        void AddAccountBiding()
        {
            txbUserName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            txbDisplayName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            cbAccountType.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "TYPE", true, DataSourceUpdateMode.Never));
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

        void LoadAcount()
        {
            accountlist.DataSource = AccountDAO.Instance.GetListAccount();
        }

        void LoadCategory()
        {
            categorylist.DataSource = CategoryDAO.Instance.GetListFoodCategory();
        }

        void LoadTable()
        {
            tablelist.DataSource = TableDAO.Instance.GetListTableFood();

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

        #endregion

        #region Events
        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }
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

            }
            
        }
        private void btnAddFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int categoryID = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;

            if(FoodDAO.Instance.InsertFood(name, categoryID, price))
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
        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            foodList.DataSource =  SearchFoodByName(txbSearchNameFood.Text);
        }
        private void btnShowAccount_Click(object sender, EventArgs e)
        {
            LoadAcount();
        }


        #endregion

        private void btnShowCategory_Click(object sender, EventArgs e)
        {
            LoadCategory();
        }

        private void btnShowTable_Click(object sender, EventArgs e)
        {
            LoadTable();
        }
    }
}
