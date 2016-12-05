using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task01.ORM
{
    /// <summary>
    /// LINQ queries 1-6 from TASK
    /// </summary>
    public static class LINQqueries
    {
        /// <summary>
        /// Querie for adding ingridient to database
        /// </summary>
        /// <param name="ingradientName">name of ingridient (for example "tomato")</param>
        /// <param name="categoryName">name of ingridient category (for example "vegetables")</param>
        /// <param name="weight">weight of ingridient (if ingredient already exist it's weight increments)</param>
        /// <returns> bool value wich indicates success of operation </returns>
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

        /// <summary>
        /// querie for adding new ingredient category 
        /// </summary>
        /// <param name="categoryName">inredient category (for example "vegetables")</param>
        /// <returns>bool value wich indicates success of operation</returns>
        public static bool AddIngradientCategory(string categoryName)
        {
            using (var context = new ShawarmaContext())
            {
                context.IngradientCategories.Add(new IngradientCategory() {CategoryName = categoryName});
                return Commit(context);
            }
        }

        /// <summary>
        /// querie for selling shawarma (success selling decrease weight of ingredients)
        /// </summary>
        /// <param name="shawarmaName">shawarma name (for example "slim-double chease")</param>
        /// <returns>bool value wich indicates success of operation</returns>
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

        /// <summary>
        /// querie for adding new recipe to database
        /// </summary>
        /// <param name="shawarmaName"> shawarma name</param>
        /// <param name="cookingTime"> cooking time of this shawarma </param>
        /// <param name="ingradientNames">array of needed ingradients </param>
        /// <param name="weights">array of weights of ingradients</param>
        /// <returns>bool value wich indicates success of operation</returns>
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

        /// <summary>
        /// querie for set new price for shawarma in some sell point
        /// </summary>
        /// <param name="shawarmaName">shawarma name</param>
        /// <param name="newPrice">new price for this shawarma</param>
        /// <param name="sellPointName">name of sell point</param>
        /// <param name="comment">comment for new price (for example "50% for this weekend")</param>
        /// <returns>bool value wich indicates success of operation</returns>
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

        /// <summary>
        /// querie for adding new category of selling point
        /// </summary>
        /// <param name="sellingCategoryName"> category name (for example "bistro","fast food","cafe" etc.)</param>
        /// <returns>bool value wich indicates success of operation</returns>
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

        /// <summary>
        /// querie for adding new selling point
        /// </summary>
        /// <param name="address"> address of this selling point</param>
        /// <param name="sellCategoryName">category of this selling point</param>
        /// <param name="title">title of this selling point</param>
        /// <returns>bool value wich indicates success of operation</returns>
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

        /// <summary>
        /// querie for adding new Seller
        /// </summary>
        /// <param name="name">name of seller</param>
        /// <param name="shawarmaTitle">name of selling point</param>
        /// <returns>bool value wich indicates success of operation</returns>
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

        /// <summary>
        /// querie for adding time controller for exist seller
        /// </summary>
        /// <param name="sellerName">seller name</param>
        /// <param name="workStart">date when seller start work</param>
        /// <param name="workEnd"> date when seller end work</param>
        /// <returns>bool value wich indicates success of operation</returns>
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

        /// <summary>
        /// querie for adding new order
        /// </summary>
        /// <param name="shawarmaName">shawarma name</param>
        /// <param name="date">date-time of order</param>
        /// <param name="sellerName">seller name</param>
        /// <param name="quantity"> quantity of shawarmas</param>
        /// <returns>bool value wich indicates success of operation</returns>
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

        /// <summary>
        /// SaveChanges in context
        /// </summary>
        /// <param name="context">database context</param>
        /// <returns>bool value wich indicates success of operation</returns>
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
