using TesInspiro.Data.DataAccessControl;
using TesInspiro.Domain;

namespace TesInspiro.Business
{
    public class TesInspiroBusinessProcessor : TesInspiroAccessControl
    {
        #region View
        public List<MsItemsViewModel> GetListItems(string? item_name)
        {
            try
            {
                List<MsItemsViewModel> result = ListItems(item_name);
                return result;
            }
            catch
            {
                throw;
            }
        }
        public MsItemsViewModel GetDetailItem(int id)
        {
            try
            {
                MsItemsViewModel result = DetailItem(id);
                return result;
            }
            catch
            {
                throw;
            }
        }
        public List<ShoppingCartViewModel> GetListShoppingCart(string? username)
        {
            try
            {
                List<ShoppingCartViewModel> result = ListShoppingCart(username);
                return result;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Transaction
        public bool InsertUpdateCart(ShoppingCart_DTO data)
        {
            try
            {
                bool result = DoInsertUpdateCart(data);
                return result;
            }
            catch
            {
                throw;
            }
        }
        public int CreateShoppingCart(ShoppingCart_DTO data)
        {
            try
            {
                int result = CreateCart(data);
                return result;
            }
            catch
            {
                throw;
            }
        }
        public int UpdateShoppingCart(ShoppingCart_DTO data)
        {
            try
            {
                int result = UpdateCart(data);
                return result;
            }
            catch
            {
                throw;
            }
        }
        public bool Payment(string username, int pay, int grandTotal)
        {
            try
            {
                bool result = DoPayment(username, pay, grandTotal);
                return result;
            }
            catch
            {
                throw;
            }
        }
        public List<ShoppingCartViewModel> DeleteShoppingCart(int id, string username)
        {
            try
            {
                List<ShoppingCartViewModel> result = DoDeleteShoppingCart(id, username);
                return result;
            }
            catch
            {
                throw;
            }
        }
        public List<ShoppingCartViewModel> UpdateItemCart(int id, int qty, string username)
        {
            try
            {
                List<ShoppingCartViewModel> result = DoUpdateItemCart(id, qty, username);
                return result;
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}