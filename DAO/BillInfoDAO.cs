﻿using Phan_Mem_Quan_Ly_Quan_Tra_Sua.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phan_Mem_Quan_Ly_Quan_Tra_Sua.DAO
{
    public class BillInfoDAO
    {
        private static BillInfoDAO instance;
        public static BillInfoDAO Instance
        {
            get { if (instance == null) instance = new BillInfoDAO(); return instance; }
            private set { instance = value; }
        }
        private BillInfoDAO() { }

        public void DeleteBillInfoByFoodID(int id)
        {
            DataProvider.Instance.ExecuteQuery("delete BillInfo where idFood = " + id);
        }
        
        public void DeleteBillInfoByIDBill(int id)
        {
            DataProvider.Instance.ExecuteQuery("delete BillInfo where idBill = " + id);
        }

        public void DeleteBillInfoByIDTable(int id)
        {
            List<Bill> listbill = BillDAO.Instance.GetBillByIDTable(id);
            foreach (Bill items in listbill)
            {
                DeleteBillInfoByIDBill(items.ID);
            }
            
        }

        public void DeleteFoodByIDCategory(int id)
        {
            DataProvider.Instance.ExecuteQuery("delete Food where idCategory = " + id);
        }

        public List<BillInfo> GetListBillInfo(int id)
        {
            List<BillInfo> listBillInfo = new List<BillInfo>();

            DataTable data = DataProvider.Instance.ExecuteQuery("select * from BillInfo where idBill = " + id );

            foreach(DataRow items in data.Rows)
            {
                BillInfo info = new BillInfo(items);
                listBillInfo.Add(info);
            }

            return listBillInfo;
        }

        public void InsertBillInfo(int idBill, int idFood, int count)
        {
            DataProvider.Instance.ExecuteNonQuery("USP_InsertBillInfo @idBill , @idFood , @count ", new object[] { idBill, idFood, count });
        }
    }
}
