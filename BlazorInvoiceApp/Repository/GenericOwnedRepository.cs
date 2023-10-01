using AutoMapper;
using BlazorInvoiceApp.Data;
using BlazorInvoiceApp.DTOS;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using System.Security.Claims;

namespace BlazorInvoiceApp.Repository
{
    public class GenericOwnedRepository<TEntity, TDTO> : IGenericOwnedRepository<TEntity, TDTO>
         where TEntity : class, IEntity, IOwnedEntity
         where TDTO : class, IDTO, IOwnedDTO
    {
        public readonly ApplicationDbContext context;
        public readonly IMapper mapper;
        public GenericOwnedRepository(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        
        
        protected string? getMyUserId(ClaimsPrincipal User)
        {
            Claim? uid = User?.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            if (uid is not null)
                return uid.Value;
            else
                return null;
        }
        public virtual async Task<List<TDTO>> GetAllMine(ClaimsPrincipal User)
        {
            string? userid = getMyUserId(User);
            if (userid is not null)
            {
                List<TEntity> entities = 
                    await context.Set<TEntity>().Where(e => e.UserId == userid).ToListAsync();
                List<TDTO> result = mapper.Map<List<TDTO>>(entities);
                return result;
            }
            else
            {
                return new List<TDTO>();
            }
        }

        public virtual async Task<TDTO> GetMineById(ClaimsPrincipal User, string id)
        {
            string? userid = getMyUserId(User);
            if (userid is not null)
            {
                TEntity? entity = await 
                    context.Set<TEntity>().Where(entity => entity.Id == id && entity.UserId == userid).FirstOrDefaultAsync(); ;
                TDTO result = mapper.Map<TDTO>(entity);
                return result;
            }
            else
            {
                return null!;
            }

        }

        public virtual async Task<TDTO> UpdateMine(ClaimsPrincipal User, TDTO dto)
        {
            string? userid = getMyUserId(User);
            if (userid is not null)
            {
                TEntity? toupdate = 
                    await context.Set<TEntity>().Where(entity => entity.Id == dto.Id && entity.UserId == userid).FirstOrDefaultAsync();

                if (toupdate is not null)
                {
                    mapper.Map<TDTO, TEntity>(dto, toupdate);
                    context.Entry(toupdate).State = EntityState.Modified;
                    TDTO result = mapper.Map<TDTO>(toupdate);
                    return result;
                    
                }
                else
                {
                    return null!;
                }
            }
            else
            {
                return null!;
            }
        }

        public virtual async Task<bool> DeleteMine(ClaimsPrincipal User, string id)
        {
            string? userid = getMyUserId(User);
            if (userid is not null)
            {
                TEntity? entity = await context.Set<TEntity>().Where(entity => entity.Id == id && entity.UserId == userid).FirstOrDefaultAsync();
                if (entity is not null)
                {
                    context.Remove(entity);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        public virtual async Task<string> AddMine(ClaimsPrincipal User, TDTO dto)
        {
            string? userid = getMyUserId(User);
            if (userid is not null)
            {
                dto.UserId = userid;
                dto.Id = System.Guid.NewGuid().ToString();
                TEntity toadd = mapper.Map<TEntity>(dto);
                await context.Set<TEntity>().AddAsync(toadd);

                return toadd.Id;
            }
            else
            {
                return null!;
            }
        }

        protected async Task<List<TDTO>> GenericQuery(ClaimsPrincipal User, Expression<Func<TEntity, bool>>? expression, List<Expression<Func<TEntity, object>>> includes)
        {
            string? userid = getMyUserId(User);
            if (userid is not null)
            {
                IQueryable<TEntity> query = context.Set<TEntity>().Where(e => e.UserId == userid);
                if (expression is not null)
                    query = query.Where(expression);

                if (includes is not null)
                {
                    foreach (var include in includes)
                    {
                        query = query.Include(include);
                    }
                }
                List<TEntity> entities = await query.ToListAsync();

                List<TDTO> result = mapper.Map<List<TDTO>>(entities);
                return result;
            }
            else
            {
                return new List<TDTO>();
            }
        }


    }
}
