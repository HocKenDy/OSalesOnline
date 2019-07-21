using Erp.Domain.Sale.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Erp.Domain.Sale.Interfaces
{
    public interface IUsingServiceRepository
    {

        IQueryable<UsingService> GetAllUsingService();
        IQueryable<vwUsingService> GetAllvwUsingService();


        UsingService GetUsingServiceById(int Id);
        vwUsingService GetvwUsingServiceById(int Id);


        void InsertUsingService(UsingService UsingService);
        void InsertUsingService(List<UsingService> ListUsingService);

        void DeleteUsingService(int Id);


        void DeleteUsingServiceRs(int Id);


        void UpdateUsingService(UsingService UsingService);

        //UsingServiceDetail
        IQueryable<UsingServiceDetail> GetAllUsingServiceDetail();
        IQueryable<vwUsingServiceDetail> GetAllvwUsingServiceDetail();

        UsingServiceDetail GetUsingServiceDetailById(int Id);
        vwUsingServiceDetail GetvwUsingServiceDetailById(int Id);

        void InsertUsingServiceDetail(UsingServiceDetail UsingServiceDetail);
        void InsertUsingServiceDetail(List<UsingServiceDetail> ListUsingServiceDetail);
    }
}
