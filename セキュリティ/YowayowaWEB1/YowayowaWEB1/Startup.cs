using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using Repository;
using Service;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace YowayowaWEB1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public const string CookieScheme = CookieAuthenticationDefaults.AuthenticationScheme;

        // This method gets called by the runtime. Use this meAddSessionthod to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // �N�b�L�[�p�ݒ�
            services.AddDistributedMemoryCache();
            
            services.AddSession(o =>
            {
                //�R���e�L�X�g�p�X������ꍇ��Cookie���ɕt�^����B(�����u���E�U�ł݂��ꍇ�ɃN�b�L�[�l���㏑�����Ȃ��悤�ɁB)
                o.Cookie.Name = $".WEAK.Session";
                o.IdleTimeout = TimeSpan.FromMinutes(3000); //�Z�b�V�����̗L������
                o.Cookie.HttpOnly = false;
                o.Cookie.SecurePolicy = CookieSecurePolicy.None;
                o.Cookie.SameSite = SameSiteMode.None;
                o.Cookie.IsEssential = true;
            });

            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    options.CheckConsentNeeded = context => !context.User.Identity.IsAuthenticated;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //    options.Secure = CookieSecurePolicy.None;
            //    options.HttpOnly = HttpOnlyPolicy.None;
            //});

            // Cookie �ɂ��F�؃X�L�[����ǉ�����
            services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            // .AddCookie();
            //.AddCookie(CookieScheme, options =>
            .AddCookie(options =>
            {
                options.Cookie.Name = ".WEAK.Auth";
            });

            //// �U���h�~�p�ݒ�
            // services.AddAntiforgery(opt =>
            // {
            //    opt.Cookie.Name = $".WEAK.Token";
            //    //HttpOnly�Ȃǂ́AStartup.Configure���ňꗥ�Őݒ肵�Ă���B
            //    //opt.Cookie.HttpOnly = false;
            //    //opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            //    opt.Cookie.SameSite = SameSiteMode.None; //���̃h���C����iframe������ł�cookie�����M�����悤�ݒ�
            // });

            services.AddAuthorization(options =>
            {
                // AllowAnonymous �������w�肳��Ă��Ȃ����ׂĂ� Action �Ȃǂɑ΂��ă��[�U�[�F�؂��K�v�ƂȂ�
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                  .RequireAuthenticatedUser()
                  .Build();
            });

            //services.AddMvc(option => option.EnableEndpointRouting = false);

            //DB�ւ�Connection
            string connectionString = @"server=localhost;port=3305;userid=root;password=root;database=weak";
            services.AddScoped<DbConnection>(p =>
            {
                return new MySqlConnection(connectionString);
            });


            // DI
            services.AddScoped<SqlInjectionService>();
            services.AddScoped<ISqlInjectionRepository, SqlInjectionRepository>();
            services.AddScoped<XssService>();
            services.AddScoped<IXssRepository, XssRepository>();
            services.AddScoped<CsrfService>();
            services.AddScoped<ICsrfRepository, CsrfRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseCookiePolicy();

            app.UseCookiePolicy(new CookiePolicyOptions()
            {
                HttpOnly = HttpOnlyPolicy.None,
                Secure = CookieSecurePolicy.None,
                MinimumSameSitePolicy = SameSiteMode.None
            });

            app.UseSession(); //�Z�b�V����

            app.UseRouting();

            app.UseAuthentication(); //�F��
            app.UseAuthorization(); //�F��

            //app.UseMvc();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
