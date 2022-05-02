using Phan_Mem_Quan_Ly_Quan_Tra_Sua.DAO;
using Phan_Mem_Quan_Ly_Quan_Tra_Sua.DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Phan_Mem_Quan_Ly_Quan_Tra_Sua
{
    public partial class TableManager : Form
    {
        private Account loginAccount;

        public Account LoginAccount {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount.Type);  }
        }

        public TableManager(Account account)
        {
            InitializeComponent();

            this.LoginAccount = account;

            LoadTable();
            LoadCategory();
        }

        #region Method

        void ChangeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1;
            TypeLastName(LoginAccount.DisplayName);
        }

        public void TypeLastName(string name)
        {
            string LastName = name.Trim();
            int index = LastName.LastIndexOf(" ");
            LastName = LastName.Remove(0, index + 1);

            if (LoginAccount.Type == 1)
            {
                thôngTinTàiKhoảnToolStripMenuItem.Text += $" (Admin: {LastName})";
            }
            else
            {
                thôngTinTàiKhoảnToolStripMenuItem.Text += $" (Nhân viên: {LastName})";
            }
        }

        void LoadCategory()
        {
            List<Category> listCategory = CategoryDAO.Instance.GetListCategory();
            cbCategory.DataSource = listCategory;
            cbCategory.DisplayMember = "Name";
            cbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cbFood.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        void LoadFoodListByCategoryID(int id)
        {
            List<Food> listFood = FoodDAO.Instance.GetFoodByCategoryID(id);
            cbFood.DataSource = listFood;
            cbFood.DisplayMember = "Name";
            cbFood.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        public void LoadTable()
        {

            flpTable.Controls.Clear();

            List<Table> tablelist = TableDAO.Instance.LoadTableList();

            foreach(Table items in tablelist)
            {
                Button btn = new Button() { Width = TableDAO.TableWidth, Height = TableDAO.TableHeight};
                btn.Text = items.Name + Environment.NewLine + items.Status;
                btn.Click += btn_Click;
                btn.Tag = items;

                switch (items.Status)
                {
                    case "Trống":
                        btn.BackColor = Color.LightCyan;
                        break;
                    default:
                        btn.BackColor = Color.BlueViolet ;
                        btn.ForeColor = Color.White;
                        break;
                }
                
                flpTable.Controls.Add(btn);
            }
        }

        void ShowBill(int id)
        {

            lsvBill.Items.Clear();
            List<Phan_Mem_Quan_Ly_Quan_Tra_Sua.DTO.Menu> listBillInfo = MenuDAO.Instance.GetListMenuByTable(id);
            float totalPrice = 0;

            foreach(Phan_Mem_Quan_Ly_Quan_Tra_Sua.DTO.Menu items in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(items.FoodName.ToString());
                lsvItem.SubItems.Add(items.Count.ToString());
                lsvItem.SubItems.Add(items.Price.ToString());
                lsvItem.SubItems.Add(items.TotalPrice.ToString());
                totalPrice += items.TotalPrice;
                lsvBill.Items.Add(lsvItem);
            }
            CultureInfo culture = new CultureInfo("vi-VN");
            txbTotalPrice.Text = totalPrice.ToString();
        }

        #endregion

        #region Events
        private void btn_Click(object sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as Table).ID;
            lsvBill.Tag = (sender as Button).Tag;
            ShowBill(tableID);
        }
        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AccountProfile f = new AccountProfile(LoginAccount);          
            f.ShowDialog();
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Admin f = new Admin();
            f.loginAccount = LoginAccount;

            f.InsertFood += F_InsertFood;
            f.DeleteFood += F_DeleteFood;
            f.UpdateFood += F_UpdateFood;

            f.InsertCategory += F_InsertCategory;
            f.DeleteCategory += F_DeleteCategory;
            f.UpdateCategory += F_UpdateCategory;

            f.InsertTable += F_InsertTable;
            f.UpdateTable += F_UpdateTable;
            LoadCategory();
            LoadTable();
            f.ShowDialog();
        }

        void F_InsertTable(object sender, EventArgs e)
        {
            LoadTable();
            LoadCategory();
            if (cbCategory.SelectedItem == null)
            {
                return;
            }
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
            {
                ShowBill((lsvBill.Tag as Table).ID);
            }
            LoadTable();
        }

        void F_UpdateTable(object sender, EventArgs e)
        {
            LoadTable();
            LoadCategory();
            if (cbCategory.SelectedItem == null)
            {
                return;
            }
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
            {
                ShowBill((lsvBill.Tag as Table).ID);
            }
            LoadTable();

        }

        void F_InsertCategory(object sender, EventArgs e)
        {
            LoadCategory();
            if (cbCategory.SelectedItem == null)
            {
                return;
            }
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
            {
                ShowBill((lsvBill.Tag as Table).ID);
            }
            LoadTable();
        }
        void F_DeleteCategory(object sender, EventArgs e)
        {
            LoadCategory();
            if (cbCategory.SelectedItem == null)
            {
                return;
            }
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
            {
                ShowBill((lsvBill.Tag as Table).ID);
            }

            LoadTable();
        }
        void F_UpdateCategory(object sender, EventArgs e)
        {
            LoadCategory();
            if (cbCategory.SelectedItem == null)
            {
                return;
            }
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
            {
                ShowBill((lsvBill.Tag as Table).ID);
            }
            LoadTable();
        }

        void F_InsertFood(object sender, EventArgs e)
        {
            if(cbCategory.SelectedItem == null)
            {
                return;
            }
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
            {
                ShowBill((lsvBill.Tag as Table).ID);
            }
            LoadTable();
        }

        void F_DeleteFood(object sender, EventArgs e)
        {
            if (cbCategory.SelectedItem == null)
            {
                return;
            }
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
            {
                ShowBill((lsvBill.Tag as Table).ID);
            }
            LoadTable();
        }

        void F_UpdateFood(object sender, EventArgs e)
        {
            if (cbCategory.SelectedItem == null)
            {
                return;
            }
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
            {
                ShowBill((lsvBill.Tag as Table).ID);
            }
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;
            ComboBox cb = sender as ComboBox;

            if (cb.SelectedItem == null) return;

            Category selected = cb.SelectedItem as Category;
            id = selected.ID;
            LoadFoodListByCategoryID(id);
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            if (cbFood.Text != "" && cbCategory.Text != "")
            {
                Table table = lsvBill.Tag as Table;

                if (table == null)
                {
                    MessageBox.Show("Hãy chọn bàn.", "Thông báo");
                    return;
                }
                int idBill = BillDAO.Instance.GetUnCheckBillIDByTableID(table.ID);

                int foodID = (cbFood.SelectedItem as Food).ID;
                int count = (int)nmFoodCount.Value;

                if (idBill == -1)
                {
                    BillDAO.Instance.InsertBill(table.ID);
                    BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIDBill(), foodID, count);
                }
                else
                {
                    BillInfoDAO.Instance.InsertBillInfo(idBill, foodID, count);
                }

                ShowBill(table.ID);

                LoadTable();
            }
            else
            {
                MessageBox.Show("Chưa có món ăn nào.", "Thông báo");
            }
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;

            if(table == null)
            {
                MessageBox.Show("Hãy chọn bàn để thanh toán.", "Thông báo");
                return;
            }

            int idBill = BillDAO.Instance.GetUnCheckBillIDByTableID(table.ID);
            int discount = (int)nmDisCount.Value;

            double totalPrice = Convert.ToDouble(txbTotalPrice.Text.Split(',')[0]);
            double finalTotalPrice = totalPrice - (totalPrice / 100) * discount;

            if (idBill != -1)
            {
                if (MessageBox.Show("Bạn có chắc muốn thanh toán cho bàn " + table.Name + ".\n Tổng tiền:  " + finalTotalPrice.ToString() + " VNĐ", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    BillDAO.Instance.CheckOut(idBill, discount, (float)finalTotalPrice);

                    ShowBill(table.ID);

                    LoadTable();
                }
            }
        }
        #endregion

        private void thôngTinTàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadCategory();
            LoadTable();
        }
    }
}
