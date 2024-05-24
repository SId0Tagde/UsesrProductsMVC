using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Diagnostics;
using UsersProducts.Attribute;
using UsersProducts.Data;
using UsersProducts.Models;
using UsersProducts.Services;

namespace UsersProducts.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        #region Private Variables.
        #region Depenency.
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IProductRepository _productRepository;
        #endregion
        #endregion

        #region Constructor
        public HomeController(SignInManager<IdentityUser> signInManager, IProductRepository productRepository)
        {
            _signInManager = signInManager;
            _productRepository = productRepository;
        }
        #endregion

        #region Public methods.
        [RoleAuthorize("Admin", "User")]
        public async Task<ActionResult<List<Product>>> Index()
        {
            try
            {
                return View(await _productRepository.GetAllProductsAsync());
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Error occurred while fetching products.");
                return RedirectToAction("ErrorTwo", "Home");
            }
        }

        [RoleAuthorize("Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleAuthorize("Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,Price")] Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _productRepository.AddProductAsync(product);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, "Error occurred while creating a new product.");
                    return View(product);
                }
            }
            return View(product);
        }

        [RoleAuthorize("Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var product = await _productRepository.GetByIdAsync(Convert.ToInt32(id));
                if (product == null)
                {
                    return NotFound();
                }
                return View(product);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Error occurred while fetching product for edit with ID: {Id}", id);
                return RedirectToAction("ErrorTwo", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleAuthorize("Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _productRepository.UpdateProduct(product);
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, "Error occurred while updating product with ID: {Id}", product.Id);
                    return View(product);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        [RoleAuthorize("Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound(nameof(id));
            }
            try
            {
                var product = await _productRepository.GetByIdAsync(Convert.ToInt32(id));

                if (product == null)
                {
                    return NotFound("not found any product associated with the id");
                }
                return View(product);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Error occurred while fetching product for deletion with ID: {Id}", id);
                return RedirectToAction("ErrorTwo", "Home");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [RoleAuthorize("Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(Convert.ToInt32(id));
                if (product != null)
                    await _productRepository.DeleteProductAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Error occurred while deleting product with ID: {Id}", id);
                return RedirectToAction("ErrorTwo", "Home");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("AccessDenied")]
        public IActionResult Error()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult ErrorTwo()
        {
            return View();
        }
        public async Task<IActionResult> Signout()
        {
            await _signInManager.SignOutAsync();
            return new RedirectResult("/Identity/Account/Login");
        }
        #endregion
    }
}
