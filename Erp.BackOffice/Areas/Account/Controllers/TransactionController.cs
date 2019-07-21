using System.Globalization;
using Erp.BackOffice.Account.Models;
using Erp.BackOffice.Filters;
using Erp.Domain.Entities;
using Erp.Domain.Interfaces;
using Erp.Domain.Account.Entities;
using Erp.Domain.Account.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Erp.Utilities;
using WebMatrix.WebData;
using Erp.BackOffice.Helpers;
using Erp.Domain.Account.Repositories;

namespace Erp.BackOffice.Account.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    [Erp.BackOffice.Helpers.NoCacheHelper]
    public class TransactionController : Controller
    {
        private readonly ITransactionRepository TransactionRepository;
        private readonly IUserRepository userRepository;

        public TransactionController(
            ITransactionRepository _Transaction
            , IUserRepository _user
            )
        {
            TransactionRepository = _Transaction;
            userRepository = _user;
        }

        #region Index

        public ViewResult Index(string txtSearch)
        {

            IQueryable<TransactionViewModel> q = TransactionRepository.GetAllTransaction()
                .Select(item => new TransactionViewModel
                {
                    Id = item.Id,
                    CreatedUserId = item.CreatedUserId,
                    //CreatedUserName = item.CreatedUserName,
                    CreatedDate = item.CreatedDate,
                    ModifiedUserId = item.ModifiedUserId,
                    //ModifiedUserName = item.ModifiedUserName,
                    ModifiedDate = item.ModifiedDate,
                }).OrderByDescending(m => m.ModifiedDate);

            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];
            return View(q);
        }
        #endregion

        #region Create
        public ViewResult Create()
        {
            var model = new TransactionViewModel();

            return View(model);
        }

        public static void Create(TransactionViewModel model)
        {
            ITransactionRepository transactionRepository = DependencyResolver.Current.GetService<ITransactionRepository>();
            var transaction = transactionRepository.GetAllTransaction()
                .Where(item => item.TransactionCode == model.TransactionCode).FirstOrDefault();

            if (transaction == null)
            {
                transaction = new Transaction();
                AutoMapper.Mapper.Map(model, transaction);
                transaction.IsDeleted = false;
                transaction.CreatedUserId = WebSecurity.CurrentUserId;
                transaction.ModifiedUserId = WebSecurity.CurrentUserId;
                transaction.AssignedUserId = WebSecurity.CurrentUserId;
                transaction.CreatedDate = DateTime.Now;
                transaction.ModifiedDate = DateTime.Now;
                transactionRepository.InsertTransaction(transaction);
            }
            else
            {
                transaction.ModifiedUserId = WebSecurity.CurrentUserId;
                transaction.ModifiedDate = DateTime.Now;
                transaction.TransactionName = model.TransactionName;
                transactionRepository.UpdateTransaction(transaction);
            }
        }

        public static void CreateRelationship(TransactionRelationshipViewModel model)
        {
            TransactionRepository transactionRelationshipRepository = new Domain.Account.Repositories.TransactionRepository(new Domain.Account.ErpAccountDbContext());
            var transactionRelationship = new TransactionRelationship();
            AutoMapper.Mapper.Map(model, transactionRelationship);
            transactionRelationship.IsDeleted = false;
            transactionRelationship.CreatedUserId = WebSecurity.CurrentUserId;
            transactionRelationship.ModifiedUserId = WebSecurity.CurrentUserId;
            transactionRelationship.AssignedUserId = WebSecurity.CurrentUserId;
            transactionRelationship.CreatedDate = DateTime.Now;
            transactionRelationship.ModifiedDate = DateTime.Now;
            transactionRelationshipRepository.InsertTransactionRelationship(transactionRelationship);
        }
        #endregion

        #region Edit
        public ActionResult Edit(int? Id)
        {
            var Transaction = TransactionRepository.GetTransactionById(Id.Value);
            if (Transaction != null && Transaction.IsDeleted != true)
            {
                var model = new TransactionViewModel();
                AutoMapper.Mapper.Map(Transaction, model);
                
                if (model.CreatedUserId != Helpers.Common.CurrentUser.Id && Helpers.Common.CurrentUser.UserTypeId != 1)
                {
                    TempData["FailedMessage"] = "NotOwner";
                    return RedirectToAction("Index");
                }                

                return View(model);
            }
            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(TransactionViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Request["Submit"] == "Save")
                {
                    var Transaction = TransactionRepository.GetTransactionById(model.Id);
                    AutoMapper.Mapper.Map(model, Transaction);
                    Transaction.ModifiedUserId = WebSecurity.CurrentUserId;
                    Transaction.ModifiedDate = DateTime.Now;
                    TransactionRepository.UpdateTransaction(Transaction);

                    TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.UpdateSuccess;
                    return RedirectToAction("Index");
                }

                return View(model);
            }

            return View(model);

            //if (Request.UrlReferrer != null)
            //    return Redirect(Request.UrlReferrer.AbsoluteUri);
            //return RedirectToAction("Index");
        }

        #endregion

        #region Detail
        public ActionResult Detail(int? Id)
        {
            var Transaction = TransactionRepository.GetTransactionById(Id.Value);
            if (Transaction != null && Transaction.IsDeleted != true)
            {
                var model = new TransactionViewModel();
                AutoMapper.Mapper.Map(Transaction, model);
                
                if (model.CreatedUserId != Helpers.Common.CurrentUser.Id && Helpers.Common.CurrentUser.UserTypeId != 1)
                {
                    TempData["FailedMessage"] = "NotOwner";
                    return RedirectToAction("Index");
                }                

                return View(model);
            }
            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        [HttpPost]
        public ActionResult Delete()
        {
            try
            {
                string idDeleteAll = Request["DeleteId-checkbox"];
                string[] arrDeleteId = idDeleteAll.Split(',');
                for (int i = 0; i < arrDeleteId.Count(); i++)
                {
                    var item = TransactionRepository.GetTransactionById(int.Parse(arrDeleteId[i], CultureInfo.InvariantCulture));
                    if(item != null)
                    {
                        if (item.CreatedUserId != Helpers.Common.CurrentUser.Id && Helpers.Common.CurrentUser.UserTypeId != 1)
                        {
                            TempData["FailedMessage"] = "NotOwner";
                            return RedirectToAction("Index");
                        }

                        item.IsDeleted = true;
                        TransactionRepository.UpdateTransaction(item);
                    }
                }
                TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.DeleteSuccess;
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                TempData[Globals.FailedMessageKey] = App_GlobalResources.Error.RelationError;
                return RedirectToAction("Index");
            }
        }
        #endregion
    }
}
