using System;
using System.Threading.Tasks;
using HamstersRocket.Contracts.Models.Publisher;

namespace HamstersRocket.Contracts.Domain
{
    public interface IStorage
    {
        Task SaveReportAsync(Report report);
        void SaveReport(Report report);
    }
}