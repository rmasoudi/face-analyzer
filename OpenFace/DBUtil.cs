using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using LiteDB;
using OpenFace.constants;
using OpenFace.models;
using Supremes;
using Supremes.Nodes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace OpenFace
{
    public class DBUtil
    {
        private static string BASE_PATH = @"D:\sent";
        private static string DB_PATH = @"C:\Users\User\Documents\Visual Studio 2015\Projects\OpenFace\database.db";
        /*
         * https://www.digikala.com/search/category-lipstick/?brand[0]=4593&pageno=1&sortby=4
         * https://www.digikala.com/search/category-lip-gloss/
         * https://www.digikala.com/search/category-lipstick-pen/
         * https://www.digikala.com/search/category-lip-liner/
         * https://www.digikala.com/search/category-foundation/
         * https://www.digikala.com/search/category-powder/
         * https://www.digikala.com/search/category-blush/
         * https://www.digikala.com/search/category-concealer/
         * https://www.digikala.com/search/category-mascara/
         * https://www.digikala.com/search/category-eye-liner/
         * https://www.digikala.com/search/category-eye-pencil/
         * https://www.digikala.com/search/category-eyeshadow/
         * https://www.digikala.com/search/category-eyebrow-pencil/
         */
        public static void insertAll()
        {
            using (var db = new LiteDatabase(DB_PATH))
            {
                int id = 1;
                List<string> tools = Directory.GetDirectories(BASE_PATH).ToList();
                foreach (string tool in tools)
                {
                    try
                    {
                        string toolName = tool.Split('\\')[2];
                        List<string> brandFiles = Directory.GetFiles(BASE_PATH + "\\" + toolName, "*.json").ToList();
                        try
                        {
                            foreach (string brandFile in brandFiles)
                            {
                                string contents = File.ReadAllText(brandFile);
                                BrandModel brandModel = Newtonsoft.Json.JsonConvert.DeserializeObject<BrandModel>(contents);
                                foreach (ProductModel productModel in brandModel.Products)
                                {
                                    try
                                    {
                                        string value = null;
                                        if (productModel.Properties.TryGetValue("نوع", out value))
                                        {
                                            productModel.Type = value;
                                        }
                                        if (productModel.Properties.TryGetValue("شماره رنگ", out value))
                                        {
                                            productModel.ColorCode = value;
                                        }
                                        value = getColor(toolName, productModel.ImageId);
                                        if (value.Length > 0)
                                        {
                                            productModel.Color = value;
                                        }
                                        FinalProductModel product = new FinalProductModel();
                                        product.Tool = toolName;
                                        product.BrandFarsi = brandModel.FarsiName;
                                        product.BrandEnglish = brandModel.EnglishName;
                                        product.Color = productModel.Color;
                                        product.ColorCode = productModel.ColorCode;
                                        product.Title = productModel.Title;
                                        product.Type = productModel.Type;
                                        product.Url = productModel.Url;

                                        var products = db.GetCollection<FinalProductModel>("products");
                                        products.Insert(product);
                                        products.EnsureIndex(x => x.BrandFarsi);
                                        products.EnsureIndex(x => x.BrandEnglish);
                                        products.EnsureIndex(x => x.Color);
                                        id++;
                                    }
                                    catch (Exception e)
                                    {
                                    }
                                }

                            }
                        }
                        catch (Exception e)
                        {
                        }
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
        }

        private static string getColor(string toolName, string imageFileId)
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(BASE_PATH + "\\" + toolName + "\\images\\" + imageFileId + ".jpg");
            Image<Gray, byte> grayImage = image.Convert<Gray, byte>();
            double cannyThreshold = 180.0;
            double circleAccumulatorThreshold = 120;
            Rectangle rect = new Rectangle(grayImage.Width / 2, grayImage.Height / 2, grayImage.Width / 2, grayImage.Height / 2);
            grayImage.ROI = rect;
            CircleF[] circles = CvInvoke.HoughCircles(grayImage, HoughType.Gradient, 2.0, 20.0, cannyThreshold, circleAccumulatorThreshold, image.Width / 20, image.Width / 6);
            if (circles.Length == 0)
            {
                image.Save(BASE_PATH + "\\" + toolName + "\\bad\\" + imageFileId + ".jpg");
            }
            else
            {
                PointF center = new PointF(circles[0].Center.X + rect.Width, circles[0].Center.Y + rect.Height);
                image.Draw(new CircleF(center, circles[0].Radius), new Bgr(Color.Blue), 3);
                Bgr bgr = image[new Point((int)center.X, (int)center.Y)];
                string color = ColorTranslator.ToHtml(Color.FromArgb((int)bgr.Red, (int)bgr.Green, (int)bgr.Blue));
                image.Save(BASE_PATH + "\\" + toolName + "\\output\\" + imageFileId + ".jpg");
                return color;
            }
            return "";
        }

        public static void DeleteBadColors()
        {
            using (var db = new LiteDatabase(DB_PATH))
            {
                var products = db.GetCollection<FinalProductModel>("products");
                List<string> tools = Directory.GetDirectories(BASE_PATH).ToList();
                foreach (string tool in tools)
                {
                    string toolName = tool.Split('\\')[2];
                    List<string> badFiles = Directory.GetFiles(BASE_PATH + "\\" + toolName + "\\bad", "*.jpg").ToList();

                    foreach (string badFile in badFiles)
                    {
                        string[] parts = badFile.Split('\\');
                        string item = parts[parts.Length - 1].Replace(".jpg", "");
                        products.Delete(x => x.Tool.Equals(toolName) && x.ImageId.Equals(item));
                    }

                }
            }
        }

        private static int GetPrice(string url)
        {
            Document doc = Dcsoup.Parse(new Uri(url), 30000);
            Element el = doc.Select(".c-price--primary").Select(".js-price-value").First;
            if (el == null)
            {
                return 0;
            }
            return Int32.Parse(normalizePrice(el.Text));
        }

        private static string normalizePrice(string price)
        {
            price = price.Trim().Replace(",", "").Replace("۱", "1").Replace("۲", "2").Replace("۳", "3").Replace("۴", "4").Replace("۵", "5").Replace("۶", "6").Replace("۷", "7").Replace("۸", "8").Replace("۹", "9").Replace("۰", "0");
            return price;
        }

        public static void GetTypes()
        {
            Dictionary<string, List<string>> map = new Dictionary<string, List<string>>();
            using (var db = new LiteDatabase(DB_PATH))
            {
                var products = db.GetCollection<FinalProductModel>("products");
                List<FinalProductModel> selected = products.FindAll().ToList();
                foreach (FinalProductModel fp in selected)
                {
                    if (fp.ColorCode != null)
                    {
                        List<string> list = null;
                        map.TryGetValue(fp.Tool, out list);
                        if (list == null)
                        {
                            list = new List<string>();
                        }
                        if (!list.Contains(fp.ColorCode))
                        {
                            list.Add(fp.ColorCode);
                        }
                        map.Remove(fp.Tool);
                        map.Add(fp.Tool, list);
                    }

                }
            }
            File.WriteAllText("d:\\colors.json", Newtonsoft.Json.JsonConvert.SerializeObject(map));
        }

        public static void SeparateTables()
        {
            using (var db = new LiteDatabase(DB_PATH))
            {

                var products = db.GetCollection<FinalProductModel>("products");

                List<string> tools = Directory.GetDirectories(BASE_PATH).ToList();
                foreach (string tool in tools)
                {
                    string toolName = tool.Split('\\')[2];
                    List<FinalProductModel> selected = products.Find(x => x.Tool.Equals(toolName)).ToList();
                    var subProducts = db.GetCollection<FinalProductModel>("tool_" + toolName);
                    //subProducts.InsertBulk(selected);
                }
            }
        }

        public static void SetTypes()
        {
            using (var db = new LiteDatabase(DB_PATH))
            {
                foreach (string tool in ToolConstants.TOOLS_ARRAY)
                {
                    var products = db.GetCollection<FinalProductModel>("tool_" + tool);
                    List<FinalProductModel> selected = products.FindAll().ToList();
                    foreach (FinalProductModel fp in selected)
                    {
                        if (fp.Type != null && fp.Type.Length > 0)
                        {
                            string[] parts = fp.Type.Split(',');
                            foreach (string part in parts)
                            {
                                int typeId = FeatureConstants.GetFeatureCode(part);
                                if (typeId > 0)
                                {
                                    fp.Features.Add(typeId);
                                }
                            }
                            //products.Update(fp);
                        }
                    }
                }
            }
        }
        public static void SetPrices()
        {
            using (var db = new LiteDatabase(DB_PATH))
            {
                var products = db.GetCollection<FinalProductModel>("products");
                List<FinalProductModel> selected = products.FindAll().ToList();
                foreach (FinalProductModel fp in selected)
                {
                    try
                    {
                        int price = GetPrice(fp.Url);
                        fp.Price = price;
                        products.Update(fp);

                    }
                    catch (Exception e)
                    {
                    }

                }
            }
        }
        public static void SetImageIds()
        {
            using (var db = new LiteDatabase(DB_PATH))
            {
                var products = db.GetCollection<FinalProductModel>("products");
                List<FinalProductModel> selected = products.FindAll().ToList();
                foreach (FinalProductModel fp in selected)
                {
                    string imageId = GetImageId(fp.Tool, fp.BrandEnglish, fp.Url);
                    fp.ImageId = imageId;
                    products.Update(fp);
                }
            }
        }

        private static string GetImageId(string tool, string brand, string url)
        {
            string contents = File.ReadAllText(BASE_PATH + "\\" + tool + "\\" + brand + ".json");
            BrandModel brandModel = Newtonsoft.Json.JsonConvert.DeserializeObject<BrandModel>(contents);
            foreach (ProductModel p in brandModel.Products)
            {
                if (p.Url.Equals(url))
                {
                    return p.ImageId;
                }
            }
            return null;
        }

        public static Color[] GetToolColors(string tool)
        {
            Color[] result;
            using (var db = new LiteDatabase(DB_PATH))
            {
                var products = db.GetCollection<FinalProductModel>("products");
                products.Delete(x => x.Color == null);
                List<FinalProductModel> selected = products.Find(x => x.Color.Equals("#ffffff")).ToList();
                result = new Color[selected.Count];
                for (int i = 0; i < selected.Count; i++)
                {
                    result[i] = ColorTranslator.FromHtml(selected[i].Color);
                }
            }
            return result;
        }

        public static List<FinalProductModel> GetProducts()
        {
            using (var db = new LiteDatabase(DB_PATH))
            {
                var products = db.GetCollection<FinalProductModel>("tool_lipstick");
                return products.Find(x => true, 0, 10).ToList();
            }
        }


    }
}
