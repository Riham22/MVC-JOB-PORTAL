using BL.Dtos.Dashboard;
using BL.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IAdminDashboardService
{

    Task<DashboardSummaryDto> GetDashboardSummaryAsync(string period);

    //Task<(List<string> labels, List<int> counts)> GetJobGrowthOverTimeAsync(string period);
    //Task<(List<string> labels, List<int> counts)> GetTopCompaniesByJobsAsync(string period);

    Task<List<EmployerProfileDto>> GetEmployersAsync();


    Task<List<JobPostDto>> GetJobsAsync();
    Task<JobPostDto?> GetJobByIdAsync(Guid id);
    Task DeleteJobAsync(Guid id);
    Task<List<JobPostDto>> GetJobsByCategoryAsync(Guid categoryId);


    Task<List<CompanyDto>> GetCompaniesAsync();
    Task<List<CompanyDto>> GetCompaniesByCategoryAsync(Guid categoryId);


    Task<List<SeekerDto>> GetJobSeekersAsync();
    Task<SeekerDto> GetJobSeekerByIdAsync(Guid id);


    Task<List<JobCategoryDto>> GetIndustriesAsync();
    Task CreateJobCategoryAsync(JobCategoryDto dto);
    Task UpdateJobCategoryAsync(JobCategoryDto dto);
    Task DeleteJobCategoryAsync(Guid id);

    Task<List<JobTypeDto>> GetJobTypesAsync();
    Task CreateJobTypeAsync(JobTypeDto dto);
    Task UpdateJobTypeAsync(JobTypeDto dto);
    Task DeleteJobTypeAsync(Guid id);



    Task<EmployerProfileDto> GetEmployerByIdAsync(Guid id);
    Task<CompanyDto> GetCompanyByIdAsync(Guid id);
    Task<JobTypeDto> GetJobTypeByIdAsync(Guid id);
    Task<List<ApplicationUserDto>> GetUsersAsync();

}
