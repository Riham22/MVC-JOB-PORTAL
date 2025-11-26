using Domains;
using Domains.UserModel;

namespace DAL.Contracts
{
    public interface IAdminDashboardRepository
    {

        Task<int> GetUserCountAsync();
        Task<int> GetCompanyCountAsync();
        Task<int> GetJobCountAsync(DateTime? fromDate = null);
        Task<int> GetCategoryCountAsync();
        Task<(List<string> labels, List<int> counts)> GetJobsByCategoryAsync(DateTime? fromDate = null);
        Task<(List<string> labels, List<int> counts)> GetJobsByCompanyAsync(DateTime? fromDate = null);


        Task<List<EmployerProfile>> GetEmployersAsync();
        Task<List<JobPost>> GetJobsAsync();
        Task<JobPost?> GetJobByIdAsync(Guid id);
        Task DeleteJobAsync(Guid id);
        Task<List<ApplicationUser>> GetUsersAsync();
        Task<List<Company>> GetCompaniesAsync();
        Task<List<JobSeekerProfile>> GetJobSeekersAsync();
        Task<List<JobCategory>> GetIndustriesAsync();
        Task<List<JobPost>> GetJobsByCategoryAsync(Guid categoryId);
        Task<List<Company>> GetCompaniesByCategoryAsync(Guid categoryId);
        Task AddJobTypeAsync(JobType type);
        Task UpdateJobTypeAsync(JobType type);
        Task DeleteJobTypeAsync(Guid id);


        Task AddJobCategoryAsync(JobCategory category);
        Task UpdateJobCategoryAsync(JobCategory category);
        Task DeleteJobCategoryAsync(Guid id);
        Task<List<JobType>> GetJobTypesAsync();
        Task<EmployerProfile> GetEmployerByIdAsync(Guid id);
        Task<Company> GetCompanyByIdAsync(Guid id);
        Task<JobType> GetJobTypeByIdAsync(Guid id);
        Task<(List<string> labels, List<int> counts)> GetJobGrowthOverTimeAsync(DateTime? fromDate = null);
        Task<(List<string> labels, List<int> counts)> GetTopCompaniesByJobsAsync(DateTime? fromDate = null, int top = 5);
        Task<JobSeekerProfile> GetJobSeekerByIdAsync(Guid id);
    }
}
