﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Model;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var result = getUrls("https://gist.githubusercontent.com/anonymous/d2ec2461468d4a0372db/raw/b1eb88fa20b147deaafa9e38768174d79f705805/gistfile1.txt");
            var result = readFile("Data/Urls.txt");
            deleteBlogs();

            foreach (var x in result.Select(x => x).Distinct())
            {
                addBlog(new Blog { Url = x });
            }
            getBlogs();
        }

        public static void getBlogs()
        {
            using (var db = new BloggingContext())
            {
                foreach (var x in db.Blogs)
                {
                    Console.WriteLine("Blog Id " + "\t" + x.BlogId + "\t" + "Blog Url " + "\t" + x.Url);
                }
            }
        }

        public static void addBlog(Blog blog)
        {
            using (var db = new BloggingContext())
            {
                db.Blogs.Add(blog);
                db.SaveChanges();
            }
        }
        public static void deleteBlogs()
        {
            using (var db = new BloggingContext())
            {
                var itemsToDelete = db.Set<Blog>();

                db.Blogs.RemoveRange(itemsToDelete);
                db.SaveChanges();
            }
        }

        public static async Task<String> getUrls(string url)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }

        public static IEnumerable<string> readFile(string file)
        {
            foreach (var x in File.ReadLines(file))
            {
                yield return x;
            }
        }
    }
}
