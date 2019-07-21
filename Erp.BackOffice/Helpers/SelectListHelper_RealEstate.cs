using Erp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
namespace Erp.BackOffice.Helpers
{
    public class SelectListHelper_RealEstate
    {
        public static SelectList GetSelectList_Project(object sValue)
        {
            var selectListItems = new List<SelectListItem>();
            ProjectRepository projectRepository = new ProjectRepository(new Domain.RealEstate.ErpRealEstateDbContext());
            //SelectListItem itemEmpty = new SelectListItem();
            //itemEmpty.Text = App_GlobalResources.Wording.ContractTypeName;
            //itemEmpty.Value = null;
            //selectListItems.Add(itemEmpty);
            try
            {
                var q = projectRepository.GetAllProject().OrderBy(item => item.Name).ToList();
                foreach (var i in q)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = i.Name;
                    item.Value = i.Id.ToString();
                    selectListItems.Add(item);
                }
            }
            catch { }

            var selectList = new SelectList(selectListItems, "Value", "Text", sValue);

            return selectList;
        }

        public static SelectList GetSelectList_Condos(object sValue)
        {
            var selectListItems = new List<SelectListItem>();
            CondosRepository CondosRepository = new CondosRepository(new Domain.RealEstate.ErpRealEstateDbContext());
            SelectListItem itemEmpty = new SelectListItem();
            itemEmpty.Text = App_GlobalResources.Wording.Empty;
            itemEmpty.Value = null;
            selectListItems.Add(itemEmpty);
            try
            {
                var q = CondosRepository.GetAllCondos().OrderBy(item => item.Id);
                foreach (var i in q)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = i.Code + " - " + i.Name;
                    item.Value = i.Id.ToString();
                    selectListItems.Add(item);
                }
            }
            catch { }

            var selectList = new SelectList(selectListItems, "Value", "Text", sValue);

            return selectList;
        }

        public static SelectList GetSelectList_Block(object ProjectId, object sValue)
        {
            var selectListItems = new List<SelectListItem>();
            BlockRepository blockRepository = new BlockRepository(new Domain.RealEstate.ErpRealEstateDbContext());
            //SelectListItem itemEmpty = new SelectListItem();
            //itemEmpty.Text = App_GlobalResources.Wording.ContractTypeName;
            //itemEmpty.Value = null;
            //selectListItems.Add(itemEmpty);
            try
            {
                if (ProjectId != null)
                {
                    int id = Convert.ToInt32(ProjectId);
                    var q = blockRepository.GetAllBlock().Where(item => item.ProjectId == id).OrderBy(item => item.Name).ToList();
                    foreach (var i in q)
                    {
                        SelectListItem item = new SelectListItem();
                        item.Text = "Block " + i.Name;
                        item.Value = i.Id.ToString();
                        selectListItems.Add(item);
                    }
                }
            }
            catch { }

            var selectList = new SelectList(selectListItems, "Value", "Text", sValue);

            return selectList;
        }

        public static SelectList GetSelectList_Floor(object BlockId, object sValue)
        {
            var selectListItems = new List<SelectListItem>();
            FloorRepository floorRepository = new FloorRepository(new Domain.RealEstate.ErpRealEstateDbContext());
            //SelectListItem itemEmpty = new SelectListItem();
            //itemEmpty.Text = App_GlobalResources.Wording.ContractTypeName;
            //itemEmpty.Value = null;
            //selectListItems.Add(itemEmpty);
            try
            {
                if (BlockId != null)
                {
                    int id = Convert.ToInt32(BlockId);
                    var q = floorRepository.GetAllFloor().Where(item => item.BlockId == id).OrderBy(item => item.OrderNo).ToList();
                    foreach (var i in q)
                    {
                        SelectListItem item = new SelectListItem();
                        item.Text = "Tầng " + i.Name;
                        item.Value = i.Id.ToString();
                        selectListItems.Add(item);
                    }
                }
            }
            catch { }

            var selectList = new SelectList(selectListItems, "Value", "Text", sValue);

            return selectList;
        }
    }
}