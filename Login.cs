using Phan_Mem_Quan_Ly_Quan_Tra_Sua.DAO;
using Phan_Mem_Quan_Ly_Quan_Tra_Sua.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Phan_Mem_Quan_Ly_Quan_Tra_Sua
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            string passWord = txbPassWord.Text;
            if (login(userName, passWord))
            {
                Account loginAccount = AccountDAO.Instance.GetAccountByUserName(userName);
                TableManager f = new TableManager(loginAccount);
                this.Hide();
                f.ShowDialog();
                this.Show();

            } else
            {
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu.");
            }
        }

        bool login(string userName, string passWord)
        {
            return AccountDAO.Instance.login(userName, passWord);
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát phần mềm ?", "Thông báo", MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                e.Cancel = true;
            }
        }


        private void btnRegister_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
