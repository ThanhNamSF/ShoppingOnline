using System;
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
        public DbSet<Customer> Customers { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<FeedbackDetail> FeedbackDetails { get; set; }
        public DbSet<Footer> Footers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Slide> Slides { get; set; }
        public DbSet<Receive> Receives { get; set; }
        public DbSet<ReceiveDetail> ReceiveDetails { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public DbSet<GroupUser> GroupUsers { get; set; }
        public DbSet<CodeGenerating> CodeGeneratings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
