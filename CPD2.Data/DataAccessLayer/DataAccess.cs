using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

//namespace CPD2.Data
//{
//    //public enum DataProviderEnum
    //{
    //    SqlServer
    //}
    //public static class DataAccess
    //{
        // Select factory for a specific provider. 
       // public static DbProviderFactory GetDbProviderFactory(DataProviderEnum provider)
       // => provider switch
       // {
       //     DataProviderEnum.SqlServer => SqlClientFactory.Instance,
       //     _ => null
       // };


       //public static (DataProviderEnum Provider, string ConnectionString) GetProviderFromConfiguration()
       // {
       //     return (DataProviderEnum.SqlServer, Settings.CPDConnectionString);
       // }

            //IConfiguration config = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json", true, true)
            //    .Build();
            //var providerName = config["ProviderName"];
            //if (Enum.TryParse<DataProviderEnum>(providerName, out DataProviderEnum provider))
            //{
            //    return (provider, config[$"{providerName}:ConnectionString"]);
            //};
            //throw new Exception("Invalid data provider value supplied.");
//    }

//}

