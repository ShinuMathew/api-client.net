using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;
using Restful;
using RestSharp;
using Newtonsoft.Json;
using DocumentFormat.OpenXml;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Json.NET;
using Json.NET.Web;
using System.Xml;
using System.IO;
using System.Collections;
using System.Linq.Expressions;
using System.Linq;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;

namespace WebserviceDemo
{
    public enum WeekDays
    {
        monday, tuesday, wednesday, thursday, friday
    }

    [CodedUITest]
    public class TestRun
    {

        [TestMethod]
        public void test()
        {
            String days = WebserviceDemo.WeekDays.monday.ToString();


            List<String> list = new List<string>();
            IEnumerator enumerator = list.GetEnumerator();

            enumerator.MoveNext();
        }
                
        [TestMethod]
        public void CodedUITestMethod1()
        {
            //Creating a blank request body and passing the base url
            RestClient client = new RestClient("http://wddceqcaapi01:18929");

            //Passing the API url
            RestRequest request = new RestRequest("/consultants/{ContactId}/profile");

            //Passing the method of the service 
            request.Method = Method.GET;

            //Adding the request parameters           
            request.AddParameter("Culture", "en-CA");
            request.AddParameter("SubsidiaryCode", "CA");
            request.AddParameter("ContactId", "49856109");
            //request.AddParameter("OrderType", "1");

            //Executing the service
            IRestResponse response = client.Execute(request);

            //Status Code
            Console.WriteLine("The Response status is :" + response.StatusCode);

            //Response Content
            Console.WriteLine("The response content is :\n" + response.Content);

            //Parsing the content
            dynamic d = JObject.Parse(response.Content);
            //Console.WriteLine(JSONresponse);

            Console.WriteLine(d.RequestContactId);

            //JSonResponse
            //------------------------------------------------------
            // Console.WriteLine("The JSON respone:" + JSONresponse);                      

            //String OrderID = (String)JSONresponse["OrderId"];

            ////---------------------------------------------------------
            //// String ContactID = (String)JSONresponse["RequestedContact"];
            //// Console.WriteLine("The OrderID is: " + OrderID);
            ////---------------------------------------------------------
            //Console.ReadLine();

            //String Country = (String)JSONresponse.SelectToken("Consultant.PrimaryAddress.CountryName");

            //Console.WriteLine("THe Country is: " + Country);

        }
        [TestMethod]
        public void CA_QA_FindProductByCategory()
        {
            RestClient client = new RestClient("http://wddcedglws21:18961");

            RestRequest request = new RestRequest("/products/hierarchies/{HierarchyCode}");

            request.Method = Method.GET;
            request.AddUrlSegment("HierarchyCode", "/MaryKay_CA");
            request.AddParameter("Languages", "en-CA");

            IRestResponse response = client.Execute(request);

            Console.WriteLine("Initiall response" + response.Content);

            JObject JSONObject = JObject.Parse(response.Content);
            Console.WriteLine("The response is: " + JSONObject);

            String hierarchy = null;
            String description = null;

            //Console.WriteLine("The hierarchy is:" + hierarchy);


            int i = 0;
            int j = 0;

            List<String> resp1 = new List<String>();
            do
            {
                hierarchy = (String)JSONObject.SelectToken("Hierarchies[0].HierarchyDetail[" + i + "].HierarchyNodeId");
                i++;
                resp1.Add(hierarchy);

            }
            while (hierarchy != null);

            List<String> resp2 = new List<String>();
            do
            {
                description = (String)JSONObject.SelectToken("Hierarchies[0].HierarchyDetail[" + j + "].Description");
                j++;
                resp2.Add(description);
            }
            while (description != null);


            IRestResponse response1 = null;

            JObject JsonResp1 = null;

            String TotalProd = null;


            /*foreach(var o in resp1)
            {
                try 
            {
                RestRequest request1 = new RestRequest("/products/findbyhierarchy");

                request.Method = Method.GET;

                Console.WriteLine("The total parts in hierarchy: "+o);
                request1.AddParameter("SubsidiaryCode", "CA");
                request1.AddParameter("Languages", "en-CA");
                request1.AddParameter("Hierarchy", o);

                response1 = client.Execute(request1);

                JsonResp1 = JObject.Parse(response1.Content);

                TotalProd = (String)JsonResp1.SelectToken("Total");
                //TotalProd = (String)JsonResp1["Total"];

                //Console.WriteLine(JsonResp1);

                Console.WriteLine(TotalProd);
                Console.WriteLine("------------------------------------------------------");
                Console.WriteLine("------------------------------------------------------");
            }            
            catch(Exception e)
            {
                Console.WriteLine("------------------------------------------------------");
                Console.WriteLine("The hierarchy" + o + "is invalid");
                Console.WriteLine("------------------------------------------------------");
            }
            }*/
            Console.WriteLine("resp1 length" + resp1.Count);
            Console.WriteLine("resp2 length" + resp2.Count);
            for (int o = 0; o < resp1.Count; o++)
            {
                try
                {
                    RestRequest request1 = new RestRequest("/products/findbyhierarchy");

                    request.Method = Method.GET;

                    Console.WriteLine("The total parts in hierarchy: " + resp2[o]);
                    request1.AddParameter("SubsidiaryCode", "CA");
                    request1.AddParameter("Languages", "en-CA");
                    request1.AddParameter("Hierarchy", resp1[o]);

                    response1 = client.Execute(request1);

                    JsonResp1 = JObject.Parse(response1.Content);

                    TotalProd = (String)JsonResp1.SelectToken("Total");
                    //TotalProd = (String)JsonResp1["Total"];

                    //Console.WriteLine(JsonResp1);

                    Console.WriteLine(TotalProd);
                    Console.WriteLine("------------------------------------------------------");
                    Console.WriteLine("------------------------------------------------------");
                }
                catch (Exception e)
                {
                    Console.WriteLine("------------------------------------------------------");
                    Console.WriteLine("The hierarchy" + o + "is invalid");
                    Console.WriteLine("------------------------------------------------------");
                }
            }
        }

