using Phan_Mem_Quan_Ly_Quan_Tra_Sua.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phan_Mem_Quan_Ly_Quan_Tra_Sua
{
    public class AccountEvent:EventArgs
    {
        private Account account;

        public Account Account { get => account; set => account = value; }

        public AccountEvent(Account account)
        {
            this.Account = account;
        }
    }
}
