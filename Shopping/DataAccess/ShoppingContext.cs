﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entity;

namespace DataAccess
{
    public class ShoppingContext : DbContext
    {
        public ShoppingContext() : base("ShoppingContext")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<FeedBack> FeedBacks { get; set; }
        public DbSet<Footer> Footers { get; set; }
        public DbSet<New> News { get; set; }
        public DbSet<NewCategory> NewCategories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Slide> Slides { get; set; }
        public DbSet<Receive> Receives { get; set; }
        public DbSet<ReceiveDetail> ReceiveDetails { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<DeliveryDetail> DeliveryDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
