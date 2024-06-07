using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Recycle.Pages.Entries
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public string ItemNo { get; set; }

        [BindProperty]
        public string Category { get; set; }

        [BindProperty]
        public string ItemName { get; set; }

        public string ErrorMessage { get; set; }

        public List<Items> items { get; set; }

        public class Items
        {
            public string ItemNo { get; set; }
            public string Category { get; set; }
            public string ItemName { get; set; }
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(ItemNo) || string.IsNullOrEmpty(Category) || string.IsNullOrEmpty(ItemName))
            {
                ErrorMessage = "All fields are required.";
                return Page();
            }

            string connectionString = "Data Source=DESKTOP-12BH48N\\SQLEXPRESS;Initial Catalog=Recycle;Integrated Security=True;TrustServerCertificate=True";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO items (ItemNo, Category, ItemName) VALUES (@ItemNo, @Category, @ItemName)";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ItemNo", ItemNo);
                        command.Parameters.AddWithValue("@Category", Category);
                        command.Parameters.AddWithValue("@ItemName", ItemName);

                        command.ExecuteNonQuery();
                    }
                }

                return RedirectToPage("/Entries/EntryForm");
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error: " + ex.Message;
                return Page();
            }
        }
    }
}