        [TestMethod]
        public void CA_QA_FindByhierarchyComposite()
        {
            //Creating a server interaction
            RestClient server = new RestClient("http://wddcedglws21:18961");

            //Passing the service URI
            RestRequest apiUri = new RestRequest("/products/hierarchies/{HierarchyCode}");

            //Specifying the service method
            apiUri.Method = Method.GET;

            //Passing the request params
            apiUri.AddUrlSegment("HierarchyCode", "MaryKay_CA");
            apiUri.AddParameter("Languages", "en-CA");

            //Executing the service
            IRestResponse response = server.Execute(apiUri);

            //Parsing the response
            JObject JsonResponse = JObject.Parse(response.Content);

            String hierarchy = null;
            String description = null;

            List<String> hier = new List<string>();
            List<String> desc = new List<string>();
            int nodes1 = 0;
            int nodes2 = 0;
            do
            {
                hierarchy = (String)JsonResponse.SelectToken("Hierarchies[0].HierarchyDetail[" + nodes1 + "].HierarchyNodeId");
                nodes1++;
                hier.Add(hierarchy);

            } while (hierarchy != null);

            do
            {
                description = (String)JsonResponse.SelectToken("Hierarchies[0].HierarchyDetail[" + nodes2 + "].Description");
                nodes2++;
                desc.Add(description);
            } while (description != null);

            Hashtable hsb = new Hashtable();
            try
            {
                for (int index = 0; index < hier.Count - 1; index++)
                {
                    String hier1 = hier[index];
                    String desc1 = desc[index];
                    hsb.Add(hier1, desc1);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Invalid hierarchy");
            }
            int cst = 0;

            foreach (DictionaryEntry d in hsb)
            {
                //Console.WriteLine(cst+++" :" + d.Key + " : " + d.Value);
                RestRequest requestServ1 = new RestRequest("/products/findbyhierarchy");

                requestServ1.Method = Method.GET;
                requestServ1.AddParameter("SubsidiaryCode", "CA");
                requestServ1.AddParameter("Languages", "en-CA");
                requestServ1.AddParameter("hierarchy", d.Key);

                IRestResponse respServ1 = server.Execute(requestServ1);

                JObject parsedContent = JObject.Parse(respServ1.Content);

                String totalProd = (String)parsedContent.SelectToken("Total");

                Console.WriteLine(d.Value + " :" + totalProd + " Products");
                Console.WriteLine("-------------------------------------------------");



            }

        }

        [TestMethod]
        public void GetAllProducts()
        {
            List<MarketSkus> wdAllProducts = new List<MarketSkus>();
            List<MarketSkus> wdSection1Products = new List<MarketSkus>();
            List<MarketSkus> wdSection2Products = new List<MarketSkus>();
            List<MarketSkus> wdDiscontinuedFutureProducts = new List<MarketSkus>();
            List<MarketSkus> wdDiscontinuedPastProducts = new List<MarketSkus>();
            List<MarketSkus> wdExpiredFutureProducts = new List<MarketSkus>();
            List<MarketSkus> wdExpiredPastProducts = new List<MarketSkus>();
            List<MarketSkus> wdExcludedProducts = new List<MarketSkus>();
            List<MarketSkus> wdNonOrderableProducts = new List<MarketSkus>();
            List<MarketSkus> wdOrderEntrySourceExclude = new List<MarketSkus>();
            List<MarketSkus> wdOrderTypeExclude = new List<MarketSkus>();
            List<MarketSkus> wdActivityStatusExclude = new List<MarketSkus>();
            List<MarketSkus> wdCareerLevelExclude = new List<MarketSkus>();
            List<MarketSkus> wdNewProductsPastDate = new List<MarketSkus>();
            List<MarketSkus> wdNewProductsFutureDate = new List<MarketSkus>();
            List<MarketSkus> wdLimitedEdition = new List<MarketSkus>();

            RestClient client = new RestClient(System.Environment.GetEnvironmentVariable("CA_HOST"));

            RestRequest request = new RestRequest("/products/findbylanguage");

            request.Method = Method.GET;
            request.AddParameter("languages", "en-CA");
            request.AddParameter("SubsidiaryCode", "CA");
            request.AddParameter("PageSize", "1500");

            IRestResponse response = client.Execute(request);
            Console.WriteLine("*****************************************************************");
            Console.WriteLine("GetProductsByLanguage service requested");
            Console.WriteLine("*****************************************************************");

            dynamic d = JObject.Parse(response.Content);

            Console.WriteLine("The GetProductsByLanguage service response is: {0}", response.StatusCode);

            String NoOfProducts = d.Total.ToString();

            int TotalProducts = Convert.ToInt32(NoOfProducts);
            int productsFound = 1;

            //Get all the required products and store it in its respective arraylists

            try
            {
                for (int count = 1; count < TotalProducts; count++)
                {
                    //All products
                    String sku = d.Products[count].MarketSKU;
                    wdAllProducts.Add((new MarketSkus { skuID = sku }));

                    //Section1 Products
                    if (d.Products[count].ProductSectionCode == "1")
                    {
                        wdSection1Products.Add(new MarketSkus { skuID = sku });
                    }

                    //Products with Past Discontinued date
                    if (d.Products[count].ConsultantDiscontinuedDate < DateTime.Now)
                    {
                        wdDiscontinuedPastProducts.Add((new MarketSkus { skuID = sku }));
                    }

                    //Products with Future Expiration date
                    if (d.Products[count].ConsultantProductExpirationDate > DateTime.Now)
                    {
                        wdExpiredFutureProducts.Add((new MarketSkus { skuID = sku }));
                    }

                    //Products with Past Discontinued date
                    if (d.Products[count].ConsultantProductExpirationDate < DateTime.Now)
                    {
                        wdExpiredPastProducts.Add((new MarketSkus { skuID = sku }));
                    }

                    //OrderEntrySource excluded Products
                    if (d.Products[count].OrderEntrySourceExclude != null)
                    {
                        wdOrderEntrySourceExclude.Add((new MarketSkus { skuID = sku }));
                    }

                    //OrderType excluded Products
                    if (d.Products[count].OrderTypeExclude != null)
                    {
                        wdOrderTypeExclude.Add((new MarketSkus { skuID = sku }));
                    }

                    //ActivityStatus excluded Products
                    if (d.Products[count].ActivityStatusExclude != null)
                    {
                        wdActivityStatusExclude.Add((new MarketSkus { skuID = sku }));
                    }

                    //CareerLevel excluded Products
                    if (d.Products[count].CareerLevelExclude != null)
                    {
                        wdCareerLevelExclude.Add((new MarketSkus { skuID = sku }));
                    }

                    //New labled Products with Past end date
                    if (d.Products[count].ConsultantNewProductEndDate < DateTime.Now)
                    {
                        wdNewProductsPastDate.Add((new MarketSkus { skuID = sku }));
                    }

                    //New labled Products with fututre end date
                    if (d.Products[count].ConsultantNewProductEndDate > DateTime.Now)
                    {
                        wdNewProductsFutureDate.Add((new MarketSkus { skuID = sku }));
                    }

                    //Limited Edition Products
                    if (d.Products[count].LimitedEditionProduct == true)
                    {
                        wdLimitedEdition.Add((new MarketSkus { skuID = sku }));
                    }
                    productsFound++;
                    //DateTime date= Convert.ToDateTime(d.Products[count].ConsultantNewProductEndDate);
                    //Console.WriteLine("The date format: " + date);
                }
            }

            catch (NullReferenceException nre)
            {
                Console.WriteLine("there is no such parameter");
            }
            catch (Exception e)
            {
                Console.WriteLine("system encountered {0} exception", e.GetType());
            }

            Console.WriteLine("=========================================================================");
            Console.WriteLine("Total no. of products are: {0}.", TotalProducts);
            Console.WriteLine("Total no. of products actually found: {0}.", productsFound);
            Console.WriteLine("=========================================================================");
            Console.WriteLine("-----------------------------------------------------------------------");
            Console.WriteLine("Total no. of Section1 Products:  {0}", wdSection1Products.Count);
            Console.WriteLine("-----------------------------------------------------------------------");
            Console.WriteLine("Total no. of Section2 Products:  {0}", wdSection2Products.Count);
            Console.WriteLine("-----------------------------------------------------------------------");
            Console.WriteLine("Total no. of Products with Discontinued Future date:  {0}", wdDiscontinuedFutureProducts.Count);
            Console.WriteLine("-----------------------------------------------------------------------");
            Console.WriteLine("Total no. of Products with Discontinued Past date:  {0}", wdDiscontinuedPastProducts.Count);
            Console.WriteLine("-----------------------------------------------------------------------");
            Console.WriteLine("Total no. of Products with Expired Future date:  {0}", wdExpiredFutureProducts.Count);
            Console.WriteLine("-----------------------------------------------------------------------");
            Console.WriteLine("Total no. of Products with Expired Past date:  {0}", wdExpiredPastProducts.Count);
            Console.WriteLine("-----------------------------------------------------------------------");
            Console.WriteLine("Total no. of Non Orderable Products:  {0}", wdNonOrderableProducts.Count);
            Console.WriteLine("-----------------------------------------------------------------------");
            Console.WriteLine("Total no. of Entry source excluded products:  {0}", wdOrderEntrySourceExclude.Count);
            Console.WriteLine("-----------------------------------------------------------------------");
            Console.WriteLine("Total no. of Activity status excluded products:  {0}", wdActivityStatusExclude.Count);
            Console.WriteLine("-----------------------------------------------------------------------");
            Console.WriteLine("Total no. of Career level excluded products  {0}", wdCareerLevelExclude.Count);
            Console.WriteLine("-----------------------------------------------------------------------");
            Console.WriteLine("Total no. of new lable products with future end date:  {0}", wdNewProductsFutureDate.Count);
            Console.WriteLine("-----------------------------------------------------------------------");
            Console.WriteLine("Total no. of Limited edition products  {0}", wdLimitedEdition.Count);
            Console.WriteLine("-----------------------------------------------------------------------");
            Console.WriteLine("Total no. of new lable products with Past end date:  {0}", wdNewProductsPastDate.Count);
            Console.WriteLine("*************************************************************************");


            //var top10SKUs = ((from String s in wdSection1Products
                              //where s.Contains("101")
                              //select s).Take(10));

            Console.WriteLine("Products with SKU Id starting with \"101\":");


            


        }

        [TestMethod]
        public void Additems()
        {

                IRestClient client = null;
                RestRequest req = null;
                IRestResponse res = null;

                String addItemsToCartResponse = null;
                String getReviewOrderCompositeResponse = null;
                String deleteItemsResponse = null;
                String orderId = "f5ca7c95-359b-4147-8aca-df3968b6404b";
                String OrderType = "1";
                String EntrySourceId = "2";
                String contactId = "21864444";
                String processingMessage = null;
                int itemQty = 1;
                String partID="10026941";

                String addItemsRequest = "{\"items\":[{\"Sku\": \"10026955\",\"Quantity\": 10}],\"SubsidiaryCode\":\"CA\",\"Culture\":\"en-CA\",\"OrderType\":\"1\"}}";
                String Items="{\"Items\":[{\"Sku\":\""+partID+"\",\"Quantity\":1,\"Status\":\"Valid\"}]}";

              
                    client = new RestClient("http://wddceqcaapi01:18929");

                    req = new RestRequest("consultants/{ContactId}/cartitems", Method.PUT);
                    req.AddUrlSegment("ContactId", contactId);                    
                    req.AddParameter("application/json", addItemsRequest, ParameterType.RequestBody);
           
                               
                    res = client.Execute(req);

                    List<String> ErrorItems = JsonConvert.DeserializeObject<AddItemsResponse>(res.Content).ErrorItems;
                    dynamic addItemsToCartResp = JObject.Parse(res.Content);

                    String resStatusCode = res.ResponseStatus.ToString();

                    if(res.StatusCode.ToString()!="200")
                    {
                        Assert.Fail("AddItemsToShoppingCart returned the following status code: "+ resStatusCode);
                    }
                    //ErrorItems.Count == 0;
                    //var ErrorItems = addItemsToCartResp.ErrorItems.ToString();

            //Adding part to cart using AdditemsToShoppoingCart service

            //client.AddDefaultHeader("Content-Type", "application/json");

            //req.AddHeader("Accept", "text/json");
            //req.AddHeader("Content-Type",  "application/json");
            //RootObject yourobject = new RootObject();
            //yourobject.Items.Add(new Item { Sku = partID, Quantity = 1, Status = "Valid" });
            //yourobject.OrderType = "1";
            //yourobject.SubsidiaryCode = "CA";
            //yourobject.Culture = "en-CA";
            //var json = JsonConvert.SerializeObject(yourobject);
            //var json = JsonConvert.SerializeObject(addItemsRequest);
            //req.AddParameter("SubsidiaryCode", "CA");
            //req.AddParameter("Culture", "en-CA");
            //req.AddParameter("OrderId", orderId);

            //req.RequestFormat = DataFormat.Json;
            //req.AddJsonBody(json);

            //req.AddJsonBody(Items); 

        }


        [TestMethod]
        public void GetDiscountProducts()
        {
            RestClient client = new RestClient(System.Environment.GetEnvironmentVariable("CA_HOST"));

            RestRequest request = new RestRequest("/products/findbylanguage");

            request.Method = Method.GET;
            request.AddParameter("languages", "es-MX");
            request.AddParameter("SubsidiaryCode", "MX");
            request.AddParameter("PageSize", "1500");
            Console.WriteLine("GetProducts service started");

            List<String> ListOfSec1Products = new List<String>();

            List<String> ListOfSec2Products = new List<String>();

            IRestResponse response = client.Execute(request);

            dynamic d = JObject.Parse(response.Content);

            String NoOfProducts = d.Total.ToString();

            int TotalProducts = Convert.ToInt32(NoOfProducts);
            int productsFound = 1;

            try
            {
                for (int count = 1; count < TotalProducts-1; count++)
                {
                    if (d.Products[count].OrderEntrySourceExclude == null &&  d.Products[count].ActivityStatusExclude == null && d.Products[count].CareerLevelExclude == null)
                    {
                        if (d.Products[count].ProductClass != "60e6f7c7612b44cd903f09567f5a978e" && d.Products[count].SKUIsActive == true && d.Products[count].IncludeInSearch == true && d.Products[count].ProductIsActive == true)
                        {
                            if (d.Products[count].ConsultantProductStartDate < DateTime.Now && d.Products[count].ConsultantProductExpirationDate > DateTime.Now)
                            {
                                if (d.Products[count].ConsultantDiscontinuedDate == null || d.Products[count].ConsultantDiscontinuedDate > DateTime.Now)
                                {

                                    if (d.Products[count].ProductSectionCode == "1")
                                    {
                                        ListOfSec1Products.Add(d.Products[count].MarketSKU.ToString());
                                    }

                                    if (d.Products[count].ProductSectionCode == "2")
                                    {
                                        ListOfSec2Products.Add(d.Products[count].MarketSKU.ToString());
                                    }

                                }
                            }
                        }
                    }
                }
            }

            catch (NullReferenceException nre)
            {
                Console.WriteLine("there is no such parameter");
            }
            catch (Exception e)
            {
                Console.WriteLine("system encountered {0} exception", e.GetType());
            }



            Console.WriteLine("List of Sec1 products. {0} products", ListOfSec1Products.Count);
            Console.WriteLine("======================");
            foreach (String products in ListOfSec1Products)
            {
                Console.WriteLine("{0}", products);
            }

            Console.WriteLine("List of Sec2 products. {0} products", ListOfSec2Products.Count);
            Console.WriteLine("======================");
            foreach (String products in ListOfSec2Products)
            {
                Console.WriteLine("{0}", products);
            }
        }

        [TestMethod]
        public static void GetValidProducts()
        {
            List<String> ListOfSec1Products = new List<String>();
            List<String> ListOfSec2Products = new List<String>();


            Console.WriteLine("Getting all the Valid section 1 and 2 products");
            Console.WriteLine("************************************************************");
            RestClient client = new RestClient(System.Environment.GetEnvironmentVariable("CA_HOST"));

            RestRequest request = new RestRequest("/products/findbylanguage");

            request.Method = Method.GET;
            request.AddParameter("languages", "en-CA");
            request.AddParameter("SubsidiaryCode", "CA");
            request.AddParameter("PageSize", "1500");
            Console.WriteLine("GetProducts service started");

            IRestResponse response = client.Execute(request);

            dynamic d = JObject.Parse(response.Content);

            String NoOfProducts = d.Total.ToString();

            int TotalProducts = Convert.ToInt32(NoOfProducts);
            int productsFound = 1;

            try
            {
                for (int count = 1; count < TotalProducts; count++)
                {
                    if (d.Products[count].OrderEntrySourceExclude == null && d.Products[count].OrderTypeExclude == null && d.Products[count].ActivityStatusExclude == null && d.Products[count].CareerLevelExclude == null)
                    {
                        if (d.Products[count].ProductClass != "60e6f7c7612b44cd903f09567f5a978e" && d.Products[count].SKUIsActive == true && d.Products[count].IncludeInSearch == true && d.Products[count].ProductIsActive == true)
                        {
                            if (d.Products[count].ConsultantProductStartDate < DateTime.Now && d.Products[count].ConsultantProductExpirationDate > DateTime.Now)
                            {
                                if (d.Products[count].ConsultantDiscontinuedDate == null || d.Products[count].ConsultantDiscontinuedDate > DateTime.Now)
                                {

                                    if (d.Products[count].ProductSectionCode == "1")
                                    {
                                        String sec1product = (d.Products[count].MarketSKU);
                                        ListOfSec1Products.Add(sec1product);
                                    }

                                    if (d.Products[count].ProductSectionCode == "2")
                                    {
                                        String sec2product = (d.Products[count].MarketSKU);
                                        ListOfSec2Products.Add(sec2product);
                                    }

                                }
                            }
                        }
                    }
                }
            }

            catch (NullReferenceException nre)
            {
                Console.WriteLine("there is no such parameter");
            }
            catch (Exception e)
            {
                Console.WriteLine("system encountered {0} exception", e.GetType());
            }
            Console.WriteLine("List of Valid Sec1 products. {0} products", ListOfSec1Products.Count);
            Console.WriteLine("======================");
            //foreach (String products in Variables.Variables.ListOfSec1Products)
            //{
            //    Console.WriteLine("{0}", products);
            //}

            Console.WriteLine("List of Valid Sec2 products. {0} products", ListOfSec1Products.Count);
            Console.WriteLine("======================");
        }
        [TestMethod]
         public void rtest()
        {
            String s = "testString";
            String s1 = s.Substring(2, 8);
            Console.WriteLine(s1);
        }
        
        [TestMethod]
        public void GetAllProductsOOPS()
        {
            List<String> validProduct = new List<string>();
            List<MarketSkus> wdAllProducts = new List<MarketSkus>();
            List<oosProducts> wdoutOfStock = new List<oosProducts>();
            
            System.DateTime OrderDate;
            //GetOrderDate service response
            RestClient client = new RestClient("http://wddceqamapi01:18929");

            RestRequest getOrderDate = new RestRequest("/orders/orderdate");

            getOrderDate.Method = Method.GET;
            getOrderDate.AddParameter("languages", "es-MX");
            getOrderDate.AddParameter("SubsidiaryCode", "MX");

            IRestResponse getOrderDateResponse = client.Execute(getOrderDate);
            Console.WriteLine("*****************************************************************");
            Console.WriteLine("GetOrderDate service requested");
            Console.WriteLine("*****************************************************************");

            dynamic d = JObject.Parse(getOrderDateResponse.Content);

            String Date = d.OrderDateLocal;
            OrderDate = Convert.ToDateTime(Date);
                        
            //GetOutOfStock service response.
            RestClient client1 = new RestClient("http://wddceqamapi01:18929");

            RestRequest getOutOfStockRequest = new RestRequest("/inventory/OutOfStock");

            getOutOfStockRequest.Method = Method.GET;
            getOutOfStockRequest.AddParameter("languages", "es-MX");
            getOutOfStockRequest.AddParameter("SubsidiaryCode", "MX");

            IRestResponse getOutOfStockResponse = client1.Execute(getOutOfStockRequest);
            Console.WriteLine("*****************************************************************");
            Console.WriteLine("GetOutOfStockProduct service requested");
            Console.WriteLine("*****************************************************************");

            //Console.WriteLine(getOutOfStockResponse.StatusCode);
            //Console.WriteLine(getOutOfStockResponse.Content);
            dynamic d1 = JObject.Parse(getOutOfStockResponse.Content);
            int i=1;
            try
            {
                while(true)
                {
                    wdoutOfStock.Add(new oosProducts(){skuID= d1.ProductsStatus[i].Sku.ToString()});
                    i++;
                }
            }
            catch(Exception e)
            {

            }                     

            //GetproductsByLanguage service response.
            RestClient client2 = new RestClient(System.Environment.GetEnvironmentVariable("CA_HOST"));

            RestRequest getProductsByLanguageRequest = new RestRequest("/products/findbylanguage");

            getProductsByLanguageRequest.Method = Method.GET;
            getProductsByLanguageRequest.AddParameter("languages", "es-MX");
            getProductsByLanguageRequest.AddParameter("SubsidiaryCode", "MX");
            getProductsByLanguageRequest.AddParameter("PageSize", "1500");

            IRestResponse getProductsByLanguageResponse = client2.Execute(getProductsByLanguageRequest);
            Console.WriteLine("*****************************************************************");
            Console.WriteLine("GetProductsByLanguage service requested");
            Console.WriteLine("*****************************************************************");

            dynamic d2 = JObject.Parse(getProductsByLanguageResponse.Content);

            Console.WriteLine("The GetProductsByLanguage service response is: {0}", getProductsByLanguageResponse.StatusCode);

            String NoOfProducts = d2.Total.ToString();

            int TotalProducts = Convert.ToInt32(NoOfProducts);
            int productsFound = 1;

            //Get all the required products and store it in its respective arraylists

            try
            {
                for (int count = 1; count < TotalProducts; count++)
                {
                    String SKUID = "";
                    String PRODUCTSECTIONCODE = "";           
                    String ORDERENTRYSOURCE ="";
                    String ORDERTYPE = "";
                    String ACTIVITYSTATUS = "";
                    String CAREERLEVEL = "";
                    bool LIMITEDEDITION = false;
                    String PRODUCTCLASS = "";
                    String CHILDREN = "";
                    bool SKUISACTIVE = false;
                    bool INCLUDEINSEARCH = false;
                    bool PRODUCTISACTIVE = false;

                    DateTime discontinuedDateformat = DateTime.MinValue;
                    DateTime expiredDateformat = DateTime.MinValue;
                    DateTime startDateformat = DateTime.MinValue;
                    DateTime newstartDateformat = DateTime.MinValue;
                    //All products
                    try
                    {
                        SKUID = d2.Products[count].MarketSKU.ToString();
                    }
                    catch(Exception ex)
                    {
                        //Console.WriteLine("SKUID does not exist for the product");
                        SKUID = null;
                    }
                    
                    //Section code
                    try
                    {
                        PRODUCTSECTIONCODE = d2.Products[count].ProductSectionCode.ToString();
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine("PRODUCTSECTIONCODE does not exist for the product");
                        PRODUCTSECTIONCODE = null;
                    }
                    
                    //Discontinued date
                    try
                    {
                        discontinuedDateformat = Convert.ToDateTime(d2.Products[count].ConsultantDiscontinuedDate.ToString());
                    }
                    catch (Exception ex)
                    {
                        discontinuedDateformat = DateTime.MinValue;                        
                    }

                    //Expired Date
                    try
                    {
                        expiredDateformat = Convert.ToDateTime(d2.Products[count].ConsultantProductExpirationDate.ToString());
                    }
                    catch(Exception ex)
                    {
                        expiredDateformat = DateTime.MinValue;
                    }
               
                    //OrderEntrysource
                    try
                    {
                        ORDERENTRYSOURCE = d2.Products[count].OrderEntrySourceExclude.ToString();
                    }
                    catch(Exception ex)
                    {
                        ORDERENTRYSOURCE = null;
                    }

                    //OrderType
                    try
                    {
                        ORDERTYPE = d2.Products[count].OrderTypeExclude.ToString();
                    }
                    catch (Exception ex)
                    {
                        ORDERTYPE = null;
                    }

                    //ActivityStatus
                    try
                    {
                        ACTIVITYSTATUS = d2.Products[count].ActivityStatusExclude.ToString();
                    }
                    catch (Exception ex)
                    {
                        ACTIVITYSTATUS = null;
                    }

                    //CareerLevel
                    try
                    {
                        CAREERLEVEL = d2.Products[count].CareerLevelExclude.ToString();
                    }
                    catch (Exception ex)
                    {
                        CAREERLEVEL = null;
                    }

                    //StartDate
                    try
                    {
                        startDateformat = Convert.ToDateTime(d2.Products[count].ConsultantProductStartDate.ToString());
                    }
                    catch (Exception ex)
                    {
                        startDateformat = DateTime.MinValue;
                    }

                    //NewStartDate
                    try
                    {
                        newstartDateformat = Convert.ToDateTime(d2.Products[count].ConsultantProductStartDate.ToString());
                    }
                    catch (Exception ex)
                    {
                        newstartDateformat = DateTime.MinValue;
                    }

                    //LimitedEdition
                    try
                    {
                        LIMITEDEDITION = d2.Products[count].LimitedEditionProduct;
                    }
                    catch(Exception ex)
                    {
                        LIMITEDEDITION = false;
                    }

                    //ProductClass
                    try
                    {
                        PRODUCTCLASS = d2.Products[count].ProductClass.ToString();
                    }
                    catch(Exception ex)
                    {
                        PRODUCTCLASS = null;
                    }

                    //Children
                    try
                    {
                        CHILDREN = d2.Products[count].Children[0].ChildrenID.ToString();                       
                    }
                    catch(Exception ex)
                    {
                        CHILDREN = null;
                    }

                    //SKUisActive
                    try
                    {
                        SKUISACTIVE = d2.Products[count].SKUIsActive;
                    }
                    catch(Exception ex)
                    {
                        SKUISACTIVE = false;
                    }

                    //IncludeInSearch
                    try
                    {
                        INCLUDEINSEARCH = d2.Products[count].IncludeInSearch;
                    }
                    catch(Exception ex)
                    {
                        INCLUDEINSEARCH = false;
                    }

                    //ProductIsActive
                    try
                    {
                        PRODUCTISACTIVE = d2.Products[count].ProductIsActive;
                    }                  
                    catch(Exception ex)
                    {
                        PRODUCTISACTIVE = false;
                    }
                    wdAllProducts.Add((new MarketSkus
                    {
                        skuID = SKUID,
                        ProductSectionCode = PRODUCTSECTIONCODE,
                        ConsultantDiscontinuedDate = discontinuedDateformat,
                        ConsultantProductExpirationDate = expiredDateformat,
                        OrderEntrySourceExclude = ORDERENTRYSOURCE,
                        OrderTypeExclude = ORDERTYPE,
                        ActivityStatusExclude = ACTIVITYSTATUS,
                        CareerLevelExclude = CAREERLEVEL,
                        ConsultantProductStartDate = startDateformat,
                        ConsultantNewProductEndDate = newstartDateformat,
                        LimitedEditionProduct = LIMITEDEDITION,
                        productClass=PRODUCTCLASS,             
                        childProduct=CHILDREN,
                        skuIsActive=SKUISACTIVE,
                        includeInSearch=INCLUDEINSEARCH,
                        productIsActive=PRODUCTISACTIVE
                    }));

                    productsFound++;
                }
                Console.WriteLine("Total products executed: " + productsFound );
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception Occured. GetAllProduct stopped at " + productsFound + "th product");
            }

            List<String> product = ((from sku1 in wdAllProducts
                            where (sku1.ProductSectionCode == "1" || sku1.ProductSectionCode == "2") && sku1.ActivityStatusExclude == null
                            && sku1.OrderEntrySourceExclude == null && sku1.CareerLevelExclude == null
                            && sku1.OrderTypeExclude == null && (sku1.ConsultantDiscontinuedDate == DateTime.MinValue || sku1.ConsultantDiscontinuedDate > OrderDate)
                            && (sku1.ConsultantProductExpirationDate == DateTime.MinValue || sku1.ConsultantProductExpirationDate > OrderDate) && sku1.productClass != "60e6f7c7612b44cd903f09567f5a978e"
                            && sku1.ConsultantProductStartDate < OrderDate && sku1.childProduct == null
                            && sku1.skuIsActive==true && sku1.productIsActive==true && sku1.includeInSearch==true
                            select sku1.skuID).ToList());

            List<String> labledProduct= ((from sku1 in wdAllProducts
                                        where sku1.ProductSectionCode == "1" && sku1.ActivityStatusExclude == null
                            && sku1.OrderEntrySourceExclude == null && sku1.CareerLevelExclude == null
                            && sku1.OrderTypeExclude == null && (sku1.ConsultantDiscontinuedDate == DateTime.MinValue || sku1.ConsultantDiscontinuedDate > OrderDate)
                            && (sku1.ConsultantProductExpirationDate == DateTime.MinValue || sku1.ConsultantProductExpirationDate > OrderDate) && sku1.productClass != "60e6f7c7612b44cd903f09567f5a978e"
                            && sku1.ConsultantProductStartDate < OrderDate && (sku1.LimitedEditionProduct == true) //sku1.ConsultantNewProductEndDate > OrderDate || 
                            && sku1.skuIsActive==true && sku1.productIsActive==true && sku1.includeInSearch==true
                            select sku1.skuID).ToList());


            List<String> oosProduct = (from sku2 in wdoutOfStock
                             select sku2.skuID).ToList();

            Console.WriteLine("Total no. of OOS products: {0}" ,oosProduct.Count);
                                   
            foreach (String skuid in oosProduct)
            {
                labledProduct.Remove(skuid);                     
            }

            Console.WriteLine("Get all the valid product. Total no. of products: " + labledProduct.Count);

            Console.WriteLine("Labled products are: " + labledProduct.Count);
            foreach (String validSKUID in labledProduct)
            {
                Console.WriteLine(validSKUID);
            }
                        
        }    

        [TestMethod]
        public static void tester()
        {
            String test = "Delete from basket..AddressHistory  where ConsultantID='$ConsultantID'";

        }
 
        [TestMethod]
        public void CreateAddressHistory()
        {
            String addressJSON = "{\"SubsidiaryCode\":\"CA\",\"Culture\":\"en-CA\",\"Name\": \"QATester\",\"Line1\": \"23 Robinson drive\"," +
                                    "\"Line2\": \"\",\"City\": \"Fredericton\",\"State\": \"NB\",\"Zip\": \"e3a1l7\",\"County\": \"CA\",\"CountryId\": 103,"+
                                    "\"Phone1\": \"5197332343\",\"IsDefault\": false}";
            Console.WriteLine(addressJSON);

            JObject obj = JObject.Parse(addressJSON);

            Console.WriteLine(obj);
            String addressDetails;

            addressDetails = "ContactId=21864438|SubsidiaryCode=CA|Culture=en-CA|Name=tester|Line1=57DIVISION ST N|Line2=57 DIVISION ST N|City=KINGSVILLE|State=ON|Zip=N9Y 1E1|County=CA|CountryId=6|Phone1=5197332343|Phone2=5197332343|EmailAddress=test@corp.com";

            String[] addressParam = addressDetails.Split('|');

            String[] paramKeys;
            String[] paramValues;          
            
            String url = "http://wddceqcaapi01:18929";
            String apiURI = "/consultants/{ContactId}/addresses";

            RestClient client = new RestClient(url);
            RestRequest request = new RestRequest(apiURI, Method.POST);

            //request.AddHeader("Accept", "application/json");
            //request.AddHeader("Content-Type", "application/json");
            for (int i = 0; i < addressParam.Length; i++)
            {
                request.AddParameter(addressParam[i].Split('=')[0] , addressParam[i].Split('=')[1]);
            }
            //request.AddParameter("ContactId", "49856109");

            //request.AddParameter("application/json", obj, ParameterType.RequestBody);
            //request.RequestFormat = DataFormat.Json;
            //request.AddJsonBody(obj);

            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
        }

        [TestMethod]
        public void ConfigParsing()
        {
            RestClient client = new RestClient("http://wddceqcaws21:18901");

            RestRequest request = new RestRequest("/taxservice/CATaxservice.asmx/GetTaxRateDetail", Method.GET);

            request.AddParameter("OrderDate", "6/1/2018 12:00:00 AM");
            request.AddParameter("PurchaserID", "A16665");
            request.AddParameter("City", "null");
            request.AddParameter("State", "SK");
            request.AddParameter("County", "null");
            request.AddParameter("ZipCode", "null");

            IRestResponse response = client.Execute(request);

            String responseContent = response.Content;

            //JObject jsonObject = JObject.Parse(responseContent);
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(responseContent);

            foreach(XmlNode node in xmlDoc.ChildNodes[1].ChildNodes)
            {
                try
                {
                    String LineItemTaxDetail = node.Attributes["LineItemNumber"].Value;
                }
                catch(NullReferenceException nrx)
                {
                    Console.WriteLine(nrx.Message);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            
        }

        [TestMethod]
        public void Xmlparsing()
        {
            int childNodeCount = 0;
            
            Dictionary<int, ShippingFee> shippingFeeDetails = new Dictionary<int, ShippingFee>();
            XmlDocument xmlRes = new XmlDocument();
            xmlRes.Load("file://wddceqcaapi01/OctopusDeployments/EcommServices/Config/CA/ShippingFee.config");

            foreach (XmlNode node in xmlRes.DocumentElement.ChildNodes[0].ChildNodes)
            {
                childNodeCount++;
                try
                {
                   int shippingMethodID = Int32.Parse(node.Attributes["ID"].Value);
                   shippingFeeDetails.Add(shippingMethodID, new ShippingFee() { orderLowerThreshold = float.Parse(node.ChildNodes[0].Attributes["orderTotalMax"].Value),
                                                                                orderUpperThreshold = float.Parse(node.ChildNodes[1].Attributes["orderTotalMin"].Value),
                                                                                lowerFreight = float.Parse(node.ChildNodes[0].Attributes["value"].Value),
                                                                                upperFreight = float.Parse(node.ChildNodes[1].Attributes["value"].Value),
                                                                              });
                    
                }               
                catch(NullReferenceException nex)
                {
                    Console.WriteLine(node.ParentNode+"'s child node no. "+childNodeCount+" is not a valid one");
                }
                
            }

        }
        [TestMethod]
        public void TFSApiDemo()
        {
            Double d = -455.98;

            var p = Math.Floor(d);  //455.0         -455.0
            var c = Math.Ceiling(d);    //456.0     -456.0
            var t = Math.Truncate(d);   //455       -456.0
            var r = Math.Round(d);  //456.0         -455.0/-456.0
        }

        [TestMethod]
        public void AddItemsToShoppingCart()
        {

        }

        [TestMethod]
        public void ReadDoc()
        {
            String userName = Environment.UserName;
            String directory = @"C:\Users\{UserName}\Downloads";
            directory = directory.Replace("{UserName}", userName);
            String file = "OrderDetail";

            String[] array = Directory.GetFiles(directory);
           // String[] array = Directory.GetDirectories(directory);
            foreach(String dir in array)
            {
                if(dir.Contains(file) && dir.EndsWith(".pdf"))
                {
                    File.Delete(dir);
                }
            }

            if (File.Exists(directory))
            {
                Console.WriteLine("Directory exists");
                File.Delete(directory);
                Console.WriteLine("Directory deleted");
            }
            if (File.Exists(directory))
            {
                Console.WriteLine("Directory exists");
                //File.Delete(directory);
            }
            else
            {
                Console.WriteLine("No such directories");
            }
        }
}

    public class ShippingFee
    {        
        public float orderLowerThreshold { get; set; }
        public float orderUpperThreshold { get; set; }
        public float lowerFreight { get; set; }
        public float upperFreight { get; set; }
        public float onlySection2 { get; set; }
    }
    public class Item
    {
        public string Sku { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
    }

    public class RootObject
    {
        public List<Item> Items = new List<Item>();
        public string SubsidiaryCode { get; set; }
        public string Culture { get; set; }
        public string OrderType { get; set; }

    }

    public class AddItemsResponse
    {
        public long RequestContactId { get; set; }
        public List<String> ProcessingMessages = new List<String>();
        public String Message { get; set; }
        public List<String> ErrorItems = new List<String>();
    }
    public class MarketSkus
    {
        public String skuID;
        public String ProductSectionCode;
        public System.DateTime ConsultantDiscontinuedDate;
        public System.DateTime ConsultantProductExpirationDate;
        public String OrderEntrySourceExclude;
        public String OrderTypeExclude;
        public String ActivityStatusExclude;
        public String CareerLevelExclude;
        public System.DateTime ConsultantProductStartDate;
        public System.DateTime ConsultantNewProductEndDate;
        public bool LimitedEditionProduct;
        public String productClass;
        public String childProduct;
        public bool skuIsActive;
        public bool includeInSearch;
        public bool productIsActive;

    }
    public class oosProducts
    {
        public String skuID;
    }


    //    public void dump()
    //{
    //     int pexcount=0;
    //            int noPexcount = 0;          

    //            for (int i = 1; i < TotalProducts; i++)
    //            {
    //                if (d.Products[i].OrderEntrySourceExclude == null && d.Products[i].OrderTypeExclude == null && d.Products[i].ActivityStatusExclude == null && d.Products[i].CareerLevelExclude == null)
    //                {
    //                    noPexcount++;
    //                }

    //            }
    //            Console.WriteLine("Products with no exclusions are {0}", noPexcount);
    //}



    //Deserializing JSON

    //var result = JsonConvert.DeserializeObject(response.Content);
    //Console.WriteLine(result.ToString());     

    //Console.WriteLine(response.Content);
    //String[] schema = response.Content.Split(',');
    //Console.WriteLine(response.StatusDescription);

    //for (int i = 0; i < schema.Length; i++)
    //{            
    //Console.WriteLine(schema[i]);
    //}

    //XmlDocument doc = new XmlDocument();
    //To convert JSON text contained in string json into an XML node        
    //String json = response.Content.ToString();
    //StreamReader r = new StreamReader(@"D:\Shinu C#\JSON.txt");
    //string json = r.ReadToEnd();
    //var perso = JsonConvert.DeserializeObject<dynamic>(json);
    //Console.WriteLine("The String object is:"+perso);
    //XmlDocument convertedXML = (XmlDocument)JsonConvert.DeserializeXmlNode(perso.ToString());
    //Console.WriteLine(convertedXML.ToString()); 
}

