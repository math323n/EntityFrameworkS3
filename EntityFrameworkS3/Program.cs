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
            
      
            
        }

        #region¨Select

        /// <summary>
        /// 1. Find alle produkter der ikke længere føres. 
        /// </summary>
        public static void FindDiscontinuedProducts()
        {
            List<Product> allProducts = context.Products
                   .Where(o => o.Discontinued)
                   .ToList();

            foreach(Product item in allProducts)
            {
                Console.WriteLine($"{item.ProductName}: Discontinued? {item.Discontinued}");
            }
        }

        /// <summary>
        /// 2. Find alle leverandører fra Québec. 
        /// </summary>
        private void FindAllQuebecSuppliers()
        {
            DbSet<Supplier> allSuppliers = context.Suppliers;

            foreach(Supplier supplier in allSuppliers.Where(o => o.Region.Equals("Québec")))
            {
                Console.WriteLine($"{supplier.CompanyName} {supplier.Region}");
            }
        }

        /// <summary>
        /// 3. Find alle leverandører fra Tyskland og Frankrig. 
        /// </summary>
        public void FindAllSuppliersGermanyFrance()
        {
            DbSet<Supplier> allSuppliers = context.Suppliers;

            foreach(Supplier supplier in allSuppliers.Where(o => o.Country.Equals("France") || o.Country.Equals("Germany")))
            {
                Console.WriteLine($"{supplier.CompanyName} {supplier.Country}");
            }
        }

        /// <summary>
        /// 4. Find alle leverandører der ikke har en hjemmeside. 
        /// </summary>
        private void AllSuppliersWithoutWebsite()
        {
            DbSet<Supplier> allSuppliers = context.Suppliers;
            foreach(Supplier supplier in allSuppliers.Where(o => o.HomePage == null))
            {
                Console.WriteLine($"{supplier.CompanyName} Has no homepage {supplier.HomePage}");
            }
        }


        /// <summary>
        /// 5. Find alle leverandører fra europæsiske lande, der har en hjemmeside. 
        /// </summary>
        public void GetAllEuropeanSuppliersWithHomePage()
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

        /// <summary>
        /// 6. Find alle ansatte hvis fornavn begynder med M. 
        /// </summary>
        public void GetAllEmployessFirstNameM()
        {
            DbSet<Employee> allEmployees = context.Employees;

            foreach(Employee employee in allEmployees.Where(o => o.FirstName.Equals("M")))
            {
                Console.WriteLine($"Employee: {employee.FirstName} {employee.LastName}");
            }
        }

        /// <summary>
        /// 7. Find alle ansatte hvis efternavn slutter på an. 
        /// </summary>
        public void allEmployeesLastNameAn()
        {
            DbSet<Employee> allEmployees = context.Employees;

            foreach(Employee employee in allEmployees.Where(o => o.LastName.EndsWith("an")))
            {
                Console.WriteLine($"Employee: {employee.FirstName} {employee.LastName}");
            }
        }

        /// <summary>
        /// 8. Find alle kvindelige ansatte der ikke er læger (benyt en OR). 
        /// </summary>
        private static void FindAllEmployeesFemaleNotDoctor()
        {
            DbSet<Employee> allEmployees = context.Employees;
            foreach(Employee employee in allEmployees.Where(o => o.TitleOfCourtesy.Equals("Ms.") || o.Title != "Doctor"))
            {
                Console.WriteLine($"Employee: {employee.FirstName} {employee.LastName}");
            }
        }

        /// <summary>
        /// 9. Find alle medarbejdere der er Sales Representative og kommer fra UK. 
        /// </summary>
        public static void AllSalesRepresentativesFromUK()
        {
            DbSet<Employee> allEmployees = context.Employees;
            foreach(Employee employee in allEmployees.Where(o => o.Title.Equals("Sales Representative") && o.Country.Equals("UK")))
            {
                Console.WriteLine($"Employee: {employee.FirstName} {employee.Title} {employee.Country}");
            }
        }

        /// <summary>
        /// 10. Find ud af hvor mange produkter der er. 
        /// </summary>
        public static void GetAmountOfProducts()
        {
            // Get product amount
            int productAmount = context.Products.ToList().Count;

            // Output result
            Console.WriteLine($"Amount of products: {productAmount}");
        }

        /// <summary>
        /// 11. Find gennemsnitsprisen for alle produkter. 
        /// </summary>
        public static void AverageProductPrice()
        {
            decimal? averagePrice = context.Products.Sum(p => p.UnitPrice) / context.Products.Count<Product>();

            // Output result
            Console.WriteLine($"{averagePrice:c}");
        }

        /// <summary>
        /// 12. Find antal produkter med en enhedspris over 20,00. Sorter efter dyreste. 
        /// </summary>
        public static void GetProductOverTwentyHighestPrice()
        {
            // Find products with unit price over twenty sort by highest
            IOrderedQueryable<Product> products = (
                from p in context.Products
                where p.UnitPrice >= 20
                select p
            ).OrderByDescending(p => p.UnitPrice);

            // Output result
            foreach(Product product in products)
            {
                Console.WriteLine($"{product.ProductId} : {product.ProductName} : {product.UnitPrice}");
            }
        }

        /// <summary>
        /// 13. Find de produkter der ikke er flere af, sorter alfabetisk. 
        /// </summary>
        public static void FindSoldOutProductsSortAlphabetically()
        {
            // Using IOrderedQueryable. Find sold out Products
            IOrderedQueryable<Product> soldOutProducts = (
                from p in context.Products
                where p.UnitsInStock == 0
                select p
            ).OrderBy(p => p.ProductName);

            // Output result
            foreach(Product product in soldOutProducts)
            {
                Console.WriteLine($"Product name: {product.ProductName} : Price: {product.UnitPrice:c} : Amount: {product.UnitsInStock}");
            }
        }

        /// <summary>
        /// 14. Find alle de produkter der ikke er flere af, og som ikke er bestilt, men heller ikke udgået, sorter efter produktnavn, omvendt alfabetisk rækkefølge. 
        /// </summary>
        public static void FindProductsOutOfStockWithNoOrders()
        {
            // Using IOrderedQueryable.
            // Find products out of stock with no orders
            IOrderedQueryable<Product> products = (
                from p in context.Products
                where p.UnitsInStock == 0 && p.UnitsOnOrder == 0 && p.Discontinued == false
                select p
            ).OrderBy(p => p.ProductName);

            // Output result
            foreach(Product product in products)
            {
                Console.WriteLine($"Product name: {product.ProductName} : Price: {product.UnitPrice:c} : Amount: {product.UnitsInStock}");
            }
        }

        /// <summary>
        /// 15. Find alle kunder der er enten er franske ejere eller britiske sælgere, sorter efter land, dernæst navn.
        /// </summary>
        public static void FindFrenchOwnersAndBritishSellers()
        {
            // Using IOrderedQueryable Interface
            // Find french owners and british sellers
            IOrderedQueryable<Customer> customers = (
                from c in context.Customers
                where c.Country == "France" && c.ContactTitle == "Owner" || c.Country == "UK" && c.ContactTitle.Contains("Sales")
                select c
            ).OrderBy(c => c.Country)
            .ThenBy(c => c.ContactName);

            // Output result
            foreach(Customer customer in customers)
            {
                Console.WriteLine($"{customer.Country} : {customer.ContactName} : {customer.ContactTitle}");
            }
        }

        /// <summary>
        /// 16. Find alle nord-, mellem-, og sydamerikanske kunder der ikke har en fax, sorter alfabetisk
        /// </summary>
        public static void FindAmericanCustomersWithoutFax()
        {
            // Find american customers without fax number
            IOrderedQueryable<Customer> customers = (
                from c in context.Customers
                where c.Fax == null && c.Country == "Mexico"
                || c.Fax == null && c.Country == "Argentina"
                || c.Fax == null && c.Country == "Brazil"
                || c.Fax == null && c.Country == "Venezuela"
                || c.Fax == null && c.Country == "USA"
                select c
            ).OrderBy(c => c.CompanyName);

            // Output result
            foreach(Customer customer in customers)
            {
                Console.WriteLine($"{customer.CompanyName} : {customer.ContactName}");
            }
        }
        #endregion

        #region Update

        /// <summary>
        /// 1. Opdater alle leverandørers fax til no fax number, hvis ikke der er et fax nummer.
        /// Gør det samme for alle kunder (Hint: adskil disse to opdateringer med ; 
        /// og kør dem begge i samme transaktion) 
        /// </summary>
        public static void UpdateSupplierFaxNumber()
        {
            // Get all suppliers
            IQueryable<Supplier> suppliers = context.Suppliers.Where(o => o.Fax == null);
            // Get all customers
            IQueryable<Customer> customers = context.Customers.Where(o => o.Fax == null);

            // Update all Suppliers
            foreach(Supplier supplier in suppliers)
            {
                supplier.Fax = "No fax number";
            }
            // Update all Customers
            foreach(Customer customer in customers)
            {
                customer.Fax = "No fax number";
            }
            // Save the changes
            context.SaveChanges();
        }

        /// <summary>
        /// 2.Opdater genbestillingsmængden for alle ikke-ugåede produkter, 
        /// hvis nuværende genbestillings-mængde er 0 og nuværende beholdning
        /// er mindre en 20, til 10. 
        /// </summary>
        public static void UpdateReorderAmount()
        {
            IQueryable<Product> products = context.Products.Where(o => o.ReorderLevel == 0 && o.ReorderLevel < 20);

            foreach(Product product in products)
            {
                product.ReorderLevel = 10;
            }
            context.SaveChanges();
        }

        /// <summary>
        /// 3. Opdater alle spanske kunder med den korrekte region. 
        /// Se spanske regioner på wikipedia og/eller google maps. 
        /// </summary>
        public static void UpdateSpanishCustomers()
        {
            // Get all Spanish Customers
            IQueryable<Customer> customers = context.Customers.Where(o => o.Country == "Spain");

            foreach(Customer customer in customers)
            {
                // Switch statement to change Region of customer 
                switch(customer.City)
                {
                    case "Madrid":
                        customer.Region = "Madrid";
                        break;

                    case "Barcelona":
                        customer.Region = "Catalonia";
                        break;

                    case "Sevilla":
                        customer.Region = "Andalusia";
                        break;
                }
            }
            // Remember to save!
            context.SaveChanges();
        }

        /// <summary>
        /// 4. Simons bistro har ændret navn til Simons Vaffelhus 
        /// og flyttet til Strandvejen 65, 7100 Vejle. Foretag opdateringen. 
        /// </summary>
        public static void UpdateSimonsAddress()
        {
            Customer simons = context.Customers.FirstOrDefault(c => c.CompanyName.Equals("Simons bistro"));

            simons.CompanyName = "Simons Vaffelhus";
            simons.City = "Vejle";
            simons.Address = "Strandvejen 65";
            simons.PostalCode = "7100";
            context.SaveChanges();

        }

        /// <summary>
        /// 5. White Clover Markets er flyttet til 247 New Avenue, 
        /// Chicago og har skifter nummer til 555-20159. Foretag opdateringen. 
        /// </summary>
        public static void UpdateWhiteClover()
        {
            Customer whiteClover = context.Customers.FirstOrDefault(c => c.CompanyName.Equals("White Clover Markets"));

            whiteClover.Address = "247 New Avenue";
            whiteClover.City = "Chicago";
            whiteClover.Phone = "555-20159";

            context.SaveChanges();
        }

        /// <summary>
        /// 6. Medarbejderen Janet er flyttet ind hos medarbejderen Andrew. Foretag opdateringen. 
        /// </summary>
        public static void UpdateJanet()
        {
            // Get Employee Janet, using much better FirstOrDefault method
            Employee janet = context.Employees.FirstOrDefault(e => e.FirstName.Equals("Janet") && e.LastName.Equals("Leverling"));

            // Get Employee Andrew
            Employee andrew = context.Employees.FirstOrDefault(e => e.FirstName.Equals("Andrew") && e.LastName.Equals("Fuller"));

            // Simple null check
            if(janet is null || andrew is null)
            {
                Console.WriteLine("Error, Janet or Andrew not found");
            }
            else
            {
                // Set Janets address to Andrews address
                janet.Address = andrew.Address;
                janet.City = andrew.City;
            }

            // Save
           context.SaveChanges();
        }
        #endregion

        #region Insert

        #endregion
    }
}