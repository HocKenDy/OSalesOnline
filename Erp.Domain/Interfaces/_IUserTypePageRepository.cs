using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Interfaces
{
    public interface IUserTypePageRepository
    {
        IEnumerable<UserTypePage> GetAllItem();
        IEnumerable<UserTypePage> GetAllItemByPageId(int pageId);
        IEnumerable<UserTypePage> GetAllItemByUserTypeID(int userTypeId);
        //IEnumerable<UserTypePage> GetAllViewItem();
        UserTypePage GetByUserTypeIdPageId(int userTypeId, int pageId);
        void Insert(UserTypePage obj);
        void Delete(int userTypeId, int pageId);
        int DeleteByUserTypeId(int UserTypeId, int UserId);
        int DeleteAll();
        void Update(UserTypePage obj);
        int InsertAll(int UserTypeId, int UserId, DataTable ListPage);
    }
}
