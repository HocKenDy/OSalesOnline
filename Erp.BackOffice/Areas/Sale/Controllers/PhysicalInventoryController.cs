﻿using System.Globalization;
using Erp.BackOffice.Sale.Models;
using Erp.BackOffice.Filters;
using Erp.Domain.Sale.Entities;
using Erp.Domain.Interfaces;
using Erp.Domain.Sale.Interfaces;
using Erp.Domain.Sale.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Erp.Utilities;
using WebMatrix.WebData;
using Erp.BackOffice.Helpers;
using System.Web.Script.Serialization;

namespace Erp.BackOffice.Sale.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    [Erp.BackOffice.Helpers.NoCacheHelper]
    public class PhysicalInventoryController : Controller
    {
        private readonly IWarehouseRepository WarehouseRepository;
        private readonly IProductRepository ProductRepository;
        private readonly IInventoryRepository InventoryRepository;
        private readonly IProductInboundRepository productInboundRepository;
        private readonly IProductOutboundRepository productOutboundRepository;
        private readonly IPhysicalInventoryRepository PhysicalInventoryRepository;
        private readonly IUserRepository userRepository;
        private readonly IWarehouseLocationItemRepository warehouseLocationItemRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ITemplatePrintRepository templatePrintRepository;

        public PhysicalInventoryController(
            IInventoryRepository _Inventory
            , IProductRepository _Product
            , IWarehouseRepository _Warehouse
            , IProductInboundRepository _ProductInbound
            , IProductOutboundRepository _ProductOutbound
            , IPhysicalInventoryRepository _PhysicalInventory
            , IUserRepository _user
            , IWarehouseLocationItemRepository _WarehouseLocationItem
            , ICategoryRepository _CategoryRepository
            , ITemplatePrintRepository _templatePrint
            )
        {
            WarehouseRepository = _Warehouse;
            ProductRepository = _Product;
            InventoryRepository = _Inventory;
            productInboundRepository = _ProductInbound;
            productOutboundRepository = _ProductOutbound;
            userRepository = _user;
            PhysicalInventoryRepository = _PhysicalInventory;
            warehouseLocationItemRepository = _WarehouseLocationItem;
            categoryRepository = _CategoryRepository;
            templatePrintRepository = _templatePrint;
        }

        public ActionResult Index(string txtSearch, int? WarehouseId, int? BranchId)
        {
            IQueryable<PhysicalInventoryViewModel> list = PhysicalInventoryRepository.GetAllvwPhysicalInventory()
              .Where(x => x.Type == PhysicalInventoryType.Product)
                .Select(item => new PhysicalInventoryViewModel
                {
                    Id = item.Id,
                    IsDeleted = item.IsDeleted,
                    Code = item.Code,
                    CreatedDate = item.CreatedDate,
                    ModifiedDate = item.ModifiedDate,
                    Note = item.Note,
                    WarehouseId = item.WarehouseId,
                    WarehouseName = item.WarehouseName,
                    IsExchange = item.IsExchange,
                    CancelReason = item.CancelReason,
                    CreatedUserName = item.CreatedUserName,
                    ProductInboundCode = item.ProductInboundCode,
                    ProductOutboundCode = item.ProductOutboundCode,
                    BranchId = item.BranchId,
                    BranchName = item.BranchName,
                }).OrderByDescending(x => x.CreatedDate);
            if (!SecurityFilter.IsAdmin())
            {
                list = list.Where(x => x.BranchId == Helpers.Common.CurrentUser.BranchId);
            }
            else
            {
                if (BranchId != null)
                {
                    list = list.Where(x => x.BranchId == BranchId);
                }
            }

            if (!string.IsNullOrEmpty(txtSearch))
            {
                txtSearch = txtSearch.ToLower();
                list = list.Where(x => x.Code.ToLower().Contains(txtSearch));
            }
            if (WarehouseId != null)
            {
                list = list.Where(x => x.WarehouseId == WarehouseId);
            }

            return View(list);
        }
        public ActionResult ListForCard(string txtSearch, int? WarehouseId, int? BranchId)
        {
            IQueryable<PhysicalInventoryViewModel> list = PhysicalInventoryRepository.GetAllvwPhysicalInventory()
                .Where(x => x.Type == PhysicalInventoryType.Card)
                .Select(item => new PhysicalInventoryViewModel
                {
                    Id = item.Id,
                    IsDeleted = item.IsDeleted,
                    Code = item.Code,
                    CreatedDate = item.CreatedDate,
                    ModifiedDate = item.ModifiedDate,
                    Note = item.Note,
                    WarehouseId = item.WarehouseId,
                    WarehouseName = item.WarehouseName,
                    IsExchange = item.IsExchange,
                    CancelReason = item.CancelReason,
                    CreatedUserName = item.CreatedUserName,
                    ProductInboundCode = item.ProductInboundCode,
                    ProductOutboundCode = item.ProductOutboundCode,
                    BranchId = item.BranchId,
                    BranchName = item.BranchName,
                }).OrderByDescending(x => x.CreatedDate);
            if (!SecurityFilter.IsAdmin())
            {
                list = list.Where(x => x.BranchId == Helpers.Common.CurrentUser.BranchId);
            }
            else
            {
                if (BranchId != null)
                {
                    list = list.Where(x => x.BranchId == BranchId);
                }
            }

            if (!string.IsNullOrEmpty(txtSearch))
            {
                txtSearch = txtSearch.ToLower();
                list = list.Where(x => x.Code.ToLower().Contains(txtSearch));
            }
            if (WarehouseId != null)
            {
                list = list.Where(x => x.WarehouseId == WarehouseId);
            }

            return View(list);
        }
        public ActionResult ListForGift(string txtSearch, int? WarehouseId, int? BranchId)
        {
            IQueryable<PhysicalInventoryViewModel> list = PhysicalInventoryRepository.GetAllvwPhysicalInventory()
                .Where(x => x.Type == PhysicalInventoryType.Gift)
                .Select(item => new PhysicalInventoryViewModel
                {
                    Id = item.Id,
                    IsDeleted = item.IsDeleted,
                    Code = item.Code,
                    CreatedDate = item.CreatedDate,
                    ModifiedDate = item.ModifiedDate,
                    Note = item.Note,
                    WarehouseId = item.WarehouseId,
                    WarehouseName = item.WarehouseName,
                    IsExchange = item.IsExchange,
                    CancelReason = item.CancelReason,
                    CreatedUserName = item.CreatedUserName,
                    ProductInboundCode = item.ProductInboundCode,
                    ProductOutboundCode = item.ProductOutboundCode,
                    BranchId = item.BranchId,
                    BranchName = item.BranchName,
                }).OrderByDescending(x => x.CreatedDate);
            if (!SecurityFilter.IsAdmin())
            {
                list = list.Where(x => x.BranchId == Helpers.Common.CurrentUser.BranchId);
            }
            else
            {
                if (BranchId != null)
                {
                    list = list.Where(x => x.BranchId == BranchId);
                }
            }
            if (!string.IsNullOrEmpty(txtSearch))
            {
                txtSearch = txtSearch.ToLower();
                list = list.Where(x => x.Code.ToLower().Contains(txtSearch));
            }
            if (WarehouseId != null)
            {
                list = list.Where(x => x.WarehouseId == WarehouseId);
            }
            return View(list);
        }

        public ActionResult Create()
        {
            var model = new PhysicalInventoryViewModel();
            model.Type = PhysicalInventoryType.Product;
            var warehouseList = WarehouseRepository.GetAllWarehouse().Where(x => x.BranchId == Helpers.Common.CurrentUser.BranchId).AsEnumerable()
               .Select(item => new SelectListItem
               {
                   Text = item.Name,
                   Value = item.Id.ToString()
               });
            ViewBag.warehouseList = warehouseList;
            var manufacturer = categoryRepository.GetCategoryByCode("manufacturerList").AsEnumerable()
                .Select(item => new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Value
                });
            ViewBag.manufacturer = manufacturer;
            return View(model);
        }
        public ActionResult CreateForCard()
        {
            var model = new PhysicalInventoryViewModel();
            model.Type = PhysicalInventoryType.Card;
            var warehouseList = WarehouseRepository.GetAllWarehouse().Where(x => x.BranchId == Helpers.Common.CurrentUser.BranchId && x.Categories.Contains("CARD")).AsEnumerable()
               .Select(item => new SelectListItem
               {
                   Text = item.Name,
                   Value = item.Id.ToString()
               });
            ViewBag.warehouseList = warehouseList;
            return View(model);
        }
        public ActionResult CreateForGift()
        {
            var model = new PhysicalInventoryViewModel();
            model.Type = PhysicalInventoryType.Gift;
            var warehouseList = WarehouseRepository.GetAllWarehouse().Where(x => x.BranchId == Helpers.Common.CurrentUser.BranchId && x.Categories.Contains("GIFT")).AsEnumerable()
               .Select(item => new SelectListItem
               {
                   Text = item.Name,
                   Value = item.Id.ToString()
               });
            ViewBag.warehouseList = warehouseList;
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(PhysicalInventoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                PhysicalInventory PhysicalInventory = new Domain.Sale.Entities.PhysicalInventory();
                AutoMapper.Mapper.Map(model, PhysicalInventory);
                PhysicalInventory.CreatedUserId = WebSecurity.CurrentUserId;
                PhysicalInventory.CreatedDate = DateTime.Now;
                PhysicalInventory.ModifiedUserId = WebSecurity.CurrentUserId;
                PhysicalInventory.ModifiedDate = DateTime.Now;
                PhysicalInventory.IsDeleted = false;
                PhysicalInventory.IsExchange = false;
                PhysicalInventory.BranchId = Helpers.Common.CurrentUser.BranchId;
                List<PhysicalInventoryDetail> PhysicalInventoryDetailList = new List<PhysicalInventoryDetail>();
                if (model.DetailList != null)
                {
                    foreach (var item in model.DetailList)
                    {
                        Inventory inventoryProduct = InventoryRepository.GetInventoryByWarehouseIdAndProductId(model.WarehouseId.Value, item.ProductId);
                        int QuantityInInventory = 0;
                        if (inventoryProduct != null)
                        {
                            QuantityInInventory = inventoryProduct.Quantity.Value;
                            inventoryProduct.Quantity = item.QuantityRemaining;
                            InventoryRepository.UpdateInventory(inventoryProduct);
                        }

                        PhysicalInventoryDetailList.Add(new PhysicalInventoryDetail
                        {
                            CreatedDate = DateTime.Now,
                            CreatedUserId = WebSecurity.CurrentUserId,
                            IsDeleted = false,
                            Note = item.Note,
                            ProductId = item.ProductId,
                            QuantityInInventory = QuantityInInventory,
                            QuantityRemaining = item.QuantityRemaining,
                            QuantityDiff = item.QuantityRemaining - QuantityInInventory
                        });
                    }
                }

                PhysicalInventoryRepository.InsertPhysicalInventory(PhysicalInventory, PhysicalInventoryDetailList);

                //cập nhật lại mã kiểm kho
                PhysicalInventory.Code = Erp.BackOffice.Helpers.Common.GetOrderNo("InventoryCheck");
                PhysicalInventoryRepository.UpdatePhysicalInventory(PhysicalInventory);
                Erp.BackOffice.Helpers.Common.SetOrderNo("InventoryCheck");
                TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.InsertSuccess;
                if (model.Type == PhysicalInventoryType.Product)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("ListForCard");
                }
            }

            var warehouseList = WarehouseRepository.GetAllWarehouse().Where(x => x.BranchId == Helpers.Common.CurrentUser.BranchId).AsEnumerable();
            if (model.Type == PhysicalInventoryType.Card)
            {
                warehouseList = warehouseList.Where(x => x.Categories.Contains("CARD"));
            }
            else if (model.Type == PhysicalInventoryType.Gift)
            {
                warehouseList = warehouseList.Where(x => x.Categories.Contains("GIFT"));
            }
            var warehouseSelectList = warehouseList
           .Select(item => new SelectListItem
           {
               Text = item.Name,
               Value = item.Id.ToString()
           });
            ViewBag.warehouseList = warehouseSelectList;
            var manufacturer = categoryRepository.GetCategoryByCode("manufacturerList").AsEnumerable()
             .Select(item => new SelectListItem
             {
                 Text = item.Name,
                 Value = item.Value
             });
            ViewBag.manufacturer = manufacturer;
            return View(model);
        }

        public ActionResult Detail(int? Id)
        {
            var PhysicalInventory = PhysicalInventoryRepository.GetAllvwPhysicalInventory()
                .Where(item => item.Id == Id).FirstOrDefault();
            if (PhysicalInventory != null)
            {
                var model = new PhysicalInventoryViewModel();
                AutoMapper.Mapper.Map(PhysicalInventory, model);

                var detailList = PhysicalInventoryRepository.GetAllvwPhysicalInventoryDetail(Id.Value);
                model.DetailList = detailList.Select(item => new PhysicalInventoryDetailViewModel
                {
                    Note = item.Note,
                    ProductId = item.ProductId,
                    ProductCode = item.ProductCode,
                    ProductName = item.ProductName,
                    PhysicalInventoryId = item.PhysicalInventoryId,
                    QuantityInInventory = item.QuantityInInventory,
                    QuantityRemaining = item.QuantityRemaining,
                    QuantityDiff = item.QuantityDiff,
                    CategoryCode = item.CategoryCode
                })
                .OrderBy(item => item.CategoryCode)
                .ThenBy(item => item.ProductCode)
                .ToList();

                return View(model);
            }

            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(int? id, string CancelReason)
        {
            try
            {
                var item = PhysicalInventoryRepository.GetPhysicalInventoryById(id.Value);
                if (item != null)
                {
                    var detailList = PhysicalInventoryRepository.GetAllPhysicalInventoryDetail(item.Id).ToList();

                    foreach (var detail in detailList)
                    {
                        //lấy inventory của sản phẩm trong kho
                        var inventory = InventoryRepository.GetInventoryByWarehouseIdAndProductId(item.WarehouseId.Value, detail.ProductId);
                        int? quantity_remaining_difference = detail.QuantityInInventory - detail.QuantityRemaining;

                        //cập nhật SL inevntory theo sự chênh lệch SL lúc ban đầu được ghi nhận và SL cập nhật
                        inventory.Quantity += quantity_remaining_difference;
                        inventory.ModifiedDate = DateTime.Now;
                        inventory.ModifiedUserId = WebSecurity.CurrentUserId;
                        InventoryRepository.UpdateInventory(inventory);


                        //cập nhật tình trạng chi tiết kiểm kho thành xóa
                        detail.IsDeleted = true;
                        PhysicalInventoryRepository.UpdatePhysicalInventoryDetail(detail);
                    }

                    item.IsDeleted = true;
                    item.CancelReason = CancelReason;
                    PhysicalInventoryRepository.UpdatePhysicalInventory(item);
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

        [HttpPost]
        public ActionResult Exchange(int Id)
        {
            var PhysicalInventory = PhysicalInventoryRepository.GetPhysicalInventoryById(Id);
            if (PhysicalInventory == null)
            {
                TempData[Globals.FailedMessageKey] = App_GlobalResources.Wording.NotfoundObject;
                if (PhysicalInventory.Type == PhysicalInventoryType.Product)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("ListForCard");
                }
            }

            List<ProductOutboundDetail> outboundDetails = new List<ProductOutboundDetail>();
            List<ProductInboundDetail> inboundDetails = new List<ProductInboundDetail>();

            var listDetail = PhysicalInventoryRepository.GetAllPhysicalInventoryDetail(Id).Where(x => x.QuantityInInventory != x.QuantityRemaining).ToList();
            foreach (var item in listDetail)
            {
                var product = ProductRepository.GetProductById(item.ProductId);

                if (item.QuantityDiff < 0) //Chênh lệch dương thì thuộc về xuất
                {
                    outboundDetails.Add(
                        new ProductOutboundDetail
                        {
                            IsDeleted = false,
                            CreatedDate = DateTime.Now,
                            CreatedUserId = Helpers.Common.CurrentUser.Id,
                            ModifiedDate = DateTime.Now,
                            ModifiedUserId = Helpers.Common.CurrentUser.Id,
                            Price = product.PriceOutbound,
                            ProductId = product.Id,
                            Quantity = Math.Abs(item.QuantityDiff),
                        }
                    );

                }
                else if (item.QuantityDiff > 0) //Chênh lệch âm thì thuộc về nhập
                {
                    inboundDetails.Add(
                        new ProductInboundDetail
                        {
                            IsDeleted = false,
                            CreatedDate = DateTime.Now,
                            CreatedUserId = Helpers.Common.CurrentUser.Id,
                            ModifiedDate = DateTime.Now,
                            ModifiedUserId = Helpers.Common.CurrentUser.Id,
                            Price = product.PriceInbound.Value,
                            ProductId = product.Id,
                            Quantity = Math.Abs(item.QuantityDiff),
                        }
                    );
                }
            }

            if (outboundDetails.Count != 0)
            {
                var outbound = new ProductOutbound
                {
                    IsDeleted = false,
                    CreatedDate = DateTime.Now,
                    CreatedUserId = Helpers.Common.CurrentUser.Id,
                    ModifiedDate = DateTime.Now,
                    ModifiedUserId = Helpers.Common.CurrentUser.Id,
                    BranchId = Helpers.Common.CurrentUser.BranchId,
                    IsDone = true,
                    Type = PhysicalInventory.Type == PhysicalInventoryType.Product ? ProductOutboundType.Physical : ProductOutboundType.PhysicalCard,
                    TotalAmount = outboundDetails.Sum(x => x.Quantity * x.Price),
                    WarehouseSourceId = PhysicalInventory.WarehouseId,
                    PhysicalInventoryId = PhysicalInventory.Id,
                    Note = "Xuất kho kiểm kê"
                };

                productOutboundRepository.InsertProductOutbound(outbound);

                foreach (var item in outboundDetails)
                {
                    item.ProductOutboundId = outbound.Id;
                    item.IsDeleted = false;
                    item.CreatedUserId = WebSecurity.CurrentUserId;
                    item.ModifiedUserId = WebSecurity.CurrentUserId;
                    item.CreatedDate = DateTime.Now;
                    item.ModifiedDate = DateTime.Now;
                    productOutboundRepository.InsertProductOutboundDetail(item);
                }

                //cập nhật lại mã xuất kho
                outbound.Code = Erp.BackOffice.Helpers.Common.GetOrderNo("ProductOutbound");
                productOutboundRepository.UpdateProductOutbound(outbound);
                Erp.BackOffice.Helpers.Common.SetOrderNo("ProductOutbound");
                foreach (var item in listDetail.Where(x => x.QuantityDiff < 0))
                {
                    item.ModifiedDate = DateTime.Now;
                    item.ModifiedUserId = Helpers.Common.CurrentUser.Id;
                    item.ReferenceVoucher = outbound.Code;
                    PhysicalInventoryRepository.UpdatePhysicalInventoryDetail(item);
                }
                ProductOutboundController.Archive(productOutboundRepository, outbound, TempData);
            }

            if (inboundDetails.Count != 0)
            {
                var inbound = new ProductInbound
                {
                    IsDeleted = false,
                    CreatedDate = DateTime.Now,
                    CreatedUserId = Helpers.Common.CurrentUser.Id,
                    ModifiedDate = DateTime.Now,
                    ModifiedUserId = Helpers.Common.CurrentUser.Id,
                    BranchId = Helpers.Common.CurrentUser.BranchId,
                    IsDone = true,
                    Type = PhysicalInventory.Type == PhysicalInventoryType.Product ? ProductInboundType.Physical : ProductInboundType.PhysicalCard,
                    TotalAmount = inboundDetails.Sum(x => x.Quantity * x.Price),
                    WarehouseDestinationId = PhysicalInventory.WarehouseId,
                    PhysicalInventoryId = PhysicalInventory.Id,
                    Note = "Nhập kho kiểm kê"
                };

                productInboundRepository.InsertProductInbound(inbound);

                //Thêm chi tiết phiếu nhập
                foreach (var item in inboundDetails)
                {
                    item.ProductInboundId = inbound.Id;
                    item.IsDeleted = false;
                    item.CreatedUserId = WebSecurity.CurrentUserId;
                    item.ModifiedUserId = WebSecurity.CurrentUserId;
                    item.CreatedDate = DateTime.Now;
                    item.ModifiedDate = DateTime.Now;
                    productInboundRepository.InsertProductInboundDetail(item);
                }

                //cập nhật lại mã xuất kho
                inbound.Code = Erp.BackOffice.Helpers.Common.GetOrderNo("ProductInbound");
                productInboundRepository.UpdateProductInbound(inbound);
                Erp.BackOffice.Helpers.Common.SetOrderNo("ProductInbound");
                foreach (var item in listDetail.Where(x => x.QuantityDiff > 0))
                {
                    item.ModifiedDate = DateTime.Now;
                    item.ModifiedUserId = Helpers.Common.CurrentUser.Id;
                    item.ReferenceVoucher = inbound.Code;
                    PhysicalInventoryRepository.UpdatePhysicalInventoryDetail(item);
                }
                ProductInboundController.Archive(productInboundRepository, inbound, TempData);
            }

            PhysicalInventory.IsExchange = true;
            PhysicalInventory.ModifiedDate = DateTime.Now;
            PhysicalInventory.ModifiedUserId = Helpers.Common.CurrentUser.Id;
            PhysicalInventoryRepository.UpdatePhysicalInventory(PhysicalInventory);
            TempData[Globals.SuccessMessageKey] += App_GlobalResources.Wording.Success;
            return RedirectToAction("Detail", new { Id = PhysicalInventory.Id });
        }

        [HttpPost]
        public ActionResult CheckData(int Id)
        {
            var physicalInventory = PhysicalInventoryRepository.GetAllvwPhysicalInventory().Where(item => item.Id == Id).FirstOrDefault();
            if (physicalInventory == null)
            {
                TempData[Globals.FailedMessageKey] = App_GlobalResources.Wording.NotfoundObject;
                return RedirectToAction("Index");
            }

            //Cập nhật lại tồn kho hệ thống đến thời điểm kiểm kê
            var listDetail = PhysicalInventoryRepository.GetAllPhysicalInventoryDetail(Id).ToList();
            foreach (var item in listDetail)
            {
                //Số lượng nhập
                var soLuongNhap = productInboundRepository.GetAllvwProductInboundDetailByProductId(item.ProductId)
                    .Where(x => x.IsArchive == true && x.WarehouseDestinationId == physicalInventory.WarehouseId).Sum(x => x.Quantity);

                //Số lượng xuất
                var soLuongXuat = productOutboundRepository.GetAllvwProductOutboundDetailByProductId(item.ProductId)
                    .Where(x => x.IsArchive == true && x.WarehouseSourceId == physicalInventory.WarehouseId).Sum(x => x.Quantity);

                //Cập nhật lại
                item.QuantityInInventory = (soLuongNhap - soLuongXuat);
                item.QuantityDiff = item.QuantityRemaining - item.QuantityInInventory;

                PhysicalInventoryRepository.UpdatePhysicalInventoryDetail(item);
            }

            //Tách dữ liệu cần nhập/xuất kiểm kê
            List<ProductOutboundDetail> outboundDetails = new List<ProductOutboundDetail>();
            List<ProductInboundDetail> inboundDetails = new List<ProductInboundDetail>();

            listDetail = PhysicalInventoryRepository.GetAllPhysicalInventoryDetail(Id).Where(x => x.QuantityInInventory != x.QuantityRemaining).ToList();
            foreach (var item in listDetail)
            {
                var product = ProductRepository.GetProductById(item.ProductId);

                if (item.QuantityDiff < 0) //Chênh lệch dương thì thuộc về xuất
                {
                    outboundDetails.Add(
                        new ProductOutboundDetail
                        {
                            IsDeleted = false,
                            CreatedDate = DateTime.Now,
                            CreatedUserId = Helpers.Common.CurrentUser.Id,
                            ModifiedDate = DateTime.Now,
                            ModifiedUserId = Helpers.Common.CurrentUser.Id,
                            Price = product.PriceOutbound,
                            ProductId = product.Id,
                            Quantity = Math.Abs(item.QuantityDiff),
                        }
                    );

                }
                else if (item.QuantityDiff > 0) //Chênh lệch âm thì thuộc về nhập
                {
                    inboundDetails.Add(
                        new ProductInboundDetail
                        {
                            IsDeleted = false,
                            CreatedDate = DateTime.Now,
                            CreatedUserId = Helpers.Common.CurrentUser.Id,
                            ModifiedDate = DateTime.Now,
                            ModifiedUserId = Helpers.Common.CurrentUser.Id,
                            Price = product.PriceInbound.Value,
                            ProductId = product.Id,
                            Quantity = Math.Abs(item.QuantityDiff),
                        }
                    );
                }
            }

            //Cập nhật phiếu xuất
            if (outboundDetails.Count != 0)
            {
                var outbound = productOutboundRepository.GetAllProductOutbound().Where(item => item.Code == physicalInventory.ProductOutboundCode).FirstOrDefault();

                if (outbound != null)
                {
                    //Xóa chi tiết xuất cũ
                    var outboundDetails_old = productOutboundRepository.GetAllProductOutboundDetailByOutboundId(outbound.Id).Select(item => item.Id).ToList();
                    foreach (var item in outboundDetails_old)
                    {
                        productOutboundRepository.DeleteProductOutboundDetail(item);
                    }

                    //Thêm chi tiết xuất mới
                    foreach (var item in outboundDetails)
                    {
                        item.ProductOutboundId = outbound.Id;
                        item.IsDeleted = false;
                        item.CreatedUserId = WebSecurity.CurrentUserId;
                        item.ModifiedUserId = WebSecurity.CurrentUserId;
                        item.CreatedDate = DateTime.Now;
                        item.ModifiedDate = DateTime.Now;
                        productOutboundRepository.InsertProductOutboundDetail(item);
                    }

                    //Cập nhật tham chiếu trong chi tiết kiểm kê
                    foreach (var item in listDetail.Where(x => x.QuantityDiff < 0))
                    {
                        item.ModifiedDate = DateTime.Now;
                        item.ModifiedUserId = Helpers.Common.CurrentUser.Id;
                        item.ReferenceVoucher = outbound.Code;
                        PhysicalInventoryRepository.UpdatePhysicalInventoryDetail(item);
                    }
                }
            }

            //Cập nhật phiếu nhập
            if (inboundDetails.Count != 0)
            {
                var inbound = productInboundRepository.GetAllProductInbound().Where(item => item.Code == physicalInventory.ProductInboundCode).FirstOrDefault();
                //Xóa chi tiết nhập cũ
                var inboundDetails_old = productInboundRepository.GetAllProductInboundDetailByInboundId(inbound.Id).Select(item => item.Id).ToList();
                foreach (var item in inboundDetails_old)
                {
                    productInboundRepository.DeleteProductInboundDetail(item);
                }

                //Thêm chi tiết phiếu nhập
                foreach (var item in inboundDetails)
                {
                    item.ProductInboundId = inbound.Id;
                    item.IsDeleted = false;
                    item.CreatedUserId = WebSecurity.CurrentUserId;
                    item.ModifiedUserId = WebSecurity.CurrentUserId;
                    item.CreatedDate = DateTime.Now;
                    item.ModifiedDate = DateTime.Now;
                    productInboundRepository.InsertProductInboundDetail(item);
                }

                //Cập nhật tham chiếu trong chi tiết kiểm kê
                foreach (var item in listDetail.Where(x => x.QuantityDiff > 0))
                {
                    item.ModifiedDate = DateTime.Now;
                    item.ModifiedUserId = Helpers.Common.CurrentUser.Id;
                    item.ReferenceVoucher = inbound.Code;
                    PhysicalInventoryRepository.UpdatePhysicalInventoryDetail(item);
                }
            }

            TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.Success;
            return RedirectToAction("Detail", new { Id = physicalInventory.Id });
        }

        public ActionResult Print(int? Id)
        {
            var PhysicalInventory = PhysicalInventoryRepository.GetAllvwPhysicalInventory()
                .Where(item => item.Id == Id).FirstOrDefault();

            //lấy logo công ty
            var logo = Erp.BackOffice.Helpers.Common.GetSetting("LogoCompany");
            var company = Erp.BackOffice.Helpers.Common.GetSetting("companyName");
            var address = Erp.BackOffice.Helpers.Common.GetSetting("addresscompany");
            var phone = Erp.BackOffice.Helpers.Common.GetSetting("phonecompany");
            var fax = Erp.BackOffice.Helpers.Common.GetSetting("faxcompany");
            var bankcode = Erp.BackOffice.Helpers.Common.GetSetting("bankcode");
            var bankname = Erp.BackOffice.Helpers.Common.GetSetting("bankname");
            var ImgLogo = "<div class=\"logo\"><img src=" + logo + " height=\"73\" /></div>";

            if (PhysicalInventory != null && PhysicalInventory.IsDeleted != true)
            {
                var model = new TemplatePrintViewModel();

                //lấy danh sách sản phẩm 
                var user = userRepository.GetUserById(PhysicalInventory.CreatedUserId.Value);
                var detailList = PhysicalInventoryRepository.GetAllvwPhysicalInventoryDetail(Id.Value).AsEnumerable()
                    .Select(item => new PhysicalInventoryDetailViewModel
                    {
                        Note = item.Note,
                        ProductId = item.ProductId,
                        ProductCode = item.ProductCode,
                        ProductName = item.ProductName,
                        PhysicalInventoryId = item.PhysicalInventoryId,
                        QuantityInInventory = item.QuantityInInventory,
                        QuantityRemaining = item.QuantityRemaining,
                        QuantityDiff = item.QuantityDiff,
                        CategoryCode = item.CategoryCode
                    })
                .OrderBy(item => item.CategoryCode)
                .ThenBy(item => item.ProductCode)
                .ToList();
                var groupList = detailList.GroupBy(x => new { x.CategoryCode }, (key, group) => new
                {
                    CategoryCode = key.CategoryCode,
                    ProductList = group.ToList()
                });
                var ListRow = "";
                int n = 1;
                foreach (var item in groupList)
                {
                    var Row = "<tr style=\"background:#eee\"><td colspan=\"5\"><b>" + item.CategoryCode + "</b></td></tr>";
                    foreach (var g in item.ProductList)
                    {

                        Row += "<tr>"
                           + "<td class=\"text-center\">" + n++ + "</td>"
                           + "<td class=\"text-left\">" + g.ProductCode + "-" + g.ProductName + "</td>"
                           + "<td>" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan(g.QuantityInInventory) + "</td>"
                           + "<td>" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan(g.QuantityRemaining) + "</td>"
                           + "<td>" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan(g.QuantityDiff) + "</td></tr>";
                    }

                    ListRow += Row;
                }



                var style = "<style>.invoice-detail{ width:100%;margin-top: 10px;border-spacing: 0px;}"
                    + ".invoice-detail th{border: 1px solid #000;border-right: none;padding: 5px;}"
                    + " .invoice-detail tr th:last-child {border-right: 1px solid #000;}"
                    + " .invoice-detail td{padding: 5px 5px; border-bottom: 1px solid #000; border-left: 1px solid #000; height: 15px;font-size: 12px;}"
                    + " .invoice-detail tr td:last-child {border-right: 1px solid #000;}"
                    + ".invoice-detail tbody tr:last-child td {border-bottom: 1px solid #000;}"
                    + " .invoice-detail tfoot td{font-weight:bold;border-bottom: 1px solid #000;}"
                    + " .invoice-detail tfoot tr:first-child td{border-top: 1px solid #000;}"
                    + ".text-center{text-align:center;}"
                    + ".text-right{text-align:right;}"
                    + ".text-left{text-align:left;}"
                    + " .logo{ width: 100px;float: left; margin: 0 20px;height: 100px;line-height: 100px;}"
                    + ".logo img{ width:100%;vertical-align: middle;}"
                    + "</style>";

                var table = style + "<table class=\"invoice-detail\"><thead><tr> <th>STT</th> <th>Tên sản phẩm</th><th>SL hệ thống</th><th>SL thực tế</th><th>SL chênh lệch</th></tr></thead><tbody>"
                             + ListRow
                             + "</tbody></table>";

                //lấy template phiếu xuất.
                var template = templatePrintRepository.GetAllTemplatePrint().Where(x => x.Code.Contains("PhysicalInventory")).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                model.Content = template.Content;
                model.Content = model.Content.Replace("{Note}", PhysicalInventory.Note);
                model.Content = model.Content.Replace("{WarehouseName}", PhysicalInventory.WarehouseName);
                model.Content = model.Content.Replace("{ProductInboundCode}", PhysicalInventory.ProductInboundCode);
                model.Content = model.Content.Replace("{ProductOutboundCode}", PhysicalInventory.ProductOutboundCode);
                model.Content = model.Content.Replace("{Code}", PhysicalInventory.Code);
                model.Content = model.Content.Replace("{Day}", PhysicalInventory.CreatedDate.Value.Day.ToString());
                model.Content = model.Content.Replace("{Month}", PhysicalInventory.CreatedDate.Value.Month.ToString());
                model.Content = model.Content.Replace("{Year}", PhysicalInventory.CreatedDate.Value.Year.ToString());
                model.Content = model.Content.Replace("{DataTable}", table);
                model.Content = model.Content.Replace("{Logo}", ImgLogo);
                model.Content = model.Content.Replace("{System.CompanyName}", company);
                model.Content = model.Content.Replace("{System.AddressCompany}", address);
                model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
                model.Content = model.Content.Replace("{System.Fax}", fax);
                model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
                model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
                return View(model);
            }

            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            return RedirectToAction("Index");
        }
    }
}
