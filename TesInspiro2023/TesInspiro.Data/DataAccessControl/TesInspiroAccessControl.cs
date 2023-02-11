using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesInspiro.Domain;

namespace TesInspiro.Data.DataAccessControl
{
    public class TesInspiroAccessControl
    {
        #region View
        protected List<MsItemsViewModel> ListItems(string? item_name)
        {
            using (var context = new TesInspiroDataContext())
            {
                if (context == null || context.MsItems == null)
                    throw new ArgumentNullException("Db context not found");

                try
                {
                    List<MsItemsViewModel> result = (from c in context.MsItems.ToList()
                                                     orderby c.id
                                                     where (string.IsNullOrEmpty(item_name) || c.item_name.ToLower().Contains(item_name.ToLower()))
                                                     select new MsItemsViewModel
                                                     {
                                                         id = c.id,
                                                         item_name = c.item_name,
                                                         price = c.price,
                                                         qty = c.qty
                                                     }).ToList();
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        protected MsItemsViewModel DetailItem(int id)
        {
            using (var context = new TesInspiroDataContext())
            {
                if (context == null || context.MsItems == null)
                    throw new ArgumentNullException("Db context not found");

                try
                {
                    MsItemsViewModel result = new MsItemsViewModel();

                    var qry = context.MsItems.FirstOrDefault(x => x.id == id);
                    if (qry != null)
                    {
                        result.id = qry.id;
                        result.item_name = qry.item_name;
                        result.qty = qry.qty;
                        result.price = qry.price;
                    }
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        protected List<ShoppingCartViewModel> ListShoppingCart(string? username)
        {
            using (var context = new TesInspiroDataContext())
            {
                if (context == null || context.MsItems == null || context.ShoppingCarts == null)
                    throw new ArgumentNullException("Db context not found");

                try
                {
                    //var a = context.ShoppingCarts.ToList();
                    List<ShoppingCartViewModel> result = (from c in context.MsItems.ToList()
                                                          join s in context.ShoppingCarts.ToList() on c.id equals s.item_id
                                                          where (string.IsNullOrEmpty(username) || s.username.ToLower() == username.ToLower()) &&
                                                                (s.is_paid == null || !s.is_paid.Value)
                                                          select new ShoppingCartViewModel
                                                          {
                                                             id = s.id,
                                                             item_name = c.item_name,
                                                             username = s.username,
                                                             qty = s.qty,
                                                             price = c.price,
                                                             total_price = s.total_price,
                                                             created_dtm = s.created_dtm,
                                                             updated_dtm = s.updated_dtm
                                                          }).ToList();
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }
        #endregion

        #region Transaction
        protected bool DoInsertUpdateCart(ShoppingCart_DTO data)
        {
            using (var context = new TesInspiroDataContext())
            {
                if (context == null || context.MsItems == null || context.ShoppingCarts == null)
                    throw new ArgumentNullException("Db context not found");

                var transaction = context.Database.BeginTransaction();
                try
                {
                    bool result = false;
                    bool ready = false;

                    var qry = context.ShoppingCarts.FirstOrDefault(x =>
                                                                x.item_id == data.item_id &&
                                                                x.username.ToLower() == data.username.ToLower());
                    if(qry != null)
                    {
                        int totalQty = qry.qty.Value + data.qty.Value;
                        qry.qty = totalQty;
                        qry.total_price = data.price * totalQty;
                        qry.updated_dtm = DateTime.Now;
                        context.ShoppingCarts.Update(qry);

                        if (context.SaveChanges() > 0)
                            ready = true;
                    }
                    else
                    {
                        ShoppingCart dto = new ShoppingCart
                        {
                            item_id = data.item_id,
                            username = data.username,
                            qty = data.qty,
                            total_price = data.price * data.qty,
                            created_dtm = DateTime.Now
                        };
                        context.ShoppingCarts.Add(dto);
                        if (context.SaveChanges() > 0)
                            ready = true;
                    }
                    if (ready)
                    {
                        transaction.Commit();
                        result = true;
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                    return result;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        protected int CreateCart(ShoppingCart_DTO data)
        {
            using (var context = new TesInspiroDataContext())
            {
                if (context == null || context.MsItems == null || context.ShoppingCarts == null)
                    throw new ArgumentNullException("Db context not found");

                var transaction = context.Database.BeginTransaction();
                try
                {
                    int result = 0;
                    ShoppingCart dto = new ShoppingCart
                    {
                        item_id = data.item_id,
                        username = data.username,
                        qty = data.qty,
                        total_price = data.price * data.qty,
                        created_dtm = DateTime.Now
                    };
                    context.ShoppingCarts.Add(dto);
                    if (context.SaveChanges() > 0)
                    {
                        transaction.Commit();
                        result = dto.id.Value;
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                    return result;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        protected int UpdateCart(ShoppingCart_DTO data)
        {
            using (var context = new TesInspiroDataContext())
            {
                if (context == null || context.MsItems == null || context.ShoppingCarts == null)
                    throw new ArgumentNullException("Db context not found");

                var transaction = context.Database.BeginTransaction();
                try
                {
                    int result = 0;

                    var qry = context.ShoppingCarts.FirstOrDefault(x => x.id == data.id);
                    if (qry != null)
                    {
                        qry.qty = data.qty;
                        qry.total_price = data.price * data.qty;
                        qry.updated_dtm = DateTime.Now;

                        context.ShoppingCarts.Update(qry);
                        if (context.SaveChanges() > 0)
                        {
                            transaction.Commit();
                            result = qry.id.Value;
                        }
                        else
                        {
                            transaction.Rollback();
                        }
                    }
                    return result;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        protected bool DoPayment(string username, int pay, int grandTotal)
        {
            using (var context = new TesInspiroDataContext())
            {
                if (context == null || context.MsItems == null || context.ShoppingCarts == null || context.Payments == null)
                    throw new ArgumentNullException("Db context not found");

                var transaction = context.Database.BeginTransaction();
                try
                {
                    bool result = false;
                    bool ready = false;

                    var qry = context.ShoppingCarts.Where(x => x.username == username && 
                                                               x.is_paid == null || !x.is_paid.Value
                                                         ).ToList();
                    if (qry != null)
                    {
                        foreach(var item in qry)
                        {
                            var qryItem = context.MsItems.FirstOrDefault(x => x.id == item.item_id);
                            if(qryItem != null)
                            {
                                if (qryItem.qty < 1)
                                    throw new Exception("There's an item in your shopping cart that has out of stock. Please update your shopping cart");
                                else
                                {
                                    qryItem.qty = qryItem.qty - item.qty;
                                    context.MsItems.Update(qryItem);
                                    context.SaveChanges();
                                }
                            }
                            item.is_paid = true;
                            item.updated_dtm = DateTime.Now;
                            context.Update(item);

                            context.SaveChanges();
                        }
                        Payment dto = new Payment
                        {
                            username = username,
                            grand_total = grandTotal,
                            pay = pay,
                            created_dtm = DateTime.Now
                        };
                        context.Payments.Add(dto);
                        context.SaveChanges();

                        ready = true;
                        if (ready)
                        {
                            transaction.Commit();
                            result = true;
                        }
                        else
                        {
                            transaction.Rollback();
                        }
                    }
                    return result;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        protected List<ShoppingCartViewModel> DoUpdateItemCart(int id, int qty, string username)
        {
            using (var context = new TesInspiroDataContext())
            {
                if (context == null || context.MsItems == null || context.ShoppingCarts == null)
                    throw new ArgumentNullException("Db context not found");

                var transaction = context.Database.BeginTransaction();
                try
                {
                    List<ShoppingCartViewModel> result = new List<ShoppingCartViewModel>();

                    var qry = context.ShoppingCarts.FirstOrDefault(x => x.id == id);
                    if (qry != null)
                    {
                        if (qty < 1)
                        {
                            context.ShoppingCarts.Remove(qry);
                        }
                        else
                        {
                            var qryItem = context.MsItems.FirstOrDefault(x => x.id == qry.item_id);
                            qry.qty = qty;
                            qry.total_price = qryItem.price * qty;
                            qry.updated_dtm = DateTime.Now;
                            context.Update(qry);
                        }
                        if (context.SaveChanges() > 0)
                        {
                            transaction.Commit();
                            result = (from c in context.MsItems.ToList()
                                      join s in context.ShoppingCarts.ToList() on c.id equals s.item_id
                                      where (s.username.ToLower() == username.ToLower()) &&
                                            (s.is_paid == null || !s.is_paid.Value)
                                      select new ShoppingCartViewModel
                                      {
                                          id = s.id,
                                          item_name = c.item_name,
                                          username = s.username,
                                          qty = s.qty,
                                          price = c.price,
                                          total_price = s.total_price,
                                          created_dtm = s.created_dtm,
                                          updated_dtm = s.updated_dtm
                                      }).ToList();
                        }
                        else
                        {
                            transaction.Rollback();
                        }
                    }
                    else
                    {
                        throw new Exception("Data already deleted");
                    }
                    return result;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        protected List<ShoppingCartViewModel> DoDeleteShoppingCart(int id, string username)
        {
            using (var context = new TesInspiroDataContext())
            {
                if (context == null || context.MsItems == null || context.ShoppingCarts == null)
                    throw new ArgumentNullException("Db context not found");

                var transaction = context.Database.BeginTransaction();
                try
                {
                    List<ShoppingCartViewModel> result = new List<ShoppingCartViewModel>();

                    var qry = context.ShoppingCarts.FirstOrDefault(x => x.id == id);
                    if (qry != null)
                    {
                        context.ShoppingCarts.Remove(qry);
                        if (context.SaveChanges() > 0)
                        {
                            transaction.Commit();
                            result = (from c in context.MsItems.ToList()
                                      join s in context.ShoppingCarts.ToList() on c.id equals s.item_id
                                      where (s.username.ToLower() == username.ToLower()) &&
                                            (s.is_paid == null || !s.is_paid.Value)
                                      select new ShoppingCartViewModel
                                      {
                                          id = s.id,
                                          item_name = c.item_name,
                                          username = s.username,
                                          qty = s.qty,
                                          price = c.price,
                                          total_price = s.total_price,
                                          created_dtm = s.created_dtm,
                                          updated_dtm = s.updated_dtm
                                      }).ToList();
                        }
                        else
                        {
                            transaction.Rollback();
                        }
                    }
                    else
                    {
                        throw new Exception("Data already deleted");
                    }
                    return result;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        #endregion
    }
}
