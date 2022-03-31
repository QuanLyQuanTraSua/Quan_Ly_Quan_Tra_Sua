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
    public partial class RegisterAccount : Form
    {
        public RegisterAccount()
        {
            InitializeComponent();
        }

        void AddAccount(string userName, string DislayName, string passWord)
        {
            AccountDAO.Instance.RegisterAccount(userName, DislayName, passWord);
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string userName = txbName.Text;
            string display = txbDisplay.Text;
            string pass = txbPass.Text;
            string repass = txbRePassW.Text;


            if (userName == "" || display == "" || pass == "" || repass == "")
            {
                MessageBox.Show("Vui lòng điền đủ thông tin.", "Thông báo");

            } else
            {
                if (AccountDAO.Instance.GetAccountByUserName(userName) == null)
                {
                    if (pass.Equals(repass))
                    {
                        byte[] temp = ASCIIEncoding.ASCII.GetBytes(pass);
                        byte[] hasData = new MD5CryptoServiceProvider().ComputeHash(temp);
                        string hasPass = "";
                        foreach (byte items in hasData)
                        {
                            hasPass += items;
                        }

                        AddAccount(userName, display, hasPass);
                        MessageBox.Show("Đăng ký tài khoản thành công.", "Thông báo");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Mật khẩu nhập lại chưa đúng.", "Thông báo");
                    }
                }
                else
                {
                    MessageBox.Show("Tài khoản đã bị trùng vui lòng nhập lại.", "Thông báo");
                }
            }
        }

        private void btnExitRegister_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
