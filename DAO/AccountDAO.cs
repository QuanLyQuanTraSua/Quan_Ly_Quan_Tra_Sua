using Phan_Mem_Quan_Ly_Quan_Tra_Sua.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Phan_Mem_Quan_Ly_Quan_Tra_Sua.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;

        public static AccountDAO Instance
        {
            get { if (instance == null) instance = new AccountDAO(); return instance; }
            private set { instance = value; }
        }
        private AccountDAO() { }

        public bool login(string userName, string passWord)
        {
            byte[] temp = ASCIIEncoding.ASCII.GetBytes(passWord);
            byte[] hasData = new MD5CryptoServiceProvider().ComputeHash(temp);
            string hasPass = "";
            foreach(byte items in hasData)
            {
                hasPass += items;
            }
            string query = "exec USP_LoginAccount @userName , @passWWord";
            DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[] { userName, hasPass });

            return result.Rows.Count > 0;
        }

        public Account GetAccountByUserName(string userName)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("select * from Account where UserName = '" + userName + "'");

            foreach(DataRow items in data.Rows)
            {
                return new Account(items);
            }

            return null;
        }

        public DataTable GetListAccount()
        {
            return DataProvider.Instance.ExecuteQuery("select UserName , DisplayName , TYPE  from Account");
        }

        public List<Account> GetlistAccount()
        {
            List<Account> list = new List<Account>();

            string query = "select * from Account";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow items in data.Rows)
            {
                Account category = new Account(items);
                list.Add(category);
            }

            return list;
        }

        public bool UpdateAccount(string userName, string displayName, string passWord, string newPassWord)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("exec USP_UpdateAccount @userName , @displayName , @passWord , @newPassWord ", new object[] {userName, displayName, passWord, newPassWord  });

            return result > 0;
        }
        public bool InsertAccount(string name, string displayName, int type)
        {
            string query = string.Format("insert Account(UserName, DisplayName, PassWWord, TYPE) values(N'{0}', N'{1}', N'1962026656160185351301320480154111117132155', {2})", name, displayName, type);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool RegisterAccount(string name, string displayName, string passWord)
        {
            string query = string.Format("insert Account(UserName, DisplayName, PassWWord, TYPE) values(N'{0}', N'{1}', N'{2}', 0)", name, displayName, passWord);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool UpdateAccount(string name, string displayName, int type)
        {
            string query = string.Format("update Account set DisplayName = N'{0}', TYPE = {1} where UserName = N'{2}'", displayName, type, name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool DeleteAccount(string name)
        {
            string query = string.Format("delete Account where UserName = N'{0}'", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool ResetPassWord(string name)
        {
            string query = string.Format("update Account set PassWWord = N'{0}' where UserName = N'{1}'", "1962026656160185351301320480154111117132155", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
    }
}
