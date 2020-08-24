using Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityFrameworkS3
{
    class Program
    {
        // Create Context
        private static NorthwindContext context = new NorthwindContext();


        static void Main()
        {
            /*
            NorthwindContext context = new NorthwindContext();
            IQueryable<Order> allOrders = context.Orders.Include("Customer")
                .Where(o => o.Customer.CompanyName.StartsWith("A"));
            foreach(Order item in allOrders)
            {
                Console.WriteLine($"{item.Customer.CompanyName} {item.OrderDate.Value.ToShortDateString()}: {item.Freight} $");
            }*/

            // EfCore - Object Relational Mapping
            // 1 Find all Discontinued Products


            // 2 Find Supplier in Québec


            // 3 Find all Suppliers in France & Germany


            // 4 Find all suppliers without a website



            AverageProductPrice();
        }

        // 1 Find all Discontinued Products
        private static void FindDiscontinuedProducts()
        {
            List<Product> allProducts = context.Products
                   .Where(o => o.Discontinued)
                   .ToList();

            foreach(Product item in allProducts)
            {
                Console.WriteLine($"{item.ProductName}: Discontinued? {item.Discontinued}");
            }
        }

        // 2 Find all Suppliers from Quebec
        private void FindAllQuebecSuppliers()
        {
            DbSet<Supplier> allSuppliers = context.Suppliers;

            foreach(Supplier supplier in allSuppliers.Where(o => o.Region.Equals("Québec")))
            {
                Console.WriteLine($"{supplier.CompanyName} {supplier.Region}");
            }
        }

        // 3 Find all Suppliers in Germany & France
        private void FindAllSuppliersGermanyFrance()
        {
            DbSet<Supplier> allSuppliers = context.Suppliers;

            foreach(Supplier supplier in allSuppliers.Where(o => o.Country.Equals("France") || o.Country.Equals("Germany")))
            {
                Console.WriteLine($"{supplier.CompanyName} {supplier.Country}");
            }
        }

        // 4 Find all Suppliers without website
        private void AllSuppliersWithoutWebsite()
        {
            DbSet<Supplier> allSuppliers = context.Suppliers;
            foreach(Supplier supplier in allSuppliers.Where(o => o.HomePage == null))
            {
                Console.WriteLine($"{supplier.CompanyName} Has no homepage {supplier.HomePage}");
            }
        }


        // 5 Find all European suppliers with a website
        private void GetAllEuropeanSuppliersWithHomePage()
        {
            IQueryable<Supplier> suppliers = context.Suppliers.Where(s => s.Country == "Germany"
            || s.Country == "France" || s.Country == "UK" || s.Country == "Sweden" ||
            s.Country == "Spain" || s.Country == "Italy" || s.Country == "Norway" ||
            s.Country == "Denmark" || s.Country == "Netherlands" || s.Country == "Finland"
            && s.HomePage != null);
            foreach(Supplier supplier in suppliers)
            {
                Console.WriteLine(supplier.CompanyName);
            }
        }

        // 6 Get all employees with M as first name
        private void GetAllEmployessFirstNameM()
        {
            DbSet<Employee> allEmployees = context.Employees;

            foreach(Employee employee in allEmployees.Where(o => o.FirstName.Equals("M")))
            {
                Console.WriteLine($"Employee: {employee.FirstName} {employee.LastName}");
            }
        }

        //7  Get all employees whose last name ends with an
        private void allEmployeesLastNameAn()
        {
            DbSet<Employee> allEmployees = context.Employees;

            foreach(Employee employee in allEmployees.Where(o => o.LastName.EndsWith("an")))
            {
                Console.WriteLine($"Employee: {employee.FirstName} {employee.LastName}");
            }
        }
        // 8 Find all non-doctor female employees
        private static void FindAllEmployeesFemaleNotDoctor()
        {
            DbSet<Employee> allEmployees = context.Employees;
            foreach(Employee employee in allEmployees.Where(o => o.TitleOfCourtesy.Equals("Ms.") || o.Title != "Doctor"))
            {
                Console.WriteLine($"Employee: {employee.FirstName} {employee.LastName}");
            }
        }

        // 9 Find all Sales representatives from UK
        private static void AllSalesRepresentativesFromUK()
        {
            DbSet<Employee> allEmployees = context.Employees;
            foreach(Employee employee in allEmployees.Where(o => o.Title.Equals("Sales Representative") && o.Country.Equals("UK")))
            {
                Console.WriteLine($"Employee: {employee.FirstName} {employee.Title} {employee.Country}");
            }
        }

        // 10 Find amount of Products
        private static void GetAmountOfProducts()
        {
            DbSet<Product> allProducts = context.Products;
            int count = 1;
            foreach(Product product in allProducts)
            {

                Console.WriteLine($"{count} Product: {product.ProductName}");
                count++;
            }

            Console.WriteLine($"Total amount: {allProducts.Count()}");
        }

        // 11 Find average price of product
        private static void AverageProductPrice()
        {
            decimal? averagePrice = context.Products.Sum(p => p.UnitPrice) / context.Products.Count<Product>();

            // Output result
            Console.WriteLine($"{averagePrice:c}");
        }

        // 12 Find products over 20,00, sort by highest price.
        private static void GetProductOverTwentyHighestPrice()
        {
            DbSet<Product> allProducts = context.Products;

            foreach(Product product in allProducts.Where(o => o.UnitPrice > 20))
            {

                Console.WriteLine($"{count} Product: {product.ProductName}");
                count++;
            }
        }

    }
}