using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phan_Mem_Quan_Ly_Quan_Tra_Sua.DTO
{
    public class Bill
    {
        private int iD;
        private DateTime? dataCheckIn;
        private DateTime? dataCheckOut;
        private int status;
        private int discount;

        public int ID { get => iD; set => iD = value; }
        public DateTime? DataCheckIn { get => dataCheckIn; set => dataCheckIn = value; }
        public DateTime? DataCheckOut { get => dataCheckOut; set => dataCheckOut = value; }
        public int Status { get => status; set => status = value; }
        public int Discount { get => discount; set => discount = value; }

        public Bill(int id, DateTime? dataCheckin, DateTime? dataCheckout, int status, int discount)
        {
            this.ID = id;
            this.DataCheckIn = dataCheckin;
            this.DataCheckOut = dataCheckout;
            this.Status = status;
            this.Discount = discount;
        }

        public Bill(DataRow row)
        {
            this.ID = (int)row["id"];
            this.DataCheckIn = (DateTime?)row["DateCheckIn"];

            var dataCheckOutTemp = row["DateCheckOut"];
            if(dataCheckOutTemp.ToString() != "")
                this.DataCheckOut = (DateTime?)row["DateCheckOut"];

            this.Status = (int)row["status"];

            if(row["discount"].ToString() != "")
            {
                this.Discount = (int)row["discount"];
            }
            
        }
    }
}
