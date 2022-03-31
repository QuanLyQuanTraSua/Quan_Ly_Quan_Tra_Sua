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
    public partial class AccountProfile : Form
    {
        private Account loginAccount;

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(); }
        }
        public AccountProfile(Account account)
        {
            InitializeComponent();
            LoginAccount = account;
        }

        void ChangeAccount()
        {
            txbUserName.Text = LoginAccount.UserName;
            txbDisplayName.Text = LoginAccount.DisplayName;
        }

        void UpdateAccount()
        {
            string displayName = txbDisplayName.Text;
            string passWord = txbPassWord.Text;
            string newPassWord = txbNewPassWord.Text;
            string reEnterPass = txbReEnterPassWord.Text;
            string userName = txbUserName.Text;           


            if (!newPassWord.Equals(reEnterPass))
            {
                MessageBox.Show("Vui lòng nhập lại mật khẩu đúng với mật khẩu mới", "Thông báo");
                return;
            } else
            {
                // Mã hóa PassWord
                byte[] temp = ASCIIEncoding.ASCII.GetBytes(passWord);
                byte[] hasData = new MD5CryptoServiceProvider().ComputeHash(temp);
                string hasPass = "";
                foreach (byte items in hasData)
                {
                    hasPass += items;
                }
                // Mã hóa RePassWord
                byte[] temp2 = ASCIIEncoding.ASCII.GetBytes(reEnterPass);
                byte[] hasData2 = new MD5CryptoServiceProvider().ComputeHash(temp2);
                string hasPass2 = "";
                foreach (byte items in hasData2)
                {
                    hasPass2 += items;
                }

                
                if(reEnterPass == "")
                {
                    if (AccountDAO.Instance.UpdateAccount(userName, displayName, hasPass, hasPass))
                    {
                        MessageBox.Show("Cập nhật thành công", "Thông báo");
                        this.Close();
                    } else
                    {
                        MessageBox.Show("Vui lòng nhập đúng mật khẩu", "Thông báo");
                    }
                } else {
                    if (AccountDAO.Instance.UpdateAccount(userName, displayName, hasPass, hasPass2))
                    {
                        MessageBox.Show("Cập nhật thành công", "Thông báo");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng nhập đúng mật khẩu", "Thông báo");
                    }
                }
                              
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateAccount();
            
        }
    }

}
