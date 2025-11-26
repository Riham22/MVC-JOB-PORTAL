using AutoMapper;
using BL.Contracts;
using BL.Dtos;
using BL.Dtos.Dashboard;
using DAL.Contracts;
using Domains;
using Domains.UserModel;
using System.Linq;

public class AdminDashboardService : IAdminDashboardService
{
    private readonly IAdminDashboardRepository _repo;
    private readonly IMapper _mapper;

    public AdminDashboardService(IAdminDashboardRepository repo,IMapper mapper)
    {
        _repo = repo;
        _mapper= mapper;
    }


    private DateTime? GetStartDate(string period)
    {
        var now = DateTime.Now;
        return period switch
        {
            "week" => now.AddDays(-7),
            "month" => now.AddMonths(-1),
            _ => null
        };
    }

    public async Task<DashboardSummaryDto> GetDashboardSummaryAsync(string period)
    {
        var fromDate = GetStartDate(period);
        var (catLabels, catCounts) = await _repo.GetJobsByCategoryAsync(fromDate);
        var (compLabels, compCounts) = await _repo.GetJobsByCompanyAsync(fromDate);

        // New data
        var (growthLabels, growthCounts) = await _repo.GetJobGrowthOverTimeAsync(fromDate);
        var (topLabels, topCounts) = await _repo.GetTopCompaniesByJobsAsync(fromDate, 5);

        return new DashboardSummaryDto
        {
            UserCount = await _repo.GetUserCountAsync(),
            CompanyCount = await _repo.GetCompanyCountAsync(),
            JobCount = await _repo.GetJobCountAsync(fromDate),
            CategoryCount = await _repo.GetCategoryCountAsync(),
            CategoryLabels = catLabels,
            CategoryJobCounts = catCounts,
            CompanyLabels = compLabels,
            CompanyJobCounts = compCounts,
            JobGrowthLabels = growthLabels,
            JobGrowthCounts = growthCounts,
            TopCompaniesLabels = topLabels,
            TopCompaniesCounts = topCounts
        };
    }



    public async Task<List<EmployerProfileDto>> GetEmployersAsync()
    {
        var employers = await _repo.GetEmployersAsync();  

        var result = employers.Select(e => new EmployerProfileDto
        {
            Id = e.Id,
            EmpName = e.User != null ? $"{e.User.FName} {e.User.LName}" : "N/A",
            EmpEmail = e.User?.Email,
            CompanyName = e.Company?.Name,
            CompanyId = e.Company.Id
        }).ToList();

        return result;
    }


    public async Task<EmployerProfileDto> GetEmployerByIdAsync(Guid id)
    {
        var employer = await _repo.GetEmployerByIdAsync(id);
        var employerDto = new EmployerProfileDto
        {
            Id = employer.Id,
            EmpName = employer.User.FName + employer.User.LName,
            EmpEmail = employer.User?.Email,
            CompanyName = employer.Company?.Name,
            CompanyId = employer.Company.Id,
            CreatedAt = employer.CreatedDate ?? DateTime.Now
        };
        return employerDto;

    }

    public async Task<List<ApplicationUserDto>> GetUsersAsync()
    {

        var users = await _repo.GetUsersAsync();
        var userDtos = users.Select(u => new ApplicationUserDto
        {
            Id = u.Id,
            DisplayName = u.FName +" "+ u.LName,
            Email = u.Email,

        }).ToList();
        return userDtos;

    }
    public async Task<List<SeekerDto>> GetJobSeekersAsync()
    {
        var jobSeekers = await _repo.GetJobSeekersAsync();
        var jobSeekerDtos = jobSeekers.Select(js => new SeekerDto
        {
            Id = js.Id,
            FullName =js.User.FName?? "Unknown",
            Headline = js.Headline,
            Email=js.User.Email,
            CreatedAt=js.CreatedDate?? DateTime.Now,
           
            Summary = js.Summary,
            Country = js.Country,
            City = js.City,


        }).ToList();
        return jobSeekerDtos;
    }
    public async Task<SeekerDto> GetJobSeekerByIdAsync(Guid id)
    {
        var jobSeeker = await _repo.GetJobSeekerByIdAsync(id);
        var jobSeekerDto = new SeekerDto
        {
            Id = jobSeeker.Id,
            FullName = jobSeeker.User != null ? $"{jobSeeker.User.FName} {jobSeeker.User.LName}" : "N/A",

            Headline = jobSeeker.Headline,
            Summary = jobSeeker.Summary,
            Country = jobSeeker.Country,
            City = jobSeeker.City,
            CreatedAt = jobSeeker.CreatedDate ?? DateTime.Now
        };
        return jobSeekerDto;
    }

