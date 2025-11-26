using DAL.Contracts;
using DAL.DbContext;
using Domains;
using Domains.UserModel;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class AdminDashboardRepository : IAdminDashboardRepository
    {
        private readonly PortalContext _context;

        public AdminDashboardRepository(PortalContext context)
        {
            _context = context;
        }
        public async Task<(List<string> labels, List<int> counts)> GetTopCompaniesByJobsAsync(DateTime? fromDate = null, int top = 5)
        {
            var query = _context.JobPosts.AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(j => j.CreatedDate >= fromDate.Value);

            var topCompanies = await query
                .Where(j => j.CompanyId != null)
                .GroupBy(j => j.Company!.Name)
                .Select(g => new
                {
                    CompanyName = g.Key,
                    JobCount = g.Count()
                })
                .OrderByDescending(g => g.JobCount)
                .Take(top)
                .ToListAsync();

            var labels = topCompanies.Select(g => g.CompanyName).ToList();
            var counts = topCompanies.Select(g => g.JobCount).ToList();

            return (labels, counts);
        }

        public async Task<(List<string> labels, List<int> counts)> GetJobGrowthOverTimeAsync(DateTime? fromDate = null)
        {
            var query = _context.JobPosts.AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(j => j.CreatedDate >= fromDate.Value);

            var growthData = await query
                .Where(j => j.CreatedDate.HasValue)
                .GroupBy(j => j.CreatedDate.Value.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .OrderBy(g => g.Date)
                .ToListAsync();

            var labels = growthData.Select(g => g.Date.ToString("yyyy-MM-dd")).ToList();
            var counts = growthData.Select(g => g.Count).ToList();

            return (labels, counts);
        }






        public async Task<int> GetUserCountAsync()
            => await _context.Users.CountAsync();

        public async Task<int> GetCompanyCountAsync()
            => await _context.Companies.CountAsync();

        public async Task<int> GetJobCountAsync(DateTime? fromDate = null)
        {
            var query = _context.JobPosts.AsQueryable();
            if (fromDate.HasValue)
                query = query.Where(j => j.CreatedDate >= fromDate.Value);
            return await query.CountAsync();
        }

        public async Task<int> GetCategoryCountAsync()
            => await _context.JobCategories.CountAsync();

        public async Task<(List<string> labels, List<int> counts)> GetJobsByCategoryAsync(DateTime? fromDate = null)
        {
            var jobs = _context.JobPosts.AsQueryable();
            if (fromDate.HasValue)
                jobs = jobs.Where(j => j.CreatedDate >= fromDate.Value);

            var categories = await _context.JobCategories.ToListAsync();
            var labels = categories.Select(c => c.Name).ToList();
            var counts = categories.Select(c => jobs.Count(j => j.JobCategoryId == c.Id)).ToList();

            return (labels, counts);
        }

        public async Task<(List<string> labels, List<int> counts)> GetJobsByCompanyAsync(DateTime? fromDate = null)
        {
            var jobs = _context.JobPosts.AsQueryable();
            if (fromDate.HasValue)
                jobs = jobs.Where(j => j.CreatedDate >= fromDate.Value);

            var companies = await _context.Companies.Take(8).ToListAsync();
            var labels = companies.Select(c => c.Name).ToList();
            var counts = companies.Select(c => jobs.Count(j => j.CompanyId == c.Id)).ToList();

            return (labels, counts);
        }


        public async Task<List<EmployerProfile>> GetEmployersAsync()
            => await _context.EmployerProfiles
                .Include(e => e.User)
                .Include(e => e.Company)
                .ToListAsync();

        public async Task<List<JobPost>> GetJobsAsync()
            => await _context.JobPosts
                .Include(j => j.Company)
                .Include(j => j.JobCategory)
                .Include(j => j.JobType)
                .Include(j => j.Applications)
                .ToListAsync();

        public async Task<JobPost?> GetJobByIdAsync(Guid id)
            => await _context.JobPosts
                .Include(j => j.Company)
                .Include(j => j.JobCategory)
                .Include(j => j.JobType)
                .FirstOrDefaultAsync(j => j.Id == id);

        public async Task DeleteJobAsync(Guid id)
        {
            var job = await _context.JobPosts.FindAsync(id);
            if (job != null)
            {
                _context.JobPosts.Remove(job);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<ApplicationUser>> GetUsersAsync()
            => await _context.Users
                .Include(u => u.EmployerProfile)
                .Include(u => u.CreatedJobPosts)
                .Include(u => u.Applications)
                .ToListAsync();

        public async Task<List<Company>> GetCompaniesAsync()
            => await _context.Companies
                .Include(c => c.JobPosts)
                .Include(c => c.EmployerProfiles)
                .ToListAsync();

        public async Task<List<JobSeekerProfile>> GetJobSeekersAsync()
            => await _context.JobSeekerProfiles
            .Include(j=>j.User)
                .Include(j => j.Applications)
                .Include(j => j.CVFiles)
                .Include(j => j.SavedJobs)
                .ToListAsync();
        public async Task<JobSeekerProfile> GetJobSeekerByIdAsync(Guid id)
            => await _context.JobSeekerProfiles
            .Include(j=>j.User)
                .Include(j => j.Applications)
                .Include(j => j.CVFiles)
                .Include(j => j.SavedJobs)
                .FirstOrDefaultAsync(j => j.Id == id);
        public async Task<List<JobCategory>> GetIndustriesAsync()
            => await _context.JobCategories
                .Include(c => c.JobPosts)
                //.ThenInclude(p => p.Company)
                .ToListAsync();

        public async Task<List<JobPost>> GetJobsByCategoryAsync(Guid categoryId)
            => await _context.JobPosts
                 .Include(j => j.JobCategory)
                .Include(j => j.Company)
                .Where(j => j.JobCategoryId == categoryId)
                .ToListAsync();

        public async Task<List<Company>> GetCompaniesByCategoryAsync(Guid categoryId)
            => await _context.Companies
                //.Include(c => c.EmployerProfiles)?
                //.Include(c => c.EmployerProfiles)
                //.Where(c => c.JobCategoryId == categoryId)
                .ToListAsync();

        public async Task AddJobTypeAsync(JobType type)
        {
            _context.JobTypes.Add(type);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateJobTypeAsync(JobType type)
        {
            _context.JobTypes.Update(type);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteJobTypeAsync(Guid id)
        {
            var type = await _context.JobTypes.FindAsync(id);
            if (type != null)
            {
                _context.JobTypes.Remove(type);
                await _context.SaveChangesAsync();
            }
        }


        public async Task AddJobCategoryAsync(JobCategory category)
        {
            _context.JobCategories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateJobCategoryAsync(JobCategory category)
        {
            _context.JobCategories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteJobCategoryAsync(Guid id)
        {
            var category = await _context.JobCategories.FindAsync(id);
            if (category != null)
            {
                _context.JobCategories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public async  Task<List<JobType>> GetJobTypesAsync()
        {
            var types =  await _context.JobTypes
                .Include(t => t.JobPosts)
                .ToListAsync();

            return types;
        }

        public async Task<EmployerProfile> GetEmployerByIdAsync(Guid id)
        {
            var employer = await _context.EmployerProfiles
                .Include(e => e.User)
                .Include(e => e.Company)
                .FirstOrDefaultAsync(e => e.Id == id);
            return employer;

        }

        public async Task<Company>  GetCompanyByIdAsync(Guid id)
        {
            var company = await _context.Companies
                .Include(c => c.JobPosts)
                .Include(c => c.EmployerProfiles)
                .ThenInclude(c=>c.User)
                .FirstOrDefaultAsync(c => c.Id == id);
            return company;

        }

        public async Task<JobType> GetJobTypeByIdAsync(Guid id)
        {

            var jobtypes =   _context.JobTypes.Include(c => c.JobPosts);

                var jobtype= await jobtypes.FirstOrDefaultAsync(c => c.Id == id);

            return jobtype;
        }


    }
}
