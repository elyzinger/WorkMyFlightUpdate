using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WorkMyFlight;

namespace WorkMyFlightWeb.Models
{
    public static class Redis
    {
        // adding a new customer information to redis 
        public static bool RedisSaveNewCustomer(string host, string key, Customer value)
        {
            bool isSuccess = false;
            using (RedisClient redisClient = new RedisClient(host))
            {
                if (redisClient.Get<Customer>(key) == null)
                {

                    isSuccess = redisClient.Set(key, value);
                    //redisClient.Remove(key);
                }
            }
            return isSuccess;
        }
        // adding a new airline information to redis 
        public static bool RedisSaveNewAirline(string host, string key, AirLineCompany value)
        {
            bool isSuccess = false;
            using (RedisClient redisClient = new RedisClient(host))
            {
                if (redisClient.Get<AirLineCompany>(key) == null)
                {

                    isSuccess = redisClient.Set(key, value);
                    //redisClient.Remove(key);
                }
            }
            return isSuccess;
        }
        // getting a customer from redis with username as a key
        public static Customer RedisGetCustomer(string host, string key)

        {
            using (RedisClient redisClient = new RedisClient(host))
            {
                return redisClient.Get<Customer>(key);
            }
        }
        // getting a airline from redis with username as a key
        public static AirLineCompany RedisGetAirline(string host, string key)

        {
            using (RedisClient redisClient = new RedisClient(host))
            {
                return redisClient.Get<AirLineCompany>(key);
            }
        }


        //static void Main(string[] args)
        //{
        //    string host = "localhost";

        //    string key = "IDG";

        //    // Store data in the cache

        //    bool success = Save(host, key, "Hello World!");

        //    // Retrieve data from the cache using the key

        //    Console.WriteLine("Data retrieved from Redis Cache: " + Get(host, key));

        //    Console.Read();
        //}
    }
}