    public async Task<List<JobPostDto>> GetJobsAsync()
     
    {
        var jobs = await _repo.GetJobsAsync();
        return _mapper.Map<List<JobPostDto>>(jobs);
    }

    public async Task<JobPostDto?> GetJobByIdAsync(Guid id)
       {
        var job = await _repo.GetJobByIdAsync(id);


        return job == null ? null : _mapper.Map<JobPostDto>(job);
    }

    public async Task DeleteJobAsync(Guid id)
        => await _repo.DeleteJobAsync(id);


    public async Task<List<CompanyDto>> GetCompaniesAsync()
    {
        var companies = await _repo.GetCompaniesAsync();
        return _mapper.Map<List<CompanyDto>>(companies);
    }

   

    public async Task<List<JobCategoryDto>> GetIndustriesAsync()
     {
        var categories = await _repo.GetIndustriesAsync();
       
        var categoryDtos = categories.Select(c => new JobCategoryDto
        {
            Id = c.Id,
            Name = c.Name
        }).ToList();    
        return categoryDtos;

    }
   


    public async Task<List<JobPostDto>> GetJobsByCategoryAsync(Guid categoryId)
    {
        var jobs = await _repo.GetJobsByCategoryAsync(categoryId);
        return _mapper.Map<List<JobPostDto>>(jobs);
    }

    public async Task<List<CompanyDto>> GetCompaniesByCategoryAsync(Guid categoryId)
    {
        var companies = await _repo.GetCompaniesByCategoryAsync(categoryId);
        return _mapper.Map<List<CompanyDto>>(companies);
    }
   

    public async Task DeleteJobTypeAsync(Guid id)
        => await _repo.DeleteJobTypeAsync(id);


    public async Task<List<JobTypeDto>> GetJobTypesAsync()
    {
        var jobTypes = await _repo.GetJobTypesAsync();
        var typeDto = jobTypes.Select(c => new JobTypeDto
        {
            Id = c.Id,
            Name = c.Name
        }).ToList();

        return typeDto;

    }
    public async Task CreateJobTypeAsync(JobTypeDto type)
    {
        var jobType = new JobType
        {
            Id = Guid.NewGuid(),
            Name = type.Name
        };
        await _repo.AddJobTypeAsync(jobType);

    }

    public async Task UpdateJobTypeAsync(JobTypeDto type)
    {

        var jobType = new JobType
        {
            Id = type.Id,
            Name = type.Name
        };
        await _repo.UpdateJobTypeAsync(jobType);
    }

    public async Task<JobTypeDto> GetJobTypeByIdAsync(Guid id)
    {
        var jobtype = await _repo.GetJobTypeByIdAsync(id);
        if (jobtype == null)
        {
            return null;
        }
        return _mapper.Map<JobTypeDto>(jobtype);

    }


    public async Task CreateJobCategoryAsync(JobCategoryDto dto)
    {
        var category = new JobCategory
        {
            Id = Guid.NewGuid(),
            Name = dto.Name
        };

        await _repo.AddJobCategoryAsync(category);
    }
    public async Task UpdateJobCategoryAsync(JobCategoryDto dto)
    {
        var category = _mapper.Map<JobCategory>(dto);

        await _repo.UpdateJobCategoryAsync(category);

    }



    public async Task DeleteJobCategoryAsync(Guid id)
        => await _repo.DeleteJobCategoryAsync(id);
 public async Task<CompanyDto> GetCompanyByIdAsync(Guid id)
    {
        var company = await _repo.GetCompanyByIdAsync(id);
        //if (company == null)
        //    return null;

        var dto = new CompanyDto
        {
            Id = company.Id,
            Name = company.Name,
            Description = company.Description,
            Country = company.Country,
            City = company.City,
            Website = company.Website,
            LogoUrl = company.LogoUrl,
            Status = company.Status,
            CreatedAt = company.CreatedDate ?? DateTime.Now,
            JobPosts = company.JobPosts.ToList(),
           EmployerProfiles= company.EmployerProfiles.ToList()
        };

        return dto;
    }






}

