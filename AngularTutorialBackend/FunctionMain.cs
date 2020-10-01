using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CommonLibrary;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace AngularTutorialBackend
{
    public static class FunctionMain
    {
        /**
         * Variable where we store the list of the products,
         * Currently static because i'm not willing to link the application with a database BUT
         * You can link it with yours :)
         */

        static List<Product> products = new List<Product>() {
        new Product()
        {
            _id=Guid.NewGuid().ToString(),
            prod_name="Dummy",
            prod_desc="Dummy product",
            prod_price="50",
            update_date=DateTime.Now.ToShortDateString()
        }
        };

        /*
         Here, i just implemented 5 methods :
            
            deleteProduct: http://localhost:7071/api/products/{id} (DELETE)

            productById: http://localhost:7071/api/products/{id} (GET)

            productsAdd: http://localhost:7071/api/products (POST)

            productsList: http://localhost:7071/api/products (GET)

            updateProduct: http://localhost:7071/api/products/{id} (PUT)

        Consider taking a look at "local.settings.json" where some CORS related settings were implemented for local testing.
        */
        [FunctionName("productsList")]
        public static async Task<HttpResponseMessage> productsList([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "products")] HttpRequestMessage req, TraceWriter log)
        {
            return req.CreateResponse(HttpStatusCode.OK, products.ToArray(), "application/json");
        }

        [FunctionName("productById")]
        public static async Task<HttpResponseMessage> productById([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "products/{id}")] HttpRequestMessage req, string id, TraceWriter log)
        {
            Product res = new Product();
            foreach (Product P in products)
            {
                if (P._id.Equals(id))
                {
                    res = P;
                    break;
                }
            }
            return req.CreateResponse(HttpStatusCode.OK, res, "application/json");
        }

        [FunctionName("productsAdd")]
        public static async Task<HttpResponseMessage> productsAdd([HttpTrigger(AuthorizationLevel.Anonymous, methods: "POST", Route = "products")] HttpRequestMessage req, TraceWriter log)
        {
            Product P = Converters.DeserializeFromJson<Product>(req.Content.ReadAsStringAsync().Result);
            P._id = Guid.NewGuid().ToString();
            P.update_date = DateTime.Now.ToShortDateString();
            products.Add(P);
            return req.CreateResponse(HttpStatusCode.OK, P, "application/json");
        }

        [FunctionName("deleteProduct")]
        public static async Task<HttpResponseMessage> DeleteProduct([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "products/{id}")] HttpRequestMessage req, string id, TraceWriter log)
        {
            Product res = new Product();
            foreach (Product P in products)
            {
                if (P._id.Equals(id))
                {
                    res = P;
                    break;
                }
            }
            products.Remove(res);
            return req.CreateResponse(HttpStatusCode.OK, String.Empty, "application/json");
        }

        [FunctionName("updateProduct")]
        public static async Task<HttpResponseMessage> UpdateProduct([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "products/{id}")] HttpRequestMessage req, string id, TraceWriter log)
        {
            Product P = Converters.DeserializeFromJson<Product>(req.Content.ReadAsStringAsync().Result);
            foreach (Product Prd in products)
            {
                if (Prd._id.Equals(id))
                {
                    Prd.prod_name = P.prod_name;
                    Prd.prod_desc = P.prod_desc;
                    Prd.prod_price = P.prod_price;
                    Prd.update_date = DateTime.Now.ToShortDateString();
                    break;
                }
            }
            return req.CreateResponse(HttpStatusCode.OK, P, "application/json");
        }
    }
}
