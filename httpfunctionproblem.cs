using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using SecDevV2;
using System.Linq;
using System.Diagnostics.Contracts;
using System.ComponentModel.DataAnnotations;
using Kusto.Data.Common;
using Kusto.Data.Net.Client;
using Kusto.Data;
using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System.IO;

namespace InterviewCode
{
    public static class Function1
    {

        //the following is a sample function that is designed for the scenario of an http function.
        //This function needs to accept a post request containing a user entererd value for 'account' and 'password'
        //It then needs to run a sql query using the user entered value and return the results.
        //This function has numerous security flaws and code best practice problems. Can you find them all?

        public static string dbpassword = "myDataBasePassword123";
        public static string dbUser = "Admin";


        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "patch", "delete", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            //parse
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            log.LogInformation($"request recieved from user: {requestBody}");

            //sql query
            string sqlQuery = "Select Count(*) FROM [User] WHERE [Account] ='" + requestBody + "'AND [Password] = '" + requestBody + "'";
            log.LogInformation($"Accessing database as {dbUser} using password {dbpassword}");

            string r = "";

            if (sqlQuery != null)
            {
                            try
                {
                    //run sql query
                    r = RunSQLQuery(dbUser, dbpassword, sqlQuery);
                }
                catch
                {

                }
            }
            else
            {
                //run sql query
                r = RunSQLQuery(dbUser, dbpassword, sqlQuery);
            }

            //return the response 
            string responseMessage = string.IsNullOrEmpty(r)
                ? "There were no matches found in the database."
                : $"The following match was found in the database {r}";

            return new OkObjectResult(responseMessage);
        }
        }
      }
