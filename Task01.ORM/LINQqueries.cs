using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task01.ORM
{
    public static class LINQqueries
    {
        public static bool AddIngradient(string ingradientName, string categoryName, int weight)
        {
            using (var context = new ShawarmaContext())
            {
                var ingradient =
                    context.Ingradients.FirstOrDefault(ing => ing.IngradientName == ingradientName);
                if (ingradient != null)
                {
                    ingradient.TotalWeight += weight;
                }
                else
                {
                    var ingradientCategory =
                        context.IngradientCategories.FirstOrDefault(
                            ingCategory => ingCategory.CategoryName == categoryName);
                    if (ingradientCategory == null)
                    {
                        return false;
                    }
                    else
                    {
                        context.Ingradients.Add(new Ingradient()
                        {
                            CategoryId = ingradientCategory.CategoryId,
                            IngradientName = ingradientName,
                            TotalWeight = weight
                        });
                    }
                }
                return Commit(context);
            }
        }

        public static bool AddIngradientCategory(string categoryName)
        {
            using (var context = new ShawarmaContext())
            {
                context.IngradientCategories.Add(new IngradientCategory() {CategoryName = categoryName});
                return Commit(context);
            }
        }


        public static bool SellShawarma(string shawarmaName)
        {
            using (var context = new ShawarmaContext())
            {
                var shawarma = context.Shawarmas.FirstOrDefault(shaw => shaw.ShawarmaName == shawarmaName);
                if (shawarma == null)
                {
                    return false;
                }
                else
                {
                    var shawRecipe = shawarma.ShawarmaRecipes;
                    foreach (var recipe in shawRecipe)
                    {
                        if (recipe.Ingradient.TotalWeight < recipe.Weight)
                        {
                            return false;
                        }

                        recipe.Ingradient.TotalWeight -= recipe.Weight;
                    }
                }
                return Commit(context);
            }
        }

        public static bool AddNewRecipe(string shawarmaName, int cookingTime,
            string[] ingradientNames, int[] weights)
        {
            var shawarma = new Shawarma() {CookingTime = cookingTime, ShawarmaName = shawarmaName};
            using (var context = new ShawarmaContext())
            {
                context.Shawarmas.Add(shawarma);
                var ingradients = context.Ingradients.Where(ingr => ingradientNames.Contains(ingr.IngradientName));
                if (ingradients.Count() != ingradientNames.Count())
                {
                    return false;
                }
                else
                {
                    for (int i = 0; i < ingradients.Count(); i++)
                    {
                        string ingrName = ingradientNames[i];
                        int weight = weights[i];
                        context.ShawarmaRecipes.Add(new ShawarmaRecipe()
                        {
                            IngradientId = context.Ingradients.First(ing => ing.IngradientName == ingrName).IngradientId,
                            ShawarmaId = shawarma.ShawarmaId,
                            Weight = weight
                        });
                    }
                }
                Commit(context);
            }
            return false;
        }

        public static bool SetNewPrice(string shawarmaName, decimal newPrice, string sellPointName, string comment)
        {
            using (var context = new ShawarmaContext())
            {
                var shawarma = context.Shawarmas.FirstOrDefault(sh => sh.ShawarmaName == shawarmaName);
                var sellPoint = context.SellingPoints.FirstOrDefault(sp => sp.ShawarmaTitle == sellPointName);
                if (shawarma == null || sellPoint == null)
                {
                    return false;
                }
                var priceController =
                    context.PriceControllers.FirstOrDefault(
                        pc =>
                            pc.Shawarma.ShawarmaName == shawarmaName &&
                            pc.SellingPoint.ShawarmaTitle == sellPointName);
                if (priceController != null)
                {
                    priceController.Price = newPrice;
                    priceController.Comment = comment;
                }
                else
                {
                    context.PriceControllers.Add(new PriceController()
                    {
                        Comment = comment,
                        Price = newPrice,
                        ShawarmaId = shawarma.ShawarmaId,
                        SellingPointId = sellPoint.SellingPointId
                    });
                }
                return Commit(context);
            }
        }

        public static bool AddSellingPointcategory(string sellingCategoryName)
        {
            using (var context = new ShawarmaContext())
            {
                context.SellingPointCategories.Add(new SellingPointCategory()
                {
                    SellingPointCategoryName = sellingCategoryName
                });
                return Commit(context);
            }
        }

        public static bool AddNewSellPoint(string address, string sellCategoryName, string title)
        {
            using (var context = new ShawarmaContext())
            {
                var sellCategory =
                    context.SellingPointCategories.FirstOrDefault(sp => sp.SellingPointCategoryName == sellCategoryName);
                if (sellCategory == null)
                {
                    return false;
                }
                else
                {
                    context.SellingPoints.Add(new SellingPoint()
                    {
                        Address = address,
                        SellingPointCategoryId = sellCategory.SellingPointCategoryId,
                        ShawarmaTitle = title
                    });
                }
                return Commit(context);
            }
        }

        public static bool AddSeller(string name, string shawarmaTitle)
        {
            using (var context = new ShawarmaContext())
            {
                var sellPoint = context.Shawarmas.FirstOrDefault(sp => sp.ShawarmaName == shawarmaTitle);
                if (sellPoint == null)
                {
                    return false;
                }
                context.Sellers.Add(new Seller()
                {
                    SellerName = name,
                    SellerPointId = sellPoint.ShawarmaId
                });
                return Commit(context);
            }
        }

        public static bool AddTimeController(string sellerName, DateTime workStart, DateTime workEnd)
        {
            using (var context = new ShawarmaContext())
            {
                var seller = context.Sellers.FirstOrDefault(slr => slr.SellerName == sellerName);
                if (seller == null)
                {
                    return false;
                }
                else
                {
                    context.TimeControllers.Add(new TimeController()
                    {
                        SellerId = seller.SellerId,
                        WorkStart = workStart,
                        WorkEnd = workEnd
                    });
                }
                return Commit(context);
            }
        }

        public static bool AddOrder(string shawarmaName, DateTime date, string sellerName, int quantity)
        {
            using (var context = new ShawarmaContext())
            {
                var shawarma = context.Shawarmas.FirstOrDefault(sh => sh.ShawarmaName == shawarmaName);
                var seller = context.Sellers.FirstOrDefault(slr => slr.SellerName == sellerName);
                if (shawarma == null || seller == null)
                {
                    return false;
                }

                var orderHeader = new OrderHeader() {OrderDate = date, SellerId = seller.SellerId};
                context.OrderDetails.Add(new OrderDetail()
                {
                    OrderHeaderId = orderHeader.OrderHeaderId,
                    ShawarmaId = shawarma.ShawarmaId,
                    Quantity = quantity
                });
                return Commit(context);
            }
        }

        private static bool Commit(DbContext context)
        {
            try
            {
                context.SaveChanges();
                return true;
            }
            catch (DbUpdateException ex)
            {
                return false;
            }
        }
    }
}
