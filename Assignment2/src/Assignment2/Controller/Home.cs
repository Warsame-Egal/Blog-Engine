using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Assignment2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Assignment1.Controllers
{
    public class Home : Controller
    {
        private Assignment2DataContext _blogContext;

        /// <param name="context"></param>
        public Home(Assignment2DataContext context)
        {
            _blogContext = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View(_blogContext.BlogPosts.ToList());
        }

        // GET: /<controller>/
        public IActionResult Register()
        {

            return View();
        }

        public IActionResult CreateUser(User user)
        {
            _blogContext.Users.Add(user);
            _blogContext.SaveChanges();

            return RedirectToAction("Login");
        }

        // GET: /<controller>/
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult LoginUser(User user, Role role)
        {
            var userI = (from c in _blogContext.Users where c.EmailAddress == user.EmailAddress select c.UserId).FirstOrDefault();
            var roleI = (from c in _blogContext.Roles where c.Name == role.Name select c.RoleId).FirstOrDefault();
            var email = (from c in _blogContext.Users where c.EmailAddress == user.EmailAddress select c.EmailAddress).FirstOrDefault();
            var password = (from c in _blogContext.Users where c.Password == user.Password select c.Password).FirstOrDefault();

            /*User log in security checks*/
            if (userI == 0) //need to register if the database does not contain any accounts
            {
                return RedirectToAction("Register");
            }
            if (user.EmailAddress == null) //redirect if email is empty
            {
                return RedirectToAction("Login");
            }
            if (user.EmailAddress != email) //redirect if email is incorrect
            {
                return RedirectToAction("Login");
            }
            if (user.Password == null) //redirect if password is empty
            {
                return RedirectToAction("Login");
            }
            if (user.Password != password) //redirect if password is incorrect
            {
                return RedirectToAction("Login");
            }

            int id = Convert.ToInt32(userI); //convert userI to int
            int id1 = Convert.ToInt32(roleI); //convert userI to int

            HttpContext.Session.SetInt32("userId", id); //add user to the session 
            HttpContext.Session.SetInt32("roleId", id1); //add roleId to the session 
            HttpContext.Session.SetString("emailAddress", email); //add email to the session 
            HttpContext.Session.SetString("password", password); //add password to the session 

            return RedirectToAction("Index");
        }

        // GET: /<controller>/
        public IActionResult AddBlogPost()
        {
            return View();
        }

        public IActionResult CreateBlog(BlogPost blog)
        {

            var id = HttpContext.Session.GetInt32("userId"); //get the userId from the session 
            //TODO GUEST CANNOT CREATE A BLOG SO REDIRECT THEM TO REGISTER PAGE
            blog.UserId = id.Value; // pass the session userId to the database blog userId

            _blogContext.BlogPosts.Add(blog); //add a blog instance
            _blogContext.SaveChanges(); //save the value to the database

            var userI = (from c in _blogContext.BlogPosts where c.Title == blog.Title select c.BlogPostId).FirstOrDefault();
            int iddd = Convert.ToInt32(userI); //convert userI to int
            HttpContext.Session.SetInt32("blogId", iddd); //add user to the session


            return RedirectToAction("AddPhoto");
        }

        // GET: /<controller>/
        public IActionResult DisplayFullBlogPost(int id)
        {
            var viewModel = new DisplayBlogPostViewModel();
            var user = HttpContext.Session.GetInt32("userId"); //get the userId from the session 
            var comment = HttpContext.Session.GetInt32("commentId"); //get the userId from the session 

            viewModel.Blog = (from c in _blogContext.BlogPosts where c.BlogPostId == id select c).ToList();
            viewModel.Users = (from c in _blogContext.Users where c.UserId == user select c).ToList();
            viewModel.Photos = (from c in _blogContext.Photos where c.BlogPostId == id select c).ToList();
            viewModel.Comments = (from c in _blogContext.Comments where c.BlogPostId == id select c).ToList();

            return View(viewModel);
        }

        public IActionResult AddComment(Comment comment)
        {

            var id = HttpContext.Session.GetInt32("userId"); //get the userId from the session 
            comment.UserId = id.Value; // pass the session userId to the database comment userId



            String blogId = HttpContext.Request.Form["BlogId"];
            String comments = HttpContext.Request.Form["Comment"];
           String ratings = HttpContext.Request.Form["Rating"];

            int idddd = Convert.ToInt32(blogId); //convert userI to int

            comment.BlogPostId = idddd;

            comment.Content = comments;
            int iddd = Convert.ToInt32(ratings); //convert userI to int

            comment.Rating = iddd;

            _blogContext.Comments.Add(comment); //add a comment 
            _blogContext.SaveChanges(); //save the value to the database

            return Redirect("DisplayFullBlogPost");
        }

        // GET: /<controller>/
        public IActionResult EditBlogPost()
        {
            return View();
        }

        // GET: /<controller>/
        public IActionResult EditBlog(BlogPost blog, Comment comment)
        {
            var blogToUpdate = (from c in _blogContext.BlogPosts where c.BlogPostId == blog.BlogPostId select c).FirstOrDefault();//get the blog title to edit
            blogToUpdate.Title = blog.Title; //edit the content of the blog
            blogToUpdate.ShortDescription = blog.ShortDescription; //edit the content of the blog
            blogToUpdate.Content = blog.Content; //edit the content of the blog
            blogToUpdate.Posted = blog.Posted; // edit the new date for the blog
            blogToUpdate.IsAvailable = blog.IsAvailable; //edit the content of the blog

            _blogContext.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: /<controller>/
        public IActionResult EditProfile()
        {
            return View();
        }

        // GET: /<controller>/
        public IActionResult EditUser(User user)
        {
            var userToUpdate = (from c in _blogContext.Users where c.UserId == user.UserId select c).FirstOrDefault();//get the blog title to edit
            userToUpdate.FirstName = user.FirstName; //edit the content of the user
            userToUpdate.LastName = user.LastName; //edit the content of the user
            userToUpdate.EmailAddress = user.EmailAddress; //edit the content of the user
            userToUpdate.Password = user.Password; // edit the new date for the user
            userToUpdate.DateOfBirth = user.DateOfBirth; //edit the content of the user
            userToUpdate.City = user.City; //edit the content of the user
            userToUpdate.Address = user.Address; // edit the new date for the user
            userToUpdate.PostalCode = user.PostalCode; //edit the content of the user
            userToUpdate.Country = user.Country; //edit the content of the user

            _blogContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Logout(User user)
        {
            HttpContext.Session.Clear();

            _blogContext.SaveChanges();

            return RedirectToAction("Login");
        }

        public IActionResult DeleteBlog(BlogPost blog)
        {

            var deleteId = (from c in _blogContext.BlogPosts where c.BlogPostId == c.BlogPostId select c).FirstOrDefault(); //TODO DELETE THE RIGHT ONE NOT THE FIRST ONE EVERY TIME
            _blogContext.Remove(deleteId); //delete the id object

            _blogContext.SaveChanges(); //save the changes
            return RedirectToAction("Index");
        }

        public IActionResult DeletePhoto(Photo photo)
        {

            var deleteId = (from c in _blogContext.Photos where c.PhotoId == c.PhotoId select c).FirstOrDefault(); //TODO DELETE THE RIGHT ONE NOT THE FIRST ONE EVERY TIME
            _blogContext.Remove(deleteId); //delete the id object

            _blogContext.SaveChanges(); //save the changes
            return RedirectToAction("DisplayFullBlogPost");
        }

        // GET: /<controller>/
        public IActionResult AddPhoto()
        {
            return View();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UploadFileNow(ICollection<IFormFile> files, Photo photos, BlogPost blog)
        {

            // get your storage accounts connection string
            var storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=cst8359;AccountKey=ecMPpNU6vimZKMDTJG4seALrY7Kq7UJYjgl0/yLanXn857C8xtUJ2sF4ciB6wy9gg+e/YeYbRTaly2DVOxWhXQ==");

            // create an instance of the blob client
            var blobClient = storageAccount.CreateCloudBlobClient();

            // create a container to hold your blob (binary large object.. or something like that)
            // naming conventions for the curious https://msdn.microsoft.com/en-us/library/dd135715.aspx
            var container = blobClient.GetContainerReference("justinsphotostorage");
            await container.CreateIfNotExistsAsync();

            // set the permissions of the container to 'blob' to make them public
            var permissions = new BlobContainerPermissions();
            permissions.PublicAccess = BlobContainerPublicAccessType.Blob;
            await container.SetPermissionsAsync(permissions);

            // for each file that may have been sent to the server from the client
            foreach (var file in files)
            {
                try
                {
                    // create the blob to hold the data
                    var blockBlob = container.GetBlockBlobReference(file.FileName);
                    if (await blockBlob.ExistsAsync())
                        await blockBlob.DeleteAsync();

                    using (var memoryStream = new MemoryStream())
                    {
                        // copy the file data into memory
                        await file.CopyToAsync(memoryStream);

                        // navigate back to the beginning of the memory stream
                        memoryStream.Position = 0;

                        // send the file to the cloud
                        await blockBlob.UploadFromStreamAsync(memoryStream);
                    }

                    // add the photo to the database if it uploaded successfully

                    photos.Url = blockBlob.Uri.AbsoluteUri;
                    photos.FileName = file.FileName;
                }
                catch
                {

                }
            }

            var id = HttpContext.Session.GetInt32("blogId"); //get the userId from the session 
            photos.BlogPostId = id.Value; // pass the session userId to the database blog userId

            _blogContext.Photos.Add(photos);
            _blogContext.SaveChanges();


            return RedirectToAction("Index");
        }
    }
}