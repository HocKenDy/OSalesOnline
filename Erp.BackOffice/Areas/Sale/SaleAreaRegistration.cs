using Erp.BackOffice.Sale.Models;
using Erp.Domain.Sale.Entities;
using System.Web.Mvc;

namespace Erp.BackOffice.Sale
{
    public class SaleAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Sale";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                 "Sale_SaleCategory",
                 "SaleCategory/{action}/{id}",
                 new { controller = "SaleCategory", action = "Index", id = UrlParameter.Optional }
             );

            context.MapRoute(
                 "Sale_Product",
                 "Product/{action}/{id}",
                 new { controller = "Product", action = "Index", id = UrlParameter.Optional }
             );
            context.MapRoute(
            "Sale_Card",
            "Card/{action}/{id}",
            new { controller = "Card", action = "Index", id = UrlParameter.Optional }
             );
            context.MapRoute(
                "Sale_Gift",
                "Gift/{action}/{id}",
                new { controller = "Gift", action = "Index", id = UrlParameter.Optional }
                );
            context.MapRoute(
                 "Sale_Service",
                 "Service/{action}/{id}",
                 new { controller = "Service", action = "Index", id = UrlParameter.Optional }
             );

            context.MapRoute(
                 "Sale_Supplier",
                 "Supplier/{action}/{id}",
                 new { controller = "Supplier", action = "Index", id = UrlParameter.Optional }
             );

            context.MapRoute(
                 "Sale_Warehouse",
                 "Warehouse/{action}/{id}",
                 new { controller = "Warehouse", action = "Index", id = UrlParameter.Optional }
             );

            context.MapRoute(
                 "Sale_PurchaseOrder",
                 "PurchaseOrder/{action}/{id}",
                 new { controller = "PurchaseOrder", action = "Index", id = UrlParameter.Optional }
             );

            context.MapRoute(
                 "Sale_ProductInvoice",
                 "ProductInvoice/{action}/{id}",
                 new { controller = "ProductInvoice", action = "Index", id = UrlParameter.Optional }
             );

            context.MapRoute(
                 "Sale_Inventory",
                 "Inventory/{action}/{id}",
                 new { controller = "Inventory", action = "Index", id = UrlParameter.Optional }
             );

            context.MapRoute(
                 "Sale_ProductOutbound",
                 "ProductOutbound/{action}/{id}",
                 new { controller = "ProductOutbound", action = "Index", id = UrlParameter.Optional }
             );

            context.MapRoute(
                 "Sale_ProductInBound",
                 "ProductInBound/{action}/{id}",
                 new { controller = "ProductInBound", action = "Index", id = UrlParameter.Optional }
             );
            context.MapRoute(
                 "Sale_PhysicalInventory",
                 "PhysicalInventory/{action}/{id}",
                 new { controller = "PhysicalInventory", action = "Index", id = UrlParameter.Optional }
             );

            context.MapRoute(
                 "Sale_PhysicalInventoryDetail",
                 "PhysicalInventoryDetail/{action}/{id}",
                 new { controller = "PhysicalInventoryDetail", action = "Index", id = UrlParameter.Optional }
             );

            context.MapRoute(
                 "Sale_ObjectAttribute",
                 "ObjectAttribute/{action}/{id}",
                 new { controller = "ObjectAttribute", action = "Index", id = UrlParameter.Optional }
             );

            context.MapRoute(
                 "Sales_Commision",
                 "Commision/{action}/{id}",
                 new { controller = "Commision", action = "Index", id = UrlParameter.Optional }
             );

            context.MapRoute(
                "Sale_vwCommision_Branch",
                "vwCommision_Branch/{action}/{id}",
                new { controller = "vwCommision_Branch", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
            "Sale_vwBranchDepartment",
            "vwBranchDepartment/{action}/{id}",
            new { controller = "vwBranchDepartment", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
            "Sale_SaleOrder",
            "SaleOrder/{action}/{id}",
            new { controller = "SaleOrder", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
            "Sale_SaleOrderDetail",
            "SaleOrderDetail/{action}/{id}",
            new { controller = "SaleOrderDetail", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
            "Sale_Report",
            "SaleReport/{action}/{id}",
            new { controller = "SaleReport", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
            "Sale_WarehouseLocationItem",
            "WarehouseLocationItem/{action}/{id}",
            new { controller = "WarehouseLocationItem", action = "Index", id = UrlParameter.Optional }
            );


            context.MapRoute(
            "Sale_Promotion",
            "Promotion/{action}/{id}",
            new { controller = "Promotion", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
            "Sale_SalesReturns",
            "SalesReturns/{action}/{id}",
            new { controller = "SalesReturns", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
            "Sale_CommisionSale",
            "CommisionSale/{action}/{id}",
            new { controller = "CommisionSale", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
            "Sale_UsingService",
            "UsingService/{action}/{id}",
            new { controller = "UsingService", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
            "Sale_TemplatePrint",
            "TemplatePrint/{action}/{id}",
            new { controller = "TemplatePrint", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
               "Sale_Branch",
               "Branch/{action}/{id}",
               new { controller = "Branch", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
             "Sale_Commision_Customer",
             "CommisionCustomer/{action}/{id}",
             new { controller = "CommisionCustomer", action = "Index", id = UrlParameter.Optional }
             );
            context.MapRoute(
         "Sale_RequestInbound",
         "RequestInbound/{action}/{id}",
         new { controller = "RequestInbound", action = "Index", id = UrlParameter.Optional }
         );

            context.MapRoute(
            "Sale_RequestInboundDetail",
            "RequestInboundDetail/{action}/{id}",
            new { controller = "RequestInboundDetail", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute(
            "Sale_Configs",
            "Configs/{action}/{id}",
            new { controller = "Configs", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute("Sale_Cars",
            "Cars/{action}/{id}",
            new { controller = "Cars", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute("Sale_CarLine",
            "CarLine/{action}/{id}",
            new { controller = "CarLine", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute("Sale_MemberCardType",
            "MemberCardType/{action}/{id}",
            new { controller = "MemberCardType", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute("Sale_HistoryPoint",
            "HistoryPoint/{action}/{id}",
            new { controller = "HistoryPoint", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute("Sale_MemberCard",
            "MemberCard/{action}/{id}",
            new { controller = "MemberCard", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute("Sale_PriceLog",
            "PriceLog/{action}/{id}",
            new { controller = "PriceLog", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute("Sale_RePayPoint",
            "RePayPoint/{action}/{id}",
            new { controller = "RePayPoint", action = "Index", id = UrlParameter.Optional }
            );

            context.MapRoute("Sale_RePayPoints",
            "RePayPoints/{action}/{id}",
            new { controller = "RePayPoints", action = "Index", id = UrlParameter.Optional }
            );
            //<append_content_route_here>

            RegisterAutoMapperMap();
        }

        private static void RegisterAutoMapperMap()
        {
            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.Product, ProductViewModel>();
            AutoMapper.Mapper.CreateMap<ProductViewModel, Domain.Sale.Entities.Product>();
            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.vwProduct, ProductViewModel>();

            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.Supplier, SupplierViewModel>();
            AutoMapper.Mapper.CreateMap<SupplierViewModel, Domain.Sale.Entities.Supplier>();
            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.vwSupplier, SupplierViewModel>();
            AutoMapper.Mapper.CreateMap<SupplierViewModel, Domain.Sale.Entities.vwSupplier>();

            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.Warehouse, WarehouseViewModel>();
            AutoMapper.Mapper.CreateMap<WarehouseViewModel, Domain.Sale.Entities.Warehouse>();

            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.PurchaseOrder, PurchaseOrderViewModel>();
            AutoMapper.Mapper.CreateMap<PurchaseOrderViewModel, Domain.Sale.Entities.PurchaseOrder>();
            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.vwPurchaseOrder, PurchaseOrderViewModel>();

            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.ProductInvoice, ProductInvoiceViewModel>();
            AutoMapper.Mapper.CreateMap<ProductInvoiceViewModel, Domain.Sale.Entities.ProductInvoice>();
            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.vwProductInvoice, ProductInvoiceViewModel>();
            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.vwProductInvoiceDetail, ProductInvoiceDetailViewModel>();
            AutoMapper.Mapper.CreateMap<ProductInvoiceDetailViewModel, ProductOutboundDetailViewModel>();

            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.PurchaseOrderDetail, PurchaseOrderDetailViewModel>();
            AutoMapper.Mapper.CreateMap<PurchaseOrderDetailViewModel, Domain.Sale.Entities.PurchaseOrderDetail>();
            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.vwPurchaseOrderDetail, PurchaseOrderDetailViewModel>();
            AutoMapper.Mapper.CreateMap<PurchaseOrderDetailViewModel, Domain.Sale.Entities.vwPurchaseOrderDetail>();

            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.Inventory, InventoryViewModel>();
            AutoMapper.Mapper.CreateMap<InventoryViewModel, Domain.Sale.Entities.Inventory>();

            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.ProductOutbound, ProductOutboundTransferViewModel>();
            AutoMapper.Mapper.CreateMap<ProductOutboundTransferViewModel, Domain.Sale.Entities.ProductOutbound>();
            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.ProductOutbound, ProductOutboundViewModel>();
            AutoMapper.Mapper.CreateMap<ProductOutboundViewModel, Domain.Sale.Entities.ProductOutbound>();
            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.vwProductOutbound, ProductOutboundViewModel>();
            AutoMapper.Mapper.CreateMap<ProductOutboundViewModel, Domain.Sale.Entities.vwProductOutbound>();

            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.ProductInbound, ProductInboundViewModel>();
            AutoMapper.Mapper.CreateMap<ProductInboundViewModel, Domain.Sale.Entities.ProductInbound>();
            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.vwProductInbound, ProductInboundViewModel>();

            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.ProductInboundDetail, ProductInboundDetailViewModel>();
            AutoMapper.Mapper.CreateMap<ProductInboundDetailViewModel, Domain.Sale.Entities.ProductInboundDetail>();
            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.vwProductInboundDetail, ProductInboundDetailViewModel>();
            AutoMapper.Mapper.CreateMap<ProductInboundDetailViewModel, Domain.Sale.Entities.vwProductInboundDetail>();

            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.ProductOutboundDetail, ProductOutboundDetailViewModel>();
            AutoMapper.Mapper.CreateMap<ProductOutboundDetailViewModel, Domain.Sale.Entities.ProductOutboundDetail>();
            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.vwProductOutboundDetail, ProductOutboundDetailViewModel>();
            AutoMapper.Mapper.CreateMap<ProductOutboundDetailViewModel, Domain.Sale.Entities.vwProductOutboundDetail>();

            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.PhysicalInventory, PhysicalInventoryViewModel>();
            AutoMapper.Mapper.CreateMap<PhysicalInventoryViewModel, Domain.Sale.Entities.PhysicalInventory>();
            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.vwPhysicalInventory, PhysicalInventoryViewModel>();

            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.PhysicalInventoryDetail, PhysicalInventoryDetailViewModel>();
            AutoMapper.Mapper.CreateMap<PhysicalInventoryDetailViewModel, Domain.Sale.Entities.PhysicalInventoryDetail>();

            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.ObjectAttribute, ObjectAttributeViewModel>();
            AutoMapper.Mapper.CreateMap<ObjectAttributeViewModel, Domain.Sale.Entities.ObjectAttribute>();

            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.ObjectAttributeValue, ObjectAttributeValueViewModel>();
            AutoMapper.Mapper.CreateMap<ObjectAttributeValueViewModel, Domain.Sale.Entities.ObjectAttributeValue>();

            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.Commision, CommisionViewModel>();
            AutoMapper.Mapper.CreateMap<CommisionViewModel, Domain.Sale.Entities.Commision>();

            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.WarehouseLocationItem, WarehouseLocationItemViewModel>();
            AutoMapper.Mapper.CreateMap<WarehouseLocationItemViewModel, Domain.Sale.Entities.WarehouseLocationItem>();

            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.SalesReturns, SalesReturnsViewModel>();
            AutoMapper.Mapper.CreateMap<SalesReturnsViewModel, Domain.Sale.Entities.SalesReturns>();
            AutoMapper.Mapper.CreateMap<SalesReturnsDetailViewModel, Domain.Sale.Entities.SalesReturnsDetail>();
            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.vwSalesReturns, SalesReturnsViewModel>();
            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.vwProductInvoiceDetail, SalesReturnsDetailViewModel>();
            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.vwSalesReturnsDetail, SalesReturnsDetailViewModel>();

            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.Promotion, PromotionViewModel>();
            AutoMapper.Mapper.CreateMap<PromotionViewModel, Domain.Sale.Entities.Promotion>();

            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.PromotionDetail, PromotionDetailViewModel>();
            AutoMapper.Mapper.CreateMap<PromotionDetailViewModel, Domain.Sale.Entities.PromotionDetail>();

            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.vwProductInvoice, SalesReturnsViewModel>();
            AutoMapper.Mapper.CreateMap<SalesReturnsViewModel, Domain.Sale.Entities.vwProductInvoice>();

            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.CommisionSale, CommisionSaleViewModel>();
            AutoMapper.Mapper.CreateMap<CommisionSaleViewModel, Domain.Sale.Entities.CommisionSale>();

            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.UsingService, UsingServiceViewModel>();
            AutoMapper.Mapper.CreateMap<UsingServiceViewModel, Domain.Sale.Entities.UsingService>();

            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.TemplatePrint, TemplatePrintViewModel>();
            AutoMapper.Mapper.CreateMap<TemplatePrintViewModel, Domain.Sale.Entities.TemplatePrint>();

            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.Branch, BranchViewModel>();
            AutoMapper.Mapper.CreateMap<BranchViewModel, Domain.Sale.Entities.Branch>();
            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.vwBranch, BranchViewModel>();
            AutoMapper.Mapper.CreateMap<BranchViewModel, Domain.Sale.Entities.vwBranch>();

            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.CommisionCustomer, CommisionCustomerViewModel>();
            AutoMapper.Mapper.CreateMap<CommisionCustomerViewModel, Domain.Sale.Entities.CommisionCustomer>();
            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.vwCommisionCustomer, CommisionCustomerViewModel>();
            AutoMapper.Mapper.CreateMap<CommisionCustomerViewModel, Domain.Sale.Entities.vwCommisionCustomer>();

            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.RequestInbound, RequestInboundViewModel>();
            AutoMapper.Mapper.CreateMap<RequestInboundViewModel, Domain.Sale.Entities.RequestInbound>();
            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.vwRequestInbound, RequestInboundViewModel>();
            AutoMapper.Mapper.CreateMap<RequestInboundViewModel, Domain.Sale.Entities.vwRequestInbound>();

            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.RequestInboundDetail, RequestInboundDetailViewModel>();
            AutoMapper.Mapper.CreateMap<RequestInboundDetailViewModel, Domain.Sale.Entities.RequestInboundDetail>();
            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.vwRequestInboundDetail, RequestInboundDetailViewModel>();
            AutoMapper.Mapper.CreateMap<RequestInboundDetailViewModel, Domain.Sale.Entities.vwRequestInboundDetail>();

            AutoMapper.Mapper.CreateMap<qts.webapp.backend.domain.Models.Sale.Cars, CarsViewModel>();
            AutoMapper.Mapper.CreateMap<CarsViewModel, qts.webapp.backend.domain.Models.Sale.Cars>();

            AutoMapper.Mapper.CreateMap<qts.webapp.backend.domain.Models.Sale.vwCar, CarsViewModel>();
            AutoMapper.Mapper.CreateMap<CarsViewModel, qts.webapp.backend.domain.Models.Sale.vwCar>();

            AutoMapper.Mapper.CreateMap<qts.webapp.backend.domain.Models.Sale.CarLine, CarLineViewModel>();
            AutoMapper.Mapper.CreateMap<CarLineViewModel, qts.webapp.backend.domain.Models.Sale.CarLine>();


            AutoMapper.Mapper.CreateMap<qts.webapp.backend.domain.Models.Sale.MemberCardType, MemberCardTypeViewModel>();
            AutoMapper.Mapper.CreateMap<MemberCardTypeViewModel, qts.webapp.backend.domain.Models.Sale.MemberCardType>();


            AutoMapper.Mapper.CreateMap<qts.webapp.backend.domain.Models.Sale.HistoryPoint, HistoryPointViewModel>();
            AutoMapper.Mapper.CreateMap<HistoryPointViewModel, qts.webapp.backend.domain.Models.Sale.HistoryPoint>();


            AutoMapper.Mapper.CreateMap<qts.webapp.backend.domain.Models.Sale.MemberCard, MemberCardViewModel>();
            AutoMapper.Mapper.CreateMap<MemberCardViewModel, qts.webapp.backend.domain.Models.Sale.MemberCard>();
            AutoMapper.Mapper.CreateMap<qts.webapp.backend.domain.Models.Sale.vwMemberCard, MemberCardViewModel>();
            AutoMapper.Mapper.CreateMap<MemberCardViewModel, qts.webapp.backend.domain.Models.Sale.vwMemberCard>();

            AutoMapper.Mapper.CreateMap<Domain.Sale.Entities.PriceLog, PriceLogViewModel>();
            AutoMapper.Mapper.CreateMap<PriceLogViewModel, Domain.Sale.Entities.PriceLog>();

            AutoMapper.Mapper.CreateMap<qts.webapp.backend.domain.Models.Sale.RePayPoints, RePayPointsViewModel>();
            AutoMapper.Mapper.CreateMap<qts.webapp.backend.domain.Models.Sale.vwRePayPoints, RePayPointsViewModel>();
            AutoMapper.Mapper.CreateMap<RePayPointsViewModel, qts.webapp.backend.domain.Models.Sale.RePayPoints>();


            AutoMapper.Mapper.CreateMap<qts.webapp.backend.domain.Models.Sale.RePayPointsDetail, RePayPointsDetailViewModel>();
            AutoMapper.Mapper.CreateMap<qts.webapp.backend.domain.Models.Sale.vwRePayPointsDetail, RePayPointsDetailViewModel>();
            AutoMapper.Mapper.CreateMap<RePayPointsDetailViewModel, ProductOutboundDetailViewModel>();
            AutoMapper.Mapper.CreateMap<RePayPointsDetailViewModel, qts.webapp.backend.domain.Models.Sale.RePayPointsDetail>();


            //<append_content_mapper_here>
        }
    }
}
